using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Factories;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.CountingTemplate;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscillogramLoadingParameters;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services.Formatting;

namespace Unicon2.Fragments.Oscilliscope.Model.Helpers
{
    public static class OscillogramHelper
    {
        public static async Task SaveOscillogram(Oscillogram oscillogram, string directoryPath,
            string oscillogramSignature, ICountingTemplate countingTemplate, IOscillogramLoadingParameters oscillogramLoadingParameters, string parentDeviceName, DeviceContext deviceContext)
        {
            var journalRecordFactory = StaticContainer.Container.Resolve<IJournalRecordFactory>();

            List<int[]> countingArraysToFile = new List<int[]>();
            foreach (IJournalParameter journalParameter in countingTemplate.RecordTemplate.JournalParameters)
            {
                if (journalParameter.UshortsFormatter is ILoadable)
                {
                    await (journalParameter.UshortsFormatter as ILoadable).Load();
                }
            }
            for (int i = 0; i < oscillogramLoadingParameters.GetOscillogramCountingsNumber(); i++)
            {
                int[] countingArray = new int[countingTemplate.GetAllChannels()];

                ushort[] countingUshortsFromDevice = oscillogram.OscillogramResultUshorts
                    .Skip(oscillogramLoadingParameters.GetSizeOfCountingInWords() * i)
                    .Take(oscillogramLoadingParameters.GetSizeOfCountingInWords()).ToArray();

                int indexIfValueInCounting = 0;

                foreach (IJournalParameter journalParameter in countingTemplate.RecordTemplate.JournalParameters)
                {
                    List<IFormattedValue> formattedValues =
                        await journalRecordFactory.GetValuesForRecord(journalParameter, countingUshortsFromDevice,
                            deviceContext);
                      
                    foreach (IFormattedValue formattedValue in formattedValues)
                    {
                        if (formattedValue is INumericValue)
                        {
                            countingArray[indexIfValueInCounting++] =
                                (int)Math.Ceiling((formattedValue as INumericValue).NumValue * 1000);
                        }
                        if (formattedValue is IBitMaskValue)
                        {
                            foreach (bool bit in (formattedValue as IBitMaskValue).GetAllBits())
                            {
                                if (countingArray.Length == indexIfValueInCounting)
                                {
                                    break;
                                }
                                countingArray[indexIfValueInCounting++] = (short)(bit ? 1 : 0);
                            }
                        }
                    }
                }
                countingArraysToFile.Add(countingArray);
            }

            string hdrPath = Path.ChangeExtension(Path.Combine(directoryPath, oscillogramSignature), "hdr");
            using (StreamWriter hdrFile = new StreamWriter(hdrPath))
            {
                hdrFile.WriteLine(
                    $"{parentDeviceName} {oscillogramLoadingParameters.GetDateTime()}  ступень - {oscillogramLoadingParameters.GetAlarm()}");
                hdrFile.WriteLine("Size, ms = {0}", oscillogramLoadingParameters.GetOscillogramCountingsNumber());
                hdrFile.WriteLine(
                    $"Alarm = {oscillogramLoadingParameters.GetOscillogramCountingsNumber() - oscillogramLoadingParameters.GetSizeAfter()}");
            }
            //todo encoding oscillogram switched to utf-8 from 1251
            string cgfPath = Path.ChangeExtension(Path.Combine(directoryPath, oscillogramSignature), "cfg");
            using (StreamWriter cgfFile = new StreamWriter(cgfPath, false, Encoding.GetEncoding("UTF-8")))
            {
                cgfFile.WriteLine($"{parentDeviceName}");

                cgfFile.WriteLine(
                    $"{countingTemplate.GetAllChannels()},{countingTemplate.GetNumberOfAnalogs()}A,{countingTemplate.GetNumberOfDiscrets()}D");
                int index = 1;
                List<string> analogNames = countingTemplate.GetAnalogsNames();
                for (int i = 0; i < countingTemplate.GetNumberOfAnalogs(); i++)
                {
                    NumberFormatInfo format = new NumberFormatInfo { NumberDecimalSeparator = "." };

                    double factorA = 0.001;
                    cgfFile.WriteLine(
                        $"{index},{analogNames[i]},,,{countingTemplate.GetMeasureUnit(i)},{factorA.ToString(format)},0,0,{int.MinValue},{int.MaxValue}");
                    index++;
                }
                List<string> discretNames = countingTemplate.GetDiscretsNames();
                if (discretNames.Count < countingTemplate.GetNumberOfDiscrets())
                {
                    discretNames = new List<string>();
                    for (int i = 0; i < countingTemplate.GetNumberOfDiscrets(); i++)
                    {
                        discretNames.Add($"D {i + 1}");
                    }
                }
                for (int i = 0; i < countingTemplate.GetNumberOfDiscrets(); i++)
                {
                    cgfFile.WriteLine("{0},{1},0", i + 1, discretNames[i]);
                }
                cgfFile.WriteLine("50");
                cgfFile.WriteLine("1");
                cgfFile.WriteLine("1000,{0}", oscillogramLoadingParameters.GetOscillogramCountingsNumber());

                cgfFile.WriteLine(oscillogramLoadingParameters.GetDateTime());
                cgfFile.WriteLine(oscillogramLoadingParameters.GetDataTimeofAlarm());
                cgfFile.WriteLine("1251");
            }

            string datPath = Path.ChangeExtension(Path.Combine(directoryPath, oscillogramSignature), "dat");
            using (StreamWriter datFile = new StreamWriter(datPath))
            {
                for (int i = 0; i < oscillogramLoadingParameters.GetOscillogramCountingsNumber(); i++)
                {
                    datFile.Write("{0:D6},{1:D6}", i, i * 1000);

                    foreach (int value in countingArraysToFile[i])
                    {
                        datFile.Write(",{0}", value);
                    }
                    datFile.WriteLine();
                }
            }
        }


        public static void InvertOscillogram(Oscillogram oscillogram, IOscillogramLoadingParameters oscillogramLoadingParameters, int pageSizeInWord)
        {

            int oscLengthInWords = oscillogramLoadingParameters.GetOscillogramCountingsNumber() * oscillogramLoadingParameters.GetSizeOfCountingInWords();

            //Результирующий массив
            ushort[] resultMassiv = new ushort[oscLengthInWords];
            //Если осцилограмма начинается с начала страницы
            ushort[] startPageValueArray = oscillogram.Pages[0];
            int destanationIndex = 0;


            int startWordIndex = oscillogramLoadingParameters.GetPointOfStart() % pageSizeInWord;
            Array.ConstrainedCopy(startPageValueArray, startWordIndex, resultMassiv, destanationIndex,
                startPageValueArray.Length - startWordIndex);
            destanationIndex += startPageValueArray.Length - startWordIndex;
            for (int i = 1; i < oscillogram.Pages.Count - 1; i++)
            {
                ushort[] pageValue = oscillogram.Pages[i];
                Array.ConstrainedCopy(pageValue, 0, resultMassiv, destanationIndex, pageValue.Length);
                destanationIndex += pageValue.Length;
            }
            ushort[] endPage = oscillogram.Pages[oscillogram.Pages.Count - 1];
            Array.ConstrainedCopy(endPage, 0, resultMassiv, destanationIndex, oscLengthInWords - destanationIndex);
            int oscLengthInCountings = oscillogramLoadingParameters.GetOscillogramCountingsNumber();
            int sizeAfter = oscillogramLoadingParameters.GetSizeAfter();
            int sizeOfCounting = oscillogramLoadingParameters.GetSizeOfCountingInWords();
            int beginAddress = oscillogramLoadingParameters.GetBeginAddresInData();
            int pointOfStart = oscillogramLoadingParameters.GetPointOfStart();
            //----------------------------------ПЕРЕВОРОТ---------------------------------//
            int c;
            int b = (oscLengthInCountings - sizeAfter) * sizeOfCounting;
            //b = LEN – AFTER (рассчитывается в отсчётах, далее в словах, переведём в слова)
            if (beginAddress < pointOfStart) // Если BEGIN меньше POINT, то:
            {
                //c = BEGIN + OSCSIZE - POINT  
                c = beginAddress + oscillogramLoadingParameters.MaxSizeOfRewritableOscillogramInMs * sizeOfCounting -
                    pointOfStart;
            }
            else //Если BEGIN больше POINT, то:
            {
                c = beginAddress - pointOfStart; //c = BEGIN – POINT
            }
            int start = c - b; //START = c – b
            if (start < 0) //Если START меньше 0, то:
            {
                start += oscLengthInCountings * sizeOfCounting; //START = START + LEN•REZ
            }
            //-----------------------------------------------------------------------------//

            //Перевёрнутый массив
            ushort[] invertedMass = new ushort[oscLengthInWords];
            Array.ConstrainedCopy(resultMassiv, start, invertedMass, 0, resultMassiv.Length - start);
            Array.ConstrainedCopy(resultMassiv, 0, invertedMass, invertedMass.Length - start, start);
            oscillogram.OscillogramResultUshorts = invertedMass.ToList();
        }
    }
}
