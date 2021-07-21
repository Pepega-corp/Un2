using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.GraphicalMenu.Infrastructure.Keys;
using Unicon2.Fragments.GraphicalMenu.Infrastructure.Model;
using Unicon2.Fragments.GraphicalMenu.Infrastructure.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.GraphicalMenu.Editor.ViewModel
{
    public class GraphicalMenuFragmentEditorViewModel :ViewModelBase, IGraphicalMenuFragmentEditorViewModel
    {
        private readonly Func<IGraphicalMenu> _graphicalMenuFactory;
        private int _displayWidth;
        private int _displayHeight;
        private int _cellWidth;
        private int _cellHeight;

        public GraphicalMenuFragmentEditorViewModel(Func<IGraphicalMenu> graphicalMenuFactory)
        {
            _graphicalMenuFactory = graphicalMenuFactory;
        }

        public async Task<Result> Initialize(IDeviceFragment deviceFragment)
        {
            if (deviceFragment is IGraphicalMenu graphicalMenu)
            {
                DisplayHeight = graphicalMenu.DisplayHeight;
                DisplayWidth = graphicalMenu.DisplayWidth;
                CellHeight = graphicalMenu.CellHeight;
                CellWidth = graphicalMenu.CellWidth;
            }
            return Result.Create(true);
        }

        public string StrongName => GraphicalMenuKeys.GRAPHICAL_MENU +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public string NameForUiKey => GraphicalMenuKeys.GRAPHICAL_MENU;

        public IDeviceFragment BuildDeviceFragment()
        {
            var res = _graphicalMenuFactory();
            res.DisplayHeight = DisplayHeight;
            res.DisplayWidth = DisplayWidth;
            res.CellHeight = CellHeight;
            res.CellWidth = CellWidth;
            return res;
        }

        public int DisplayWidth
        {
            get => _displayWidth;
            set
            {
                _displayWidth = value; 
                RaisePropertyChanged();
            }
        }

        public int DisplayHeight
        {
            get => _displayHeight;
            set
            {
                _displayHeight = value;
                RaisePropertyChanged();
            }
        }

        public int CellWidth
        {
            get => _cellWidth;
            set
            {
                _cellWidth = value;
                RaisePropertyChanged();
            }
        }

        public int CellHeight
        {
            get => _cellHeight;
            set
            {
                _cellHeight = value;
                RaisePropertyChanged();
            }
        }
    }
}
