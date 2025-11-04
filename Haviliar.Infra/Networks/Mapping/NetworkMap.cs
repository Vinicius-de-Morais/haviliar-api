using Haviliar.Domain.Networks.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haviliar.Infra.Networks.Mapping;

public class NetworkMap : IEntityTypeConfiguration<Network>
{
    public void Configure(EntityTypeBuilder<Network> builder)
    {
        builder.ToTable("Networks");

        builder.HasKey(n => n.NetworkId);

        builder.Property(n => n.NetworkName)
            .IsRequired()
            .HasMaxLength(300);

        builder.HasOne(n => n.OperationCenter)
            .WithMany(op => op.Networks)
            .HasForeignKey(n => n.OperationCenterId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
