(function () {
    "use strict";
    angular.module('report-casinodaily-application').factory('CasinoDailyService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getCasinoDailySummary = function (fromDate, toDate) {
            var deferred = $q.defer();
            $http.get(window.rootUrl + 'CasinoReport/CasinoDailySummary?fromDate=' + fromDate + '&toDate=' + toDate)
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

        serviceInstance.getCasinoDailyDetail = function (fromDate, toDate, page, size) {
            var url = window.rootUrl + 'CasinoReport/CasinoDailyDetail?fromDate=' + fromDate + '&toDate=' + toDate +
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