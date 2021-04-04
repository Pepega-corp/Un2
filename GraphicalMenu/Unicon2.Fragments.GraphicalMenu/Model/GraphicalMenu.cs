using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Fragments.GraphicalMenu.Infrastructure.Keys;
using Unicon2.Fragments.GraphicalMenu.Infrastructure.Model;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;

namespace Unicon2.Fragments.GraphicalMenu.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class GraphicalMenu: IGraphicalMenu
    {
        public string StrongName => GraphicalMenuKeys.GRAPHICAL_MENU;
        [JsonProperty]
        public IFragmentSettings FragmentSettings { get; set; }

        [JsonProperty]
        public int DisplayWidth { get; set; }

        [JsonProperty]
        public int DisplayHeight { get; set; }

        [JsonProperty]
        public int CellWidth { get; set; }

        [JsonProperty]
        public int CellHeight { get; set; }
    }
}
