/* global sharedScriptManager */
/* exported settlementReportApplication */
var settlementReportApplication = angular.module('report-settlementreport-application',
    ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(settlementReportApplication);
sharedScriptManager.setCommonHeaderHttpRequest(settlementReportApplication);