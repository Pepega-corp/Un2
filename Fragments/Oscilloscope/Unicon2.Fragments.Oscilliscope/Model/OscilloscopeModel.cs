using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.CountingTemplate;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscillogramLoadingParameters;
using Unicon2.Fragments.Oscilliscope.Model.Helpers;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Progress;
using Unicon2.Infrastructure.Services.UniconProject;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Oscilliscope.Model
{
    [DataContract(Namespace = "OscilloscopeModelNS")]
    public class OscilloscopeModel : IOscilloscopeModel, IDataProviderContaining, IInitializableFromContainer
    {
        private bool _isInitialized;
        private List<Oscillogram> _oscillograms;
        private IDataProvider _dataProvider;
        private Func<ITaskProgressReport> _taskProgressReportGettingFunc;
        private string _parentDeviceName;
        private const int PAGE_SIZE_IN_WORD = 1024;
        private const int MAX_QUERY_SIZE = 125;
        private IUniconProjectService _uniconProjectService;

        public OscilloscopeModel(IUniconJournal uniconJournal, Func<ITaskProgressReport> taskProgressReportGettingFunc,
            ICountingTemplate countingTemplate)
        {
            this._taskProgressReportGettingFunc = taskProgressReportGettingFunc;
            this.OscilloscopeJournal = uniconJournal;
            this.CountingTemplate = countingTemplate;

        }


        [DataMember]
        public IUniconJournal OscilloscopeJournal { get; set; }

        [DataMember]
        public IOscillogramLoadingParameters OscillogramLoadingParameters { get; set; }

        [DataMember]
        public ICountingTemplate CountingTemplate { get; set; }

        public async Task LoadOscillogramsByNumber(List<int> numberOfOscillograms, IProgress<ITaskProgressReport> progress, CancellationToken cancellationToken)
        {
            ITaskProgressReport progressReport = this._taskProgressReportGettingFunc();
            int numberOfPages = 0;
            if (this._oscillograms == null) this._oscillograms = new List<Oscillogram>();

            foreach (int numberOfOscillogram in numberOfOscillograms)
            {
                if (this._oscillograms.Any((oscillogram1 => oscillogram1.OscillogramNumber == numberOfOscillogram)))
                    continue;
                IJournalRecord oscilligramRecord = this.OscilloscopeJournal.JournalRecords[numberOfOscillogram - 1];
                this.OscillogramLoadingParameters.Initialize(oscilligramRecord.FormattedValues, this.OscilloscopeJournal.RecordTemplate);
                int numberOfPoints = this.OscillogramLoadingParameters.GetOscillogramCountingsNumber() * this.OscillogramLoadingParameters.GetSizeOfCountingInWords();
                numberOfPages += (int)Math.Ceiling((double)numberOfPoints / PAGE_SIZE_IN_WORD);
            }
            progressReport.TotalProgressAmount = numberOfPages;
            progress.Report(progressReport);

            foreach (int numberOfOscillogram in numberOfOscillograms)
            {
                IJournalRecord oscilligramRecord = this.OscilloscopeJournal.JournalRecords[numberOfOscillogram - 1];
                this.OscillogramLoadingParameters.Initialize(oscilligramRecord.FormattedValues, this.OscilloscopeJournal.RecordTemplate);
                double oscLengthInCountings = this.OscillogramLoadingParameters.GetOscillogramCountingsNumber();
                double sizeOfCounting = this.OscillogramLoadingParameters.GetSizeOfCountingInWords();
                double oscLengthInWords = oscLengthInCountings * sizeOfCounting;
                double pointOfOscStart = this.OscillogramLoadingParameters.GetPointOfStart();
                double pageCount = (int)Math.Ceiling(oscLengthInWords / PAGE_SIZE_IN_WORD);
                ushort oscStartPageIndex = (ushort)(pointOfOscStart / PAGE_SIZE_IN_WORD);
                double endPage = oscStartPageIndex + pageCount;
                Oscillogram oscillogram = new Oscillogram();
                try
                {
                    await this.ReadOscilligramToEndRecursive(oscStartPageIndex, oscillogram, (ushort)endPage, progress,
                        progressReport, cancellationToken);
                }
                catch
                {
                    progressReport.CurrentProgressAmount = 0;
                    progressReport.TotalProgressAmount = 1;
                    progress.Report(progressReport);
                    return;
                }

                OscillogramHelper.InvertOscillogram(oscillogram, this.OscillogramLoadingParameters, PAGE_SIZE_IN_WORD);
                oscillogram.OscillogramNumber = numberOfOscillogram - 1;
                if (this._oscillograms.Any((oscillogram1 =>
                    oscillogram1.OscillogramNumber == oscillogram.OscillogramNumber)))
                {
                    this._oscillograms.Remove(this._oscillograms.First((oscillogram1 =>
                        oscillogram1.OscillogramNumber == oscillogram.OscillogramNumber)));
                }
                this._oscillograms.Add(oscillogram);


                await OscillogramHelper.SaveOscillogram(oscillogram, this.GetOscillogramDirectoryPath(), this.GetOscillogramSignature(this.OscillogramLoadingParameters, this._parentDeviceName), this.CountingTemplate, this.OscillogramLoadingParameters, this._parentDeviceName);

                oscillogram.OscillogramPath = Path.Combine(this.GetOscillogramDirectoryPath(), this.GetOscillogramSignature(this.OscillogramLoadingParameters, this._parentDeviceName));
            }
        }


        private string GetOscillogramDirectoryPath()
        {
            string oscDir = Path.Combine(this._uniconProjectService.GetProjectPath(), this._parentDeviceName, "Oscillograms");
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



        public bool TryGetOscillogram(int index, out string oscillogramPath)
        {
            if ((this._oscillograms != null) && this._oscillograms.Any((oscillogram => oscillogram.OscillogramNumber == index)))
            {
                oscillogramPath = this._oscillograms.First((oscillogram => oscillogram.OscillogramNumber == index)).OscillogramPath;
                return true;
            }
            IJournalRecord oscilligramRecord = this.OscilloscopeJournal.JournalRecords[index];
            this.OscillogramLoadingParameters.Initialize(oscilligramRecord.FormattedValues, this.OscilloscopeJournal.RecordTemplate);
            string oscHdrFile = this.GetOscillogramSignature(this.OscillogramLoadingParameters, this._parentDeviceName) + ".hdr";
            string[] oscFiles = Directory.GetFiles(this.GetOscillogramDirectoryPath());
            if (oscFiles.Any((s => s.Contains(oscHdrFile))))
            {
                oscillogramPath = Path.Combine(this.GetOscillogramDirectoryPath(), oscHdrFile);
                return true;
            }
            oscillogramPath = string.Empty;
            return false;
        }


        private async Task ReadOscilligramToEndRecursive(ushort currentPageIndex, Oscillogram oscillogram,
            ushort endPageIndex, IProgress<ITaskProgressReport> progress, ITaskProgressReport reportProgress, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IQueryResult queryResult = await this.RetryWrapperMethod(()
                => this._dataProvider.WriteSingleRegisterAsync(this.OscillogramLoadingParameters.AddressOfOscillogram, currentPageIndex, "Write"), 2);


            await this.LoadOscPage(oscillogram, (currentPageIndex + 1) == endPageIndex);
            reportProgress.CurrentProgressAmount++;
            progress.Report(reportProgress);
            currentPageIndex++;
            if (currentPageIndex >= endPageIndex) return;
            //Если вылазим за пределы размера осцилографа - начинаем читать с нулевой страницы
            if (currentPageIndex < this.OscillogramLoadingParameters.MaxSizeOfRewritableOscillogramInMs * this.OscillogramLoadingParameters.GetSizeOfCountingInWords() / PAGE_SIZE_IN_WORD)
            {
                await this.ReadOscilligramToEndRecursive(currentPageIndex, oscillogram, endPageIndex, progress,
                    reportProgress, cancellationToken);
            }
            else
            {
                await this.ReadOscilligramToEndRecursive(
                    (ushort)(currentPageIndex - this.OscillogramLoadingParameters.MaxSizeOfRewritableOscillogramInMs * this.OscillogramLoadingParameters.GetSizeOfCountingInWords() / PAGE_SIZE_IN_WORD), oscillogram,
                    endPageIndex, progress, reportProgress, cancellationToken);
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


        private async Task LoadOscPage(Oscillogram oscillogram, bool isLastPage)
        {

            ushort address = this.OscillogramLoadingParameters.AddressOfOscillogram;
            List<ushort> pageUshorts = new List<ushort>();
            while (address <= PAGE_SIZE_IN_WORD + this.OscillogramLoadingParameters.AddressOfOscillogram)
            {
                ushort numberOfPointsToRead = MAX_QUERY_SIZE;
                if ((PAGE_SIZE_IN_WORD + this.OscillogramLoadingParameters.AddressOfOscillogram - address) < MAX_QUERY_SIZE)
                {
                    numberOfPointsToRead = (ushort)(PAGE_SIZE_IN_WORD + this.OscillogramLoadingParameters.AddressOfOscillogram - address);
                }
                IQueryResult<ushort[]> queryResult =
                   await this.RetryWrapperMethod(async () =>
                       {
                           return await this._dataProvider.ReadHoldingResgistersAsync(address, numberOfPointsToRead, "Read")
                               as IQueryResult;
                       }, 2) as IQueryResult<ushort[]>;


                if (queryResult.IsSuccessful)
                {
                    pageUshorts.AddRange(queryResult.Result);
                }
                if ((PAGE_SIZE_IN_WORD + this.OscillogramLoadingParameters.AddressOfOscillogram - address) < MAX_QUERY_SIZE)
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

                int pointToTake = PAGE_SIZE_IN_WORD - (this.OscillogramLoadingParameters.MaxSizeOfRewritableOscillogramInMs * this.OscillogramLoadingParameters.GetSizeOfCountingInWords()) % PAGE_SIZE_IN_WORD;
                oscillogram.Pages.Add(pageUshorts.Take(pointToTake).ToArray());

            }
            else
            {
                oscillogram.Pages.Add(pageUshorts.ToArray());
            }
        }


        public string StrongName => OscilloscopeKeys.OSCILLOSCOPE;

        [DataMember]
        public IFragmentSettings FragmentSettings { get; set; }

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
            this.CountingTemplate.SetDataProvider(dataProvider);
            this.OscilloscopeJournal?.SetDataProvider(dataProvider);
        }

        public bool IsInitialized
        {
            get { return this._isInitialized; }
        }

        public void InitializeFromContainer(ITypesContainer container)
        {
            this._taskProgressReportGettingFunc = container.Resolve<Func<ITaskProgressReport>>();
            this.OscilloscopeJournal.InitializeFromContainer(container);
            this.CountingTemplate.InitializeFromContainer(container);
            this._uniconProjectService = container.Resolve<IUniconProjectService>();
            this._isInitialized = true;
        }

        public void SetParentDeviceName(string parentDeviceName)
        {
            this._parentDeviceName = parentDeviceName;
        }
    }
}