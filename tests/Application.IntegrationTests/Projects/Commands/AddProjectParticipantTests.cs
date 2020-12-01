using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Projects.Commands.AddProjectParticipant;
using CleanArchitecture.Domain.Entities.Participants;
using CleanArchitecture.Domain.Entities.Projects;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application.IntegrationTests.Projects.Commands
{
    using static Testing;

    public class AddProjectParticipantTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new AddProjectParticipantCommand();

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldAddProjectParticipant()
        {
            var participantUserId = await RunAsUserAsync("participant@local", "Testing1234!", new string[] { });
            var participant = new Participant(participantUserId, "participant firstName", "participant lastName");

            var ownerUserId = await RunAsUserAsync("owner@local", "Testing1234!", new string[] { });
            var owner = new Participant(ownerUserId, "owner firstName", "owner lastName");

            var entity = new Project(owner, "Project Name", "Key");
            await AddAsync(entity);

            await AddAsync(participant);

            var command = new AddProjectParticipantCommand
            {
                ProjectId = entity.Id,
                ParticipantEmail = "participant@local"
            };

            var id = await SendAsync(command);

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var project = await context.Projects.Include(x => x.Participants).SingleOrDefaultAsync(x => x.Id == entity.Id);
            project.Should().NotBeNull();
            project.Participants.First().FirstName.Should().Be(owner.FirstName);
            project.Participants.First().LastName.Should().Be(owner.LastName);
            project.Participants.Last().FirstName.Should().Be(participant.FirstName);
            project.Participants.Last().LastName.Should().Be(participant.LastName);
            project.CreatedBy.Should().Be(ownerUserId);
            project.Created.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}