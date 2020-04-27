(function (angular) {
    "use strict";
    angular.module('addmintool-settingmanagement-application', ['$uiBlockerApplication', '$resultMessageApplication', 'ui.bootstrap', 'multileveldropdown']);

    sharedScriptManager.setNoCachingHttpRequest(angular.module('addmintool-settingmanagement-application'));
})(angular);