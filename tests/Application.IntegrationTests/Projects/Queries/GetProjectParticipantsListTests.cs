using CleanArchitecture.Application.Projects.Queries.GetProjectParticipantsList;
using CleanArchitecture.Domain.Entities.Participants;
using CleanArchitecture.Domain.Entities.Projects;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.IntegrationTests.Projects.Queries
{
    using static Testing;

    public class GetProjectParticipantsListTests : TestBase
    {
        [Test]
        public async Task ShouldReturnProjectParticipants()
        {
            // Arange
            var userId = await RunAsDefaultUserAsync();

            var owner = new Participant( userId, "firstName", "lastName");

            var project = new Project(owner, "Project Name", "Key");

            await AddAsync(project);

            var query = new GetProjectParticipantsListQuery { ProjectId = project.Id };
            // Act
            var result = await SendAsync(query);

            // Assert
            result.Participants.Should().NotBeEmpty();
            result.Participants.Should().HaveCount(project.Participants.Count);
            result.Participants.First().Name.Should().Be($"{owner.FirstName} {owner.LastName}");
            result.Participants.First().Id.Should().Be(owner.Id);
            result.Participants.First().Title.Should().Be(owner.Title);
        }
    }
}