$(document).ready(function ()
{
    var timerVal = moment().format('hh:mm A');

    // Verification
    if ($('#timerValId').data() &&
        $('#timerValId').val() != null &&
        $('#timerValId').val() != '')
    {
        timerVal = moment($('#timerValId').val()).format('hh:mm A');
    }
    else if ($('#timerValId').data() &&
        ($('#timerValId').val() == null ||
        $('#timerValId').val() == ''))
    {
        $('#timerValId').val(timerVal);
    }

    $('#timerId').timepicker(
        {
            template: 'dropdown',
            minuteStep: 1,
            secondStep: 1,
            showMeridian: true,
            defaultTime: timerVal
        });

    $('#timerId').timepicker().on('hide.timepicker', function (e)
    {
        console.log('The time is ' + e.time.value);

        $('#timerValId').val(e.time.value);
    });
});