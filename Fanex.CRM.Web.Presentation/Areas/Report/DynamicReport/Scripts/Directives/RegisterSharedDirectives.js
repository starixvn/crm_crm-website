"use strict";
/* global dynamicReportApplication,
RegisterTemplate */
function dynamicReportRegisterPicker() {
    var dynamicReportRegister = new RegisterTemplate(dynamicReportApplication);
    dynamicReportRegister.registerReportTypePicker();
    dynamicReportRegister.registerDatePicker();
    dynamicReportRegister.registerWeekPicker();
    dynamicReportRegister.registerMonthPicker();
    dynamicReportRegister.registerYearPicker();
    dynamicReportRegister.registerProductPicker();
    dynamicReportRegister.registerSitePicker();
    dynamicReportRegister.registerStatusPicker();
    dynamicReportRegister.registerResponsePicker();
    dynamicReportRegister.registerRolePicker();
    dynamicReportRegister.registerCommunicationToolPicker();
    dynamicReportRegister.registerMainCategoryPicker();
    dynamicReportRegister.registerSubCategoryPicker();
    dynamicReportRegister.registerExtensionPicker();
    dynamicReportRegister.registerDefaultFocus();
    dynamicReportRegister.registerDownloadFile();
}

dynamicReportRegisterPicker();