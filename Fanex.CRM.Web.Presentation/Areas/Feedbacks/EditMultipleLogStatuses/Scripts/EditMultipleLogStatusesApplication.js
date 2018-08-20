/* global sharedScriptManager, directiveManager */
(function () {
    "use strict";
    angular.module('edit-multiple-log-statuses-application', ['$uiBlockerApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('edit-multiple-log-statuses-application'));
    directiveManager.registerRelatedCustomerResult(angular.module('edit-multiple-log-statuses-application'));

    angular.module('edit-multiple-log-statuses-application').config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 01 Jan 2000 00:00:00 GMT';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
    }]);
})();