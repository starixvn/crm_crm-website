(function () {
    "use strict";
    angular.module('report-casinoweekly-application').factory('CasinoWeeklyService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getCasinoWeeklySummary = function (params) {
            var deferred = $q.defer();
            $http.get(window.rootUrl + 'CasinoReport/CasinoWeeklySummary?' + $.param(params))
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

        serviceInstance.getCasinoWeeklyDetail = function (fromDate, toDate, page, size) {
            var url = window.rootUrl + 'CasinoReport/CasinoWeeklyDetail?fromDate=' + fromDate + '&toDate=' + toDate +
            '&PageIndex=' + page + '&ItemsPerPage=' + size;
            var deferred = $q.defer();
            $http.get(url)
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