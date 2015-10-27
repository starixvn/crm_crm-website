(function () {
    "use strict";
    /* global RegisterTemplate */
    function editTemplateRegisterPicker() {
        var editTemplateRegister = new RegisterTemplate(angular.module('addmintool-feedbacksubjectmanagement-edittemplate'));
        editTemplateRegister.registerDefaultFocus();
    }

    editTemplateRegisterPicker();
})();