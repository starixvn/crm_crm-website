"use strict";
/* global viewListApplication,
RegisterTemplate,
directiveManager */
function viewListRegisterPicker() {
    var viewListRegister = new RegisterTemplate(viewListApplication);
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

    directiveManager.registerDropdownContainer(viewListApplication, mainSettingRegisterParams);
    viewListRegister.registerSiteMultiSelectPicker();
}

viewListRegisterPicker();