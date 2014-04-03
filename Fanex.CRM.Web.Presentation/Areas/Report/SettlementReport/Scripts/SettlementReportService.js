"use strict";
/* global settlementReportApplication */
settlementReportApplication.factory('settlementReportService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getSettlementReport = function (getReportCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "SettlementReport/GetSettlementReport", getReportCriteria)
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