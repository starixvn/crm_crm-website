(function () {
    "use strict";
    /* global RegisterTemplate */
    function wagerReportRegisterPicker() {
        var wagerReportRegister = new RegisterTemplate(angular.module('report-wagerreport-application'));

        wagerReportRegister.registerDefaultFocus();
        wagerReportRegister.registerDownloadFile();
    }

    wagerReportRegisterPicker();
})();