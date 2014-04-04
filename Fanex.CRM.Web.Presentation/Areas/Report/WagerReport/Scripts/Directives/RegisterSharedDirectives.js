"use strict";
/* global wagerReportApplication,
RegisterTemplate */
function wagerReportRegisterPicker() {
    var wagerReportRegister = new RegisterTemplate(wagerReportApplication);

    wagerReportRegister.registerDefaultFocus();
    wagerReportRegister.registerDownloadFile();
}

wagerReportRegisterPicker();