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
    public class GoalControllerTests
    {
        MockRepository repository;
        GoalController goalController;
        GoalsService goalsService;
        public GoalControllerTests()
        {
            repository = new MockRepository();
            goalsService = new GoalsService(repository);
            goalController = new GoalController(goalsService);
        }

        [Fact]
        public async Task Index_Model_Should_Have_3_Goals()
        {
            var expectedCount = 3;

            var result = await goalController.Index();            

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ToDoGoal>>(
                viewResult.ViewData.Model);
            var actualCount = model.Count();
            Assert.Equal(expectedCount, actualCount);

        }

        [Fact]
        public async Task Create_Should_Add_New_Goal()
        {
            var expectedViewResultName = "Index";
            var expectedRepositoryCount = 4;

            var newGoal = new ToDoGoal { Id = 0, Title = "new test goal", IsDone = false };

            var result = await goalController.Create(newGoal);
            var viewResult = Assert.IsType<RedirectToActionResult>(result);
            var allGoals = await repository.GetAllGoals();
            var goalsCount = allGoals.Count;

            Assert.Equal(expectedViewResultName, viewResult.ActionName);
            Assert.Equal(expectedRepositoryCount, goalsCount);
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        public async Task Edit_Should_Find_Goal_By_Id(int goalId, string expectedTitle)
        {
            var result = await goalController.Edit(goalId);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ToDoGoal>(
                viewResult.ViewData.Model);

            Assert.Equal(goalId, model.Id);
            Assert.Equal(expectedTitle, model.Title);

        }

        [Theory]
        [InlineData(10)]
        public async Task Edit_Should_Return_NotFound(int goalId)
        {
            var result = await goalController.Edit(goalId);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Should_Update_Goal()
        {
            var updatedTitle = "Updated title";
            var goal = new ToDoGoal { Id = 1, Title = updatedTitle };

            var result = await goalController.Edit(goal);

            var actualGoal = await repository.GetGoal(1);
            var actualGoalTitle = actualGoal.Title;

            Assert.Equal(updatedTitle, actualGoalTitle);
        }

        [Fact]
        public async Task Delete_Should_Remove_Goal()
        {
            var goalId = 1;
            var expectedGoalsCount = 2;

            var result = await goalController.Delete(goalId);
            var allGoals = await repository.GetAllGoals();
            var goalsCount = allGoals.Count;
            var goalsContainsRemovedGoal = allGoals.Any(x => x.Id == goalId);
            var viewResult = Assert.IsType<JsonResult>(result);
            var returnResult = viewResult.Value as ReturnResult;

            Assert.Equal(expectedGoalsCount, goalsCount);
            Assert.False(goalsContainsRemovedGoal);
            Assert.Equal(string.Empty, returnResult.Error);
            Assert.True(returnResult.Success);
        }

        [Fact]
        public async Task Delete_For_NonExist_Should_Return_Error()
        {
            var goalId = 10;
            var expectedMessage = StaticData.goalFindError;

            var result = await goalController.Delete(goalId);
            var returnResult = Assert.IsType<JsonResult>(result).Value as ReturnResult;

            Assert.False(returnResult.Success);
            Assert.Equal(expectedMessage, returnResult.Error);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(3, false)]
        public async Task ToggleDone_Should_Toggle_IsDone(int goalId, bool expectedIsDone)
        {
            var result = await goalController.ToggleDone(goalId);

            var goal = await repository.GetGoal(goalId);
            var actualIsDone = goal.IsDone;

            Assert.Equal(expectedIsDone, actualIsDone);
        }

        [Fact]
        public async Task ToggleDone_For_NonExist_Should_Return_Error()
        {
            var goalId = 10;
            var expectedMessage = StaticData.goalFindError;

            var result = await goalController.ToggleDone(goalId);
            var returnResult = Assert.IsType<JsonResult>(result).Value as ReturnResult;

            Assert.False(returnResult.Success);
            Assert.Equal(expectedMessage, returnResult.Error);
        }
    }
}
