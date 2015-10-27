(function () {
    "use strict";
    /* global RegisterTemplate */
    function editSettingRegisterPicker() {
        var editSettingRegister = new RegisterTemplate(angular.module('addmintool-settingmanagement-editsetting-application'));
        editSettingRegister.registerDefaultFocus();
    }

    editSettingRegisterPicker();
})();