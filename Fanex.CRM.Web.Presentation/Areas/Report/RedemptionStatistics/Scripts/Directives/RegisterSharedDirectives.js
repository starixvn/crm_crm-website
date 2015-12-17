(function () {
    "use strict";
    /* global RegisterTemplate */
    function redemptionStatisticsRegisterPicker() {
        var redemptionStatisticsRegister = new RegisterTemplate(angular.module('report-redemption-statistics-application'));

        redemptionStatisticsRegister.registerDatePicker();
        redemptionStatisticsRegister.registerStatusMultiSelectPicker();
        redemptionStatisticsRegister.registerCurrencyPicker();
        redemptionStatisticsRegister.registerReportByPicker();
        redemptionStatisticsRegister.registerDefaultFocus();
        redemptionStatisticsRegister.registerDownloadFile();
    }

    redemptionStatisticsRegisterPicker();
})();