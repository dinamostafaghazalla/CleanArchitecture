using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Projects.Commands.CreateProject;
using CleanArchitecture.Application.Projects.Commands.UpdateProject;
using CleanArchitecture.Domain.Entities.Projects;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Participants;
using Moq;

namespace CleanArchitecture.Application.IntegrationTests.Projects.Commands
{
    using static Testing;

    public class UpdateProjectTests : TestBase
    {
        [Test]
        public void ShouldRequireValidProjectId()
        {
            var command = new UpdateProjectCommand
            {
                Id = 99,
                Name = "New Name"
            };

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldUpdateProject()
        {
            var userId = await RunAsDefaultUserAsync();

            var owner = new Participant(userId, "firstName", "lastName");

            var entity = new Project(owner, "Project Name", "Key");

            await AddAsync(entity);

            var command = new UpdateProjectCommand
            {
                Id = entity.Id,
                Name = "Updated Project Name"
            };

            await SendAsync(command);

            var project = await FindAsync<Project>(entity.Id);

            project.Name.Should().Be(command.Name);
            project.LastModifiedBy.Should().NotBeNull();
            project.LastModifiedBy.Should().Be(userId);
            project.LastModified.Should().NotBeNull();
            project.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
        }
    }
}