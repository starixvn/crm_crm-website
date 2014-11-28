"use strict";
/* global dynamicReportApplication,
RegisterTemplate */
function redemptionStatisticsRegisterPicker() {
    var redemptionStatisticsRegister = new RegisterTemplate(redemptionStatisticsApplication);

    redemptionStatisticsRegister.registerDatePicker();
    redemptionStatisticsRegister.registerStatusMultiSelectPicker();
    redemptionStatisticsRegister.registerCurrencyPicker();
    redemptionStatisticsRegister.registerReportByPicker();
    redemptionStatisticsRegister.registerDefaultFocus();
    redemptionStatisticsRegister.registerDownloadFile();
}

redemptionStatisticsRegisterPicker();