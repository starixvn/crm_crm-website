(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function insertSettingRegisterPicker() {
        var insertSettingRegister = new RegisterTemplate(angular.module('addmintool-settingmanagement-insertnewsetting-application'));
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

        directiveManager.registerParentSettingContainer(
            angular.module('addmintool-settingmanagement-insertnewsetting-application'), 
            parentSettingRegisterParams);
    }

    insertSettingRegisterPicker();
})();