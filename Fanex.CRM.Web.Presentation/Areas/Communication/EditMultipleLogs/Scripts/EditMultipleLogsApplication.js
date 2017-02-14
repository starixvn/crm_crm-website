/* global sharedScriptManager, directiveManager */
(function () {
    "use strict";
    angular.module('edit-multiple-logs-application', ['$uiBlockerApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('edit-multiple-logs-application'));
    directiveManager.registerRelatedCustomerResult(angular.module('edit-multiple-logs-application'));
})();