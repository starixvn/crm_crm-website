/*global sharedScriptManager*/
(function () {
    "use strict";
    angular.module('communicationlog-insert-application', ['ui.bootstrap', '$uiBlockerApplication', 'selectwithsearch', 'multileveldropdown']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('communicationlog-insert-application'));
    directiveManager.registerRelatedCustomerResult(angular.module('communicationlog-insert-application'));

    angular.module('communicationlog-insert-application').config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 01 Jan 2000 00:00:00 GMT';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
    }]);
})();