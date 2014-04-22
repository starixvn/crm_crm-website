"use strict";
/* global insertLogApplication */
insertLogApplication.factory('$insertLogService', ['$http', '$q', 
function ($http, $q) {
    var serviceInstance = {};
    serviceInstance.getAllStatus = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Redemption/GetAllStatus_Insert", null)
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
        $http.post(window.rootUrl + "Setting/GetNumberOfFeedbackSubjectTemplate", null)
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
    serviceInstance.insertLog = function (insertLogCriteria) {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Redemption/InsertLog", insertLogCriteria)
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