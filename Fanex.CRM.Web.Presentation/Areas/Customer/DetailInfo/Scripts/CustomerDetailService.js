"use strict";
/* global customerDetailApplication */
customerDetailApplication.factory('$customerDetailService', ['$http', '$q',
 function ($http, $q) {
     var serviceInstance = {};

     serviceInstance.getCustomerInfoDetailById = function (getCustomerInfoDetailById) {
         var deferred = $q.defer();
         $http.post(window.rootUrl + "CustomerSettingDetail/GetCustomerInfoDetailById", getCustomerInfoDetailById)
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

     serviceInstance.updateCustomerInfoDetail = function (updatedCustomerInfoDetailParams) {
         var deferred = $q.defer();
         $http.post(window.rootUrl + "CustomerSettingDetail/UpdateCustomerInfoDetail", updatedCustomerInfoDetailParams)
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
     serviceInstance.getAllSite = function () {
         var deferred = $q.defer();
         $http.post(window.rootUrl + "Setting/AllSite", null)
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
     serviceInstance.getAllCountry = function () {
         var deferred = $q.defer();
         $http.post(window.rootUrl + "Setting/AllCountry", null)
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
     serviceInstance.getAllLanguage = function () {
         var deferred = $q.defer();
         $http.post(window.rootUrl + "Setting/AllLanguage", null)
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
     serviceInstance.getAllTitle = function () {
         var deferred = $q.defer();
         $http.post(window.rootUrl + "Setting/AllTitle", null)
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