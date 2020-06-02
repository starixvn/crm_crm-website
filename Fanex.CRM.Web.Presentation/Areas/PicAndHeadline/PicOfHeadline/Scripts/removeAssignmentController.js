(function () {
	'use strict';
	/* global hotKeyManager,
	viewResources,
	crmConstants,
	currentDate: true,
	moment,
	sharedScriptManager */
	/* jshint camelcase: false */
	angular.module('picofheadline-application').controller('removeAssignmentController',
		['$scope', '$window', '$uibModal', '$uibModalInstance', '$picOfHeadlineService', 'removeAssignmentData',
			function ($scope, $window, $uibModal, $uibModalInstance, $picOfHeadlineService, removeAssignmentData) {
				$scope.resultMessages = [];
				$scope.selectedAssignment = removeAssignmentData.selectedAssignment;
				$scope.removeAssignmentData = removeAssignmentData;

				$scope.closeModal = function () {
					$uibModalInstance.close();
				};

				$scope.onSelectAssignment = function ($event, headlineId, custId, picName) {
					let index = $scope.selectedAssignment.findIndex(assignment =>
						assignment.HeadlineId === headlineId &&
						assignment.CustId === custId &&
						assignment.PicName === picName);
					$scope.selectedAssignment[index].IsSelected = $event.currentTarget.checked;

					if (!$scope.selectedAssignment.some(assignment => assignment.IsSelected)) {
						angular.element(document).find('#remove-all-btn').attr('disabled', 'disabled');
					} else {
						angular.element(document).find('#remove-all-btn').removeAttr('disabled');
					}

					const chckboxs = angular.element(document).find(`.chkbox-${picName}`);
				};

				$scope.removeAllAssignment = function () {
					const params = $scope.selectedAssignment.map(assignment => {
						return {
							HeadlineId: assignment.HeadlineId,
							CustId: assignment.CustId,
							PicName: assignment.PicName
						};
					});
					$picOfHeadlineService.removeAssignment(params).then(function (data) {
						var result = data.Result;
						if (result.IsSuccessful) {
							const picNames = params.map(item => { return item.PicName; });
							$scope.messageResultOptions = new MessageResult(`Remove ${picNames.toString()} assignment successfully!`, true, true);
							setInterval(function () {
								$scope.closeModal();
							}, 3000);
						} else {
							$scope.messageResultOptions = new MessageResult(result.Message, true, false);
						}
					});
				};

				$scope.onClickPicNameChkbox = function ($event, picName) {
					$scope.selectedAssignment.map(assignment => {
						if (assignment.PicName === picName) {
							assignment.IsSelected = $event.currentTarget.checked;
						}
					});

					const chckboxs = angular.element(document).find(`.chkbox-${picName}`);
					for (var i = 0; i < chckboxs.length; i++) {
						chckboxs[i].checked = $event.currentTarget.checked;
					}
				};

				angular.element(document).ready(function () {
				});
			}]);
})();