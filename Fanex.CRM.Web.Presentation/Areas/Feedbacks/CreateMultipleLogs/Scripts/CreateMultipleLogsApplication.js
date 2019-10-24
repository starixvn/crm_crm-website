/* global sharedScriptManager, directiveManager */
(function () {
    "use strict";
    angular.module('create-multiple-logs-application', ['ui.bootstrap', '$uiBlockerApplication', 'selectwithsearch', 'multileveldropdown']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('create-multiple-logs-application'));
    directiveManager.registerRelatedCustomerResult(angular.module('create-multiple-logs-application'));

    angular.module('create-multiple-logs-application').config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 01 Jan 2000 00:00:00 GMT';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
    }]);
})();