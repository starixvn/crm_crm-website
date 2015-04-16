"use strict";
/* global informHeadlineApplication,
RegisterTemplate,
directiveManager */
function viewListRegisterPicker() {
    var viewListRegister = new RegisterTemplate(informHeadlineApplication);
    viewListRegister.registerDefaultFocus();

    var mainSettingRegisterParams = {
        PlugInName: 'mainSettingContainer',
        OptionName: 'mainSettingOptions',
        PickerId: 'mainsetting',
        RestrictType: 'C',

        DataPropertyName: {
            Id: "SettingType",
            Name: "SettingType"
        }
    };

    directiveManager.registerDropdownContainer(informHeadlineApplication, mainSettingRegisterParams);
}

viewListRegisterPicker();