'use strict';
/* global informHeadlineApplication,
sharedScriptManager */
informHeadlineApplication.factory('viewListService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getInforms = function (params) {
            var url = window.rootUrl + "InformHeadline/GetInforms";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        serviceInstance.updateInformStatus = function (params) {
            var url = window.rootUrl + "InformHeadline/UpdateInformStatus";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
