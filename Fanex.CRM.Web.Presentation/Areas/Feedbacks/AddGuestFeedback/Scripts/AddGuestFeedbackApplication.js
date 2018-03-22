/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('feedbacks-addguestfeedback-application', ['$uiBlockerApplication', '$resultMessageApplication', 'ngSanitize']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('feedbacks-addguestfeedback-application'));
})();