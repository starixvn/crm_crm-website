/* exported updateCustomerFeedbackApplication */
/* global sharedScriptManager */
var updateCustomerFeedbackApplication = angular.module('feedbacks-updatecustomerfeedback-application', ['$uiBlockerApplication', 'alertBoxApplication']);
sharedScriptManager.setNoCachingHttpRequest(updateCustomerFeedbackApplication);