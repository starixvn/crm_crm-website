'use strict';
/* global createInformHeadlinelApplication */
createInformHeadlinelApplication.factory('createInformHeadlineTitleService', ['$http', '$q',
 function ($http, $q) {
     var serviceInstance = {};

     serviceInstance.createTitle = function (criteria) {
         var deferred = $q.defer();
         $http.post(window.rootUrl + 'InformHeadlines/CreateTitle', criteria)
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