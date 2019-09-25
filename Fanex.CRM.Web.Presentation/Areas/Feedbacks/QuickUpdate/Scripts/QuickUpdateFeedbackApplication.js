/* global sharedScriptManager, directiveManager */
(function () {
    "use strict";
    angular.module('quick-update-feedback-application', ['$uiBlockerApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('quick-update-feedback-application'));
    directiveManager.registerRelatedCustomerResult(angular.module('quick-update-feedback-application'));

    angular.module('quick-update-feedback-application').config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 01 Jan 2000 00:00:00 GMT';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
    }]);
})();