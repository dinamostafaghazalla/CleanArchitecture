using CleanArchitecture.Domain.Entities.Issues;
using CleanArchitecture.Domain.Entities.Participants;
using CleanArchitecture.Domain.Entities.Projects;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Application.Projects.Queries.GetProjects;

namespace CleanArchitecture.Application.IntegrationTests.Projects.Queries
{
    using static Testing;

    public class GetProjectsTests : TestBase
    {
        [Test]
        public async Task ShouldReturnAllProjectssAndIssues()
        {
            // Arrange
            var userId = await RunAsDefaultUserAsync();

            var owner = new Participant(userId, "firstName", "lastName");

            var project = new Project(owner, "Project Name", "Key");

            project.AddIssue(new Issue(project, owner, IssueType.Story, "title"));

            await AddAsync(project);

            var query = new GetProjectsQuery();

            // Act
            var result = await SendAsync(query);

            // Assert
            result.Projects.Should().HaveCount(1);
            result.Projects.First().Key.Should().Be(project.Key);
            result.Projects.First().Name.Should().Be(project.Name);
            result.Projects.First().Owner.Should().Be($"{project.Owner.FirstName} {project.Owner.LastName}");
            result.Projects.First().Issues.Should().HaveCount(project.Issues.Count);
            result.Projects.First().Participants.Should().HaveCount(project.Participants.Count);
        }
    }
}