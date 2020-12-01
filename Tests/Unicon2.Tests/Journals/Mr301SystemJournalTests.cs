using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;
using Prism.Ioc;
using Unicon2.Connections.MockConnection.Model;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Model.DefaultDevice;
using Unicon2.Tests.Helpers.Query;

namespace Unicon2.Tests.Journals
{
    [TestFixture]
    public class Mr301SystemJournalTests
    {
        [Test]
        public async Task LoadMr301SystemJournalTest()
        {
            var serializerService = Program.GetApp().Container.Resolve<ISerializerService>();

            var device = serializerService.DeserializeFromFile<IDevice>("FileAssets/МР301_JA.json") as DefaultDevice;
            StaticContainer.Container.Resolve<IDevicesContainerService>()
                .AddConnectableItem(device);

           await StaticContainer.Container.Resolve<IDevicesContainerService>()
                .ConnectDeviceAsync(device, new MockConnection());

           
           List<QueryMockDefinition> queryMockDefinitions=new List<QueryMockDefinition>();
           
           const Int32 BufferSize = 128;
           using (var fileStream = File.OpenRead("FileAssets/logFileForMR301JS.txt"))
           using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, BufferSize)) {
               String line;
               while ((line = streamReader.ReadLine()) != null)
               {
                   var regex = @"\[(.*?)\]";
                   var matched = Regex.Match(line,regex).Groups[1].Value;
                   ushort func = 0;
                   ushort address = 0;
                   ushort? numberofPoints = null;
                   var data=new List<ushort>();
                   if (matched.Contains("Fun"))
                   {
                       var index = matched.IndexOf("Fun:");
                       func =ushort.Parse(matched[index + "Fun:".Length].ToString());
                   }
                   if (matched.Contains("Addr:"))
                   {
                       var regexAddress = @"Addr:(.*?) ";
                       address =ushort.Parse(Regex.Match(matched,regexAddress).Groups[1].Value);
                   }
                   if (matched.Contains("Num:"))
                   {
                       var regexNum = @"Num:(.*?) ";
                       var x = Regex.Match(line, regexNum).Groups[1].Value.Replace("]","");
                       numberofPoints =ushort.Parse(x);
                   }
                   if (matched.Contains("Data:"))
                   {
                       var regexData = @"Data:(.*?)";
                       var st = matched.Substring(matched.IndexOf("Data:") + "Data:".Length);
                       var strData = st.Split(' ');
                       data = strData.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => ushort.Parse(s)).ToList();
                   }

                   queryMockDefinitions.Add(new QueryMockDefinition(address,func,numberofPoints,data.ToArray()));
               }
               // Process line
           }
           
            Program.RefreshProject();
        }
    }
}