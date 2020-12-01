using System.Web.Mvc;

namespace Unicon2.Fragments.Configuration.Exporter.Utils
{
    public static class TagBuilderExtensions
    {
        public static void AddToInnerHtml(this TagBuilder tagBuilder, string newHtml)
        {
            if (string.IsNullOrEmpty(tagBuilder.InnerHtml))
            {
                tagBuilder.InnerHtml += newHtml;
            }
            else
            {
                tagBuilder.InnerHtml += "\n" + newHtml;
            }

        }

        public static void AddTagToInnerHtml(this TagBuilder tagBuilder, TagBuilder tagToAdd)
        {
            tagBuilder.AddToInnerHtml(tagToAdd.ToString());
        }
    }
}