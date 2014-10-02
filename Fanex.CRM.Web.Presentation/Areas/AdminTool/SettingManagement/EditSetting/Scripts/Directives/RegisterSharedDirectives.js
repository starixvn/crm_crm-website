"use strict";
/* global editSettingApplication,
RegisterTemplate */
function editSettingRegisterPicker() {
    var editSettingRegister = new RegisterTemplate(editSettingApplication);
    editSettingRegister.registerDefaultFocus();
}

editSettingRegisterPicker();