(function () {
    "use strict";
    /* global RegisterTemplate */
    function weeklyStatisticsReportRegisterPicker() {
        var weeklyStatisticsReportRegister = new RegisterTemplate(angular.module('report-weeklystatistics-application'));
        weeklyStatisticsReportRegister.registerDatePicker();
        weeklyStatisticsReportRegister.registerDefaultFocus();
        weeklyStatisticsReportRegister.registerDownloadFile();
    }

    weeklyStatisticsReportRegisterPicker();
})();