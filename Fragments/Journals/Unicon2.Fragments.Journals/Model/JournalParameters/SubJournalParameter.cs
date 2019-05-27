using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Fragments.Journals.Model.JournalParameters
{
    [DataContract(Namespace = "SubJournalParameterNS", IsReference = true)]
    public class SubJournalParameter : JournalParameter, ISubJournalParameter
    {
        public SubJournalParameter()
        {
            BitNumbersInWord = new List<int>();
        }
        #region Implementation of ISubJournalParameter
        [DataMember(Name = nameof(BitNumbersInWord), Order = 0)]
        public List<int> BitNumbersInWord { get; set; }

        public IComplexJournalParameter ParentComplexJournalParameter { get; set; }
        public ushort GetParameterUshortInRecord(ushort[] recordUshorts)
        {
            ushort valueToFormat = recordUshorts.Skip(ParentComplexJournalParameter.StartAddress).Take(1).ToArray()[0];
            BitArray parentBitArray = new BitArray(new[] { (int)valueToFormat });
            bool[] subParameterBools = new bool[BitNumbersInWord.Count];
            foreach (var numInWord in BitNumbersInWord)
            {
                if (parentBitArray.Count <= numInWord)
                {
                    subParameterBools[BitNumbersInWord.IndexOf(numInWord)] = false;
                }
                else
                {
                    subParameterBools[BitNumbersInWord.IndexOf(numInWord)] =
                        parentBitArray[numInWord];
                }

            }
            ushort res = (ushort)(new BitArray(subParameterBools).GetIntFromBitArray());
            return res;
        }

        #endregion
    }
}
