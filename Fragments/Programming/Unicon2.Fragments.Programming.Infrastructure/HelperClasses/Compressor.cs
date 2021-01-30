using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Unicon2.Fragments.Programming.Infrastructure.HelperClasses
{
    public class Compressor
    {
        /// <summary>
        /// Архивирует массив байт
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] Compress(byte[] bytes)
        {
            var ms = new MemoryStream();
            var zip = new GZipStream(ms, CompressionMode.Compress, true);
            zip.Write(bytes, 0, bytes.Length);
            zip.Close();
            var result = new byte[ms.Length];
            ms.Seek(0, SeekOrigin.Begin);
            ms.Read(result, 0, (int)ms.Length);
            ms.Close();
            return result;
        }

        /// <summary>
        /// Разархивирует массив байт
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public byte[] Decompress(byte[] bytes)
        {
            var ms = new MemoryStream(bytes);
            Stream zip = new GZipStream(ms, CompressionMode.Decompress);
            var buffer = new List<byte>();
            var temp = -1;
            while ((temp = zip.ReadByte()) != -1)
            {
                buffer.Add((byte)temp);
            }
            ms.Close();
            zip.Close();
            return buffer.ToArray();
        }
    }
}
