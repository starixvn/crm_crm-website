'use strict';
/* exported casinoDailyApplication */
/* global RegisterTemplate */
var casinoDailyApplication = angular.module('report-casinodaily-application', ['$uiBlockerApplication']);

var casinoDailyRegister = new RegisterTemplate(casinoDailyApplication);
casinoDailyRegister.registerDatePicker();
casinoDailyRegister.registerDownloadFile();