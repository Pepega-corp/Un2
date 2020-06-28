using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Fragments.FileOperations.FileOperations
{
    public class FileDriver : IFileDriver
    {
        private readonly ICommandSender _commandSender;
        private readonly ICommandStateReader _commandStateReader;
        private readonly IFileDataReader _fileDataReader;
        private readonly IFileDataWriter _fileDataWriter;
        private Dictionary<int, string> _messagesDictionary;

        private DeviceContext _deviceContext;


        public FileDriver(ICommandSender commandSender, ICommandStateReader commandStateReader, 
            IFileDataReader fileDataReader, IFileDataWriter fileDataWriter)
        {
            this._commandSender = commandSender;
            this._commandStateReader = commandStateReader;
            this._fileDataReader = fileDataReader;
            this._fileDataWriter = fileDataWriter;
            this.InitErrorMessages();
        }

        private void InitErrorMessages()
        {
            this._messagesDictionary = new Dictionary<int, string>
            {
                {0,"Операция успешно выполнена"},
                {1,"Обращение к зарезервированной памяти"},
                {2,"Подана неверная команда"},
                {5,"Слишком много открытых файлов"},
                {6,"Файл еще не открыт"},
                {7,"Неверный пароль"},
                {8,"Ошибка дескриптора файла"},
                {9,"Ошибка CRC"},
                {101,"Произошла невосстановимая ошибка на низком уровне"},
                {102,"Ошибка структуры FAT на томе или рабочая область испорчена"},
                {103,"Диск не готов"},
                {104,"Файл не найден"},
                {105,"Не найден путь"},
                {106,"Указанная строка содержит недопустимое имя"},
                {107,"Отканано в доступе"},
                {108,"Файл или папка с таким именем уже существуют"},
                {109,"Предоставленная структура объекта\nфайла/директории ошибочна"},
                {110,"Дествие произведено на защищенном\nот записи носителе данных"},
                {111,"Указан недопустимый номер диска"},
                {112,"Рабочая область логического диска\nне зарегистрирована"},
                {113,"На диске нет рабочего тома с файловой системой FAT"},
                {114,"Функция остановлена перед началом форматирования"},
                {115,"Функция остановлена из-за таймаута\nв безопасном управлении потоками"},
                {116,"Доступ к файлу отклонен управлением\nсовместного доступа к файлу.\nПерезагрузите, пожалуйста, устройство"},
                {117,"Недостаточно памяти для выполнения операции"},
                {118,"Количество открытых файлов достигло\nмаксимального количества"},
                {119,"Указанный параметр недопустим"},
                {224,"Операция не была выполнена"},
                {255,"Нет связи с устройством, невозможно выполнить операцию"}
            };
        }

        private async Task<string> GetPassword()
        {

            await this._commandSender.SetCommand("GETNUMBER");
            string[] stateStrings = await this._commandStateReader.ReadCommandStateStrings();

            byte[] jj = await this._fileDataReader.GetDataBytes(Convert.ToUInt16(stateStrings[5]));
            string s = this.GetDataString(jj);
            int sessionNum = Convert.ToUInt16(s);

            string pass = "АААА"; //русские буквы А
            Encoding ascii = Encoding.GetEncoding("windows-1251");
            byte[] asciiBytes = ascii.GetBytes(pass);
            string passCode = string.Empty;
            foreach (byte c in asciiBytes)
            {
                sessionNum = (byte)((sessionNum * 99 + 53) % 256);
                passCode += ((byte)(c + sessionNum)).ToString("X2");
            }
            return passCode;
        }


        private string GetDataString(byte[] readBytes)
        {
            List<char> list = new List<char>();
            for (int i = 0; i < readBytes.Length; i++)
            {
                if (readBytes[i] != '\0')
                {
                    list.Add((char)readBytes[i]);
                }
            }
            return new string(list.ToArray());
        }

        public DeviceContext DeviceContext
        {
            get => _deviceContext;
            set
            {
                _deviceContext = value;
                this._commandSender.DeviceContext = _deviceContext;
                this._commandStateReader.DeviceContext = _deviceContext;
                this._fileDataReader.DeviceContext = _deviceContext;
                this._fileDataWriter.DeviceContext = _deviceContext;
            }
        }

        public async Task<List<string>> GetDirectoryByPath(string directoryPath)
        {
            if (directoryPath.Contains(";"))
            {
                directoryPath = directoryPath.Split(';')[0];
            }
            List<string> directoryList = new List<string>();

            await this._commandSender.SetCommand("GETELEMENTDIR " + directoryPath);
            string[] commandStateStrings = await this._commandStateReader.ReadCommandStateStrings();
            if (this._commandStateReader.LastCommandStatus != 0) return directoryList;
            int dataLen = Convert.ToUInt16(commandStateStrings[5]);
            byte[] dataBytes = await this._fileDataReader.GetDataBytes(dataLen);
            string descriptor = this.GetDataString(dataBytes);
            if (this.CheckDescroptor(descriptor))
            {
                directoryList.Add(directoryPath + "\\" +
                                  descriptor);
            }

            while (true)
            {
                await this._commandSender.SetCommand("GETELEMENTDIR");
                commandStateStrings = await this._commandStateReader.ReadCommandStateStrings();
                dataLen = Convert.ToUInt16(commandStateStrings[5]);
                dataBytes = await this._fileDataReader.GetDataBytes(dataLen);
                descriptor = this.GetDataString(dataBytes);
                if (descriptor.IndexOf(';') == 0) break;
                if (this.CheckDescroptor(descriptor))
                {
                    directoryList.Add(directoryPath + "\\" + descriptor);
                }
            }
            //await WriteFile(new byte[] {4, 2}, "0:\\cfg\\", "228.cfg");
            return directoryList;
        }

        private bool CheckDescroptor(string descroptor)
        {
            if ((descroptor.Split(';')[0] == ".") || (descroptor.Split(';')[0] == ".."))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> CreateDirectory(string directoryPath)
        {
            await this._commandSender.SetCommand("CREATEDIR " + directoryPath);
            await this._commandStateReader.ReadCommandStateStrings();
            return this._commandStateReader.LastCommandStatus == 0;
        }

        public async Task<bool> DeleteElement(string path)
        {
            string passWord = await this.GetPassword();
            await this._commandSender.SetCommand("FILEDEL  " + passWord + ";" + path);
            await this._commandStateReader.ReadCommandStateStrings();
            return this._commandStateReader.LastCommandStatus == 0;
        }

        public async Task<string> WriteFile(byte[] fileData, string directoryPath, string fileName)
        {
            await this._commandSender.SetCommand("FILEOPEN " + await this.GetPassword() + ";" + 10 + ";" + directoryPath + "\\" + fileName);

            string[] stateStrings = await this._commandStateReader.ReadCommandStateStrings();

            int error = Convert.ToUInt16(stateStrings[1]);
            if (error != 0)
            {
                return this._messagesDictionary[error];
            }
            int dataLen = Convert.ToUInt16(stateStrings[5]);
            byte[] dataBytes = await this._fileDataReader.GetDataBytes(dataLen);
            string descriptor = this.GetDataString(dataBytes);
            try
            {
                await this._fileDataWriter.WriteData(fileData, descriptor);
            }
            catch (Exception)
            {
                return this._messagesDictionary[224];
            }
            finally
            {
                await this._commandSender.SetCommand("FILECLOSE " + Convert.ToUInt16(descriptor));
                stateStrings = await this._commandStateReader.ReadCommandStateStrings();
                error = Convert.ToUInt16(stateStrings[1]);
            }

            if (error != 0)
            {
                return this._messagesDictionary[error];
            }

            return this._messagesDictionary[error];

        }

        public Task<byte[]> ReadFile(string directoryPath, string fileName) => throw new NotImplementedException();
    }
}
