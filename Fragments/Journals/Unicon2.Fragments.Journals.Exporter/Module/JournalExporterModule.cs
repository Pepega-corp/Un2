using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Export;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Exporter.Module
{
	public class JournalExporterModule: IUnityModule
	{
		public void Initialize(ITypesContainer container)
		{
			container.Register<IHtmlRenderer<IUniconJournalViewModel, JournalExportSelector>, JournalHtmlRenderer>();
			container.Register<JournalRecordRenderer>();
		}
	}
}
