'use strict';
/* global latestFBApplication */
latestFBApplication.factory('latestFeedbackService', ['$http', '$q', function ($http, $q) {
    var serviceInstance = {};

    serviceInstance.getLatestFeedback = function (criteria) {
        var deferred = $q.defer();
        $http.get(window.rootUrl + 'Feedbacks/GetLatestFeedback?' + $.param(criteria))
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