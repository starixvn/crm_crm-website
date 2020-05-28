(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function customerReportRegisterPicker() {
        var customerReportRegister = new RegisterTemplate(angular.module('pic-management-application'));
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

            directiveManager.registerDefaultFocus(angular.module('pic-management-application'), defaultFocusRegisterParams);
        };

        customerReportRegister.registerRolePicker();
        customerReportRegister.registerDefaultFocus();
        customerReportRegister.registerDownloadFile();
    }

    customerReportRegisterPicker();
})();