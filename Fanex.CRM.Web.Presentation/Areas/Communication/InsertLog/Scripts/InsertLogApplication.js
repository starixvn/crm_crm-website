/*global sharedScriptManager*/
(function () {
    "use strict";
    angular.module('communicationlog-insert-application', ['$uiBlockerApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('communicationlog-insert-application'));
    directiveManager.registerRelatedCustomerResult(angular.module('communicationlog-insert-application'));
})();