(function () {
    "use strict";
    angular.module('changes-log-application').factory('$changesLogFactory', ['$http', '$q',
        function ($http, $q) {
            var serviceInstance = {};

            serviceInstance.getHeadlineDAReport = function (getReportCriteria) {
                var deferred = $q.defer();
                $http.post(window.rootUrl + "ChangesLog/GetHeadlineDAReport", getReportCriteria)
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

            serviceInstance.getPicReport = function (getReportCriteria) {
                var deferred = $q.defer();
                $http.post(window.rootUrl + "ChangesLog/GetPicReport", getReportCriteria)
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

            serviceInstance.getActionTypes = function () {
                var deferred = $q.defer();
                $http.post(window.rootUrl + "ChangesLog/GetActionTypes")
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

            serviceInstance.getLogActions = function () {
                var deferred = $q.defer();
                $http.post(window.rootUrl + "ChangesLog/GetLogActions")
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