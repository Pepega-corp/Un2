using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.GraphicalMenu.ViewModel;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Presentation.Infrastructure.Services.CommandStack;

namespace Unicon2.Fragments.GraphicalMenu.Commands.Stack
{
    public class GraphicalElementRemoveStackingCommand: StackingCommandBase
    {
        private readonly GraphicalElementViewModel _graphicalElementViewModel;
        private readonly GraphicalMenuFragmentViewModel _graphicalMenuFragmentViewModel;

        public GraphicalElementRemoveStackingCommand(GraphicalElementViewModel graphicalElementViewModel, GraphicalMenuFragmentViewModel graphicalMenuFragmentViewModel)
        {
            _graphicalElementViewModel = graphicalElementViewModel;
            _graphicalMenuFragmentViewModel = graphicalMenuFragmentViewModel;
            AddDependencies(_graphicalMenuFragmentViewModel);
        }

        public override string ViewName { get; }


        private List<(int, int)> _slots;

        public override Task Do()
        {
            if (_graphicalMenuFragmentViewModel.GraphicalElementViewModelsOnDisplay
                .Contains(_graphicalElementViewModel))
            {
                _graphicalMenuFragmentViewModel.GraphicalElementViewModelsOnDisplay.Remove(_graphicalElementViewModel);
                var slots = _graphicalMenuFragmentViewModel.GraphicalMenuSlotViewModels.Where(model =>
                    model.RelatedGraphicalElementViewModel == _graphicalElementViewModel).ToList();
                slots.ForEach(model => model.RelatedGraphicalElementViewModel = null);
                _slots = slots.Select(model => (model.SlotLeftOffset, model.SlotTopOffset)).ToList();
            }
            return Task.CompletedTask;
        }

        public override Task Undo()
        {
            if (!_graphicalMenuFragmentViewModel.GraphicalElementViewModelsOnDisplay
                .Contains(_graphicalElementViewModel))
            {
                var slots = _slots.Select(tuple =>
                    _graphicalMenuFragmentViewModel.GraphicalMenuSlotViewModelsArray[
                        tuple.Item1][
                        tuple.Item2]).ToList();
                slots.ForEach(model => model.RelatedGraphicalElementViewModel = _graphicalElementViewModel);

                _graphicalMenuFragmentViewModel.GraphicalElementViewModelsOnDisplay.Add(_graphicalElementViewModel);
               

            }
            return Task.CompletedTask;
        }
    }
}
