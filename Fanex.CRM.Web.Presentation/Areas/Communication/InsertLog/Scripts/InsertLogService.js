"use strict";
/* global insertLogApplication */
insertLogApplication.factory('$insertLogService', ['$http', '$q', 
function ($http, $q) {
    var serviceInstance = {};

    serviceInstance.getAllExtension = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Setting/GetAllExtension", null)
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
    serviceInstance.getAllCountry = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Setting/GetAllCountry", null)
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
    serviceInstance.getAllCommunicationTool = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Setting/GetAllCommunicationTool", null)
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
    serviceInstance.getAllLanguage = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Setting/GetAllLanguage", null)
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
    serviceInstance.getAllMainCategory = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Setting/GetAllMainCategory", null)
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
    serviceInstance.getAllProduct = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Setting/GetAllProduct", null)
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
    serviceInstance.getAllSite = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Setting/GetAllSite", null)
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
    serviceInstance.getAllStatus = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Setting/GetAllStatus", null)
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
    serviceInstance.getAllRole = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Setting/GetAllRole", null)
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
    serviceInstance.getAllResponse = function () {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Setting/GetAllResponse", null)
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
    serviceInstance.insertLog = function (insertCriteria) {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "CommunicationLog/InsertLog", insertCriteria)
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