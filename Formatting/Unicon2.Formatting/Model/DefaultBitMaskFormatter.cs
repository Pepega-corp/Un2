using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Interfaces.Visitors;

namespace Unicon2.Formatting.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DefaultBitMaskFormatter : UshortsFormatterBase, IBitMaskFormatter
    {
        
        public override object Clone()
        {
            DefaultBitMaskFormatter clone = new DefaultBitMaskFormatter();
            clone.BitSignatures = new List<string>(BitSignatures);
            return clone;
        }

        public override T Accept<T>(IFormatterVisitor<T> visitor)
        {
            return visitor.VisitBitMaskFormatter(this);
        }

        [JsonProperty]
        public string BitSignaturesInOneLine { get; set; }

        public List<string> BitSignatures { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext sc)
        {
            if (BitSignaturesInOneLine == null) return;
            string[] bitSignatures = BitSignaturesInOneLine.Split(',');
            BitSignatures = new List<string>();
            foreach (string bitSignature in bitSignatures)
            {
                string preparedString = bitSignature.Replace("&lt", "<").Replace("&gt", ">");
                if (preparedString.Contains("\""))
                {
                    preparedString = preparedString.Remove(preparedString.LastIndexOf("\""));
                    preparedString = preparedString.Remove(0, preparedString.IndexOf("\""));


                    BitSignatures.Add(preparedString.Replace("\"", ""));
                }
            }
        }


        [OnSerializing]
        private void OnSerializing(StreamingContext sc)
        {

            StringBuilder sb = new StringBuilder();
            BitSignatures.ForEach((s =>
            {
                sb.Append("\"");
                string preparedString = s.Replace("<", "&lt").Replace(">", "&gt");
                sb.Append(preparedString);
                sb.Append("\"");
                if (BitSignatures.IndexOf(s) != BitSignatures.Count - 1)
                {
                    sb.Append(",");
                }
            }));
            BitSignaturesInOneLine = sb.ToString();
        }

    }





}
