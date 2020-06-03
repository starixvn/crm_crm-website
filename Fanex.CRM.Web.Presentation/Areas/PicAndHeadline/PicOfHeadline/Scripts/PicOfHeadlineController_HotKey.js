(function () {
	"use strict";
	/* jshint camelcase: false */
	/* global hotKeyManager */
	/* exported registerHotkeyFunctions */
	window.registerHotkeyFunctions = function ($scope) {
		$scope.registerHotKey = function () {
			hotKeyManager.CachedData.ActiveDocumentManager.push({
				Value: $scope,
				Id: "PicAndHeadline_PicOfHeadline"
			});
		};
		$scope.invokeHotKey_CtrlS = function () {
			$scope.getPICAssignment();
		};

		$scope.invokeHotKey_CtrlI = function () {
			$scope.openAssignModal();
		};
	};
})();