using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Infrastructure;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Unity.Commands;
//using Unicon.Common.Queries;
using Unicon2.Unity.Interfaces;
using Unicon2.Web.Presentation.View;
using Unicon2.Web.Presentation.ViewModels;

namespace Unicon2.Web.Presentation.Module
{
    public class WebPresentationModule : IUnityModule
    {
        public async void Initialize(ITypesContainer container)
        {
            var res = "";
            using (StringWriter writer = new StringWriter())
            {
                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented,
                    TypeNameHandling = TypeNameHandling.All,
                };
                //writer.Write(JsonConvert.SerializeObject(new GetDeviceDefinitionsQuery("pupa"), jsonSerializerSettings));
                res = writer.ToString();
            }
            container.Register<WebSynchronizationViewModel>();

            container.Resolve<IApplicationGlobalCommands>().ShellLoaded += () =>
            {
                container.Resolve<IMainMenuService>().RegisterMainMenuCommand(new MainMenuCommandRegistrationOptions(
                    Guid.NewGuid(), new RelayCommand(
                        () =>
                        {
                            container.Resolve<IApplicationGlobalCommands>().ShowWindowModal(
                                () => new WebSynchronizationView(), container.Resolve<WebSynchronizationViewModel>());
                        }), ApplicationGlobalNames.UiCommandStrings.OPEN_WEB_SYNC));
            };

            // var client = new HttpClient();
            //var resg= await client.PostAsync("https://localhost:32776/api/v1/root/command", new StringContent(res, Encoding.UTF8, "application/json"));
        }
    }
}
