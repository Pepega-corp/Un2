using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

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
    }
}