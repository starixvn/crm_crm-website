(function () {
    "use strict";
    /* global RegisterTemplate */
    function settlementReportRegisterPicker() {
        var settlementReportRegister = new RegisterTemplate(angular.module('report-settlementreport-application'));

        settlementReportRegister.registerYearPicker();
        settlementReportRegister.registerDefaultFocus();
        settlementReportRegister.registerDownloadFile();
    }

    settlementReportRegisterPicker();
})();