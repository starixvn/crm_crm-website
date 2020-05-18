(function () {
    "use strict";
    angular.module('picofheadline-application').factory('$picOfHeadlineService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getAllSetting = function () {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "PicOfHeadline/AllSetting", null)
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

        serviceInstance.getPICAssignment = function (getAssignmentCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "PicOfHeadline/GetAllPICAssignment", getAssignmentCriteria)
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