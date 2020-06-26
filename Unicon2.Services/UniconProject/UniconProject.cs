using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.UniconProject;

namespace Unicon2.Services.UniconProject
{
	[JsonObject(MemberSerialization.OptIn)]
	public class UniconProject : IUniconProject
	{
		private readonly string DefaultProjectName = "DefaultProject";

		public UniconProject()
		{
			RefreshName();
		}

		private void RefreshName()
		{
			Name = DefaultProjectName;
			ProjectPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Name);

			if (ProjectPath != null && !Directory.Exists(Path.Combine(ProjectPath, Name)))
			{
				Directory.CreateDirectory(Path.Combine(ProjectPath, Name));
			}


		}

		public void Dispose()
		{
			ConnectableItems?.Clear();
			RefreshName();
		}

		[JsonProperty] public List<IConnectable> ConnectableItems { get; set; }

		public bool IsProjectSaved
		{
			get
			{
				if ((ProjectPath != null) && (Name != null) && Name != DefaultProjectName)
				{
					if (File.Exists(ProjectPath + "\\" + Name + ".uniproj")) return true;
				}

				return false;
			}

		}





		[JsonProperty] public string Name { get; set; }
		[JsonProperty] public string ProjectPath { get; set; }
		[JsonProperty] public string LayoutString { get; set; }
	}
}