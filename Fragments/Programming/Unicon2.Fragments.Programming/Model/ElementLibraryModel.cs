using System.Collections.Generic;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Programming.Model
{
    public class ElementLibraryModel : IElementLibraryModel
    {
        public ElementLibraryModel()
        {
            this.Elements = new List<ILogicElement>();
        }
        /// <summary>
        /// Библиотека всех выбранных элементов, которые реализует данное устройство
        /// </summary>
        public List<ILogicElement> Elements { get; }

        public string StrongName => ProgrammingKeys.ELEMENT_LIBRARY;
    }
}
