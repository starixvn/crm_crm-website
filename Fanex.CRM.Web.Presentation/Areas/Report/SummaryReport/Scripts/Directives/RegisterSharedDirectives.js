(function () {
    "use strict";
    /* global RegisterTemplate */
    function summaryReportRegisterPicker() {
        var summaryReportRegister = new RegisterTemplate(angular.module('report-summaryreport-application'));
        summaryReportRegister.registerCategoryMultiSelectPicker();
        summaryReportRegister.registerDefaultFocus();
        summaryReportRegister.registerDownloadFile();
    }

    summaryReportRegisterPicker();
})();