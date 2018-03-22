/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('communicationlog-viewdetailbyid-application', ['$uiBlockerApplication', 'ngSanitize']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('communicationlog-viewdetailbyid-application'));
})();