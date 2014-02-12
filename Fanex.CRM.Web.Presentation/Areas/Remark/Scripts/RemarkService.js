"use strict";
/* global remarkApplication */
remarkApplication.factory('$remarkService', function ($http, $q) {
    var serviceInstance = {};

    serviceInstance.getRemarks = function (customerId) {
        var deferred = $q.defer();
        $http.get(window.rootUrl + 'Remark/GetRemarks?customerId=' + customerId)
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

    serviceInstance.updateRemarks = function (model) {
        var deferred = $q.defer();
        $http.post(window.rootUrl + 'Remark/Update', model)
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
});