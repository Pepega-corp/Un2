using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unicon.Backend.WebApi.Entities;

namespace Unicon.Backend.WebApi.Context.Maps
{
	public class UserMap
	{
		public UserMap(EntityTypeBuilder<User> entityBuilder)
		{
			entityBuilder.HasKey(x => x.Id);
			entityBuilder.ToTable("Users");
			entityBuilder.Property(x => x.Id).HasColumnName("id");
		}
    }
}
