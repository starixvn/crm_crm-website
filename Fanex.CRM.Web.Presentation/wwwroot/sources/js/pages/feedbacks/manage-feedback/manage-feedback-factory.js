(function (angular) {
    'use strict';

    angular.module('manageFeedbackModule').factory('manageFeedbackFactory', manageFeedbackFactory);
    manageFeedbackFactory.$inject = ['$http', '$q'];

    function manageFeedbackFactory($http, $q) {
        function searchManageFeedback(criteria) {
            let deferred = $q.defer();
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
        }

        function getMainCategoriesSetting() {
            let deferred = $q.defer();
            $http.get(window.rootUrl + "Feedbacks/GetAllMainCategories", null)
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

        function getAllSetting() {
            let deferred = $q.defer();
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
        }

        return {
            getAllSetting: getAllSetting,
            getMainCategoriesSetting: getMainCategoriesSetting,
            searchManageFeedback: searchManageFeedback
        };
    }
})(angular);