function OnChangeStartDateEvent(e) {
    var start = $('#StartDate.form-control');
    var end = $('#EndDate.form-control');
    if (start.val()) {
        var dt = new Date(start.val()).toISOString().substr(0, 10);
        end.attr('min', dt);
    }
}

function OnChangeEndDateEvent(e) {
    var start = $('#StartDate.form-control');
    var end = $('#EndDate.form-control');
    if (end.val()) {
        var dt = new Date(end.val()).toISOString().substr(0, 10);
        start.attr('max', dt);
    }
}