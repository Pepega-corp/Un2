using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using Unicon2.Fragments.GraphicalMenu.Infrastructure.Keys;
using Unicon2.Fragments.GraphicalMenu.Infrastructure.Model;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services.CommandStack;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.GraphicalMenu.ViewModel
{
    public class GraphicalMenuFragmentViewModel : ViewModelBase, IFragmentViewModel, IDropTarget, IDeviceContextConsumer, ICommandStackDependencySource
    {
        private int _cellWidth;
        private int _cellHeight;
        private int _displayWidth;
        private int _displayHeight;

        public GraphicalMenuFragmentViewModel()
        {
            AvailableGraphicalElementViewModels = new List<GraphicalElementViewModel>()
            {
                new GraphicalElementViewModel(40, 80, "Test 2*1", this),
                new GraphicalElementViewModel(40, 120, "Test 3*1", this),
                new GraphicalElementViewModel(120, 40, "Test 1*3", this),
                new GraphicalElementViewModel(40, 40, "Test 1*1", this),
                new GraphicalElementViewModel(120, 80, "Test 2*3", this)
            };
            DisplayHeight = 400;
            DisplayWidth = 400;
            CellHeight = 40;
            CellWidth = 40;
          
            GraphicalElementViewModelsOnDisplay = new ObservableCollection<GraphicalElementViewModel>();
            GraphicalMenuSlotViewModels=new List<GraphicalMenuSlotViewModel>();
            RaisePropertyChanged(nameof(GraphicalMenuSlotViewModels));

        }

        public void Initialize(IDeviceFragment deviceFragment)
        {
            if (deviceFragment is IGraphicalMenu graphicalMenu)
            {
                DisplayHeight = graphicalMenu.DisplayHeight;
                DisplayWidth = graphicalMenu.DisplayWidth;
                CellHeight = graphicalMenu.CellHeight;
                CellWidth = graphicalMenu.CellWidth;
                GraphicalMenuSlotViewModels.Clear();
                var heightInSlots = DisplayHeight / CellHeight;
                var widthInSlots = DisplayWidth / CellWidth;
                GraphicalMenuSlotViewModelsArray = new GraphicalMenuSlotViewModel[heightInSlots][];

                for (int i = 0; i < heightInSlots; i++)
                {
                    GraphicalMenuSlotViewModelsArray[i] = new GraphicalMenuSlotViewModel[widthInSlots];
                    for (int j = 0; j < widthInSlots; j++)
                    {
                        var element = new GraphicalMenuSlotViewModel(i, j);
                        GraphicalMenuSlotViewModels.Add(element);
                        GraphicalMenuSlotViewModelsArray[i][j] = element;
                    }
                }

            }
        }

        public string StrongName => GraphicalMenuKeys.GRAPHICAL_MENU +
                                    ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public string NameForUiKey => GraphicalMenuKeys.GRAPHICAL_MENU;
        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }
        public List<GraphicalElementViewModel> AvailableGraphicalElementViewModels { get; }
        public ObservableCollection<GraphicalElementViewModel> GraphicalElementViewModelsOnDisplay { get; }

        public List<GraphicalMenuSlotViewModel> GraphicalMenuSlotViewModels { get; }

        public GraphicalMenuSlotViewModel[][] GraphicalMenuSlotViewModelsArray { get; set; }

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

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Copy;
            var draggingGraphicalElementViewModel = dropInfo.Data as GraphicalElementViewModel;
            int x = (int) dropInfo.DropPosition.X / CellWidth;
            int y = (int) dropInfo.DropPosition.Y / CellHeight;
            GraphicalMenuSlotViewModels.ForEach(model => model.IsHighlighted = false);

            var heightInSlots = draggingGraphicalElementViewModel.SizeHeight / CellHeight;
            var widthInSlots = draggingGraphicalElementViewModel.SizeWidth / CellWidth;

            GetMenuSlots(x, y, widthInSlots, heightInSlots)
                .OnSuccess(list => list.ForEach(model => model.IsHighlighted = true));
        }

        private Result<List<GraphicalMenuSlotViewModel>> GetMenuSlots(int x, int y, int xLength, int yLength)
        {
            var resultList = new List<GraphicalMenuSlotViewModel>();
            for (int i = 0; i < yLength; i++)
            {
                for (int j = 0; j < xLength; j++)
                {
                    GetMenuSlot(x + j, y + i).OnSuccess(model => resultList.Add(model));
                }
            }

            var res = resultList.Distinct();
            if (res.Count() == yLength * xLength)
            {
                return res.ToList();
            }

            return Result<List<GraphicalMenuSlotViewModel>>.Create(false);
        }

        private Result<GraphicalMenuSlotViewModel> GetMenuSlot(int x, int y)
        {
            if (GraphicalMenuSlotViewModelsArray.Length > x && GraphicalMenuSlotViewModelsArray[x].Length > y)
            {
                if (GraphicalMenuSlotViewModelsArray[x][y].RelatedGraphicalElementViewModel == null)
                {
                    return GraphicalMenuSlotViewModelsArray[x][y];
                }
            }

            return Result<GraphicalMenuSlotViewModel>.Create(false);
        }


        public void TryDropElement((int,int) position, GraphicalElementViewModel elementToDrop)
        {
   
            GraphicalMenuSlotViewModels.ForEach(model => model.IsHighlighted = false);


            var heightInSlots = elementToDrop.SizeHeight / CellHeight;
            var widthInSlots = elementToDrop.SizeWidth / CellWidth;
            GetMenuSlots(position.Item1, position.Item2, widthInSlots, heightInSlots).OnSuccess(list =>
            {
                list.ForEach(model => model.RelatedGraphicalElementViewModel = elementToDrop);
            });

            GraphicalElementViewModelsOnDisplay.Add(elementToDrop);
        }


        public void Drop(IDropInfo dropInfo)
        {
            GraphicalMenuSlotViewModels.ForEach(model => model.IsHighlighted = false);
            var draggingGraphicalElementViewModel = (dropInfo.Data as GraphicalElementViewModel).Copy();
            int x = (int) dropInfo.DropPosition.X / CellWidth;
            int y = (int) dropInfo.DropPosition.Y / CellHeight;
            TryDropElement((x, y), draggingGraphicalElementViewModel);
        }

        public DeviceContext DeviceContext { get; set; }
    }
}