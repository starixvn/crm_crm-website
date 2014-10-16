'use strict';
/* exported fbStatisticDetailApplication */
/* global RegisterTemplate */
var fbStatisticDetailApplication = angular.module('feedback-statistic-detail-application', ['$uiBlockerApplication']);

var fbStatisticRegister = new RegisterTemplate(fbStatisticDetailApplication);
fbStatisticRegister.registerDownloadFile();