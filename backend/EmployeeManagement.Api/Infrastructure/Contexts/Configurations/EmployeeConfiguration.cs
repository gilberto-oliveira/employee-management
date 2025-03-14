using EmployeeManagement.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeManagement.Api.Infrastructure.Contexts.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();
        builder.Property(e => e.FirstName).HasMaxLength(60);
        builder.Property(e => e.LastName).HasMaxLength(60);
        builder.Property(e => e.DocNumber).HasMaxLength(20);
        builder.Property(e => e.DateOfBirth);

        builder.HasIndex(u => u.DocNumber).IsUnique();

        builder
            .HasOne(e => e.Manager)
            .WithMany()
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasData(
            Employee.Create("Abigobaldo","Magalhães","1234567891","abigo@email.com", new List<string> {"+559191234456"}, "Test!@123456", "Leader")
        );
    }
}