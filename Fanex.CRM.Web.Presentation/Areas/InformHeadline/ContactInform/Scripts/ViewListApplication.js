/* global sharedScriptManager, RegisterTemplate */
/* exported contactInformApplication */
var contactInformApplication = angular.module('contactInformApplication',
    ['$uiBlockerApplication', '$resultMessageApplication', 'fanex.dropdowntree']);
sharedScriptManager.setNoCachingHttpRequest(contactInformApplication);
var viewListRegister = new RegisterTemplate(contactInformApplication);
viewListRegister.registerDownloadFile();
contactInformApplication.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 01 Jan 2000 00:00:00 GMT';
    $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
} ]);