(function (angular) {
    'use strict';
    /* global RegisterTemplate */

    angular.module('manageFeedbackModule', ['$uiBlockerApplication', '$resultMessageApplication']);

    let manageFeedbackRegister = new RegisterTemplate(angular.module('manageFeedbackModule'));
    manageFeedbackRegister.registerDatePicker();
    manageFeedbackRegister.registerMainCategoryPicker();
    manageFeedbackRegister.registerSubCategoryPicker();
    manageFeedbackRegister.registerDownloadFile();
})(angular);