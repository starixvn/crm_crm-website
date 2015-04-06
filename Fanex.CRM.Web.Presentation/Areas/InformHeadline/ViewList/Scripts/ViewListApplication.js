/* global sharedScriptManager */
/* exported viewListApplication */
var viewListApplication = angular.module('inform-headline-application',
    ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(viewListApplication);