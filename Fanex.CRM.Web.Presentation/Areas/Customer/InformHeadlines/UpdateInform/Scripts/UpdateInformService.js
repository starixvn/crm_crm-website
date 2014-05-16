'use strict';
/* exported updateInformHeadlineApplication */
updateInformHeadlineApplication.factory('updateInformHeadlineService', ['$http', '$q',
 function ($http, $q) {
     var serviceInstance = {};

     serviceInstance.getStatusList = function () {
         var deferred = $q.defer();
         $http.get(window.rootUrl + 'Setting/GetAllStatusInformHeadline')
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

     serviceInstance.getInforms = function (criteria) {
         var deferred = $q.defer();
         $http.get(window.rootUrl + 'InformHeadlines/GetInforms?' + $.param(criteria))
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

     serviceInstance.updateInforms = function (criteria) {
         var deferred = $q.defer();
         $http.post(window.rootUrl + 'InformHeadlines/UpdateInforms', criteria)
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