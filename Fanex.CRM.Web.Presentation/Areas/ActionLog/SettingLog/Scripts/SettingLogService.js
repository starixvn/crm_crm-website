(function () {
    "use strict";

    angular.module('settinglog-application').factory('$settingLogService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getActions = function () {
            var deferred = $q.defer();
            $http.get(window.rootUrl + 'Setting/GetAllLogAction?ActionType=2')
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

        serviceInstance.getSettingLogs = function (settingLogCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + 'ActionLog/GetSettingLog', settingLogCriteria)
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