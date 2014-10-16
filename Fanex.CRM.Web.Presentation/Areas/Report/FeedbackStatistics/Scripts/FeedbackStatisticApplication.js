'use strict';
/* exported fbStatisticApplication */
/* global RegisterTemplate */
var fbStatisticApplication = angular.module('feedback-statistic-application', ['$uiBlockerApplication', 'alertBoxApplication']);

var fbStatisticRegister = new RegisterTemplate(fbStatisticApplication);
fbStatisticRegister.registerDatePicker();
fbStatisticRegister.registerDownloadFile();