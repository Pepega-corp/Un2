using System;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.CodeCompletion;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using Unicon2.Formatting.Editor.ViewModels.Helpers;


namespace Unicon2.Formatting.Editor.Views
{
  public  class CodeCompletionItem: ICompletionData
    {
        public CodeCompletionItem(string text, object description)
        {
            Text = text;
            Description = description;
        }

        public void Complete(TextArea textArea, ISegment completionSegment, EventArgs insertionRequestEventArgs)
        {
            var length = CodeStringHelper.GetStringBetweenCaretAndPrevSpecialChar(textArea.Document.Text,
                completionSegment.EndOffset).Length;
            textArea.Document.Replace(new AnchorSegment(textArea.Document,completionSegment.Offset-length,length), this.Text);
        }

        public ImageSource Image { get; }
        public string Text { get; }
        // Use this property if you want to show a fancy UIElement in the drop down list.
        public object Content
        {
            get { return this.Text; }
        }
        public object Description { get; }
        public double Priority { get; }
    }
}
