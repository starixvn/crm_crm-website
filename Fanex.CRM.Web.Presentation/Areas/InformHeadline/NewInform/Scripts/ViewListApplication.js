/* global sharedScriptManager, RegisterTemplate */
/* exported viewListApplication */
var viewListApplication = angular.module('inform-headline-application',
    ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(viewListApplication);
var viewListRegister = new RegisterTemplate(viewListApplication);
viewListRegister.registerDownloadFile();