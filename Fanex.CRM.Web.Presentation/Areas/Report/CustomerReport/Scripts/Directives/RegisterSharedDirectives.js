(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function customerReportRegisterPicker() {
        var customerReportRegister = new RegisterTemplate(angular.module('report-customerreport-application'));
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

            directiveManager.registerDefaultFocus(angular.module('report-customerreport-application'), defaultFocusRegisterParams);
        };

        customerReportRegister.registerDatePicker();
        customerReportRegister.registerLanguageMultiSelectPicker();
        customerReportRegister.registerSiteMultiSelectPicker();
        customerReportRegister.registerTitlePicker();
        customerReportRegister.registerVIPTypePicker();
        customerReportRegister.registerDefaultFocus();
        customerReportRegister.registerDownloadFile();
        customerReportRegister.registerPositionMultiSelectPicker();
    }

    customerReportRegisterPicker();
})();