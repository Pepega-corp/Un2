using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GongSolutions.Wpf.DragDrop;
using Unicon2.Fragments.GraphicalMenu.Commands.Stack;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Services.CommandStack;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.GraphicalMenu.ViewModel
{
    public class GraphicalElementViewModel :ViewModelBase, IDragSource
    {
        private readonly GraphicalMenuFragmentViewModel _parent;

        public GraphicalElementViewModel(int sizeWidth, int sizeHeight, string name, GraphicalMenuFragmentViewModel parent)
        {
            _parent = parent;
            SizeWidth = sizeWidth;
            SizeHeight = sizeHeight;
            Name = name;
            RemoveCommand=new RelayCommand(OnRemoveExecute);
        }


        private async void OnRemoveExecute()
        {
            var command = new GraphicalElementRemoveStackingCommand(this, _parent);
            await command.ExecuteCommandAndAddToStack();
        }

        public int SizeWidth { get; }
        public int SizeHeight { get; }
        public string Name { get; }
        public ICommand RemoveCommand { get; }

        public GraphicalElementViewModel Copy()
        {
            return new GraphicalElementViewModel(SizeWidth, SizeHeight, Name,_parent);
        }

        public void StartDrag(IDragInfo dragInfo)
        {
            dragInfo.Effects = DragDropEffects.Copy;
            dragInfo.Data = this;
            if (_parent.GraphicalElementViewModelsOnDisplay.Contains(this))
            {
                var x = _parent.GraphicalMenuSlotViewModels
                    .Where(model => model.RelatedGraphicalElementViewModel == this).Min(model => model.SlotLeftOffset);
                var y = _parent.GraphicalMenuSlotViewModels
                    .Where(model => model.RelatedGraphicalElementViewModel == this).Min(model => model.SlotTopOffset);
                DragStartPosition = (x, y);

                _parent.GraphicalElementViewModelsOnDisplay.Remove(this);
                _parent.GraphicalMenuSlotViewModels.Where(model => model.RelatedGraphicalElementViewModel==this).ForEach(model =>model.RelatedGraphicalElementViewModel=null);
            }
        }

        public (int,int) DragStartPosition { get; set; }

        public bool CanStartDrag(IDragInfo dragInfo)
        {
            return true;
        }

        public void Dropped(IDropInfo dropInfo)
        {
           
        }

        public void DragDropOperationFinished(DragDropEffects operationResult, IDragInfo dragInfo)
        {
           
        }

        public void DragCancelled()
        {
            if (!_parent.AvailableGraphicalElementViewModels.Contains(this))
            {
                _parent.TryDropElement(DragStartPosition,this);
            }

            _parent.GraphicalMenuSlotViewModels.ForEach(model =>model.IsHighlighted=false);
        }

        public bool TryCatchOccurredException(Exception exception)
        {
            DragCancelled();
            return true;
        }
    }
}
