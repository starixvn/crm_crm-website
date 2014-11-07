"use strict";
/* global insertSubSettingApplication,
RegisterTemplate,
directiveManager */
function insertSubSettingRegisterPicker() {
    var insertSubSettingRegister = new RegisterTemplate(insertSubSettingApplication);
    insertSubSettingRegister.registerDefaultFocus();

    var parentSettingRegisterParams = {
        PlugInName: 'parentSettingContainer',
        OptionName: 'parentSettingOptions',
        PickerId: 'parentsetting',
        RestrictType: 'C',

        DataPropertyName: {
            Id: "Id",
            Type: "SettingType",
            Value: "Value"
        }
    };

    directiveManager.registerParentSettingContainer(insertSubSettingApplication, parentSettingRegisterParams);
}

insertSubSettingRegisterPicker();