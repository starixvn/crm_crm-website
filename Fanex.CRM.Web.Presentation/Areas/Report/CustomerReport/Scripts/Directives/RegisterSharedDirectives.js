"use strict";
/* global customerReportApplication,
RegisterTemplate,
directiveManager */
function customerReportRegisterPicker() {
    var customerReportRegister = new RegisterTemplate(customerReportApplication);
    customerReportRegister.registerDefaultFocus = function () {
        var defaultFocusRegisterParams = {
            PlugInName: 'defaultFocus',
            OptionName: 'defaultFocusOptions',
            PickerId: "firstname",
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDefaultFocus(customerReportApplication, defaultFocusRegisterParams);
    };

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