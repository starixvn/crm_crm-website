/* global sharedScriptManager */
/* exported dailyStatisticsReportApplication */
var dailyStatisticsReportApplication = angular.module('report-dailystatistics-application',
    ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(dailyStatisticsReportApplication);