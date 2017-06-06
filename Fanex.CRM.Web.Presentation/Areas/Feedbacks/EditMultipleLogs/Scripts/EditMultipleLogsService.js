(function () {
    "use strict";
    angular.module('edit-multiple-logs-application').factory('$editMultipleLogsService', ['$http', '$q',
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
        serviceInstance.editMultipleLogs = function (editCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "MultipleLogs/UpdateMultipleLogs", editCriteria)
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
        serviceInstance.getNumberOfFeedbackSubjectTemplate = function () {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Setting/NumberOfFeedbackSubjectTemplate", null)
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
        serviceInstance.getFeedbackSubjectTemplateByPagingIndex = function (pagingInfo) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Setting/GetFeedbackSubjectTemplateByPagingIndex", pagingInfo)
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
