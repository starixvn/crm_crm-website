'use strict';
/* exported casinoWeeklyApplication */
/* global RegisterTemplate */
var casinoWeeklyApplication = angular.module('report-casinoweekly-application', ['$uiBlockerApplication']);

var casinoWeeklyRegister = new RegisterTemplate(casinoWeeklyApplication);
casinoWeeklyRegister.registerDownloadFile();