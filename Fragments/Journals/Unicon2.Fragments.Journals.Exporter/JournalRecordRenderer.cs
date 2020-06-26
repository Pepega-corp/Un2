using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unicon2.Fragments.Journals.Exporter.Utils;
using Unicon2.Infrastructure.Common;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;

namespace Unicon2.Fragments.Journals.Exporter
{
	public class JournalRecordRenderer
	{
		public Maybe<List<TagBuilder>> RenderRecord(int numberOfRecord, List<IFormattedValueViewModel> valueViewModels)
		{
			TagBuilder tableRowForItems = new TagBuilder("tr");
			TagBuilder numberTag = new TagBuilder("td");
			numberTag.AddToInnerHtml(numberOfRecord.ToString());
			tableRowForItems.AddTagToInnerHtml(numberTag);
			valueViewModels.ForEach(model =>
			{
				TagBuilder tag = new TagBuilder("td");
				tag.AddToInnerHtml(model.AsString());
				tableRowForItems.AddTagToInnerHtml(tag);
			} );
			return Maybe<List<TagBuilder>>.FromValue(new List<TagBuilder>(){ tableRowForItems });
		}
	}
}
