"use strict";
/* global addRelationApplication,
RegisterTemplate,
directiveManager */
function addRelationRegisterPicker() {
    var addRelationRegister = new RegisterTemplate(addRelationApplication);
    addRelationRegister.registerDefaultFocus();

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

    directiveManager.registerDropdownContainer(addRelationApplication, mainSettingRegisterParams);
}

addRelationRegisterPicker();