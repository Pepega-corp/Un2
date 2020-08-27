using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
//using Unicon.Common.Queries;
using Unicon2.Unity.Interfaces;

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

           // var client = new HttpClient();
            //var resg= await client.PostAsync("https://localhost:32776/api/v1/root/command", new StringContent(res, Encoding.UTF8, "application/json"));
		}
	}
}
