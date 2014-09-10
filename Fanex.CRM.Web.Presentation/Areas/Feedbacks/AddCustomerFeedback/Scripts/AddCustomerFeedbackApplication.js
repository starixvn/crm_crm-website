/* global sharedScriptManager */
/* exported addCustomerFeedbackApplication */
var addCustomerFeedbackApplication = angular.module('feedbacks-addcustomerfeedback-application', ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(addCustomerFeedbackApplication);