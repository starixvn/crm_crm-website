/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('feedbacks-addcustomerfeedback-application', ['$uiBlockerApplication', '$resultMessageApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('feedbacks-addcustomerfeedback-application'));
})();