"use strict";
/* global editLogApplication,
RegisterTemplate */
function editCommunicationLogRegisterPicker() {
    var editCommunicationLogRegister = new RegisterTemplate(editLogApplication);
    editCommunicationLogRegister.registerLicenseeTypePicker();
}

editCommunicationLogRegisterPicker();