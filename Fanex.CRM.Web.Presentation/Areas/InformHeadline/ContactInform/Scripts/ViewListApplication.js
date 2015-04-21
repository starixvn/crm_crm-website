/* global sharedScriptManager, RegisterTemplate */
/* exported contactInformApplication */
var contactInformApplication = angular.module('contactInformApplication',
    ['$uiBlockerApplication', '$resultMessageApplication', 'fanex.dropdowntree']);
sharedScriptManager.setNoCachingHttpRequest(contactInformApplication);
var viewListRegister = new RegisterTemplate(contactInformApplication);
viewListRegister.registerDownloadFile();

contactInformApplication.config(['dropdowntreeConfigProvider', function (dropdowntreeConfigProvider) {
    dropdowntreeConfigProvider.set({
        hasFilter: false,
        idMember: "Id",
        nameMember: "Name",
        selectedMember: "Id",
        sourceData: null,
        width: 150,
        templateUrl: window.rootUrl + "Scripts/Directives/dropdowntree/fanex.directive.dropdowntree.template.html"        
    });
}]);	