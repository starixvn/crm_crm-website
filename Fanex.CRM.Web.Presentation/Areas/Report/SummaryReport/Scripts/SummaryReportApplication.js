'use strict';
/* global sharedScriptManager */
/* exported summaryReportApplication */
var summaryReportApplication = angular.module('report-summaryreport-application', ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setCommonHeaderHttpRequest(summaryReportApplication);