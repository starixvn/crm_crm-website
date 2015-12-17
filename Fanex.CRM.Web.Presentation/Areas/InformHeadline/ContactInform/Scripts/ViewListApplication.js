/* global sharedScriptManager, RegisterTemplate */
(function () {
    "use strict";
    angular.module('contactInformApplication',
    ['$uiBlockerApplication', '$resultMessageApplication', 'fanex.dropdowntree', 'ngSanitize']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('contactInformApplication'));
    var viewListRegister = new RegisterTemplate(angular.module('contactInformApplication'));
    viewListRegister.registerDownloadFile();

    angular.module('contactInformApplication').config(['dropdowntreeConfigProvider', function (dropdowntreeConfigProvider) {
        dropdowntreeConfigProvider.set({
            idMember: "SettingId",
            nameMember: "SettingValue",
            selectedMember: "SettingId",
            width: 200,
            checkedAll: true,
            templateUrl: window.rootUrl + "Scripts/Directives/dropdowntree/fanex.directive.dropdowntree.template.html"
        });
    } ]);
    angular.module('contactInformApplication').config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.headers.get['If-Modified-Since'] = 'Sat, 01 Jan 2000 00:00:00 GMT';
        $httpProvider.defaults.headers.get['Cache-Control'] = 'no-cache';
        $httpProvider.defaults.headers.get['Pragma'] = 'no-cache';
    } ]);
})();