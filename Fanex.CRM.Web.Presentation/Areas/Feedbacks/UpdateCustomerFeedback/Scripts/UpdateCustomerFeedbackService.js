'use strict';
/* global updateCustomerFeedbackApplication, sharedScriptManager */
updateCustomerFeedbackApplication.factory('UpdateCustomerFeedbackService', ['$http', '$q', function ($http, $q) {
    var DELETED_STATUS_ID = 11004;

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

    serviceInstance.removeDeletedStatus = function (statusList) {
        for (var i = 0; i < statusList.length; i++) {
            if (statusList[i].Id === DELETED_STATUS_ID) {
                statusList.splice(i, 1);
                break;
            }
        }
    };

    serviceInstance.removeOtherStatus = function (statusList, statusId) {
        var i = 0;
        while (i < statusList.length) {
            if (statusList[i].Id !== DELETED_STATUS_ID && statusList[i].Id !== statusId) {
                statusList.splice(i, 1);
            }
            else {
                i++;
            }
        }
    };

    return serviceInstance;
} ]);