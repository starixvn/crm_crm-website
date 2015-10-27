(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function insertSubSettingRegisterPicker() {
        var insertSubSettingRegister = new RegisterTemplate(angular.module('addmintool-settingmanagement-insertsubsetting-application'));
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

        directiveManager.registerParentSettingContainer(angular.module('addmintool-settingmanagement-insertsubsetting-application'), parentSettingRegisterParams);
    }

    insertSubSettingRegisterPicker();
})();