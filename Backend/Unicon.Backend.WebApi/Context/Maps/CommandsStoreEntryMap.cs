using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unicon.Backend.WebApi.Entities;

namespace Unicon.Backend.WebApi.Context.Maps
{
    public class CommandsStoreEntryMap
    {
        public CommandsStoreEntryMap(EntityTypeBuilder<CommandsStoreEntry> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.ToTable("CommandsStoreEntries");
        }
    }
}