using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Interfaces.Visitors;

namespace Unicon2.Formatting.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DictionaryMatchingFormatter : UshortsFormatterBase, IDictionaryMatchingFormatter
    {

        public DictionaryMatchingFormatter()
        {
            StringDictionary = new Dictionary<ushort, string>();
        }

        public override object Clone()
        {
            DictionaryMatchingFormatter cloneDictionaryMatchingFormatter = new DictionaryMatchingFormatter();
            cloneDictionaryMatchingFormatter.StringDictionary = new Dictionary<ushort, string>(StringDictionary);
            cloneDictionaryMatchingFormatter.IsKeysAreNumbersOfBits = IsKeysAreNumbersOfBits;
            return cloneDictionaryMatchingFormatter;
        }

        public override T Accept<T>(IFormatterVisitor<T> visitor)
        {
            return visitor.VisitDictionaryMatchFormatter(this);
        }

        [JsonProperty] public Dictionary<ushort, string> StringDictionary { get; set; }
        [JsonProperty] public bool IsKeysAreNumbersOfBits { get; set; }
        [JsonProperty] public bool UseDefaultMessage { get; set; }
        [JsonProperty] public string DefaultMessage { get; set; }
    }
}