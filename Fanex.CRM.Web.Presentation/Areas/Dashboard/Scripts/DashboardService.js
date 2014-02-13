"use strict";
/* global dashboardApplication */
dashboardApplication.factory('$dashboardService', function ($http, $q) {
    var serviceInstance = {};

    serviceInstance.getCustomerDashboardInfo = function (getCustomerDashboardInfoCriteria) {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "Dashboard/GetCustomerDashboardInfo", getCustomerDashboardInfoCriteria)
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
});