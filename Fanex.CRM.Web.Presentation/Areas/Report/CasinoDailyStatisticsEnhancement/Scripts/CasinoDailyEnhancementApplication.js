'use strict';
/* exported casinoDailyApplication */
/* global RegisterTemplate */
var casinoDailyEnhancementApplication = angular.module('report-casinodaily-enhancement-application', ['$uiBlockerApplication']);

var casinoDailyRegister = new RegisterTemplate(casinoDailyEnhancementApplication);
casinoDailyRegister.registerDatePicker();
casinoDailyRegister.registerDownloadFile();