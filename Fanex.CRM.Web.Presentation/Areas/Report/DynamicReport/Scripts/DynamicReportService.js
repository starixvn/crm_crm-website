"use strict";
/* global dynamicReportApplication */
dynamicReportApplication.factory('$dynamicReportService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getAllSetting = function () {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "DynamicReport/GetAllSetting", null)
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