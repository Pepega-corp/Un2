using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(IsReference = true, Namespace = "DefaultBitMaskFormatterNS")]
    public class DefaultBitMaskFormatter : UshortsFormatterBase, IBitMaskFormatter
    {
        
        public override object Clone()
        {
            DefaultBitMaskFormatter clone = new DefaultBitMaskFormatter();
            clone.BitSignatures = new List<string>(this.BitSignatures);
            return clone;
        }

        public override T Accept<T>(IFormatterVisitor<T> visitor)
        {
            return visitor.VisitBitMaskFormatter(this);
        }

        [DataMember]
        public string BitSignaturesInOneLine { get; set; }

        public List<string> BitSignatures { get; set; }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext sc)
        {
            if (this.BitSignaturesInOneLine == null) return;
            string[] bitSignatures = this.BitSignaturesInOneLine.Split(',');
            this.BitSignatures = new List<string>();
            foreach (string bitSignature in bitSignatures)
            {
                string preparedString = bitSignature.Replace("&lt", "<").Replace("&gt", ">");
                if (preparedString.Contains("\""))
                {
                    preparedString = preparedString.Remove(preparedString.LastIndexOf("\""));
                    preparedString = preparedString.Remove(0, preparedString.IndexOf("\""));


                    this.BitSignatures.Add(preparedString.Replace("\"", ""));
                }
            }
        }


        [OnSerializing]
        private void OnSerializing(StreamingContext sc)
        {

            StringBuilder sb = new StringBuilder();
            this.BitSignatures.ForEach((s =>
            {
                sb.Append("\"");
                string preparedString = s.Replace("<", "&lt").Replace(">", "&gt");
                sb.Append(preparedString);
                sb.Append("\"");
                if (this.BitSignatures.IndexOf(s) != this.BitSignatures.Count - 1)
                {
                    sb.Append(",");
                }
            }));
            this.BitSignaturesInOneLine = sb.ToString();
        }

    }





}
