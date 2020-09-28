(function () {
    "use strict";
    angular.module('pic-management-application').factory('$picManagementFactory', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getAllSetting = function () {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "PicManagement/AllSetting", null)
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

        serviceInstance.getReport = function (getReportCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "PicManagement/GetReport", getReportCriteria)
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

        serviceInstance.insertPic = function (picInsertCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "PicManagement/InsertPic", picInsertCriteria)
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

        serviceInstance.editPic = function (picEditCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "PicManagement/EditPic", picEditCriteria)
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

        serviceInstance.validateEditPic = function (criteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "PicManagement/ValidateEditPic", criteria)
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

        serviceInstance.changePicState = function (params) {
            var url = window.rootUrl + "PicManagement/ChangePicState";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();