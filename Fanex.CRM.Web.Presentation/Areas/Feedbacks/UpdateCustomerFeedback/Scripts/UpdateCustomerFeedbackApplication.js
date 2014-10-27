/* exported updateCustomerFeedbackApplication */
/* global sharedScriptManager, RegisterTemplate */
var updateCustomerFeedbackApplication = angular.module('feedbacks-updatecustomerfeedback-application', ['$uiBlockerApplication', 'alertBoxApplication']);
sharedScriptManager.setNoCachingHttpRequest(updateCustomerFeedbackApplication);

var updateCustomerFeedbackRegister = new RegisterTemplate(updateCustomerFeedbackApplication);
updateCustomerFeedbackRegister.registerSubjectAutoComplete();
updateCustomerFeedbackRegister.registerDescriptionHtml();
updateCustomerFeedbackRegister.registerSolutionHtml();
updateCustomerFeedbackRegister.registerReviewHtml();
updateCustomerFeedbackRegister.registerRawHtmlDirective();