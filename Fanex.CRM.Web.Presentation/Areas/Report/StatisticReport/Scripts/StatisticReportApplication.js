/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('report-statisticreport-application', ['$uiBlockerApplication', '$resultMessageApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('report-statisticreport-application'));
    sharedScriptManager.setCommonHeaderHttpRequest(angular.module('report-statisticreport-application'));
})();