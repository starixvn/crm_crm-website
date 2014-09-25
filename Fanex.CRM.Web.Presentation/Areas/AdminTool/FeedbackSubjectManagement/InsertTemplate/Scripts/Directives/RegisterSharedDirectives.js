"use strict";
/* global insertTemplateApplication,
RegisterTemplate */
function insertTemplateRegisterPicker() {
    var insertTemplateRegister = new RegisterTemplate(insertTemplateApplication);
    insertTemplateRegister.registerDefaultFocus();
}

insertTemplateRegisterPicker();