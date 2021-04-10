using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscillogramLoadingParameters;
using Unicon2.Fragments.Oscilliscope.Infrastructure.ViewModel;
using Unicon2.Fragments.Oscilliscope.Model;
using Unicon2.Fragments.Oscilliscope.Model.Helpers;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Progress;
using Unicon2.Infrastructure.Services.UniconProject;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.Oscilliscope.Helpers
{
    public class OscillogramLoader
    {
        private const int PAGE_SIZE_IN_WORD = 1024;
        private const int MAX_QUERY_SIZE = 125;
        private Func<ITaskProgressReport> _taskProgressReportGettingFunc;
        private readonly IUniconProjectService _uniconProjectService;

        public OscillogramLoader(Func<ITaskProgressReport> taskProgressReportGettingFunc, IUniconProjectService uniconProjectService)
        {
            _taskProgressReportGettingFunc = taskProgressReportGettingFunc;
            _uniconProjectService = uniconProjectService;
        }


        public async Task LoadOscillogramsByNumber(List<int> numberOfOscillograms,
            IProgress<ITaskProgressReport> progress, CancellationToken cancellationToken,
            IOscilloscopeModel oscilloscopeModel, IOscilloscopeViewModel oscilloscopeViewModel, DeviceContext deviceContext)
        {
            ITaskProgressReport progressReport = _taskProgressReportGettingFunc();
            int numberOfPages = 0;

            foreach (int numberOfOscillogram in numberOfOscillograms)
            {
                if (oscilloscopeViewModel.Oscillograms.Any((oscillogram1 => oscillogram1.OscillogramNumber == numberOfOscillogram)))
                    continue;
                IJournalRecord oscilligramRecord = oscilloscopeModel.OscilloscopeJournal.JournalRecords[numberOfOscillogram - 1];
                oscilloscopeModel.OscillogramLoadingParameters.Initialize(oscilligramRecord.FormattedValues,
                    oscilloscopeModel.OscilloscopeJournal.RecordTemplate);
                int numberOfPoints = oscilloscopeModel.OscillogramLoadingParameters.GetOscillogramCountingsNumber() *
                                     oscilloscopeModel.OscillogramLoadingParameters.GetSizeOfCountingInWords();
                numberOfPages += (int) Math.Ceiling((double) numberOfPoints / PAGE_SIZE_IN_WORD);
            }

            progressReport.TotalProgressAmount = numberOfPages;
            progress.Report(progressReport);

            foreach (int numberOfOscillogram in numberOfOscillograms)
            {
                IJournalRecord oscilligramRecord = oscilloscopeModel.OscilloscopeJournal.JournalRecords[numberOfOscillogram - 1];
                oscilloscopeModel.OscillogramLoadingParameters.Initialize(oscilligramRecord.FormattedValues,
                    oscilloscopeModel.OscilloscopeJournal.RecordTemplate);
                double oscLengthInCountings = oscilloscopeModel.OscillogramLoadingParameters.GetOscillogramCountingsNumber();
                double sizeOfCounting = oscilloscopeModel.OscillogramLoadingParameters.GetSizeOfCountingInWords();
                double oscLengthInWords = oscLengthInCountings * sizeOfCounting;
                double pointOfOscStart = oscilloscopeModel.OscillogramLoadingParameters.GetPointOfStart();
                double pageCount = (int) Math.Ceiling(oscLengthInWords / PAGE_SIZE_IN_WORD);
                ushort oscStartPageIndex = (ushort) (pointOfOscStart / PAGE_SIZE_IN_WORD);
                double endPage = oscStartPageIndex + pageCount;
                Oscillogram oscillogram = new Oscillogram();
                try
                {
                    await this.ReadOscilligramToEndRecursive(oscStartPageIndex, oscillogram, (ushort) endPage, progress,
                        progressReport, cancellationToken, deviceContext, oscilloscopeModel);
                }
                catch
                {
                    progressReport.CurrentProgressAmount = 0;
                    progressReport.TotalProgressAmount = 1;
                    progress.Report(progressReport);
                    return;
                }

                OscillogramHelper.InvertOscillogram(oscillogram, oscilloscopeModel.OscillogramLoadingParameters, PAGE_SIZE_IN_WORD);
                oscillogram.OscillogramNumber = numberOfOscillogram - 1;
                if (oscilloscopeViewModel.Oscillograms.Any((oscillogram1 =>
                    oscillogram1.OscillogramNumber == oscillogram.OscillogramNumber)))
                {
                    oscilloscopeViewModel.Oscillograms.Remove(oscilloscopeViewModel.Oscillograms.First((oscillogram1 =>
                        oscillogram1.OscillogramNumber == oscillogram.OscillogramNumber)));
                }

                oscilloscopeViewModel.Oscillograms.Add(oscillogram);


                await OscillogramHelper.SaveOscillogram(oscillogram, GetOscillogramDirectoryPath(deviceContext),
                    GetOscillogramSignature(oscilloscopeModel.OscillogramLoadingParameters, deviceContext.DeviceName),
                    oscilloscopeModel.CountingTemplate, oscilloscopeModel.OscillogramLoadingParameters, deviceContext.DeviceName, deviceContext);

                oscillogram.OscillogramPath = Path.Combine(GetOscillogramDirectoryPath(deviceContext),
                    GetOscillogramSignature(oscilloscopeModel.OscillogramLoadingParameters, deviceContext.DeviceName));
            }
        }

        private async Task ReadOscilligramToEndRecursive(ushort currentPageIndex, Oscillogram oscillogram,
            ushort endPageIndex, IProgress<ITaskProgressReport> progress, ITaskProgressReport reportProgress,
            CancellationToken cancellationToken, DeviceContext deviceContext, IOscilloscopeModel oscilloscopeModel)
        {
            cancellationToken.ThrowIfCancellationRequested();


            var res = await deviceContext.DataProviderContainer.DataProvider.OnSuccessAsync(async provider =>
            {
                IQueryResult queryResult = await RetryWrapperMethod(()
                    => provider.WriteSingleRegisterAsync(
                        oscilloscopeModel.OscillogramLoadingParameters.AddressOfOscillogram,
                        currentPageIndex, "Write"), 3);
                return Result<IQueryResult>.Create(queryResult, true);
            });




            await this.LoadOscPage(oscillogram, (currentPageIndex + 1) == endPageIndex, oscilloscopeModel,
                deviceContext);
            reportProgress.CurrentProgressAmount++;
            progress.Report(reportProgress);
            currentPageIndex++;
            if (currentPageIndex >= endPageIndex) return;
            //Если вылазим за пределы размера осцилографа - начинаем читать с нулевой страницы
            if (currentPageIndex < oscilloscopeModel.OscillogramLoadingParameters.MaxSizeOfRewritableOscillogramInMs *
                oscilloscopeModel.OscillogramLoadingParameters.GetSizeOfCountingInWords() / PAGE_SIZE_IN_WORD)
            {
                await this.ReadOscilligramToEndRecursive(currentPageIndex, oscillogram, endPageIndex, progress,
                    reportProgress, cancellationToken, deviceContext, oscilloscopeModel);
            }
            else
            {
                await this.ReadOscilligramToEndRecursive(
                    (ushort) (currentPageIndex -
                              oscilloscopeModel.OscillogramLoadingParameters.MaxSizeOfRewritableOscillogramInMs *
                              oscilloscopeModel.OscillogramLoadingParameters.GetSizeOfCountingInWords() /
                              PAGE_SIZE_IN_WORD), oscillogram,
                    endPageIndex, progress, reportProgress, cancellationToken, deviceContext, oscilloscopeModel);
            }
        }

        private async Task<IQueryResult> RetryWrapperMethod(Func<Task<IQueryResult>> func, int trials)
        {
            IQueryResult queryResult = await func();
            if ((!queryResult.IsSuccessful) && (trials != 0))
            {
                return await this.RetryWrapperMethod(func, trials - 1);
            }
            else if ((!queryResult.IsSuccessful) && (trials == 0))
            {
                throw new Exception();
            }

            return queryResult;
        }


        private async Task LoadOscPage(Oscillogram oscillogram, bool isLastPage, IOscilloscopeModel oscilloscopeModel, DeviceContext deviceContext)
        {

            ushort address = oscilloscopeModel.OscillogramLoadingParameters.AddressOfOscillogram;
            List<ushort> pageUshorts = new List<ushort>();
            while (address <= PAGE_SIZE_IN_WORD + oscilloscopeModel.OscillogramLoadingParameters.AddressOfOscillogram)
            {
                ushort numberOfPointsToRead = MAX_QUERY_SIZE;
                if ((PAGE_SIZE_IN_WORD + oscilloscopeModel.OscillogramLoadingParameters.AddressOfOscillogram - address) <
                    MAX_QUERY_SIZE)
                {
                    numberOfPointsToRead = (ushort) (PAGE_SIZE_IN_WORD +
                        oscilloscopeModel.OscillogramLoadingParameters.AddressOfOscillogram - address);
                }


                var res = await deviceContext.DataProviderContainer.DataProvider.OnSuccessAsync(async provider =>
                {
                    IQueryResult<ushort[]> queryResult =
                        await RetryWrapperMethod(async () =>
                        {
                            return await provider.ReadHoldingResgistersAsync(address, numberOfPointsToRead,
                                    "Read")
                                as IQueryResult;
                        }, 2) as IQueryResult<ushort[]>;
                    return Result<IQueryResult<ushort[]>>.Create(queryResult, true);
                });

            


                if (res.IsSuccess && res.Item.IsSuccessful)
                {
                    pageUshorts.AddRange(res.Item.Result);
                }

                if ((PAGE_SIZE_IN_WORD + oscilloscopeModel.OscillogramLoadingParameters.AddressOfOscillogram - address) <
                    MAX_QUERY_SIZE)
                {
                    break;
                }
                else
                {
                    address += MAX_QUERY_SIZE;
                }

            }

            if (isLastPage)
            {

                int pointToTake = PAGE_SIZE_IN_WORD -
                                  (oscilloscopeModel.OscillogramLoadingParameters.MaxSizeOfRewritableOscillogramInMs *
                                   oscilloscopeModel.OscillogramLoadingParameters.GetSizeOfCountingInWords()) % PAGE_SIZE_IN_WORD;
                oscillogram.Pages.Add(pageUshorts.Take(pointToTake).ToArray());

            }
            else
            {
                oscillogram.Pages.Add(pageUshorts.ToArray());
            }
        }


        private string GetOscillogramDirectoryPath(DeviceContext deviceContext)
        {
            string oscDir = Path.Combine(this._uniconProjectService.CurrentProjectPath, deviceContext.DeviceName, "Oscillograms");
            if (!Directory.Exists(oscDir))
            {
                Directory.CreateDirectory(oscDir);
            }
            return oscDir;
        }
        private string GetOscillogramSignature(IOscillogramLoadingParameters oscillogramLoadingParameters, string deviceName)
        {
            string oscSignature = $"{deviceName} {oscillogramLoadingParameters.GetDateTime()} {oscillogramLoadingParameters.GetAlarm()} {oscillogramLoadingParameters.GetOscillogramCountingsNumber()}";
            List<char> invalidChars = Path.GetInvalidFileNameChars().ToList();
            invalidChars.Add('.');
            invalidChars.ForEach((invalidChar => oscSignature = oscSignature.Replace(invalidChar, '_')));
            return oscSignature;
        }

        public bool TryGetOscillogram(int index, out string oscillogramPath, IOscilloscopeViewModel oscilloscopeViewModel, IOscilloscopeModel oscilloscopeModel, DeviceContext deviceContext)
        {
            if ((oscilloscopeViewModel.Oscillograms != null) && oscilloscopeViewModel.Oscillograms.Any((oscillogram => oscillogram.OscillogramNumber == index)))
            {
                oscillogramPath = oscilloscopeViewModel.Oscillograms.First((oscillogram => oscillogram.OscillogramNumber == index)).OscillogramPath;
                return true;
            }
            IJournalRecord oscilligramRecord = oscilloscopeModel.OscilloscopeJournal.JournalRecords[index];
            oscilloscopeModel.OscillogramLoadingParameters.Initialize(oscilligramRecord.FormattedValues, oscilloscopeModel.OscilloscopeJournal.RecordTemplate);
            string oscHdrFile = this.GetOscillogramSignature(oscilloscopeModel.OscillogramLoadingParameters, deviceContext.DeviceName) + ".hdr";
            string[] oscFiles = Directory.GetFiles(GetOscillogramDirectoryPath(deviceContext));
            if (oscFiles.Any((s => s.Contains(oscHdrFile))))
            {
                oscillogramPath = Path.Combine(GetOscillogramDirectoryPath(deviceContext), oscHdrFile);
                return true;
            }
            oscillogramPath = string.Empty;
            return false;
        }


    }
}