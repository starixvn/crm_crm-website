(function () {
    "use strict";
    angular.module('communicationlog-viewdetailbyid-application').factory('$viewDetailByIdService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};

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
        serviceInstance.getAllSetting = function () {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "CommunicationLog/AllSettingForEdit", null)
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