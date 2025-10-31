using Haviliar.Domain.OperationCenters.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Haviliar.Infra.OperationCenters.Mapping;

public class OperationCenterMap : IEntityTypeConfiguration<OperationCenter>
{
    public void Configure(EntityTypeBuilder<OperationCenter> builder)
    {
        builder.ToTable("OperationCenters");

        builder.HasKey(oc => oc.OperationCenterId);

        builder.Property(oc => oc.Name)
            .IsRequired();

        builder.Property(oc => oc.IsActive)
            .IsRequired();
    }
}
