using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.CodeCompletion;
using Unicon2.Formatting.Editor.ViewModels.Helpers;
using Unicon2.Formatting.Editor.Visitors;
using Unicon2.Formatting.Infrastructure.Services;
using Unicon2.Formatting.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.DeviceContext;

namespace Unicon2.Formatting.Editor.ViewModels
{
   public class CodeFormatterViewModel: UshortsFormatterViewModelBase,ICodeFormatterViewModel
    {
        private readonly ICodeFormatterService _codeFormatterService;
        private string _formatCodeString;
        private string _formatBackCodeString;

        public CodeFormatterViewModel(ICodeFormatterService codeFormatterService)
        {
            _codeFormatterService = codeFormatterService;

        }

        public override object Clone()
        {
            return new CodeFormatterViewModel(_codeFormatterService)
            {
                FormatBackCodeString = FormatBackCodeString,
                FormatCodeString = FormatCodeString
            };
        }
        
        public override T Accept<T>(IFormatterViewModelVisitor<T> visitor)
        {
            return visitor.VisitCodeFormatter(this);
        }

        public override string StrongName => nameof(CodeFormatterViewModel);
        public bool IsInEditMode { get; set; }
        public void StartEditElement()
        {
            throw new NotImplementedException();
        }

        public void StopEditElement()
        {
            throw new NotImplementedException();
        }

        public bool IsValid
        {
            get
            {
                var res = _codeFormatterService.GetFormatUshortsFunc(
                    new CodeFormatterExpression(FormatCodeString, FormatBackCodeString),
                    new DeviceContext(null, null, "Test", null, null));
                var res2 = _codeFormatterService.GetFormatBackUshortsFunc(
                    new CodeFormatterExpression(FormatCodeString, FormatBackCodeString),
                    new DeviceContext(null, null, "Test", null, null));

                return res.IsSuccess && res2.IsSuccess;
            }
        }

        public string FormatCodeString
        {
            get => _formatCodeString;
            set
            {
                _formatCodeString = value; 
                RaisePropertyChanged();
            }
        }

        public string FormatBackCodeString
        {
            get => _formatBackCodeString;
            set
            {
                _formatBackCodeString = value; 
                RaisePropertyChanged();
            }
        }
    }
}
