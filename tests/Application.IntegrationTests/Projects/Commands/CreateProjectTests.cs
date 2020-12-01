using System;
using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Projects.Commands.CreateProject;
using CleanArchitecture.Domain.Entities.Projects;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities.Participants;
using CleanArchitecture.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CleanArchitecture.Application.IntegrationTests.Projects.Commands
{
    using static Testing;

    public class CreateProjectTests : TestBase
    {
        [Test]
        public void ShouldRequireMinimumFields()
        {
            var command = new CreateProjectCommand();

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>();
        }

        [Test]
        public async Task ShouldRequireUniqueKey()
        {
            var userId = await RunAsDefaultUserAsync();
            
            Mock.Of<ICurrentUserService>(s => s.UserId == userId);

            var participant = new Participant(userId, "firstName", "lastName");

            await AddAsync(participant);

            await SendAsync(new CreateProjectCommand
            {
                Name = "Project",
                Key = "Key"
            });

            var command = new CreateProjectCommand
            {
                Name = "Project",
                Key = "Key"
            };

            FluentActions.Invoking(() => SendAsync(command)).Should().Throw<ValidationException>().Which.Errors["Key"].Should().BeEquivalentTo("The specified key already exists.");
        }

        [Test]
        public async Task ShouldCreateProject()
        {
            var userId = await RunAsDefaultUserAsync();

            var participant = new Participant(userId, "firstName", "lastName");

            await AddAsync(participant);

            var command = new CreateProjectCommand
            {
                Name = "Project",
                Key = "Key"
            };

            var id = await SendAsync(command);

            var project = await FindAsync<Project>(id);
     
            project.Should().NotBeNull();
            project.Name.Should().Be(command.Name);
            project.Key.Should().Be(command.Key.ToUpper());
            project.CreatedBy.Should().Be(userId);
            project.Created.Should().BeCloseTo(DateTime.Now, 10000);
        }
    }
}