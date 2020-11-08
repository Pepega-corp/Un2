using System.Collections.Generic;
using Unicon.Common.Interfaces;
using Unicon.Common.Model;

namespace Unicon.Common.Commands
{
	public class UploadSnapshotCommand : ICommand
	{
		public UploadSnapshotCommand(List<CommandRecord> commandRecords)
		{
			CommandRecords = commandRecords;
		}

		public List<CommandRecord> CommandRecords { get; }
	}
}
