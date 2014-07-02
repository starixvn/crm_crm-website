/* global sharedScriptManager,
RegisterTemplate */
/* exported informHeadlineStatisticApplication */
var informHeadlineStatisticApplication = angular.module('customer-informheadline-statistic', ['$uiBlockerApplication', 'alertBoxApplication']);
sharedScriptManager.setNoCachingHttpRequest(informHeadlineStatisticApplication);

var informStatisticRegister = new RegisterTemplate(informHeadlineStatisticApplication);
informStatisticRegister.registerDefaultFocus();
informStatisticRegister.registerDownloadFile();