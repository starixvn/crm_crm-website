'use strict';
/* global sharedScriptManager */
/* exported wagerReportApplication */
var wagerReportApplication = angular.module('report-wagerreport-application', ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setCommonHeaderHttpRequest(wagerReportApplication);