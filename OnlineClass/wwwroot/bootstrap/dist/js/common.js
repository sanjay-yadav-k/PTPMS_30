
$(document).ready(function () {
   //
    $('.datepicker').datepicker({
        autoclose: true,
        format: "mm/dd/yyyy",
        todayHighlight: true,
        todayBtn: true
    });

    $('.datepicker1').datepicker({
        autoclose: true,
        minDate: new Date(),
        startDate: new Date(),
        format: "mm/dd/yyyy",
        todayHighlight: true,
        todayBtn: true
    });

   // $(".CustomDateFormat").mask("00/00/0000");
   // bindmenu();
    //GetNotification();
    //setInterval(function () { GetNotification(); }, 30 * 1000);
    //$('#dvNotificationContainer').on('hidden.bs.dropdown', function (data, e, event) {
    //    MarkAsReadNotification();
    //    if ($(e.relatedTarget).parents().find('#dvNotificationContainer').length > 0)
    //        $(data.target).addClass('open');
    //});
    //$('#uNotification').on('close.bs.alert', function (data) {
    //    var ID = $(data.target).attr('id').replace("Noti_", "");
    //    DeleteNotification(ID);
    //});
    //$(".topnav").accordion({
    //    accordion: false,
    //    speed: 500,
    //    closedSign: '[+]',
    //    openedSign: '[-]'
    //});
    $('.clockpickerNew').each(function () {
        var clockpicker = $(this).clockpicker({
            autoclose: true,
            twelvehour: true,
            afterDone: function (obj) {
                clockpicker.val(clockpicker.val().slice(0, -2) + ' ' + clockpicker.val().slice(-2));
            }
        });
    });
    $('.clockpicker').each(function () {
        var clockpicker = $(this).clockpicker({
            autoclose: true,
            twelvehour: false
        });
    });
//    $(".datetimepicker").datetimepicker({
//        weekStart: 1,
//        //todayBtn: 1,
//        //autoclose: 1,
//        todayHighlight: 1,
//        startView: 2,
//        forceParse: 0,
//        //showMeridian: 1,
//        showMeridian: true,
//        format: "mm/dd/yyyy HH:ii P",
//        autoclose: true,
//        todayBtn: true,
//        startDate: "2012-02-14 10:00:00",
//        minuteStep: 1
//    });
//});

    //$(".datetimepicker").datetimepicker({
    //    format: "mm/dd/yyyy HH:ii:ss P",
    //    autoclose: true,
    //    todayBtn: true,
    //    startDate: new Date(),
    //    minuteStep: 1,
    //    showMeridian: true
    //});

    //For restrict the advance date
    $('.DenyAdvanceDateSelection').datepicker('setEndDate', new Date())

    //For restrict the past date
    //$(".datetimepicker1").datetimepicker({
    //    format: "mm/dd/yyyy HH:ii:ss P",
    //    autoclose: true,
    //    todayBtn: true,
    //    todayHighlight:true,
    //    minDate:new Date(),
    //    startDate: new Date(),
    //    minuteStep: 1,
    //    showMeridian: true
    //});


    //////////////--------------------- add on 07.06.2017 ------------------------//////////////
    //var digitsOnly = /[1234567890]/g;
    //var integerOnly = /[0-9\.]/g;
    //var alphaOnly = /[A-Za-z]/g;
    //function restrictCharacters(myfield, e, restrictionType) {
    //    if (!e) var e = window.event
    //    if (e.keyCode) code = e.keyCode;
    //    else if (e.which) code = e.which;
    //    var character = String.fromCharCode(code);
    //    if (code == 27) { this.blur(); return false; }
    //    if (!e.ctrlKey && code != 9 && code != 8 && code != 36 && code != 37 && code != 38 && (code != 39 || (code == 39 && character == "'")) && code != 40) {
    //        if (character.match(restrictionType)) {
    //            return true;
    //        } else {
    //            return false;
    //        }

    //    }
    //}
    //////////////--------------------- add on 07.06.2017 ------------------------//////////////



});


function fnGetCurentTime() {
    var fullDate = new Date();

    currentHours = fullDate.getHours();
    currentHours = ("0" + currentHours).slice(-2);

    currentMinutes= fullDate.getMinutes();
    currentMinutes = ("0" + currentMinutes).slice(-2);

    currentSeconds = fullDate.getSeconds();
    currentSeconds = ("0" + currentSeconds).slice(-2);

    var formatted = currentHours+":" + currentMinutes +":"+ currentSeconds;

    var twoDigitMonth = fullDate.getMonth() + 1+""; if (twoDigitMonth.length == 1) twoDigitMonth = "0" + twoDigitMonth;
    var twoDigitDate = fullDate.getDate() + ""; if (twoDigitDate.length == 1) twoDigitDate = "0" + twoDigitDate;
    var currentDate =  twoDigitMonth + "/" + twoDigitDate + "/"  + fullDate.getFullYear();
    return datetime = currentDate + " " + formatted;
    
}


//--------masking textboxes-----------------
//$(document).ready(function () {
//    $('.zipcode').mask("00000");
//    $('.UsCellFormat').mask('(000) 000-0000');
//    $(".SSN_AutoFormat").mask('000-00-0000');
//    $(".CustomTimeFormat").mask("00:00");
//    $('.ChargedHours').mask("00");
//    $('.Percentages').mask("000");
//    $('.CardNumbers').mask("0000000000000000");
//    $('.unitprice').mask("000.00");
//    $('.days').mask("00");
//    $('.months').mask("00");
//    $('.years').mask("0000");

//});


