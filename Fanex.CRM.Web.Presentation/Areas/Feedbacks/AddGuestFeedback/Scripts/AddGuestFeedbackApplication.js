/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('feedbacks-addguestfeedback-application', ['$uiBlockerApplication', '$resultMessageApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('feedbacks-addguestfeedback-application'));
})();