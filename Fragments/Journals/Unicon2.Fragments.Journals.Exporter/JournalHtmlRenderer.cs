using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unicon2.Fragments.Journals.Exporter.Utils;
using Unicon2.Fragments.Journals.Infrastructure.Export;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Fragments.Journals.Exporter
{
	public class JournalHtmlRenderer : IHtmlRenderer<IUniconJournalViewModel, JournalExportSelector>
    {
	    private readonly JournalRecordRenderer _journalRecordRenderer;
	    private readonly ILocalizerService _localizerService;

        public JournalHtmlRenderer(JournalRecordRenderer journalRecordRenderer, ILocalizerService localizerService)
        {
	        _journalRecordRenderer = journalRecordRenderer;
	        _localizerService = localizerService;
        }

        public async Task<string> RenderHtmlString(IUniconJournalViewModel journalViewModel,
	        JournalExportSelector configurationExportSelector)
        {
            string main =
                "<!DOCTYPE html>\n <html> " +
                "\n<style type=\"text/css\" media=\"all\" >" +
                "\nbody { font-size: small }" +
                "\n.rootItem {border: 0; background-color:#999 !important;" +
                "\n-webkit-print-color-adjust: exact;" +
                "\ncolor-adjust: exact;}" +
                "\ntr:nth-of-type(odd) {background-color:#ccc;" +
                "\n-webkit-print-color-adjust: exact;" +
                "\ncolor-adjust: exact;}" +
                "\ntd {border: 0}" +
                "\n</style> " +
                "\n<head>\n <meta charset =\"utf-8\"/>\n<title> HTML Document </title>\n</head>";
            await Task.Run((() =>
            {
                TagBuilder body = new TagBuilder("body");

                TagBuilder table = new TagBuilder("table");
                table.MergeAttribute("border", "1");
                table.AddTagToInnerHtml(CreateHeaderRows(journalViewModel));

                int numberOfRecord = 1;
                foreach (var record in journalViewModel.Table.Values)
                {

	                _journalRecordRenderer.RenderRecord(numberOfRecord, record)
                        .OnNotEmpty(list => list
                            .ForEach(builder => table
                                .AddTagToInnerHtml(builder)));
	                numberOfRecord++;
                }

                body.AddTagToInnerHtml(table);
                main += body;

                main += ("\n</html>");

            }));


            return main;
        }

        private TagBuilder CreateHeaderRows(IUniconJournalViewModel journalViewModel)
        {
            TagBuilder tableRowForHeaders = new TagBuilder("tr");
            TagBuilder numTableHeader = new TagBuilder("th");
            numTableHeader.AddToInnerHtml("№");
            tableRowForHeaders.AddTagToInnerHtml(numTableHeader);
            foreach (var columnName in journalViewModel.Table.ColumnNamesStrings)
            {
	            TagBuilder nameTableHeader = new TagBuilder("th");
	            nameTableHeader.AddToInnerHtml(columnName);
	            tableRowForHeaders.AddTagToInnerHtml(nameTableHeader);
            }

            return tableRowForHeaders;
        }
    }
}