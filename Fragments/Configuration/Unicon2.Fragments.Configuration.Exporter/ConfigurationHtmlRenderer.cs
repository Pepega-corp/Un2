using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Exporter.Utils;
using Unicon2.Fragments.Configuration.Infrastructure.Export;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Infrastructure.Services;


namespace Unicon2.Fragments.Configuration.Exporter
{
    public class ConfigurationHtmlRenderer : IHtmlRenderer<IDeviceConfiguration, List<SelectorForItemsGroup>>
    {
        private readonly IItemRendererFactory _itemRendererFactory;
        private readonly ILocalizerService _localizerService;

        public ConfigurationHtmlRenderer(IItemRendererFactory itemRendererFactory, ILocalizerService localizerService)
        {
            _itemRendererFactory = itemRendererFactory;
            _localizerService = localizerService;
        }

        #region Implementation of IHtmlRenderer<IDeviceConfiguration>

        public async Task<string> RenderHtmlString(IDeviceConfiguration deviceConfiguration,
            List<SelectorForItemsGroup> selectorsForItemsGroup)
        {
            string main =
                "<!DOCTYPE html>\n <html> " +
                "\n<style type=\"text/css\">  " +
                "\n.rootItem {border: 0; background-color:#999;}" +
                "\ntr:nth-of-type(odd) {background-color:#ccc;}" +
                "\n</style> " +
                "\n<head>\n <meta charset =\"utf-8\"/>\n<title> HTML Document </title>\n</head>";
            await Task.Run((() =>
            {
                TagBuilder body = new TagBuilder("body");

                TagBuilder table = new TagBuilder("table");
                table.MergeAttribute("border", "1");
                table.AddTagToInnerHtml(CreateHeaderRows());


                foreach (var rootConfigurationItem in deviceConfiguration.RootConfigurationItemList)
                {
                    _itemRendererFactory.GetConfigurationItemRenderer(rootConfigurationItem)
                        .RenderHtmlFromItem(rootConfigurationItem,
                            selectorsForItemsGroup.FirstOrDefault(
                                group => group.RelatedItemsGroup == rootConfigurationItem))
                        .OnNotEmpty(list => list
                            .ForEach(builder => table
                                .AddTagToInnerHtml(builder)));
                }

                body.AddTagToInnerHtml(table);
                main += body;

                main += ("\n</html>");

            }));


            return main;
        }

        private TagBuilder CreateHeaderRows()
        {
            TagBuilder tableRowForHeaders = new TagBuilder("tr");

            TagBuilder nameTableHeader = new TagBuilder("th");
            nameTableHeader.AddToInnerHtml(_localizerService.GetLocalizedString("Name"));

            TagBuilder deviceDataTableHeader = new TagBuilder("th");
            deviceDataTableHeader.AddToInnerHtml(_localizerService.GetLocalizedString("DeviceData"));

            TagBuilder localDataTableHeader = new TagBuilder("th");
            localDataTableHeader.AddToInnerHtml(_localizerService.GetLocalizedString("LocalData"));

            TagBuilder measureUnitTableHeader = new TagBuilder("th");
            measureUnitTableHeader.AddToInnerHtml(_localizerService.GetLocalizedString("MeasureUnit"));

            TagBuilder rangeTableHeader = new TagBuilder("th");
            rangeTableHeader.AddToInnerHtml(_localizerService.GetLocalizedString("Range"));

            tableRowForHeaders.AddTagToInnerHtml(nameTableHeader);
            tableRowForHeaders.AddTagToInnerHtml(deviceDataTableHeader);
            tableRowForHeaders.AddTagToInnerHtml(localDataTableHeader);
            tableRowForHeaders.AddTagToInnerHtml(measureUnitTableHeader);
            tableRowForHeaders.AddTagToInnerHtml(rangeTableHeader);
            return tableRowForHeaders;
        }

        #endregion
    }
}