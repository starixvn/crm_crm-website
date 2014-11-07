"use strict";
/* global insertSettingApplication,
RegisterTemplate,
directiveManager */
function insertSettingRegisterPicker() {
    var insertSettingRegister = new RegisterTemplate(insertSettingApplication);
    insertSettingRegister.registerDefaultFocus();

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

    directiveManager.registerParentSettingContainer(insertSettingApplication, parentSettingRegisterParams);
}

insertSettingRegisterPicker();