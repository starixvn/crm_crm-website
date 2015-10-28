(function () {
    "use strict";
    angular.module('report-summaryreport-application').factory('SummaryReportService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};

        serviceInstance.getSummaryReport = function (year, site, categories) {
            var deferred = $q.defer();
            $http.get(window.rootUrl + 'SummaryReport/GetSummaryReport?year=' + year + '&siteType=' + site + '&categories=' + categories)
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

        serviceInstance.getAllSettings = function () {
            var deferred = $q.defer();
            $http.get(window.rootUrl + 'SummaryReport/AllSettings')
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