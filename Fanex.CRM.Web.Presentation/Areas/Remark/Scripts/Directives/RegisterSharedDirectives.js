(function () {
    "use strict";
    /* global RegisterTemplate */
    function remarkRegisterPicker() {
        var remarkRegister = new RegisterTemplate(angular.module('remark-application'));
        remarkRegister.registerRawHtmlDirective();
    }

    remarkRegisterPicker();
})();