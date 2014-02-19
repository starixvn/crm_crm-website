"use strict";
/* global activityLogApplication */
activityLogApplication.factory('$activityLogService', ['$http', '$q', function ($http, $q) {
    var serviceInstance = {};

    serviceInstance.getActions = function () {
        var deferred = $q.defer();
        $http.get(window.rootUrl + 'Setting/GetAllLogAction?Type=1')
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

    serviceInstance.getActivityLogs = function (activityLogCriteria) {
        var deferred = $q.defer();
        $http.post(window.rootUrl + 'ActionLog/GetActivityLog', activityLogCriteria)
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
}]);