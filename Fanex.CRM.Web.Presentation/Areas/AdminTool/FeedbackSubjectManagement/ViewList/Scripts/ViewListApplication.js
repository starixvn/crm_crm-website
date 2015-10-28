/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('addmintool-feedbacksubjectmanagement-application',
        ['$uiBlockerApplication', '$resultMessageApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('addmintool-feedbacksubjectmanagement-application'));
})();