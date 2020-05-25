using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoApp.Controllers;
using ToDoApp.Core.Models;
using ToDoApp.Core.Services;
using ToDoApp.Tests.FakeEntities;
using Xunit;

namespace ToDoApp.Tests.ControllersTests
{
    public class TaskControllerTests
    {
        MockRepository repository;
        TaskController taskController;
        TasksService tasksService;
        public TaskControllerTests()
        {
            repository = new MockRepository();
            tasksService = new TasksService(repository);
            taskController = new TaskController(tasksService);
        }

        [Fact]
        public async Task Index_Should_Return_NonArchived_Tasks()
        {

            var expectedCount = 3;

            var result = await taskController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ToDoTask>>(viewResult.Model);

            var actualCount = model.Count();
            var containsArchivedTasks = model.Any(x => x.IsArchived);

            Assert.Equal(expectedCount, actualCount);
            Assert.False(containsArchivedTasks);
        }

        [Fact]
        public async Task Index_ViewData_Should_Have_Actual_Goals()
        {
            var expectedCount = 2;

            var result = await taskController.Index();
            var viewResult = Assert.IsType<ViewResult>(result);
            var goals = viewResult.ViewData["Goals"] as IEnumerable<ToDoGoal>;

            var containsDoneGoals = goals.Any(x => x.IsDone || x.IsArchived);

            Assert.Equal(expectedCount, goals.Count());
            Assert.False(containsDoneGoals);
        }

        [Fact]
        public async Task Create_Should_Add_New_Task()
        {
            var expectedViewResultName = "Index";
            var expectedRepositoryCount = 5;

            var newTask = new ToDoTask { Id = 0, Title = "new test task", IsDone = false };

            var result = await taskController.Create(newTask);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var allTasks = await repository.GetAllTasks();
            var tasksCount = allTasks.Count;

            Assert.Equal(expectedViewResultName, viewResult.ActionName);
            Assert.Equal(expectedRepositoryCount, tasksCount);
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        public async Task Edit_Should_Find_Task_By_Id(int taskId, string expectedTitle)
        {
            var result = await taskController.Edit(taskId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ToDoTask>(
                viewResult.ViewData.Model);

            Assert.Equal(taskId, model.Id);
            Assert.Equal(expectedTitle, model.Title);
        }

        [Theory]
        [InlineData(10)]
        public async Task Edit_Should_Return_NotFound(int taskId)
        {
            var result = await taskController.Edit(taskId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Should_Update_Task()
        {
            var updatedTitle = "Updated title";
            var task = new ToDoTask { Id = 1, Title = updatedTitle };

            var result = await taskController.Edit(task);

            var actualTask = await repository.GetTask(1);
            var actualTaskTitle = actualTask.Title;

            Assert.Equal(updatedTitle, actualTaskTitle);
        }

        [Fact]
        public async Task Delete_Should_Remove_Task()
        {
            var taskId = 1;
            var expectedTasksCount = 3;

            var result = await taskController.Delete(taskId);
            var allTasks = await repository.GetAllTasks();
            var tasksCount = allTasks.Count;
            var tasksContainsRemovedTask = allTasks.Any(x => x.Id == taskId);
            var viewResult = Assert.IsType<JsonResult>(result);
            var returnResult = viewResult.Value as ReturnResult;

            Assert.Equal(expectedTasksCount, tasksCount);
            Assert.False(tasksContainsRemovedTask);
            Assert.Equal(string.Empty, returnResult.Error);
            Assert.True(returnResult.Success);
        }

        [Fact]
        public async Task Delete_For_NonExist_Should_Return_Error()
        {
            var taskId = 10;
            var expectedMessage = StaticData.taskFindError;

            var result = await taskController.Delete(taskId);
            var returnResult = Assert.IsType<JsonResult>(result).Value as ReturnResult;

            Assert.False(returnResult.Success);
            Assert.Equal(expectedMessage, returnResult.Error);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(3, false)]
        public async Task ToggleDone_Should_Toggle_IsDone(int taskId, bool expectedIsDone)
        {
            var result = await taskController.ToggleDone(taskId);

            var task = await repository.GetTask(taskId);
            var actualIsDone = task.IsDone;

            Assert.Equal(expectedIsDone, actualIsDone);
        }

        [Fact]
        public async Task DeleteDoneTasks_Should_Remove_Done_Tasks()
        {
            var expectedCount = 2;

            var result = taskController.DeleteDoneTasks();
            var allTasks = await repository.GetAllTasks();
            var actualCount = allTasks.Count();
            var containsDoneTasks = allTasks.Any(x => x.IsDone);

            Assert.Equal(expectedCount, actualCount);
            Assert.False(containsDoneTasks);
        }
    }
}
