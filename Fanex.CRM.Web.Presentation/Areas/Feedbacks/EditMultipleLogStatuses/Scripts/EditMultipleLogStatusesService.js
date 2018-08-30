(function () {
    "use strict";
    angular.module('edit-multiple-log-statuses-application').factory('$editMultipleLogStatusesService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getSettings = function () {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "MultipleLogs/GetEditSettings", null)
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
        serviceInstance.editMultipleLogStatuses = function (editCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "MultipleLogs/UpdateMultipleLogStatuses", editCriteria)
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
    }]);
})();
