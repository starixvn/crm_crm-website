(function () {
    "use strict";
    
    angular.module('customerdetail-application').factory('$customerDetailService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getCustomerInfoDetailById = function (getCustomerInfoDetailById) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "CustomerSettingDetail/GetCustomerInfoDetailById", getCustomerInfoDetailById)
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

        serviceInstance.updateCustomerInfoDetail = function (updatedCustomerInfoDetailParams) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "CustomerSettingDetail/UpdateCustomerInfoDetail", updatedCustomerInfoDetailParams)
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
        serviceInstance.getAllSettings = function () {
            var deferred = $q.defer();
            $http.get(window.rootUrl + "CustomerSettingDetail/AllSettings")
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