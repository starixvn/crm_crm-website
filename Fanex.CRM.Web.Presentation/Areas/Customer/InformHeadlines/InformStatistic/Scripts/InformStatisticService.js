'use strict';
/* global informHeadlineStatisticApplication */
informHeadlineStatisticApplication.factory('informHeadlineStatisticService', ['$http', '$q',
 function ($http, $q) {
     var serviceInstance = {};

     serviceInstance.getInformStatistic = function () {
         var deferred = $q.defer();
         $http.get(window.rootUrl + 'InformHeadlines/GetInformStatistic')
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