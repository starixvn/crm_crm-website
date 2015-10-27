(function () {
    "use strict";
    /* global RegisterTemplate */
    function insertTemplateRegisterPicker() {
        var insertTemplateRegister = new RegisterTemplate(angular.module('addmintool-feedbacksubjectmanagement-inserttemplate'));
        insertTemplateRegister.registerDefaultFocus();
    }

    insertTemplateRegisterPicker();
})();