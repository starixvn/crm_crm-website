/* global sharedScriptManager */
/* exported communicationApplication */
var communicationApplication = angular.module('communication-application', []);
sharedScriptManager.setNoCachingHttpRequest(communicationApplication);