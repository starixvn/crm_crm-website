(function () {
    "use strict";
    angular.module('communication-application').factory('$communicationService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getLogsByCustomerId = function (getLogByCustomerIdParams) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "CommunicationLog/GetLogsByCustomerId", getLogByCustomerIdParams)
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

        serviceInstance.getCustomerInfo = function (selectedCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Dashboard/GetCustomerInfo", selectedCriteria)
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

        serviceInstance.getAllSite = function () {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Setting/AllSite")
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

        serviceInstance.getAllStatus = function () {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "CommunicationLog/AllStatus", null)
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
})();