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
        private bool _errorInFormatString;
        private bool _errorInFormatBackString;

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
                RefreshFormatBackError();
                RefreshFormatError();

           

                return !ErrorInFormatBackString && !ErrorInFormatString;
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

        public void RefreshFormatError()
        {
            var res = _codeFormatterService.GetFormatUshortsFunc(
                new CodeFormatterExpression(FormatCodeString, FormatBackCodeString));
            ErrorInFormatString = !res.IsSuccess;
        }
        public void RefreshFormatBackError()
        {
            var res2 = _codeFormatterService.GetFormatBackUshortsFunc(
                new CodeFormatterExpression(FormatCodeString, FormatBackCodeString),
                new DeviceContext(null, null, "Test", null, null), true,null);
            ErrorInFormatBackString = !res2.IsSuccess;
        }

        public bool ErrorInFormatString
        {
            get => _errorInFormatString;
            set
            {
                _errorInFormatString = value;
                RaisePropertyChanged();
            }
        }

        public bool ErrorInFormatBackString
        {
            get => _errorInFormatBackString;
            set
            {
                _errorInFormatBackString = value; 
                RaisePropertyChanged();
            }
        }
    }
}
