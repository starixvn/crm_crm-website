/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('admintool-settingmanagement-editsetting-application',
        ['$uiBlockerApplication', '$resultMessageApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('admintool-settingmanagement-editsetting-application'));
})();