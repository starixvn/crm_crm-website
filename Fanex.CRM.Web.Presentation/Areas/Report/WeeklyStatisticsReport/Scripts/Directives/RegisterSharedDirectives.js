"use strict";
/* global weeklyStatisticsReportApplication,
RegisterTemplate */
function weeklyStatisticsReportRegisterPicker() {
    var weeklyStatisticsReportRegister = new RegisterTemplate(weeklyStatisticsReportApplication);
    weeklyStatisticsReportRegister.registerDatePicker();
    weeklyStatisticsReportRegister.registerDefaultFocus();
    weeklyStatisticsReportRegister.registerDownloadFile();
}

weeklyStatisticsReportRegisterPicker();