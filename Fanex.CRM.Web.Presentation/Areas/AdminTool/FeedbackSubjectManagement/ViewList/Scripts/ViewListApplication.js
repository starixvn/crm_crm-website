/* global sharedScriptManager */
/* exported feedbackSubjectManagementApplication */
var feedbackSubjectManagementApplication = angular.module('addmintool-feedbacksubjectmanagement-application', 
    ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(feedbackSubjectManagementApplication);