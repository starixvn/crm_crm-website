(function (angular) {
    'use strict';

    angular.module('createMultipleLogsModule').factory('createMultipleLogsFactory', createMultipleLogsFactory);
    createMultipleLogsFactory.$inject = ['$http', '$q'];

    function createMultipleLogsFactory($http, $q) {
        function getSettings() {
            let deferred = $q.defer();
            $http.post(window.rootUrl + 'MultipleLogs/GetSettings', null)
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
        }

        function checkNotExistDefaultCommToolSetting(param) {
            let deferred = $q.defer();
            $http.post(window.rootUrl + 'MultipleLogs/checkNotExistDefaultCommunicationToolSetting', param)
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
        }

        function createMultipleLogs(criteria) {
            return $http.post(window.rootUrl + 'MultipleLogs/CreateMultipleLogs', criteria);
        }

        function getNumberOfFeedbackSubjectTemplate() {
            let deferred = $q.defer();
            $http.post(window.rootUrl + 'Setting/NumberOfFeedbackSubjectTemplate', null)
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
        }

        function getFeedbackSubjectTemplateByPagingIndex(pagingInfo) {
            let deferred = $q.defer();
            $http.post(window.rootUrl + 'Setting/GetFeedbackSubjectTemplateByPagingIndex', pagingInfo)
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
        }

        function verifyCustomer(param) {
            return $http.post(window.rootUrl + 'MultipleLogs/VerifyCustomer', param);
        }

        function verifyRelatedCustomer(param) {
            return $http.post(window.rootUrl + 'MultipleLogs/VerifyRelatedCustomer', param);
        }

        return {
            getSettings: getSettings,
            checkNotExistDefaultCommToolSetting: checkNotExistDefaultCommToolSetting,
            createMultipleLogs: createMultipleLogs,
            getNumberOfFeedbackSubjectTemplate: getNumberOfFeedbackSubjectTemplate,
            getFeedbackSubjectTemplateByPagingIndex: getFeedbackSubjectTemplateByPagingIndex,
            verifyCustomer: verifyCustomer,
            verifyRelatedCustomer: verifyRelatedCustomer
        };
    }
})(angular);