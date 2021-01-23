using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Fragments.FileOperations.FileOperations.Operators
{
    public class FileReadDataOperator : Operator
    {
        private const string FILEREAD_CMD_PATTERN = "FILEREAD {0};{1}";
        /// <summary>
        /// Read data by query wordsDataLen
        /// </summary>
        /// <param name="descriptor">File descriptor</param>
        /// <param name="wordsDataLen">Query data len in words</param>
        /// <returns></returns>
        public async Task<byte[]> ReadFileData(int descriptor, ushort wordsDataLen)
        {
            var dataWords = await ReadData(string.Format(FILEREAD_CMD_PATTERN, descriptor, wordsDataLen * 2));

            return dataWords.UshortArrayToByteArray(false);
        }

        protected override async Task<ushort[]> ReadData(string command)
        {
            var data = new List<ushort>();
            var dataLen = ushort.MaxValue;
            while (dataLen > 0)
            {
                await SetCommand(command);
                var stateStrings = await this.ReadCommandStateStrings();
                if (stateStrings != null && stateStrings.Length >= 6)
                {
                    dataLen = Convert.ToUInt16(stateStrings[5]);

                    if (dataLen == 0)
                        return data.ToArray();

                    data.AddRange(await ReadDataCommand((ushort)(dataLen/2)));
                }
                else
                {
                    break;
                }
            }

            throw new FileOperationException(LastCommandStatus);
        }
    }
}
