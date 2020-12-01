using CleanArchitecture.Domain.Entities.Issues;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class IssueConfiguration : IEntityTypeConfiguration<Issue>
    {
        public void Configure(EntityTypeBuilder<Issue> builder)
        {
            builder.Ignore(e => e.DomainEvents);

            builder.HasOne(c => c.Reporter).WithMany(x => x.OwnIssues).HasForeignKey(x => x.ReporterId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Project).WithMany(x => x.Issues).HasForeignKey(x => x.ProjectId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Assignee).WithMany(x => x.AssignedIssues).HasForeignKey(x=>x.AssigneeId).OnDelete(DeleteBehavior.NoAction);

            builder.Property(e => e.Title).IsRequired().HasMaxLength(255);

            builder.Property(e => e.IssueId).IsRequired().HasMaxLength(40);
        }
    }
}