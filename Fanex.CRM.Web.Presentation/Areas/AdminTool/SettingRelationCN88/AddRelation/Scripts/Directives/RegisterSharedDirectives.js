(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function addRelationRegisterPicker() {
        var addRelationRegister = new RegisterTemplate(angular.module('addmintool-settingrelation-addrelation-application'));
        addRelationRegister.registerDefaultFocus();

        var mainSettingRegisterParams = {
            PlugInName: 'mainSettingContainer',
            OptionName: 'mainSettingOptions',
            PickerId: 'mainsetting',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "SettingNameEn",
                Name: "SettingName"
            }
        };

        directiveManager.registerDropdownContainer(angular.module('addmintool-settingrelation-addrelation-application'), mainSettingRegisterParams);
    }

    addRelationRegisterPicker();
})();