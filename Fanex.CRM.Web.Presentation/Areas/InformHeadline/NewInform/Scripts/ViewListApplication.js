/* global sharedScriptManager, RegisterTemplate */
/* exported newInformApplication */
var newInformApplication = angular.module('newInformApplication',
    ['$uiBlockerApplication', '$resultMessageApplication', 'fanex.dropdowntree']);
sharedScriptManager.setNoCachingHttpRequest(newInformApplication);
var viewListRegister = new RegisterTemplate(newInformApplication);
viewListRegister.registerDownloadFile();