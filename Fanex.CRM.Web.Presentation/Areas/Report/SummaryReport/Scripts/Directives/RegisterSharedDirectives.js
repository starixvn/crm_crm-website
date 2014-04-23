"use strict";
/* global summaryReportApplication,
RegisterTemplate */
function summaryReportRegisterPicker() {
    var summaryReportRegister = new RegisterTemplate(summaryReportApplication);
    summaryReportRegister.registerCategoryMultiSelectPicker();
    summaryReportRegister.registerDefaultFocus();
    summaryReportRegister.registerDownloadFile();
}

summaryReportRegisterPicker();