(function () {
    "use strict";
    angular.module('dashboard-verifyflag-application').factory('$verifyFlagService', ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.updateCustomerVerifyFlag = function (selectedCriteria) {
            var deferred = $q.defer();
            $http.post(window.rootUrl + "Dashboard/UpdateCustomerVerifyFlag", selectedCriteria)
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