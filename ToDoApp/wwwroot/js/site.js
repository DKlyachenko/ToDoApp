

$(document).ready(function () {


    $('.task-item').on('click', function (e) {
        toggleDoneTask(e.target);
    });

    $('.goal-item').on('click', function (e) {
        toggleDoneGoal(e.target);
    });

    $('.deleteTask').on('click', function (e) {
        deleteTask(e.target);
    });

    $('.deleteGoal').on('click', function (e) {
        deleteGoal(e.target);
    });
});

function toggleDoneTask(item) {    
    var tr = item.closest('tr');

    if (!tr.hasAttribute('data-id')) {
        return;
    }

    var taskId = tr.getAttribute('data-id');

    $.ajax({
        type: "POST",
        url: UrlSettings.ToggleDoneTaskUrl,
        data: { id: taskId }
    });
    $(item).toggleClass('done');
}

function toggleDoneGoal(item) {
    var tr = item.closest('tr');

    if (!tr.hasAttribute('data-id')) {
        return;
    }

    var goalId = tr.getAttribute('data-id');

    $.ajax({
        type: "POST",
        url: UrlSettings.ToggleDoneGoalUrl,
        data: { id: goalId }
    });
    $(tr).toggleClass('done');
}

function deleteTask(item) {
    var tr = item.closest('tr');

    if (!tr.hasAttribute('data-id')) {
        return;
    }

    var taskId = tr.getAttribute('data-id');

    $.ajax({
        type: "POST",
        url: UrlSettings.DeleteTaskUrl,
        data: { id: taskId }
    });
    $(tr).remove();
}

function deleteGoal(item) {
    var tr = item.closest('tr');

    if (!tr.hasAttribute('data-id')) {
        return;
    }

    var goalId = tr.getAttribute('data-id');

    $.ajax({
        type: "POST",
        url: UrlSettings.DeleteGoalUrl,
        data: { id: goalId }
    });
    $(tr).remove();
}

function deleteDoneTasks() {
    $.ajax({
        type: "POST",
        url: UrlSettings.DeleteDoneTasksUrl
    }).done(function(){
        location.reload();
    });
}