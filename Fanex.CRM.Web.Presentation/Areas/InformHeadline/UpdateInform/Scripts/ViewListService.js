(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('updateInformApplication').factory('updateInformService',
        ['$http', '$q',
            function ($http, $q) {
                var serviceInstance = {};

                serviceInstance.getPositions = function () {
                    return [
                        { "Id": 4, "Name": "Super" },
                        { "Id": 3, "Name": "Master" },
                        { "Id": 2, "Name": "Agent" },
                        { "Id": 1, "Name": "Member" },
                        { "Id": 0, "Name": "Guest" }
                    ];
                };

                serviceInstance.getVips = function () {
                    return [
                        { "Id": 2, "Name": "All" },
                        { "Id": 1, "Name": "Yes" },
                        { "Id": 0, "Name": "No" }
                    ];
                };

                serviceInstance.getTitleDetail = function (params) {
                    var url = window.rootUrl + "InformHeadline/GetTitleDetail";
                    return sharedScriptManager.registerAngularService($http, $q, url, params);
                };

                serviceInstance.getInforms = function (params) {
                    var url = window.rootUrl + "InformHeadline/GetUpdateInforms";
                    return sharedScriptManager.registerAngularService($http, $q, url, params);
                };

                serviceInstance.getAllSettings = function () {
                    var url = window.rootUrl + 'InformHeadline/AllSettings';
                    return sharedScriptManager.registerAngularService($http, $q, url);
                };

                serviceInstance.updateInforms = function (params) {
                    var url = window.rootUrl + 'InformHeadline/UpdateInforms';
                    return sharedScriptManager.registerAngularService($http, $q, url, params);
                };

                return serviceInstance;
            }]);
})();