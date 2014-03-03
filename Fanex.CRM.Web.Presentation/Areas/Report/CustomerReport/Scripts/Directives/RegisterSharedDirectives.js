"use strict";
/* global customerReportApplication,
RegisterTemplate */
function customerReportRegisterPicker() {
    var customerReportRegister = new RegisterTemplate(customerReportApplication);
    customerReportRegister.registerDatePicker();
    customerReportRegister.registerCountryMultiSelectPicker();
    customerReportRegister.registerLanguageMultiSelectPicker();
    customerReportRegister.registerSiteMultiSelectPicker();
    customerReportRegister.registerTitlePicker();
    customerReportRegister.registerCustomerTypePicker();
    customerReportRegister.registerVIPTypePicker();
    customerReportRegister.registerDefaultFocus();
    customerReportRegister.registerDownloadFile();
}

customerReportRegisterPicker();