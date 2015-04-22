/* global sharedScriptManager, RegisterTemplate */
/* exported contactInformApplication */
var contactInformApplication = angular.module('contactInformApplication',
    ['$uiBlockerApplication', '$resultMessageApplication', 'fanex.dropdowntree']);
sharedScriptManager.setNoCachingHttpRequest(contactInformApplication);
var viewListRegister = new RegisterTemplate(contactInformApplication);
viewListRegister.registerDownloadFile();
