/* global sharedScriptManager, RegisterTemplate */
/* exported viewListApplication */
var viewListApplication = angular.module('inform-headline-application',
    ['$uiBlockerApplication', '$resultMessageApplication', 'fanex.dropdowntree']);
sharedScriptManager.setNoCachingHttpRequest(viewListApplication);
var viewListRegister = new RegisterTemplate(viewListApplication);
viewListRegister.registerDownloadFile();