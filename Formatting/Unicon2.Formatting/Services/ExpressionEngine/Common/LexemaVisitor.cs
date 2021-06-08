namespace Unicon2.Formatting.Services.ExpressionEngine.Common
{
    public abstract class LexemaVisitor
    {
        protected LexemaVisitor(LexemManager lexemManager)
        {
            LexemManager = lexemManager;
        }

        public LexemManager LexemManager { get; }
        public abstract IRuleNode VisitStringPart(string parameters);
    }
}