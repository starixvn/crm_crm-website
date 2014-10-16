"use strict";
/* global dailyStatisticsReportApplication,
RegisterTemplate */
function dailyStatisticsReportRegisterPicker() {
    var dailyStatisticsReportRegister = new RegisterTemplate(dailyStatisticsReportApplication);
    dailyStatisticsReportRegister.registerDatePicker();
    dailyStatisticsReportRegister.registerDefaultFocus();
    dailyStatisticsReportRegister.registerDownloadFile();
}

dailyStatisticsReportRegisterPicker();