/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('feedbacks-addcustomerfeedback-application', ['ui.bootstrap', '$uiBlockerApplication', '$resultMessageApplication', 'ngSanitize', 'multileveldropdown']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('feedbacks-addcustomerfeedback-application'));

    angular.module('feedbacks-addcustomerfeedback-application').config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 01 Jan 2000 00:00:00 GMT';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
    }]);
})();