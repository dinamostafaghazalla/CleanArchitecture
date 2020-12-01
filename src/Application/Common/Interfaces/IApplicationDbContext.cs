using CleanArchitecture.Domain.Entities.Issues;
using CleanArchitecture.Domain.Entities.Participants;
using CleanArchitecture.Domain.Entities.Projects;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Participant> Participants { get; set; }

        DbSet<Project> Projects { get; set; }

        DbSet<Issue> Issues { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}