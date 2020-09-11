using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon.Common.Commands;
using Unicon.Common.Model;
using Unicon.Common.Queries;
using Unicon.Common.Result;
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
                writer.Write(JsonConvert.SerializeObject(new GetDeviceDefinitionsQuery("pupa",null), jsonSerializerSettings));
                res = writer.ToString();
                var client = new HttpClient();
                var resg= await client.PostAsync("https://localhost:32778/api/v1/root/command", new StringContent(res, Encoding.UTF8, "application/json"));
                var resContent=await resg.Content.ReadAsStringAsync();
                var resTyped=JsonConvert.DeserializeObject<Result<List<DeviceDefinition>>>(resContent);


            }
            var res2 = "";
            using (StringWriter writer = new StringWriter())
            {
	            var jsonSerializerSettings = new JsonSerializerSettings()
	            {
		            Formatting = Formatting.Indented,
		            TypeNameHandling = TypeNameHandling.All,
	            };
	            writer.Write(JsonConvert.SerializeObject(new GetStoreSnapshotQuery()
	            {
                  
	            }, jsonSerializerSettings));
	            res = writer.ToString();
	            var client = new HttpClient();
	            var resg = await client.PostAsync("https://localhost:32780/api/v1/root/command", new StringContent(res, Encoding.UTF8, "application/json"));
	            var resContent = await resg.Content.ReadAsStringAsync();
	            var resTyped = JsonConvert.DeserializeObject<Result<List<CommandRecord>>>(resContent);


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

            
        }
    }
}
