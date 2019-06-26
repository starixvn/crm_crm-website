(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function viewListRegisterPicker() {
        var viewListRegister = new RegisterTemplate(angular.module('addmintool-settingrelation-application'));
        viewListRegister.registerDefaultFocus();

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

        directiveManager.registerDropdownContainer(angular.module('addmintool-settingrelation-application'), mainSettingRegisterParams);
    }

    viewListRegisterPicker();
})();