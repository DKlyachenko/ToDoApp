

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

onSuccess = function (data, callback) {
    if (data.success && callback) {
        callback();
    } else {
        alert(data.error);
    }
};

function toggleDoneTask(item) {    
    var tr = item.closest('tr');

    if (!tr.hasAttribute('data-id')) {
        return;
    }

    var taskId = tr.getAttribute('data-id');
    var callbackFunc = function () {
        $(item).toggleClass('done');
    }

    $.ajax({
        type: "POST",
        url: UrlSettings.ToggleDoneTaskUrl,
        data: { id: taskId },
        success: data => {
            onSuccess(data, callbackFunc);
        },
    });
}

function toggleDoneGoal(item) {
    var tr = item.closest('tr');

    if (!tr.hasAttribute('data-id')) {
        return;
    }

    var goalId = tr.getAttribute('data-id');

    var callbackFunc = function () {
        $(tr).toggleClass('done');
    }

    $.ajax({
        type: "POST",
        url: UrlSettings.ToggleDoneGoalUrl,
        data: { id: goalId },
        success: data => {
            onSuccess(data, callbackFunc);
        },
    });
    
}

function deleteTask(item) {
    var tr = item.closest('tr');

    if (!tr.hasAttribute('data-id')) {
        return;
    }

    var callbackFunc = function () {
        $(tr).remove();
    }

    var taskId = tr.getAttribute('data-id');

    $.ajax({
        type: "POST",
        url: UrlSettings.DeleteTaskUrl,
        data: { id: taskId },
        success: data => {
            onSuccess(data, callbackFunc);
        }
    });
    
}

function deleteGoal(item) {
    var tr = item.closest('tr');

    if (!tr.hasAttribute('data-id')) {
        return;
    }

    var callbackFunc = function () {
        $(tr).remove();
    }

    var goalId = tr.getAttribute('data-id');

    $.ajax({
        type: "POST",
        url: UrlSettings.DeleteGoalUrl,
        data: { id: goalId },
        success: data => {
            onSuccess(data, callbackFunc);
        }
    });
}

function deleteDoneTasks() {
    $.ajax({
        type: "POST",
        url: UrlSettings.DeleteDoneTasksUrl
    }).done(function(){
        location.reload();
    });
}