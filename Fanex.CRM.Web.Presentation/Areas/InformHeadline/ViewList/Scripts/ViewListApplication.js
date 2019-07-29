/* global sharedScriptManager, RegisterTemplate */
(function () {
    "use strict";
    angular.module('informHeadlineApplication',
        ['$uiBlockerApplication', '$resultMessageApplication', 'ui.bootstrap']);

    sharedScriptManager.setNoCachingHttpRequest(angular.module('informHeadlineApplication'));
    var viewListRegister = new RegisterTemplate(angular.module('informHeadlineApplication'));
    viewListRegister.registerDownloadFile();
})();