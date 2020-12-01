using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Projects.Queries.GetProjectIssuesList;
using CleanArchitecture.Domain.Entities.Issues;
using CleanArchitecture.Domain.Entities.Participants;
using CleanArchitecture.Domain.Entities.Projects;
using FluentAssertions;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.IntegrationTests.Projects.Queries
{
    using static Testing;

    public class GetProjectIssuesListTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var query = new GetProjectIssuesListQuery();

            FluentActions.Invoking(() => SendAsync(query)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldReturnProjectIssuesFilterByProjectId()
        {
            // Arange
            var userId = await RunAsDefaultUserAsync();

            var owner = new Participant(userId, "firstName", "lastName");

            var project = new Project(owner, "Project Name", "Key");

            var issue = new Issue(project, owner, IssueType.Story, "title");
            
            issue.SetAssignee(owner);

            project.AddIssue(issue);

            await AddAsync(project);

            var query = new GetProjectIssuesListQuery { ProjectId = project.Id };
            // Act
            var result = await SendAsync(query);

            // Assert
            result.Issues.Should().HaveCount(1);
            result.Issues.First().Reporter.Should().Be($"{issue.Reporter.FirstName} {issue.Reporter.LastName}");
            result.Issues.First().Assignee.Should().Be($"{issue.Assignee.FirstName} {issue.Assignee.LastName}");
            result.Issues.First().Title.Should().Be(issue.Title);
            result.Issues.First().Description.Should().Be(issue.Description);
            result.Issues.First().Status.Should().Be(issue.Status);
            result.Issues.First().IssueId.Should().Be(issue.IssueId);
            result.Issues.First().Type.Should().Be(issue.Type);
        }

        [Test]
        public async Task ShouldReturnProjectIssuesFilterByAssignee()
        {
            // Arange
            var userId = await RunAsDefaultUserAsync();

            var owner = new Participant(userId, "firstName", "lastName");

            var project = new Project(owner, "Project Name", "Key");

            var issue = new Issue(project, owner, IssueType.Story, "title");

            issue.SetAssignee(owner);

            issue.SetAssignee(owner);

            project.AddIssue(issue);

            await AddAsync(project);

            var query = new GetProjectIssuesListQuery { Assignee = owner.FirstName, ProjectId = project.Id };
            // Act
            var result = await SendAsync(query);

            // Assert
            result.Issues.Should().NotBeEmpty();
            result.Issues.Should().HaveCount(1);
            result.Issues.First().Reporter.Should().Be($"{issue.Reporter.FirstName} {issue.Reporter.LastName}");
            result.Issues.First().Assignee.Should().Be($"{issue.Assignee.FirstName} {issue.Assignee.LastName}");
            result.Issues.First().Title.Should().Be(issue.Title);
            result.Issues.First().Description.Should().Be(issue.Description);
            result.Issues.First().Status.Should().Be(issue.Status);
            result.Issues.First().IssueId.Should().Be(issue.IssueId);
            result.Issues.First().Type.Should().Be(issue.Type);
        }

        [Test]
        public async Task ShouldReturnProjectIssuesFilterByIssueType()
        {
            // Arange
            var userId = await RunAsDefaultUserAsync();

            var owner = new Participant(userId, "firstName", "lastName");
            
            var project = new Project(owner, "Project Name", "Key");
            
            var issue = new Issue(project, owner, IssueType.Story, "title");
            
            issue.SetAssignee(owner);

            project.AddIssue(issue);

            await AddAsync(project);

            var query = new GetProjectIssuesListQuery { IssueType = issue.Type, ProjectId = project.Id };
            // Act
            var result = await SendAsync(query);

            // Assert
            result.Issues.Should().HaveCount(1);
            result.Issues.First().Reporter.Should().Be($"{issue.Reporter.FirstName} {issue.Reporter.LastName}");
            result.Issues.First().Assignee.Should().Be($"{issue.Assignee.FirstName} {issue.Assignee.LastName}");
            result.Issues.First().Title.Should().Be(issue.Title);
            result.Issues.First().Description.Should().Be(issue.Description);
            result.Issues.First().Status.Should().Be(issue.Status);
            result.Issues.First().IssueId.Should().Be(issue.IssueId);
            result.Issues.First().Type.Should().Be(issue.Type);
        }

        [Test]
        public async Task ShouldReturnProjectIssuesFilterByTitle()
        {
            // Arange
            var userId = await RunAsDefaultUserAsync();

            var owner = new Participant(userId, "firstName", "lastName");

            var project = new Project(owner, "Project Name", "Key");

            var issue = new Issue(project, owner, IssueType.Story, "title");

            issue.SetAssignee(owner);

            project.AddIssue(issue);

            await AddAsync(project);

            var query = new GetProjectIssuesListQuery { Title = issue.Title, ProjectId = project.Id };
            // Act
            var result = await SendAsync(query);

            // Assert
            result.Issues.Should().HaveCount(1);
            result.Issues.First().Reporter.Should().Be($"{issue.Reporter.FirstName} {issue.Reporter.LastName}");
            result.Issues.First().Assignee.Should().Be($"{issue.Assignee.FirstName} {issue.Assignee.LastName}");
            result.Issues.First().Title.Should().Be(issue.Title);
            result.Issues.First().Description.Should().Be(issue.Description);
            result.Issues.First().Status.Should().Be(issue.Status);
            result.Issues.First().IssueId.Should().Be(issue.IssueId);
            result.Issues.First().Type.Should().Be(issue.Type);
        }
    }
}