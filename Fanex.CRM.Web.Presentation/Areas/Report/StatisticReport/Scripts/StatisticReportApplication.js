/* global sharedScriptManager */
(function () {
    "use strict";
    var statisticReportApplication = angular.module('report-statisticreport-application',
    ['$uiBlockerApplication', '$resultMessageApplication']);
    sharedScriptManager.setNoCachingHttpRequest(statisticReportApplication);
    sharedScriptManager.setCommonHeaderHttpRequest(statisticReportApplication);
})();