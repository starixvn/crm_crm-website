"use strict";
/* global settlementReportApplication,
RegisterTemplate */
function settlementReportRegisterPicker() {
    var settlementReportRegister = new RegisterTemplate(settlementReportApplication);

    settlementReportRegister.registerYearPicker();
    settlementReportRegister.registerDefaultFocus();
    settlementReportRegister.registerDownloadFile();
}

settlementReportRegisterPicker();