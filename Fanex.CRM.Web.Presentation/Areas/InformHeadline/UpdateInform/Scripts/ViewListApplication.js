/* global sharedScriptManager, RegisterTemplate */
(function () {
    "use strict";
    angular.module('updateInformApplication',
    ['$uiBlockerApplication', '$resultMessageApplication', 'fanex.dropdowntree']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('updateInformApplication'));
    var viewListRegister = new RegisterTemplate(angular.module('updateInformApplication'));
    viewListRegister.registerDownloadFile();

    angular.module('updateInformApplication').config(['dropdowntreeConfigProvider', function (dropdowntreeConfigProvider) {
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
    angular.module('updateInformApplication').config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 01 Jan 2000 00:00:00 GMT';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
    } ]);
})();