"use strict";
/* global redemptionViewLogApplication */
redemptionViewLogApplication.factory('$redemptionViewLogService', function ($http, $q) {
    var serviceInstance = {};

    serviceInstance.getAllStatus = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Setting/GetAllStatus", null)
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