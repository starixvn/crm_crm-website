(function () {
    "use strict";
    /* global RegisterTemplate */
    function editSettingRegisterPicker() {
        var editSettingRegister = new RegisterTemplate(angular.module('addmintool-settingmanagement-application'));
        editSettingRegister.registerDefaultFocus();
    }

    editSettingRegisterPicker();
})();