/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('remark-application', ['$uiBlockerApplication', 'alertBoxApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('remark-application'));
})();