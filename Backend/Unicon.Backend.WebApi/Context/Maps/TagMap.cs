using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unicon.Backend.WebApi.Entities;

namespace Unicon.Backend.WebApi.Context.Maps
{
	public class TagMap
	{
		public TagMap(EntityTypeBuilder<Tag> entityBuilder)
		{
			entityBuilder.HasKey(x => x.Id);
			entityBuilder.ToTable("Tags");
			entityBuilder.HasMany(user => user.RelatedDeviceDefinitions);
		}
	}
}
