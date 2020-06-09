(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function picRegisterPicker() {
        var picRegister = new RegisterTemplate(angular.module('pic-management-application'));
        picRegister.registerRolePicker();
        picRegister.registerDownloadFile();
    }

    picRegisterPicker();
})();