"use strict";
/* global summaryReportApplication,
RegisterTemplate */
function summaryReportRegisterPicker() {
    var summaryReportRegister = new RegisterTemplate(summaryReportApplication);

    summaryReportRegister.registerDefaultFocus();
    summaryReportRegister.registerDownloadFile();
}

summaryReportRegisterPicker();