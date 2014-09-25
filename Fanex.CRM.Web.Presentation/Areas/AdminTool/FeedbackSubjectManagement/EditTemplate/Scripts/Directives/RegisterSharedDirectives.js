"use strict";
/* global editTemplateApplication,
RegisterTemplate */
function editTemplateRegisterPicker() {
    var editTemplateRegister = new RegisterTemplate(editTemplateApplication);
    editTemplateRegister.registerDefaultFocus();
}

editTemplateRegisterPicker();