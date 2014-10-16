'use strict';
/* global fbStatisticApplication */
fbStatisticApplication.factory('FeedbackStatisticService', ['$http', '$q', function ($http, $q) {
    var serviceInstance = {};

    serviceInstance.getFeedbackStatistic = function (criteria) {
        var deferred = $q.defer();
        $http.get(window.rootUrl + 'FeedbackStatisticReport/SearchFeedbackStatistic?' + $.param(criteria))
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