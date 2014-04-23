'use strict';
/* global summaryReportApplication */
summaryReportApplication.factory('SummaryReportService', ['$http', '$q', function ($http, $q) {
    var serviceInstance = {};

    serviceInstance.getSummaryReport = function (year, categories) {
        var deferred = $q.defer();
        $http.get(window.rootUrl + 'SummaryReport/GetSummaryReport?year=' + year + '&categories=' + categories)
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

    serviceInstance.getCategories = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + 'Setting/GetAllMainCategory')
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