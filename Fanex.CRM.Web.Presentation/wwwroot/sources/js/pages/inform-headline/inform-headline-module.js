(function (angular) {
    'use strict';
    /* global sharedScriptManager, RegisterTemplate */

    angular.module('informHeadlineModule', ['$uiBlockerApplication', '$resultMessageApplication', 'ui.bootstrap']);

    sharedScriptManager.setNoCachingHttpRequest(angular.module('informHeadlineModule'));
    let viewListRegister = new RegisterTemplate(angular.module('informHeadlineModule'));
    viewListRegister.registerDownloadFile();
})(angular);