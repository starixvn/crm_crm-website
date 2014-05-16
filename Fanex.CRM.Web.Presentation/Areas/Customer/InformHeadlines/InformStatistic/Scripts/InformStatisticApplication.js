/* global sharedScriptManager */
/* exported informHeadlineStatisticApplication */
var informHeadlineStatisticApplication = angular.module('customer-informheadline-statistic', ['$uiBlockerApplication']);
sharedScriptManager.setNoCachingHttpRequest(informHeadlineStatisticApplication);

var informStatisticRegister = new RegisterTemplate(informHeadlineStatisticApplication);
informStatisticRegister.registerDefaultFocus();
informStatisticRegister.registerDownloadFile();