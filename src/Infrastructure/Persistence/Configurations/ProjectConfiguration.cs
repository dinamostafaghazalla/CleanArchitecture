using CleanArchitecture.Domain.Entities.Projects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.Property(e => e.Name).IsRequired().HasMaxLength(255);

            builder.Property(e => e.Key).IsRequired().HasMaxLength(4);

            builder.HasOne(c => c.Owner).WithMany(x => x.OwnProjects).HasForeignKey(x => x.OwnerId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.Participants).WithMany(x => x.Projects);

        }
    }
}