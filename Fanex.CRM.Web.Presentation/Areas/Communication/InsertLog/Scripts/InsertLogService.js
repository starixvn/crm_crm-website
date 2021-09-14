(function () {
    "use strict";
    angular.module('communicationlog-insert-application').factory('$insertLogService', ['$http', '$q',
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
        serviceInstance.getAllSettingForInsert = function (custId) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "CommunicationLog/AllSettingForInsert", { custId: custId })
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

        serviceInstance.checkFeedbackDuplication = function (params) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Feedbacks/CheckFeedbackDuplication", params)
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

        serviceInstance.recheckVerifyUserName = function (params) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "CustomerSetting/VerifyUsername", params)
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

        serviceInstance.getTicketInfoByRefno = function (params) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Feedbacks/GetTicketInfoByRefNoAsync", params)
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

        serviceInstance.getBalanceUpday = function () {
            var deferred = $q.defer();
            $http.get(window.rootUrl + "Feedbacks/GetBalanceUpday", null)
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