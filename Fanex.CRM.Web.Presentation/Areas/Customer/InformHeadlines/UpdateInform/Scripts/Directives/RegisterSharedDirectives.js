'use strict';
/* global updateInformHeadlineApplication,
RegisterTemplate */
function updateInformHeadlinetRegisterPicker() {
    var updateInformRegister = new RegisterTemplate(updateInformHeadlineApplication);
    updateInformRegister.registerStatusMultiSelectPicker();
    updateInformRegister.registerPositionMultiSelectPicker();
    updateInformRegister.registerCurrencyMultiSelectPicker();
    updateInformRegister.registerSiteMultiSelectPicker();
    updateInformRegister.registerDownloadFile();
}

updateInformHeadlinetRegisterPicker();