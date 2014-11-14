"use strict";
/* global insertLogApplication,
RegisterTemplate */
function insertCommunicationLogRegisterPicker() {
    var insertCommunicationLogRegister = new RegisterTemplate(insertLogApplication);
    insertCommunicationLogRegister.registerLicenseeTypePicker();
}

insertCommunicationLogRegisterPicker();