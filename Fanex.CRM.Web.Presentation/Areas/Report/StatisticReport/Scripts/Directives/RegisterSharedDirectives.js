"use strict";
/* global statisticReportApplication,
RegisterTemplate */
function statisticReportRegisterPicker() {
    var dynamicReportRegister = new RegisterTemplate(statisticReportApplication);
    dynamicReportRegister.registerReportTypePicker();
    dynamicReportRegister.registerDatePicker();
    dynamicReportRegister.registerMonthPicker();
    dynamicReportRegister.registerMainCategoryPicker();
    dynamicReportRegister.registerSubCategoryPicker();

    dynamicReportRegister.registerDefaultFocus();
    dynamicReportRegister.registerDownloadFile();
}

statisticReportRegisterPicker();