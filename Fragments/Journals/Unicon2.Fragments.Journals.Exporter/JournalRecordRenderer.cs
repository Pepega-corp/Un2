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
		public Maybe<List<TagBuilder>> RenderRecord(List<IFormattedValueViewModel> valueViewModels)
		{
			TagBuilder tableRowForItems = new TagBuilder("tr");
			valueViewModels.ForEach(model =>
			{
				TagBuilder tag = new TagBuilder("td");
				tag.AddToInnerHtml(model.AsString());
				tableRowForItems.AddTagToInnerHtml(tag);
			} );
			return new Maybe<List<TagBuilder>>();
		}
	}
}
