/* global RegisterTemplate */
(function () {
    "use strict";
    angular.module('deleted-feedback-application', ['$uiBlockerApplication']);

    var deletedFBRegister = new RegisterTemplate(angular.module('deleted-feedback-application'));
    deletedFBRegister.registerDatePicker();
    deletedFBRegister.registerMainCategoryPicker();
    deletedFBRegister.registerSubCategoryPicker();
})();