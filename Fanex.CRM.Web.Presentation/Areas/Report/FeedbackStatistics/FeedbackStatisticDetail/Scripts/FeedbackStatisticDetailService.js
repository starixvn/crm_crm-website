(function () {
    "use strict";
    angular.module('feedback-statistic-detail-application').factory('FeedbackStatisticDetailService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getFeedbackStatisticDetail = function (criteria) {
            var deferred = $q.defer();
            $http.get(window.rootUrl + 'FeedbackStatisticReport/GetFeedbackStatisticDetail?' + $.param(criteria))
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