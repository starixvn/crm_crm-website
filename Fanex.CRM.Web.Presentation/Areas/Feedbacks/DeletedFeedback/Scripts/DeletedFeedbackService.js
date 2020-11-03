(function () {
    "use strict";
    angular.module('deleted-feedback-application').factory('deletedFeedbackService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.searchDeletedFeedback = function (criteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + 'Feedbacks/SearchDeletedFeedback', criteria)
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
            $http.get(window.rootUrl + "Feedbacks/AllSettingDeleteFeedback", null)
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