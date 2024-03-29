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
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u=>u.Id).ValueGeneratedOnAdd();
            builder.Property(u=>u.FirstName).IsRequired();
            builder.Property(u=>u.FirstName).HasMaxLength(50);
            builder.Property(u => u.LastName).IsRequired();
            builder.Property(u => u.LastName).HasMaxLength(50);
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.Email).HasMaxLength(100);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.Property(u => u.UserName).IsRequired();
            builder.Property(u => u.UserName).HasMaxLength(50);
            builder.HasIndex(u=>u.UserName).IsUnique();
            builder.Property(u => u.PasswordHash).IsRequired();
            builder.Property(u => u.PasswordHash).HasColumnType("VARBINARY(500)");
            builder.Property(u => u.Description).HasMaxLength(500);
            builder.Property(u => u.Picture).IsRequired();
            builder.Property(u => u.Picture).HasMaxLength(250);
            builder.Property(u => u.CreatedByName).IsRequired();
            builder.Property(u => u.CreatedByName).HasMaxLength(50);
            builder.Property(u => u.ModifiedByName).IsRequired();
            builder.Property(u => u.ModifiedByName).HasMaxLength(50);
            builder.Property(u => u.ModifiedDate).IsRequired();
            builder.Property(u => u.CreatedDate).IsRequired();
            builder.Property(u => u.IsActive).IsRequired();
            builder.Property(u => u.IsDeleted).IsRequired();
            builder.Property(u => u.Note).HasMaxLength(500);
            builder.HasOne<Role>(u => u.Role).WithMany(r => r.Users).
               HasForeignKey(u => u.RoleId);

            builder.ToTable("Users");

            builder.HasData(new User
            {
                Id = 1,
                RoleId = 1,
                FirstName ="Kadir",
                LastName ="Eğriboyun",
                UserName = "Kean",
                Email ="kadir@gmail.com",
                IsActive = true,
                IsDeleted = false,
                CreatedByName ="InitialCreate",
                CreatedDate = DateTime.Now,
                ModifiedByName ="InitialCreate",
                ModifiedDate = DateTime.Now,    
                Description ="İlk admin kullanıcı",
                Note ="Admin Kullanıcısı",
                PasswordHash =Encoding.ASCII.GetBytes("f379eaf3c831b04de153469d1bec345e"),//666666
                Picture= "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcSX4wVGjMQ37PaO4PdUVEAliSLi8-c2gJ1zvQ&usqp=CAU",
           });
        }
    }
}
