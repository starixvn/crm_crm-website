(function () {
    "use strict";
    angular.module('report-redemption-statistics-application').factory('$redemptionStatisticsService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getAllSetting = function () {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "RedemptionStatisticsReport/AllSetting", null)
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

        serviceInstance.getRedemptionStatisticsReport = function (getReportCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "RedemptionStatisticsReport/GetRedemptionStatisticsSummaryReport", getReportCriteria)
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

        serviceInstance.getRedemptionStatisticsDetailReport = function (getDetailReportCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "RedemptionStatisticsReport/GetRedemptionStatisticsDetailReport", getDetailReportCriteria)
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