/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('addmintool-settingrelation-application',
        ['$uiBlockerApplication', '$resultMessageApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('addmintool-settingrelation-application'));
})();