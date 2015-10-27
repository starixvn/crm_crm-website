/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('report-settlementreport-application', ['$uiBlockerApplication', '$resultMessageApplication']);
    sharedScriptManager.setNoCachingHttpRequest(settlementReportApplication);
    sharedScriptManager.setCommonHeaderHttpRequest(settlementReportApplication);
})();