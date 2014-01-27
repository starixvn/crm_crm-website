communicationApplication.factory('$communicationService', function ($http, $q) {
    var serviceInstance = {};

    serviceInstance.getLogs = function (getLogByCustomerIdParams) {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "CustomerSettingDetail/GetCustomerInfoDetailById", getLogByCustomerIdParams)
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