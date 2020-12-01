using System;
using System.Linq;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Projects.Commands.CreateProject;
using CleanArchitecture.Application.Projects.Commands.RemoveProjectParticipant;
using CleanArchitecture.Domain.Entities.Projects;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using CleanArchitecture.Application.Projects.Commands.AddProjectParticipant;
using CleanArchitecture.Domain.Entities.Participants;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application.IntegrationTests.Projects.Commands
{
    using static Testing;

    public class RemoveProjectParticipantTests : TestBase
    {
        [Test]
        public void ShouldRequireValidProjectId()
        {
            var command = new RemoveProjectParticipantCommand { ProjectId = 99, ParticipantEmail = "test@local" };

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldRemoveProjectParticipant()
        {
            const string participantEmail = "participant@local";
            var participantUserId = await RunAsUserAsync(participantEmail, "Testing1234!", new string[] { });
            var participant = new Participant(participantUserId, "participant firstName", "participant lastName");

            var ownerUserId = await RunAsUserAsync("owner@local", "Testing1234!", new string[] { });
            var owner = new Participant(ownerUserId, "owner firstName", "owner lastName");

            var entity = new Project(owner, "Project Name", "Key");
            await AddAsync(entity);

            await AddAsync(participant);

            var command = new AddProjectParticipantCommand
            {
                ProjectId = entity.Id,
                ParticipantEmail = participantEmail
            };

            var id = await SendAsync(command);

            await SendAsync(new RemoveProjectParticipantCommand
            {
                ProjectId = entity.Id, ParticipantEmail = participantEmail
            });

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var project = await context.Projects.Include(x => x.Participants).SingleOrDefaultAsync(x => x.Id == entity.Id);
            project.Should().NotBeNull();
            project.Participants.Should().NotBeEmpty().And.HaveCount(1);
            project.Participants.First().FirstName.Should().Be(owner.FirstName);
            project.Participants.First().LastName.Should().Be(owner.LastName);
            project.CreatedBy.Should().Be(ownerUserId);
            project.Created.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}