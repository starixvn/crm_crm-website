(function () {
	'use strict';
	/* global hotKeyManager,
	viewResources,
	crmConstants,
	currentDate: true,
	MessageResult,
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
					$scope.selectedAssignment = {};
					$scope.removeAssignmentData = [];
				};

				$scope.onSelectAssignment = function ($event, headlineId, custId, picName) {
					let index = $scope.selectedAssignment.findIndex(assignment =>
						assignment.HeadlineId === headlineId &&
						assignment.CustId === custId &&
						assignment.PicName === picName);
					$scope.selectedAssignment[index].IsSelected = $event.currentTarget.checked;

					$scope.handleRemoveAllBtnState();
					$scope.handlePicNameChkboxState(picName);
				};

				$scope.removeAssignment = function () {
					const params = $scope.selectedAssignment.filter(a => a.IsSelected).map(assignment => {
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
							const distinctPicNames = [...new Set(picNames)];
							$scope.messageResultOptions = new MessageResult(`Remove ${distinctPicNames.join(', ')} assignment successfully!`, true, true);
							setInterval(function () {
								$scope.closeModal();
							}, 3000);
						} else {
							$scope.messageResultOptions = new MessageResult(result.Message, true, false);
						}
					});
				};

				$scope.onClickPicNameChkbox = function ($event, picName) {
					for (var i = 0; i < $scope.selectedAssignment.length; i++) {
						let assignment = $scope.selectedAssignment[i];
						if (assignment.PicName === picName) {
							assignment.IsSelected = $event.currentTarget.checked;
						}
					}

					const chckboxs = angular.element(document).find(`.chkbox-${picName}`);
					for (var j = 0; j < chckboxs.length; j++) {
						chckboxs[j].checked = $event.currentTarget.checked;
					}

					$scope.handleRemoveAllBtnState();
				};

				$scope.handleRemoveAllBtnState = function () {
					if (!$scope.selectedAssignment.some(assignment => assignment.IsSelected)) {
						angular.element(document).find('#remove-all-btn').attr('disabled', 'disabled');
					} else {
						angular.element(document).find('#remove-all-btn').removeAttr('disabled');
					}
				};

				$scope.handlePicNameChkboxState = function (picName) {
					let isCheck = true;
					const chckboxs = angular.element(document).find(`.chkbox-${picName}`);
					for (var i = 0; i < chckboxs.length; i++) {
						if (!chckboxs[i].checked) {
							isCheck = false;
						}
					}
					angular.element(document).find(`#${picName}`)[0].checked = isCheck;
				};

				$scope.openConfirmRemoveAssignmentModal = function () {
					let removeAssignmentFunction = function () { $scope.removeAssignment(); };
					const modalInstance = $uibModal.open({
						template: angular.element('#confirm-remove-assignment-modal').html(),
						backdrop: 'static',
						keyboard: false,
						size: 'lg',
						backdropClass: 'related-customer-modal-backdrop',
						windowClass: 'remove-assignment-modal',
						controller: function ($scope) {
							$scope.removeAssignment = function () {
								$scope.$dismiss();
								removeAssignmentFunction();
							};
						}
					});
					modalInstance.result.then(function () {
						$scope.getPICAssignment();
					});
				};
			}]);
})();