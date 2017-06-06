(function () {
    "use strict";
    angular.module('feedback-manage-application').factory('manageFeedbackService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.searchManageFeedback = function (criteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + 'Feedbacks/SearchManageFeedback', criteria)
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

        serviceInstance.getAllSetting = function () {
            var deferred = $q.defer();
            $http.get(window.rootUrl + "Feedbacks/AllSetting", null)
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

        serviceInstance.updateSiteLicenseeType = function (criteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Feedbacks/UpdateSiteLicenseeType", criteria)
            .success(function (data, status, headers, config) {
                deferred.resolve({
                    Result: data,
                    Status: status,
                    Headers: headers,
                    Config: config
                });
            })
            .error(function (data, status, headers, config) {
                deferred.resolve({
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