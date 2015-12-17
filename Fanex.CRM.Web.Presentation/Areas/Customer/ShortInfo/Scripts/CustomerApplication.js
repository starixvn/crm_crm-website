/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('customer-application', []);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('customer-application'));
})();
