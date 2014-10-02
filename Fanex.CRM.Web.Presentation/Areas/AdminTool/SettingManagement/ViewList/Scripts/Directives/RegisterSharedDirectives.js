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
            Id: "Type",
            Name: "Type"
        }
    };

    directiveManager.registerDropdownContainer(viewListApplication, mainSettingRegisterParams);
}

viewListRegisterPicker();