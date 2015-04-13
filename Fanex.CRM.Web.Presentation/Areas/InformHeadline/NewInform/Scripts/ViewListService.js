'use strict';
/* global viewListApplication,
sharedScriptManager */
viewListApplication.factory('viewListService',
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

        serviceInstance.getDirectAccounts = function (params) {
            var url = window.rootUrl + "InformHeadline/GetDirectAccounts";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        serviceInstance.updateInformStatus = function (params) {
            var url = window.rootUrl + "InformHeadline/UpdateInformStatus";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        serviceInstance.getAllSettings = function () {
            var deferred = $q.defer();
            $http.get(window.rootUrl + 'InformHeadline/GetAllSettings')
            .success(function (data, status, headers, config) {
                deferred.resolve({
                    Result: data,
                    Status: status,
                    Headers: headers,
                    Config: config
                });
            }).error(function (data, status, headers, config) {
                deferred.reject({
                    Result: data,
                    Status: status,
                    Headers: headers,
                    Config: config
                });
            });

            return deferred.promise;
        };

        return serviceInstance;
    } ]);
