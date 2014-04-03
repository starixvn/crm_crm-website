"use strict";
/* global statisticReportApplication,
RegisterTemplate */
function statisticReportRegisterPicker() {
    var statisticReportRegister = new RegisterTemplate(statisticReportApplication);
    statisticReportRegister.registerReportTypePicker();
    statisticReportRegister.registerDatePicker();
    statisticReportRegister.registerMonthPicker();
    statisticReportRegister.registerMainCategoryPicker();
    statisticReportRegister.registerSubCategoryPicker();

    statisticReportRegister.registerDefaultFocus();
    statisticReportRegister.registerDownloadFile();
}

statisticReportRegisterPicker();