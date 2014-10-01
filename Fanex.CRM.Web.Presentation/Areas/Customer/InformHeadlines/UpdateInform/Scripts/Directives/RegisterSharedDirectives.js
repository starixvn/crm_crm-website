'use strict';
/* global updateInformHeadlineApplication,
RegisterTemplate */
function updateInformHeadlinetRegisterPicker() {
    var updateInformRegister = new RegisterTemplate(updateInformHeadlineApplication);
    updateInformRegister.registerStatusMultiSelectPicker();
    updateInformRegister.registerDownloadFile();
}

updateInformHeadlinetRegisterPicker();