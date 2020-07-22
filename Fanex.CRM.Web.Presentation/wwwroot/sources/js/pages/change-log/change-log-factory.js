(function () {
    "use strict";
    angular.module('change-log-application').factory('$changeLogFactory', ['$http', '$q',
        function ($http, $q) {
            var serviceInstance = {};
            //serviceInstance.getAllSetting = function () {
            //    var deferred = $q.defer();
            //    $http.post(window.rootUrl + "PicManagement/AllSetting", null)
            //        .success(function (data, status, headers, config) {
            //            deferred.resolve({
            //                Result: data,
            //                Status: status,
            //                Headers: headers,
            //                Config: config
            //            });
            //        }).error(function (data, status, headers, config) {
            //            deferred.reject({
            //                Result: data,
            //                Status: status,
            //                Headers: headers,
            //                Config: config
            //            });
            //        });

            //    return deferred.promise;
            //};

            serviceInstance.getHeadlineDAReport = function (getReportCriteria) {
                var deferred = $q.defer();
                $http.post(window.rootUrl + "ChangeLog/GetHeadlineDAReport", getReportCriteria)
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
                $http.post(window.rootUrl + "ChangeLog/GetPicReport", getReportCriteria)
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