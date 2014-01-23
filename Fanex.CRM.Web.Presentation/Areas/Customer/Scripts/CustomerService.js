
customerApplication.factory('$customerService', function ($http, $q) {
    var self = {};
    self.CacheData = {
        getCustomerInfoParams: {
            Username: null,
            IsGuest: null
        }
    };

    self.getCustomerInfoByUsername = function (getCustomerInfoParams) {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "CustomerSetting/GetCustomerInfoByUsername", getCustomerInfoParams)
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

    self.updateCustomerInfo = function (updatedCustomerInfoParams) {
        var deferred = $q.defer();
        $http.post(window.rootUrl + "CustomerSetting/UpdateCustomerInfo", updatedCustomerInfoParams)
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

    return self;
});