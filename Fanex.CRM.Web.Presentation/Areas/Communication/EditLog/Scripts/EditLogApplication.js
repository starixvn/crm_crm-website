/* global sharedScriptManager, directiveManager */
(function () {
    "use strict";
    angular.module('communicationlog-edit-application', ['$uiBlockerApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('communicationlog-edit-application'));
    directiveManager.registerRelatedCustomerResult(angular.module('communicationlog-edit-application'));
})();