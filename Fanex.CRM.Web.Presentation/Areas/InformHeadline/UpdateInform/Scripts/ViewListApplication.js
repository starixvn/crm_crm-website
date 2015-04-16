/* global sharedScriptManager, RegisterTemplate */
/* exported updateInformApplication */
var updateInformApplication = angular.module('updateInformApplication',
    ['$uiBlockerApplication', '$resultMessageApplication', 'fanex.dropdowntree']);
sharedScriptManager.setNoCachingHttpRequest(updateInformApplication);
var viewListRegister = new RegisterTemplate(updateInformApplication);
viewListRegister.registerDownloadFile();