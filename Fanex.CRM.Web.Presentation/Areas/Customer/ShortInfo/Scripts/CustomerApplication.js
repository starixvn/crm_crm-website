/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('customer-application', ['selectwithsearch']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('customer-application'));

    angular.module('customer-application').config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 01 Jan 2000 00:00:00 GMT';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
    }]);
})();
