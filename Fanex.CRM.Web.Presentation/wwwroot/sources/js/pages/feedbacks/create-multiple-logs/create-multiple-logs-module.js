(function (angular) {
    'use strict';
    /* global sharedScriptManager, directiveManager */

    angular.module('createMultipleLogsModule', ['ui.bootstrap', '$uiBlockerApplication', 'selectwithsearch', 'multileveldropdown', 'focusMeModule']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('createMultipleLogsModule'));

    angular.module('createMultipleLogsModule').config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 01 Jan 2000 00:00:00 GMT';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
    }]);
})(angular);