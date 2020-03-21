using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Unicon2.Fragments.Programming.ViewModels;

namespace Unicon2.Fragments.Programming
{
    public static class CommonHelper 
    {
        /// <summary>
        /// Ищет корневой Canvas, на котором расположены все элементы
        /// </summary>
        /// <param name="dObject">Текущий объект, для которого ищется родитель</param>
        /// <returns>Designer Canvas</returns>
        public static Canvas GetDesignerCanvas(DependencyObject dObject)
        {
            while (true)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(dObject);

                if (parent == null)
                    return null;

                Canvas canvas = parent as Canvas;
                if (canvas?.DataContext is SchemeTabViewModel)
                    return canvas;

                dObject = parent;
            }
        }

        /// <summary>
        /// Ищет корневой Window, на котором расположены все элементы
        /// </summary>
        /// <param name="dObject">Текущий объект, для которого ищется Window</param>
        /// <returns>Designer Canvas</returns>
        public static Window GetWindow(DependencyObject dObject)
        {
            while (true)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(dObject);
                if (parent == null)
                    return null;

                Window window = parent as Window;
                if (window != null)
                    return window;

                dObject = parent;
            }
        }

        public static Thumb GetThumbOfPresenter(DependencyObject obj)
        {
            int count = VisualTreeHelper.GetChildrenCount(obj);
            for (int i = 0; i < count; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                Thumb retThumb = child as Thumb;
                if (retThumb != null && retThumb.Name == "DragThumb")
                {
                    return retThumb;
                }
                int c = VisualTreeHelper.GetChildrenCount(child);
                if (c > 0)
                {
                    retThumb = GetThumbOfPresenter(child);
                }
                if (retThumb != null)
                    return retThumb;
            }
            return null;
        }
    }
}
