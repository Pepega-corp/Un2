using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Editor.Interfaces.Dependencies;
using Unicon2.Fragments.Configuration.Editor.ViewModels.Dependencies.Results;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Dependencies.Results;
using Unicon2.Fragments.Configuration.Model.Dependencies.Results;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.Editor.Helpers
{
    public class ResultFillHelper
    {
        private readonly IFormatterEditorFactory _formatterEditorFactory;
        private readonly IFormatterViewModelFactory _formatterViewModelFactory;
        private readonly ISaveFormatterService _saveFormatterService;

        public ResultFillHelper(IFormatterEditorFactory formatterEditorFactory, IFormatterViewModelFactory formatterViewModelFactory, ISaveFormatterService saveFormatterService)
        {
            this._formatterEditorFactory = formatterEditorFactory;
            _formatterViewModelFactory = formatterViewModelFactory;
            _saveFormatterService = saveFormatterService;
        }

        public List<IResultViewModel> CreateEmptyResultViewModels()
        {
            return new List<IResultViewModel>()
            {
                 new ApplyFormatterResultViewModel(_formatterEditorFactory),
                 new BlockInteractionResultViewModel()
            };
        }

        public IResultViewModel CreateResultViewModel(IDependencyResult dependencyResult)
        {
            switch (dependencyResult)
            {
                case IApplyFormatterResult applyFormatterResult:

                    if (applyFormatterResult.UshortsFormatter != null)
                    {
                        var formatterParametersViewModel = StaticContainer.Container
                            .Resolve<IFormatterViewModelFactory>()
                            .CreateFormatterViewModel(applyFormatterResult.UshortsFormatter);

                        return new ApplyFormatterResultViewModel(_formatterEditorFactory)
                        {
                            FormatterParametersViewModel = formatterParametersViewModel
                        };
                    }

                    break;
                case IBlockInteractionResult blockInteractionResult:
                    return new BlockInteractionResultViewModel(); 
                    break;
            }

            
            return null;
        }
        public IDependencyResult CreateResultFromViewModel(IResultViewModel dependencyResult)
        {

            switch (dependencyResult)
            {
                case BlockInteractionResultViewModel blockInteractionResult:
                    return new BlockInteractionResult();
                    break;
                case ApplyFormatterResultViewModel applyFormatterResultViewModel:
                    var formatter =
                        _saveFormatterService.CreateUshortsParametersFormatter(applyFormatterResultViewModel
                            .FormatterParametersViewModel);
                    return new ApplyFormatterResult()
                    {
                        Name = formatter.Name,
                        UshortsFormatter = formatter
                    };

                    break;
            }


            return null;
        }
        
    }
}