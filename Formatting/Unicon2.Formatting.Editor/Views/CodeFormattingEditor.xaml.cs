using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Editing;
using Unicon2.Formatting.Editor.ViewModels;
using Unicon2.Formatting.Editor.ViewModels.Helpers;
using Unicon2.Formatting.Infrastructure.Services;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Formatting.Editor.Views
{
    /// <summary>
    /// Interaction logic for CodeFormattingEditor.xaml
    /// </summary>
    public partial class CodeFormattingEditor : UserControl
    {
        private CompletionWindow _completionWindow;
        private List<ICompletionData> _completionDatas;
        public CodeFormattingEditor()
        {


            InitializeComponent();
       
            DataContextChanged += CodeFormattingEditor_DataContextChanged;
        }

        private void CodeFormattingEditor_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            FormatCodeEditor.Text = (DataContext as CodeFormatterViewModel).FormatCodeString;
            FormatCodeEditor.TextArea.TextEntering += FormatCodeEditor_TextArea_TextEntering;
            FormatCodeEditor.TextArea.TextEntered += FormatCodeEditor_TextArea_TextEntered;
            FormatCodeEditor.TextChanged += FormatCodeEditor_TextChanged;
        }

        private void FormatCodeEditor_TextChanged(object sender, EventArgs e)
        {
            (DataContext as CodeFormatterViewModel).FormatCodeString = FormatCodeEditor.Text;
        }


         

        private void FormatCodeEditor_TextArea_TextEntered(object sender, TextCompositionEventArgs e)
        {
            // open code completion after the user has pressed dot:
            _completionWindow = new CompletionWindow(FormatCodeEditor.TextArea);
            // provide AvalonEdit with the data:
            IList<ICompletionData> data = _completionWindow.CompletionList.CompletionData;
            data.Clear();

            if (_completionDatas == null)
            {
                _completionDatas = StaticContainer.Container.Resolve<ICodeFormatterService>().GetFunctionsInfo()
                    .Select((funInfo) => new CodeCompletionItem(funInfo.name, funInfo.desc)).Cast<ICompletionData>()
                    .ToList();
            }

            _completionDatas.Where(completionData => completionData.Text.ToLowerInvariant().Contains(CodeStringHelper.GetStringBetweenCaretAndPrevSpecialChar(FormatCodeEditor.Text,FormatCodeEditor.CaretOffset).ToLowerInvariant())).ForEach(data.Add);
            _completionWindow.Show();
            _completionWindow.Closed += delegate { _completionWindow = null; };

        }

        private void FormatCodeEditor_TextArea_TextEntering(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Length > 0 && _completionWindow != null)
            {
                if (!char.IsLetterOrDigit(e.Text[0]))
                {
                    // Whenever a non-letter is typed while the completion window is open,
                    // insert the currently selected element.
                    _completionWindow.CompletionList.RequestInsertion(e);
                }
            }
            // do not set e.Handled=true - we still want to insert the character that was typed
        }
    }
}
