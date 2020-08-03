/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('feedbacks-updatecustomerfeedback-application', ['ui.bootstrap', '$uiBlockerApplication', 'alertBoxApplication', 'ngSanitize', 'multileveldropdown']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('feedbacks-updatecustomerfeedback-application'));

    angular.module('feedbacks-updatecustomerfeedback-application').config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 01 Jan 2000 00:00:00 GMT';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
    }]);
})();