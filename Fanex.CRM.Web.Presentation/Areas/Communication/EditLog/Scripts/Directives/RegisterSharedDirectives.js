(function () {
    "use strict";
    /* global RegisterTemplate */
    function editCommunicationLogRegisterPicker() {
        var editCommunicationLogRegister = new RegisterTemplate(angular.module('communicationlog-edit-application'));
        editCommunicationLogRegister.registerLicenseeTypePicker();
    }

    editCommunicationLogRegisterPicker();
})();