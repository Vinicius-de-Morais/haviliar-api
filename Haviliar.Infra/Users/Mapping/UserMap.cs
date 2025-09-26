using Haviliar.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haviliar.Infra.Users.Mapping
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.UserId);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Document)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(u => u.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(u => u.Phone)
                .IsRequired()
                .HasMaxLength(15);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.DateOfBirth)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(u => u.UpdatedAt);

            builder.Property(u => u.UserType)
                .IsRequired();

        }
    }
}
