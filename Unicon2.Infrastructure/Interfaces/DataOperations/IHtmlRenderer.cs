using System.Threading.Tasks;

namespace Unicon2.Infrastructure.Interfaces.DataOperations
{
    public interface IHtmlRenderer<T,TSelector>
    {
        Task<string> RenderHtmlString(T objectToRender, TSelector selector);
    }
}