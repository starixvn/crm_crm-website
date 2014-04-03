'use strict';
/* global wagerReportApplication */
wagerReportApplication.factory('WagerReportService', ['$http', '$q', function ($http, $q) {
    var serviceInstance = {};

    serviceInstance.getWagerReport = function (year) {
        var deferred = $q.defer();
        $http.get(window.rootUrl + 'WagerReport/GetWagerReport?year=' + year)
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