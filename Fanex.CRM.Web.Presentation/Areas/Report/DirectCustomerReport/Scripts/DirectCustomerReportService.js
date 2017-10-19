(function () {
    "use strict";
    angular.module('report-directcustomerreport-application').factory('$directCustomerReportService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        
        serviceInstance.getReport = function (getReportCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "DirectCustomerReport/GetReport", getReportCriteria)
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