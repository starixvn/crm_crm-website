/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('addmintool-settingmanagement-application',
        ['$uiBlockerApplication', '$resultMessageApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('addmintool-settingmanagement-application'));
})();