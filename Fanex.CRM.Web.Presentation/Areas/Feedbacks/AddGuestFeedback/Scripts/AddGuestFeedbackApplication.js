/* global sharedScriptManager */
/* exported addGuestFeedbackApplication */
var addGuestFeedbackApplication = angular.module('feedbacks-addguestfeedback-application', ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(addGuestFeedbackApplication);