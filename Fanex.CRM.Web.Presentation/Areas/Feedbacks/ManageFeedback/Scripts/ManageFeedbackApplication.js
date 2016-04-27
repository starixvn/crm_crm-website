/* global RegisterTemplate */
(function () {
    "use strict";
    angular.module('feedback-manage-application', ['$uiBlockerApplication']);

    var manageFBRegister = new RegisterTemplate(angular.module('feedback-manage-application'));
    manageFBRegister.registerDatePicker();
    manageFBRegister.registerMainCategoryPicker();
    manageFBRegister.registerSubCategoryPicker();
    manageFBRegister.registerDownloadFile();
})();