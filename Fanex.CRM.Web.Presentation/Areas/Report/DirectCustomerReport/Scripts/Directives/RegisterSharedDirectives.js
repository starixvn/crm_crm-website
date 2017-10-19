(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function customerReportRegisterPicker() {
        var customerReportRegister = new RegisterTemplate(angular.module('report-directcustomerreport-application'));
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

            directiveManager.registerDefaultFocus(angular.module('report-directcustomerreport-application'), defaultFocusRegisterParams);
        };

        customerReportRegister.registerDownloadFile();        
    }

    customerReportRegisterPicker();
})();