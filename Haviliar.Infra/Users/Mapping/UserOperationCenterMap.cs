using Haviliar.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haviliar.Infra.Users.Mapping;

public class UserOperationCenterMap : IEntityTypeConfiguration<UserOperationCenter>
{
    public void Configure(EntityTypeBuilder<UserOperationCenter> builder)
    {
        builder.ToTable("UserOperationCenters");

        builder.HasKey(uoc => uoc.UserOperationCenterId);

        builder.HasOne(uoc => uoc.User)
            .WithMany(u => u.UserOperationCenters)
            .HasForeignKey(uoc => uoc.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(uoc => uoc.OperationCenter)
            .WithMany(oc => oc.UserOperationCenters)
            .HasForeignKey(uoc => uoc.OperationCenterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
