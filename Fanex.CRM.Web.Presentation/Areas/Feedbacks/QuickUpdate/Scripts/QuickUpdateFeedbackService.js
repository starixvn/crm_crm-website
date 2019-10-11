(function () {
    "use strict";
    angular.module('quick-update-feedback-application').factory('$quickUpdateFeedbackService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getFeedback = function (feedbackId) {
            var deferred = $q.defer();
            $http.get(window.rootUrl + "QuickUpdateFeedback/GetFeedback?feedbackId=" + feedbackId)
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

        serviceInstance.updateFeedback = function (criteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "QuickUpdateFeedback/UpdateFeedback", criteria)
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
