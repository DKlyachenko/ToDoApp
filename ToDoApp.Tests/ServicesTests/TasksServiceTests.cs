using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Core;
using ToDoApp.Core.Models;
using ToDoApp.Core.Services;
using ToDoApp.Tests.FakeEntities;
using Xunit;

namespace ToDoApp.Tests.ServicesTests
{
    public class TasksServiceTests
    {
        TasksService _tasksService;
        IRepository _repository;

        public TasksServiceTests()
        {
            _repository = new MockRepository();
            _tasksService = new TasksService(_repository);
        }

        [Fact]
        public async Task GetNonArchivedTasks_Should_Return_Only_NonArchived_Tasks()
        {
            var expectedCount = 3;

            var result = await _tasksService.GetNonArchivedTasks();

            var actualCount = result.Count();
            var containsArchivedTasks = result.Any(x => x.IsArchived);

            Assert.Equal(expectedCount, actualCount);
            Assert.False(containsArchivedTasks);
        }

        [Fact]
        public async Task AddTask_Should_Add_Task()
        {
            var expectedRepositoryCount = 5;

            var newTask = new ToDoTask { Id = 0, Title = "new test task", IsDone = false };

            await _tasksService.AddTask(newTask);

            var allTasks = await _repository.GetAllTasks();
            var tasksCount = allTasks.Count;

            Assert.Equal(expectedRepositoryCount, tasksCount);
        }

        [Fact]
        public async Task UpdateTask_Should_Update_Task()
        {
            var updatedTitle = "Updated title";
            var task = new ToDoTask { Id = 1, Title = updatedTitle };

            await _tasksService.UpdateTask(task);

            var actualTask = await _repository.GetTask(1);
            var actualTaskTitle = actualTask.Title;

            Assert.Equal(updatedTitle, actualTaskTitle);
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        public async Task GetTask_Should_Return_Task_By_Id(int taskId, string expectedTitle)
        {
            var result = await _tasksService.GetTask(taskId);

            Assert.Equal(taskId, result.Id);
            Assert.Equal(expectedTitle, result.Title);
        }

        [Fact]
        public async Task Delete_Should_Remove_Task_By_Id()
        {
            var taskId = 1;
            var expectedTasksCount = 3;

            var result = await _tasksService.Delete(taskId);
            var allTasks = await _repository.GetAllTasks();
            var tasksCount = allTasks.Count;
            var tasksContainsRemovedTask = allTasks.Any(x => x.Id == taskId);
            

            Assert.Equal(expectedTasksCount, tasksCount);
            Assert.False(tasksContainsRemovedTask);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(3, false)]
        public async Task ToggleDone_Should_Toggle_IsDone_Of_Task_By_Id(int taskId, bool expectedIsDone)
        {
            var expectedError = string.Empty;
            var expectedSuccess = true;

            var result = await _tasksService.ToggleDone(taskId);

            var task = await _repository.GetTask(taskId);
            var actualIsDone = task.IsDone;

            Assert.Equal(expectedIsDone, actualIsDone);
            Assert.Equal(expectedError, result.Error);
            Assert.Equal(expectedSuccess, result.Success);
        }

        [Fact]
        public async Task DeleteDoneTasks_Should_Remove_All_Tasks_With_IsDone_True()
        {
            var expectedCount = 2;

            _tasksService.DeleteDoneTasks();
            var allTasks = await _repository.GetAllTasks();
            var actualCount = allTasks.Count();
            var containsDoneTasks = allTasks.Any(x => x.IsDone);

            Assert.Equal(expectedCount, actualCount);
            Assert.False(containsDoneTasks);
        }

        [Fact]
        public async Task GetActualGoals_Should_Return_NonArchived_And_NotDone_Tasks()
        {
            var expectedCount = 2;

            var result = await _tasksService.GetActualGoals();

            var containsDoneGoals = result.Any(x => x.IsDone || x.IsArchived);

            Assert.Equal(expectedCount, result.Count());
            Assert.False(containsDoneGoals);
        }
    }
}
