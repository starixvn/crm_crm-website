'use strict';
/* global updateCustomerFeedbackApplication, sharedScriptManager */
updateCustomerFeedbackApplication.factory('UpdateCustomerFeedbackService', ['$http', '$q', function ($http, $q) {
    var serviceInstance = {};

    serviceInstance.getSettings = function () {
        var deferred = $q.defer();
        $http.get(window.rootUrl + 'UpdateCustomerFeedback/AllSettings')
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

    serviceInstance.getFeedback = function (id) {
        var deferred = $q.defer();
        $http.get(window.rootUrl + 'UpdateCustomerFeedback/GetFeedback?id=' + id)
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
        var url = window.rootUrl + 'Setting/NumberOfFeedbackSubjectTemplate';
        return sharedScriptManager.registerAngularService($http, $q, url, null);
    };

    serviceInstance.getFeedbackSubjectTemplateByPagingIndex = function (pagingInfo) {
        var url = window.rootUrl + 'Setting/GetFeedbackSubjectTemplateByPagingIndex';
        return sharedScriptManager.registerAngularService($http, $q, url, pagingInfo);
    };

    serviceInstance.verifyUsernames = function (username) {
        var url = window.rootUrl + 'CustomerSetting/VerifyUsername';
        return sharedScriptManager.registerAngularService($http, $q, url, username);
    };

    serviceInstance.updateFeedback = function (criteria) {
        var url = window.rootUrl + 'UpdateCustomerFeedback/UpdateFeedback';
        return sharedScriptManager.registerAngularService($http, $q, url, criteria);
    };

    return serviceInstance;
} ]);