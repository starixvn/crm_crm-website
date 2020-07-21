(function () {
    "use strict";
    function changeLogRegisterPicker() {
        var changeLogRegister = new RegisterTemplate(angular.module('change-log-application'));
        changeLogRegister.registerDatePicker();
    }

    changeLogRegisterPicker();
})();