(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function picOfHeadlineRegisterPicker() {
        var picOfHeadlineRegister = new RegisterTemplate(angular.module('picofheadline-application'));
        picOfHeadlineRegister.registerDefaultFocus = function () {
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

            directiveManager.registerDefaultFocus(angular.module('picofheadline-application'), defaultFocusRegisterParams);
        };

        picOfHeadlineRegister.registerSiteMultiSelectPicker();
        picOfHeadlineRegister.registerCurrencyMultiSelectPicker();
        picOfHeadlineRegister.registerStatusMultiSelectPicker();
        picOfHeadlineRegister.registerDownloadFile();
        picOfHeadlineRegister.registerPositionMultiSelectPicker();
        picOfHeadlineRegister.registerPICMultiSelectPicker();
    }

    picOfHeadlineRegisterPicker();
})();