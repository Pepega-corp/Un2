namespace Unicon2.Infrastructure.Interfaces.DataOperations
{
    public interface IHtmlRenderer<T>
    {
        string RenderHtmlString(T objectToRender);
    }
}