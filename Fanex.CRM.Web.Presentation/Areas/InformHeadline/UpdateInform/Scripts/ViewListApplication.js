/* global sharedScriptManager, RegisterTemplate */
/* exported updateInformApplication */
var updateInformApplication = angular.module('updateInformApplication',
    ['$uiBlockerApplication', '$resultMessageApplication', 'fanex.dropdowntree']);
sharedScriptManager.setNoCachingHttpRequest(updateInformApplication);
var viewListRegister = new RegisterTemplate(updateInformApplication);
viewListRegister.registerDownloadFile();

updateInformApplication.config(['dropdowntreeConfigProvider', function(dropdowntreeConfigProvider){
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