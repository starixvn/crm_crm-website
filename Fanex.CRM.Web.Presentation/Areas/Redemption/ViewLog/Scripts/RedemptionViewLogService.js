(function () {
    "use strict";
    angular.module('redemption-viewlog-application').factory('$redemptionViewLogService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getAllStatus = function () {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Redemption/AllStatusEdit", null)
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
        serviceInstance.searchLogs = function (searchLogsCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Redemption/GetLogsByCustomerId", searchLogsCriteria)
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