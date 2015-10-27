(function () {
    "use strict";
    angular.module('dashboard-application').factory('$dashboardService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getCustomerRemarks = function (selectedCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Dashboard/GetCustomerRemarks", selectedCriteria)
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
        serviceInstance.getRedemptionLogs = function (selectedCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Dashboard/GetRedemptionLogs", selectedCriteria)
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
        serviceInstance.getCommunicationLogs = function (selectedCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Dashboard/GetCommunicationLogs", selectedCriteria)
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
        serviceInstance.updateCustomerVerifyFlag = function (selectedCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Dashboard/UpdateCustomerVerifyFlag", selectedCriteria)
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