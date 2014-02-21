/* global sharedScriptManager */
/* exported customerApplication */
var customerApplication = angular.module('customer-application', []);
sharedScriptManager.setNoCachingHttpRequest(customerApplication);
