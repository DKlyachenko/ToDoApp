using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;
using ToDoApp.Core;
using ToDoApp.Core.Services;
using ToDoApp.Tests.FakeEntities;
using System.Threading.Tasks;
using ToDoApp.Core.Models;

namespace ToDoApp.Tests.ServicesTests
{
    public class GoalsServiceTests
    {
        IRepository _repository;
        GoalsService _goalsService;

        public GoalsServiceTests()
        {
            _repository = new MockRepository();
            _goalsService = new GoalsService(_repository);
        }

        [Fact]
        public async Task GetAllGoals_Should_Return_All_Goals()
        {
            var expectedCount = 3;

            var result = await _goalsService.GetAllGoals();
            var actualCount = result.Count();

            Assert.Equal(expectedCount, actualCount);
        }

        [Fact]
        public async Task AddGoal_Should_Add_Goal()
        {
            var expectedRepositoryCount = 4;

            var newGoal = new ToDoGoal{ Id = 0, Title = "new test goal", IsDone = false };

            await _goalsService.AddGoal(newGoal);

            var allGoals = await _repository.GetAllGoals();
            var goalsCount = allGoals.Count;

            Assert.Equal(expectedRepositoryCount, goalsCount);
        }

        [Theory]
        [InlineData(1, "1")]
        [InlineData(2, "2")]
        public async Task GetGoal_Should_Return_Goal_By_Id(int goalId, string expectedTitle)
        {
            var result = await _goalsService.GetGoal(goalId);

            Assert.Equal(goalId, result.Id);
            Assert.Equal(expectedTitle, result.Title);
        }

        [Fact]
        public async Task UpdateGoal_Should_Update_Goal()
        {
            var updatedTitle = "Updated title";
            var goal = new ToDoGoal { Id = 1, Title = updatedTitle };

            await _goalsService.UpdateGoal(goal);

            var actualGoal = await _repository.GetGoal(1);
            var actualGoalTitle = actualGoal.Title;

            Assert.Equal(updatedTitle, actualGoalTitle);
        }

        [Fact]
        public async Task Delete_Should_Remove_Goal_By_Id()
        {
            var goalId = 1;
            var expectedGoalsCount = 2;

            var result = await _goalsService.Delete(goalId);
            var allGoals = await _repository.GetAllGoals();
            var goalsCount = allGoals.Count;
            var goalsContainsRemovedTask = allGoals.Any(x => x.Id == goalId);


            Assert.Equal(expectedGoalsCount, goalsCount);
            Assert.False(goalsContainsRemovedTask);
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(3, false)]
        public async Task ToggleDone_Should_Toggle_IsDone_Of_Goal_By_Id(int goalId, bool expectedIsDone)
        {
            var expectedError = string.Empty;
            var expectedSuccess = true;

            var result = await _goalsService.ToggleDone(goalId);

            var goal = await _repository.GetGoal(goalId);
            var actualIsDone = goal.IsDone;

            Assert.Equal(expectedIsDone, actualIsDone);
            Assert.Equal(expectedError, result.Error);
            Assert.Equal(expectedSuccess, result.Success);
        }
    }
}
