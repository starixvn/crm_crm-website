/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('feedbacks-updatecustomerfeedback-application', ['$uiBlockerApplication', 'alertBoxApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('feedbacks-updatecustomerfeedback-application'));
})();