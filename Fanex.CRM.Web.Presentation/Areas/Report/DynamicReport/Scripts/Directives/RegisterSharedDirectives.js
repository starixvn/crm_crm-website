(function () {
    "use strict";
    /* global RegisterTemplate */
    function dynamicReportRegisterPicker() {
        var dynamicReportRegister = new RegisterTemplate(angular.module('report-dynamicreport-application'));
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
        dynamicReportRegister.registerSubjectAutoComplete();
    }

    dynamicReportRegisterPicker();
})();