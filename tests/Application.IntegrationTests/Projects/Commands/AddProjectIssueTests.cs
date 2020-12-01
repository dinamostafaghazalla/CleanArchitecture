using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Issues.Commands.CreateIssue;
using CleanArchitecture.Domain.Entities.Issues;
using CleanArchitecture.Domain.Entities.Participants;
using CleanArchitecture.Domain.Entities.Projects;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Infrastructure.Persistence;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.IntegrationTests.Projects.Commands
{
    using static Testing;

    public class AddProjectIssueTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateIssueCommand();

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldAddProjectIssue()
        {
            var participantUserId = await RunAsUserAsync("participant@local", "Testing1234!", new string[] { });
            var participant = new Participant(participantUserId, "participant firstName", "participant lastName");

            var ownerUserId = await RunAsUserAsync("owner@local", "Testing1234!", new string[] { });
            var owner = new Participant(ownerUserId, "owner firstName", "owner lastName");
            //await AddAsync(participant);
            var entity = new Project(owner, "Project Name", "Key");
            entity.AddParticipant(participant);

            await AddAsync(entity);


            var command = new CreateIssueCommand
            {
                ProjectId = entity.Id,
                IssueType = IssueType.Task,
                Description = "Description",
                Title = "Title",
                Status = IssueStatus.Todo,
                AssigneeId = participant.Id
            };

            var id = await SendAsync(command);

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var project = await context.Projects
                .Include(x => x.Participants)
                .Include(x => x.Issues)
                .SingleOrDefaultAsync(x => x.Id == entity.Id);

            project.Should().NotBeNull();
            project.Participants.Last().Id.Should().Be(participant.Id);
            project.Issues.First().Description.Should().Be(command.Description);
            project.Issues.First().Title.Should().Be(command.Title);
            project.Issues.First().Type.Should().Be(command.IssueType);
            project.Issues.First().Status.Should().Be(command.Status);
            project.CreatedBy.Should().Be(ownerUserId);
            project.Created.Should().BeCloseTo(DateTime.Now, 20000);
        }
    }
}