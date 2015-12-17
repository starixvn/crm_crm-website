(function () {
    "use strict";
    /* global RegisterTemplate */
    function statisticReportRegisterPicker() {
        var statisticReportRegister = new RegisterTemplate(angular.module('report-statisticreport-application'));
        statisticReportRegister.registerReportTypePicker();
        statisticReportRegister.registerDatePicker();
        statisticReportRegister.registerMonthPicker();
        statisticReportRegister.registerMainCategoryPicker();
        statisticReportRegister.registerSubCategoryPicker();

        statisticReportRegister.registerDefaultFocus();
        statisticReportRegister.registerDownloadFile();
    }

    statisticReportRegisterPicker();
})();