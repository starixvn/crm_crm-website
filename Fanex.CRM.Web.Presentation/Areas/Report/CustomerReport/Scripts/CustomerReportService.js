"use strict";
/* global customerReportApplication */
customerReportApplication.factory('$customerReportService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getAllSetting = function () {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "CustomerReport/AllSetting", null)
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

        serviceInstance.getCustomerReport = function (getReportCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "CustomerReport/GetCustomerReport", getReportCriteria)
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