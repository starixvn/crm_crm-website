(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function viewListRegisterPicker() {
        var viewListRegister = new RegisterTemplate(angular.module('addmintool-settingmanagement-application'));
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

        directiveManager.registerDropdownContainer(angular.module('addmintool-settingmanagement-application'), mainSettingRegisterParams);
    }

    viewListRegisterPicker();
})();