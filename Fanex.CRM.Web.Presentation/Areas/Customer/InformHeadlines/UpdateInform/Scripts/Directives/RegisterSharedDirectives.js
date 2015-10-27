(function () {
    "use strict";
    /* global RegisterTemplate */
    function updateInformHeadlinetRegisterPicker() {
        var updateInformRegister = new RegisterTemplate(angular.module('customer-informheadline-update'));
        updateInformRegister.registerStatusMultiSelectPicker();
        updateInformRegister.registerPositionMultiSelectPicker();
        updateInformRegister.registerCurrencyMultiSelectPicker();
        updateInformRegister.registerSiteMultiSelectPicker();
        updateInformRegister.registerDownloadFile();
    }

    updateInformHeadlinetRegisterPicker();
})();