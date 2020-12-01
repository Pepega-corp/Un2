using Microsoft.EntityFrameworkCore;
using Unicon.Backend.WebApi.Context.Maps;
using Unicon.Backend.WebApi.Entities;

namespace Unicon.Backend.WebApi.Context
{
	public class ApplicationContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Tag> Tags { get; set; }
		public DbSet<DeviceDefinition> DeviceDefinitions { get; set; }
		public DbSet<CommandsStoreEntry> CommandsStoreEntries { get; set; }
		public DbSet<UpdateDeviceDefinitionCommand> UpdateDeviceDefinitionCommands { get; set; }


		public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			new UserMap(modelBuilder.Entity<User>());
			new TagMap(modelBuilder.Entity<Tag>());
			new DeviceDefinitionMap(modelBuilder.Entity<DeviceDefinition>());
			new UpdateDeviceDefinitionCommandMap(modelBuilder.Entity<UpdateDeviceDefinitionCommand>());
			new CommandsStoreEntryMap(modelBuilder.Entity<CommandsStoreEntry>());
			base.OnModelCreating(modelBuilder);
		}
	}
}