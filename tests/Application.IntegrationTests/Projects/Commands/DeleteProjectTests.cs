using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Projects.Commands.CreateProject;
using CleanArchitecture.Application.Projects.Commands.DeleteProject;
using CleanArchitecture.Domain.Entities.Projects;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Participants;
using Moq;

namespace CleanArchitecture.Application.IntegrationTests.Projects.Commands
{
    using static Testing;

    public class DeleteProjectTests : TestBase
    {
        [Test]
        public void ShouldRequireValidProjectId()
        {
            var command = new DeleteProjectCommand { Id = 99 };

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<NotFoundException>();
        }

        [Test]
        public async Task ShouldDeleteProject()
        {
            var userId = await RunAsDefaultUserAsync();

            Mock.Of<ICurrentUserService>(s => s.UserId == userId);

            var participant = new Participant(userId, "firstName", "lastName");

            await AddAsync(participant);

            var id = await SendAsync(new CreateProjectCommand
            {
                Name = "New Project",
                Key = "key"
            });

            await SendAsync(new DeleteProjectCommand
            {
                Id = id
            });

            var project = await FindAsync<Project>(id);

            project.Should().BeNull();
        }
    }
}