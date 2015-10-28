(function () {
    "use strict";
    /* global RegisterTemplate */
    function dailyStatisticsReportRegisterPicker() {
        var dailyStatisticsReportRegister = new RegisterTemplate(angular.module('report-dailystatistics-application'));
        dailyStatisticsReportRegister.registerDatePicker();
        dailyStatisticsReportRegister.registerDefaultFocus();
        dailyStatisticsReportRegister.registerDownloadFile();
    }

    dailyStatisticsReportRegisterPicker();
})();