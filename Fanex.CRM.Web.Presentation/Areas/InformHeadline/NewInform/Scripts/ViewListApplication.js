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