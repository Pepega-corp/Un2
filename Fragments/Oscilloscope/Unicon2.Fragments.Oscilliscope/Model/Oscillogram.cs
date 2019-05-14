using System.Collections.Generic;

namespace Unicon2.Fragments.Oscilliscope.Model
{
    public class Oscillogram
    {
        public Oscillogram()
        {
            this.Pages = new List<ushort[]>();
        }
        public int OscillogramNumber { get; set; }
        public List<ushort[]> Pages { get; set; }

        public List<ushort> OscillogramResultUshorts { get; set; }
        public string OscillogramPath { get; set; }
    }
}
