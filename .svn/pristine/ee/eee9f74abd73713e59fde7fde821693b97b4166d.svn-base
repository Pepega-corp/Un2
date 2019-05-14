using System;
using System.Windows.Controls;
using System.Windows.Markup;
using Unicon2.Infrastructure.Common;

namespace Unicon2.SharedResources.Markups
{
    /// <summary>
    ///     Represents a view resolver markup that resolves instances from the main IoC Container
    ///     Представляет класс возвращающий нужную вьюху по имени
    /// </summary>
    public class ViewResolverExtension : MarkupExtension
    {
        #region [Properties]

        /// <summary>
        ///     Gets or sets the name of the view to resolve
        ///     Свойство с именем вьюшки
        /// </summary>
        [ConstructorArgument("viewName")]
        public string ViewName { get; set; }

        #endregion [Properties]

        #region [Ctor's]

        /// <summary>
        ///     Creates an instance of <see cref="ViewResolverExtension" />
        /// </summary>
        public ViewResolverExtension()
        {
        }

        /// <summary>
        ///     Creates an instance of <see cref="ViewResolverExtension" />
        /// </summary>
        /// <param name="viewName">The name of the view to resolve</param>
        public ViewResolverExtension(string viewName)
        {
            this.ViewName = viewName;
        }

        #endregion [Ctor's]

        #region [Override members]

        /// <summary>
        ///     When implemented in a derived class, returns an object that is provided as the value of the target property for
        ///     this markup extension.
        ///     Если вьюшка с именем this.ViewName зарегистрирована в бутстрапере, то этот метод врнет её. Иначе - null
        /// </summary>
        /// <returns>
        ///     The object value to set on the property where the extension is applied.
        /// </returns>
        /// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(this.ViewName)) return null;
            var resolvedView = StaticContainer.Container.Resolve(typeof(UserControl), this.ViewName);
            return resolvedView;
        }

        #endregion [Override members]
    }
}