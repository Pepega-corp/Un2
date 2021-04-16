using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unicon2.Tests.Helpers.Query;

namespace Unicon2.Tests.Utils
{
    public class QueryUtils
    {
        public static QueryMockDefinition CreateQueryMockDefinitionFromString(string line)
        {
            var regex = @"\[(.*?)\]";
            var matched = Regex.Match(line, regex).Groups[1].Value;
            ushort func = 0;
            ushort address = 0;
            ushort? numberofPoints = null;
            var data = new List<ushort>();
            if (matched.Contains("Fun"))
            {
                var index = matched.IndexOf("Fun:");
                func = ushort.Parse(matched[index + "Fun:".Length].ToString());
            }

            if (matched.Contains("Addr:"))
            {
                var regexAddress = @"Addr:(.*?) ";
                address = ushort.Parse(Regex.Match(matched, regexAddress).Groups[1].Value);
            }

            if (matched.Contains("Num:"))
            {
                var regexNum = @"Num:(.*?) ";
                var x = Regex.Match(line, regexNum).Groups[1].Value.Replace("]", "");
                numberofPoints = ushort.Parse(x);
            }

            if (matched.Contains("Data:"))
            {
                var st = matched.Substring(matched.IndexOf("Data:") + "Data:".Length);
                var strData = st.Split(' ');
                data = strData.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => ushort.Parse(s)).ToList();
            }

            return new QueryMockDefinition(address, func, numberofPoints, data);
        }

        public static async Task<List<QueryMockDefinition>> ReadQueryMockDefinitionFromFile(string file)
        {
            
            List<QueryMockDefinition> queryMockDefinitions = new List<QueryMockDefinition>();

            const Int32 BufferSize = 128;
            using (var fileStream = File.OpenRead(file))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize))
            {
                String line;
                while ((line = await streamReader.ReadLineAsync()) != null)
                {
                    queryMockDefinitions.Add(QueryUtils.CreateQueryMockDefinitionFromString(line));
                }
            }

            return queryMockDefinitions;
        }
    }
}