using System;
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
            //var res = "";
            //var url = "http://f06c99c4389c.ngrok.io/";
            //using (StringWriter writer = new StringWriter())
            //{
            //    var jsonSerializerSettings = new JsonSerializerSettings()
            //    {
            //        Formatting = Formatting.Indented,
            //        TypeNameHandling = TypeNameHandling.All,
            //    };
            //    writer.Write(JsonConvert.SerializeObject(new GetDeviceDefinitionsQuery("pupa",null), jsonSerializerSettings));
            //    res = writer.ToString();
            //    var client = new HttpClient();
            //    var resg= await client.PostAsync(url+"api/v1/root/command", new StringContent(res, Encoding.UTF8, "application/json"));
            //    var resContent=await resg.Content.ReadAsStringAsync();
            //    var resTyped=JsonConvert.DeserializeObject<Result<List<DeviceDefinition>>>(resContent);


            //}
            //List<CommandRecord> res2;

            //using (StringWriter writer = new StringWriter())
            //{
            //    var jsonSerializerSettings = new JsonSerializerSettings()
            //    {
            //        Formatting = Formatting.Indented,
            //        TypeNameHandling = TypeNameHandling.All,
            //    };
            //    writer.Write(JsonConvert.SerializeObject(new GetStoreSnapshotQuery()
            //    {

            //    }, jsonSerializerSettings));
            //   var res = writer.ToString();
            //    var client = new HttpClient();
            //    var resg = await client.PostAsync("http://e660e222cc9c.ngrok.io/" + "api/v1/root/command", new StringContent(res, Encoding.UTF8, "application/json"));
            //    var resContent = await resg.Content.ReadAsStringAsync();
            //    var resTyped = JsonConvert.DeserializeObject<Result<List<CommandRecord>>>(resContent);
            //   var res2 = resTyped.Item;

            //}

            //using (StringWriter writer = new StringWriter())
            //{
            // var jsonSerializerSettings = new JsonSerializerSettings()
            // {
            //  Formatting = Formatting.Indented,
            //  TypeNameHandling = TypeNameHandling.All,
            // };
            // writer.Write(JsonConvert.SerializeObject(new UploadSnapshotCommand(res2)
            // {

            // }, jsonSerializerSettings));
            // res = writer.ToString();
            // var client = new HttpClient();
            // var resg = await client.PostAsync(url + "api/v1/root/command", new StringContent(res, Encoding.UTF8, "application/json"));
            // var resContent = await resg.Content.ReadAsStringAsync();



            //}
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
