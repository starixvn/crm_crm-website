/* global sharedScriptManager, RegisterTemplate */
/* exported informHeadlineApplication */
var informHeadlineApplication = angular.module('informHeadlineApplication',
    ['$uiBlockerApplication', '$resultMessageApplication']);
sharedScriptManager.setNoCachingHttpRequest(informHeadlineApplication);
var viewListRegister = new RegisterTemplate(informHeadlineApplication);
viewListRegister.registerDownloadFile();