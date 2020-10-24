using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;

namespace Unicon2.Infrastructure.Extensions
{
    public static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var cur in enumerable)
            {
                action(cur);
            }
        }

        public static void WriteObject(this DataContractSerializer serializer, XmlWriter stream, object data,
            Dictionary<string, string> namespaces)
        {
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.NewLineOnAttributes = true;
            xmlWriterSettings.OmitXmlDeclaration = true;
            xmlWriterSettings.NamespaceHandling = NamespaceHandling.Default;

            using (var writer = XmlWriter.Create(stream, xmlWriterSettings))
            {

                serializer.WriteStartObject(writer, data);
                foreach (var pair in namespaces)
                {
                    writer.WriteAttributeString("xmlns", pair.Key, null, pair.Value);

                }

                serializer.WriteObjectContent(writer, data);
                serializer.WriteEndObject(writer);
            }
        }

        public static bool CheckEquality(this ushort[] ushorts1, ushort[] ushorts2)
        {
            if ((ushorts1 == null) || (ushorts2 == null)) return false;
            if (ushorts1.Length != ushorts2.Length) return false;
            for (int i = 0; i < ushorts1.Length; i++)
            {
                if (ushorts1[i] != ushorts2[i]) return false;
            }

            return true;
        }

        public static int GetIntFromBitArray(this BitArray bitArray)
        {
            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");

            int[] array = new int[1];
            bitArray.CopyTo(array, 0);
            return array[0];

        }

        public static ushort BoolArrayToUshort(this List<bool> arr)
        {
	        return (ushort)GetIntFromBitArray(new BitArray(arr.ToArray()));
        }
		public static ushort BoolArrayToUshort(this bool[] arr)
        {
            return (ushort) GetIntFromBitArray(new BitArray(arr));
        }

        public static int GetIntFromTwoUshorts(this ushort[] ushorts)
        {
            string d1 = ushorts[1].ToString("X2");
            string d2 = ushorts[0].ToString("X2");
            if (d2.Length < 4)
                do
                {
                    d2 = "0" + d2;
                } while (d2.Length < 4);

            return int.Parse(d1 + d2, System.Globalization.NumberStyles.HexNumber);
        }

        public static bool[] GetBoolArrayFromUshortArray(this ushort[] ushorts)
        {
            bool[] arrayToReaturn = new bool[ushorts.Length * 16];
            for (int ushortIndex = 0; ushortIndex < ushorts.Length; ushortIndex++)
            {
                BitArray bitArray = new BitArray(new int[] {(int) ushorts[ushortIndex]});
                for (int bitIndex = 0; bitIndex < 16; bitIndex++)
                {
                    arrayToReaturn[ushortIndex * 16 + bitIndex] = bitArray[bitIndex];
                }
            }

            return arrayToReaturn;
        }

        public static bool[] GetBoolArrayFromUshort(this ushort ushortToProcess)
        {
	        bool[] arrayToReaturn = new bool[16];

	        BitArray bitArray = new BitArray(new int[] {(int) ushortToProcess});
	        for (int bitIndex = 0; bitIndex < 16; bitIndex++)
	        {
		        arrayToReaturn[bitIndex] = bitArray[bitIndex];
	        }
	        return arrayToReaturn;
        }

        /// <summary>
		/// Конвертирует 2 байта в слово
		/// </summary>
		/// <param name="highByte">Ст.байт</param>
		/// <param name="lowByte">Мл.байт</param>
		/// <returns>Слово.</returns>
		public static ushort TwoBytesToUshort(byte highByte, byte lowByte)
        {
            ushort ret = highByte;
            return (ushort) ((ushort) (ret << 8) + (ushort) lowByte);
        }

        public static ushort[] ByteArrayToUshortArray(this byte[] byteArray)
        {
            if (byteArray.Length % 2 != 0)
            {
                byte[] buffer = byteArray;
                byteArray = new byte[byteArray.Length + 1];
                Array.ConstrainedCopy(buffer, 0, byteArray, 0, buffer.Length);
            }

            ushort[] ushorts = new ushort[byteArray.Length / 2];
            int ind = 0;
            for (int i = 0; i < ushorts.Length; i++)
            {
                ushorts[i] = TwoBytesToUshort(byteArray[ind + 1], byteArray[ind]);
                ind += sizeof(ushort);
            }

            return ushorts;
        }

        /// <summary>
        /// Конвертирует массив слов в массив байт
        /// </summary>
        /// <param name="words"> Массив слов.</param>
        /// <param name="bDirect">Порядок байт. true - обычный, false - ст.байт меняем местом с мл.байтом.</param>
        /// <returns>Массив байт.</returns>
        public static byte[] UshortArrayToByteArray(this ushort[] words, bool bDirect)
        {
            byte[] buffer = new byte[words.Length * 2];
            for (int i = 0, j = 0; i < words.Length; i++)
            {
                if (bDirect)
                {
                    buffer[j++] = HIBYTE(words[i]);
                    buffer[j++] = LOBYTE(words[i]);

                }
                else
                {
                    buffer[j++] = LOBYTE(words[i]);
                    buffer[j++] = HIBYTE(words[i]);
                }
            }

            return buffer;
        }

        /// <summary>
        /// Возвращает младший байт слова
        /// </summary>
        /// <param name="v">Слово.</param>
        /// <returns>Мл.байт</returns>
        public static byte LOBYTE(int v)
        {
            return (byte) (v & 0xff);
        }

        /// <summary>
        /// Возвращает старший байт слова.
        /// </summary>
        /// <param name="v">Слово.</param>
        /// <returns>Ст.байт</returns>
        public static byte HIBYTE(int v)
        {
            return (byte) (v >> 8);
        }

        /// <summary>
        /// Конвертирует слово в массив байт
        /// </summary>
        /// <param name="word">слово</param>
        /// <returns>массив байт</returns>
        public static byte[] UshortToBytes(this ushort word, bool bDirect = true)
        {
            byte[] buffer = new byte[2];
            if (bDirect)
            {
                buffer[0] = HIBYTE(word);
                buffer[1] = LOBYTE(word);
            }
            else
            {
                buffer[0] = LOBYTE(word);
                buffer[1] = HIBYTE(word);
            }

            return buffer;
        }

      public static ushort[] AsCollection(this ushort word)
        {
            return new ushort[]{word};
        }


        /// <summary>
        /// Конвертирует 2 байта в слово
        /// </summary>
        /// <param name="high">Ст.байт</param>
        /// <param name="low">Мл.байт</param>
        /// <returns>Слово.</returns>
        public static ushort ToUshort(byte high, byte low)
        {
            ushort ret = high;
            return (ushort) ((ushort) (ret << 8) + (ushort) low);
        }

        public static bool IsEqual(this ushort[] first, ushort[] second)
        {
            if (first.Length != second.Length)
	        {
		        return false;
	        }

            for (int i = 0; i < first.Length; i++)
	        {
		        if (first[i] != second[i])
		        {
			        return false;
		        }
	        }

	        return true;
		}
    }
}
