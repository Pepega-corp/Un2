using System;
using System.Text;
using System.Threading.Tasks;

namespace Unicon2.Fragments.FileOperations.FileOperations.Operators
{
    public class SessionNumberOperator : Operator
    {
        private const string GETNUM_CMD = "GETNUMBER";
        public ushort SessionNumber { get; private set; }

        public async Task ReadSessionNumber()
        {
            var strNum = await ReadDataString(GETNUM_CMD);
            this.SessionNumber = Convert.ToUInt16(strNum);
        }

        public string GetPassword()
        {
            var pass = "АААА"; //русские буквы А
            var ascii = Encoding.GetEncoding("windows-1251");
            var asciiBytes = ascii.GetBytes(pass);
            var passCode = string.Empty;
            foreach (var c in asciiBytes)
            {
                this.SessionNumber = (byte)((this.SessionNumber * 99 + 53) % 256);
                passCode += ((byte)(c + this.SessionNumber)).ToString("X2");
            }
            return passCode;
        }
    }
}