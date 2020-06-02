(function () {
	"use strict";
	angular.module('picofheadline-application').factory('$picOfHeadlineService', ['$http', '$q',
		function ($http, $q) {
			var serviceInstance = {};
			serviceInstance.getAllSetting = function () {
				var deferred = $q.defer();
				$http.post(window.rootUrl + "PicOfHeadline/AllSetting", null)
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

			serviceInstance.getPICAssignment = function (getAssignmentCriteria) {
				var deferred = $q.defer();
				$http.post(window.rootUrl + "PicOfHeadline/GetAllPICAssignment", getAssignmentCriteria)
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

			serviceInstance.validatePicAssignment = function (params) {
				var deferred = $q.defer();
				$http.post(window.rootUrl + "PicOfHeadline/ValidateAssignPic", params)
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

			serviceInstance.assignPIC = function (params) {
				var deferred = $q.defer();
				$http.post(window.rootUrl + "PicOfHeadline/AssignPIC", params)
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

			serviceInstance.GetAssignPICSettings = function () {
				var deferred = $q.defer();
				$http.get(window.rootUrl + "PicOfHeadline/GetAssignPICSettings")
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

			serviceInstance.removeAssignment = function (params) {
				var deferred = $q.defer();
				$http.post(window.rootUrl + "PicOfHeadline/RemoveAssignment", params)
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