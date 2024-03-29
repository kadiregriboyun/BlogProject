﻿using BlogProject.Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Data.Concrete.EntityFramework.Mappings
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Name).ValueGeneratedOnAdd();
            builder.Property(r => r.Name).HasMaxLength(50);
            builder.Property(r => r.Description).IsRequired();
            builder.Property(r=>r.Description).HasMaxLength(500);
            builder.Property(r => r.CreatedByName).IsRequired();
            builder.Property(r => r.CreatedByName).HasMaxLength(50);
            builder.Property(r => r.ModifiedByName).IsRequired();
            builder.Property(r => r.ModifiedByName).HasMaxLength(50);
            builder.Property(r => r.ModifiedDate).IsRequired();
            builder.Property(r => r.CreatedDate).IsRequired();
            builder.Property(r => r.IsActive).IsRequired();
            builder.Property(r => r.IsDeleted).IsRequired();
            builder.Property(r => r.Note).HasMaxLength(500);

            builder.ToTable("Roles");

            builder.HasData(new Role
            {
                Id = 1,
                Name ="Admin",
                Description ="Yönetici rolüdür,tüm haklara sahiptir",
                IsActive = true,
                IsDeleted = false,
                CreatedByName ="InitialCreate",
                CreatedDate = DateTime.Now,
                ModifiedByName ="InitialCreate",
                ModifiedDate = DateTime.Now,
                Note ="Admin Rolüdür",
            });

        }
    }
}
