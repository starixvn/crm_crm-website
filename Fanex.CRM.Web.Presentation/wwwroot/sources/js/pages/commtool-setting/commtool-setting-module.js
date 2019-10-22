(function (angular) {
    'use strict';
    /* global sharedScriptManager */

    angular.module('commToolSettingModule', ['$uiBlockerApplication', '$resultMessageApplication', 'ui.bootstrap', 'multileveldropdown']);

    sharedScriptManager.setNoCachingHttpRequest(angular.module('commToolSettingModule'));
})(angular);