/* global RegisterTemplate */
(function () {
    "use strict";
    angular.module('feedback-latest-application', ['$uiBlockerApplication']);

    var latestFeedbackRegister = new RegisterTemplate(angular.module('feedback-latest-application'));
    latestFeedbackRegister.registerDownloadFile();
})();