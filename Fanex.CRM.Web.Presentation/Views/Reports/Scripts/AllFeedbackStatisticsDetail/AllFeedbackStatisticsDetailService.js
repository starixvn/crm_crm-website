"use strict";
/* global allFeedbackStatisticsDetailApplication */

allFeedbackStatisticsDetailApplication.factory('$allFeedbackStatisticsDetailService', ['$http', '$q',
function ($http, $q) {
    var serviceInstance = {};

    serviceInstance.getAllFeedbackStatisticsDetail = function (params) {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Reports/GetFeedbackStatisticDetail", params)
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

    serviceInstance.checkingExportStatus = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Reports/AllowExportFeedbackStatisticDetail")
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