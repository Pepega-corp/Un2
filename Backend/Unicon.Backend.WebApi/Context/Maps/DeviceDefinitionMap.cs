using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unicon.Backend.WebApi.Entities;

namespace Unicon.Backend.WebApi.Context.Maps
{
	public class DeviceDefinitionMap
	{
		public DeviceDefinitionMap(EntityTypeBuilder<DeviceDefinition> entityBuilder)
		{
			entityBuilder.HasKey(x => x.Id);
			entityBuilder.ToTable("DeviceDefinitions");
			entityBuilder.HasMany(user => user.Tags);
		}
	}
}
