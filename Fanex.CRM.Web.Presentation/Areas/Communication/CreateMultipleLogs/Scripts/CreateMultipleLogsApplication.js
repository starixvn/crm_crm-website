/* global sharedScriptManager, directiveManager */
(function () {
    "use strict";
    angular.module('create-multiple-logs-application', ['$uiBlockerApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('create-multiple-logs-application'));
    directiveManager.registerRelatedCustomerResult(angular.module('create-multiple-logs-application'));
})();