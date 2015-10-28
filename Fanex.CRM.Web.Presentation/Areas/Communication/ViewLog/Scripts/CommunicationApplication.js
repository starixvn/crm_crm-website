/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('communication-application', []);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('communication-application'));
})();