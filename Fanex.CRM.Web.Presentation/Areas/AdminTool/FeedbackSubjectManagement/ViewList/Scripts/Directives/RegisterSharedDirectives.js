(function () {
    "use strict";
    /* global RegisterTemplate */
    function feedbackSubjectManagementRegisterPicker() {
        var feedbackSubjectManagementRegister = new RegisterTemplate(angular.module('addmintool-feedbacksubjectmanagement-application'));
        feedbackSubjectManagementRegister.registerSubjectAutoComplete();
        feedbackSubjectManagementRegister.registerDefaultFocus();
    }

    feedbackSubjectManagementRegisterPicker();
})();