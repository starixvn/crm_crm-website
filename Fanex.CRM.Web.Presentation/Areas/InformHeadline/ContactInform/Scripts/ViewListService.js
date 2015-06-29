'use strict';
/* global contactInformApplication,
sharedScriptManager */
contactInformApplication.factory('updateInformService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getPositions = function () {
            return [
                { "Id": 4, "Name": "Super" },
                { "Id": 3, "Name": "Master" },
                { "Id": 2, "Name": "Agent" },
                { "Id": 1, "Name": "Member" }
            ];
        };

        serviceInstance.getTitleDetail = function (params) {
            var url = window.rootUrl + "InformHeadline/GetTitleDetail";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        serviceInstance.getInforms = function (params) {
            var url = window.rootUrl + "InformHeadline/GetContactInforms";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        serviceInstance.getSettings = function () {
            var url = window.rootUrl + 'InformHeadline/ContactInformSettings';
            return sharedScriptManager.registerAngularService($http, $q, url);
        };

        serviceInstance.updateInforms = function (params) {
            var url = window.rootUrl + 'InformHeadline/UpdateContentInforms';
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
