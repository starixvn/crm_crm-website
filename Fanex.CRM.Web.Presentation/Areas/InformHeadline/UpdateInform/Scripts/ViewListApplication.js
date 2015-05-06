/* global sharedScriptManager, RegisterTemplate */
/* exported updateInformApplication */
"use strict";
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
} ]);
updateInformApplication.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 01 Jan 2000 00:00:00 GMT';
    $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
} ]);