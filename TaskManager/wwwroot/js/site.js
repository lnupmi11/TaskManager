// Write your JavaScript code.
$(document).ready(function () {
    setInterval(function time() {
        var d = new Date();
        var hours = 24 - d.getHours();
        var min = 60 - d.getMinutes();
        if ((min + '').length == 1) {
            min = '0' + min;
        }
        var sec = 60 - d.getSeconds();
        if ((sec + '').length == 1) {
            sec = '0' + sec;
        }
        jQuery('#countdown #hour').html(hours);
        jQuery('#countdown #min').html(min);
        jQuery('#countdown #sec').html(sec);
    }, 1000);
});