/* global sharedScriptManager, RegisterTemplate */
/* exported newInformApplication */
var newInformApplication = angular.module('newInformApplication',
    ['$uiBlockerApplication', '$resultMessageApplication', 'fanex.dropdowntree']);
sharedScriptManager.setNoCachingHttpRequest(newInformApplication);
var viewListRegister = new RegisterTemplate(newInformApplication);
viewListRegister.registerDownloadFile();

newInformApplication.config(['dropdowntreeConfigProvider', function (dropdowntreeConfigProvider) {
    dropdowntreeConfigProvider.set({
        hasFilter: false,
        idMember: "Id",
        nameMember: "Name",
        selectedMember: "Id",
        sourceData: null,
        width: 150,
        templateUrl: window.rootUrl + "Scripts/Directives/dropdowntree/fanex.directive.dropdowntree.template.html"
    });
} ]);
newInformApplication.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 01 Jan 2000 00:00:00 GMT';
    $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.headers.get['Pragma'] = 'no-cache'
}]);