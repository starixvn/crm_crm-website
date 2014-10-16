/* global sharedScriptManager */
/* exported weeklyStatisticsReportApplication */
var weeklyStatisticsReportApplication = angular.module('report-weeklystatistics-application',
    ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(weeklyStatisticsReportApplication);