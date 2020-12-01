using CleanArchitecture.Domain.Entities.Participants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class ParticipantConfiguration : IEntityTypeConfiguration<Participant>
    {
        public void Configure(EntityTypeBuilder<Participant> builder)
        {
            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);


            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);


            builder.Property(e => e.Title).HasMaxLength(255);

            builder.HasMany(c => c.OwnIssues).WithOne(x=>x.Reporter).HasForeignKey(x=>x.ReporterId).OnDelete(DeleteBehavior.NoAction);
            
            builder.HasMany(c => c.AssignedIssues).WithOne(x=>x.Assignee).HasForeignKey(x=>x.AssigneeId).OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.OwnProjects).WithOne(x => x.Owner);

            builder.HasMany(c => c.Projects).WithMany(x => x.Participants);

        }
    }
}