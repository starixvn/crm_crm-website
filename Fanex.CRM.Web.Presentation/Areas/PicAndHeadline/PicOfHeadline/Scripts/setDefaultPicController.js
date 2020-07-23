(function () {
	'use strict';
	/* global viewResources,
	MessageResult,
	crmConstants,
	currentDate: true,
	moment,
	sharedScriptManager */
	/* jshint camelcase: false */
	angular.module('picofheadline-application').controller('setDefaultPicController',
		['$scope', '$window', '$uibModal', '$uibModalInstance', '$picOfHeadlineService', 'data',
			function ($scope, $window, $uibModal, $uibModalInstance, $picOfHeadlineService, data) {
				$scope.isSubmit = false;
				$scope.resultMessages = [];
				$scope.picOptions = [];
				$scope.headline = {
					headlineId: data.headlineId,
					headlineName: data.headlineName
				};
				$scope.defaultPic = data.defaultPIC;
				$scope.isShowRemoveBtn = data.defaultPIC.length > 0;

				$scope.onClickCancelBtn = function () {
					const pics = $scope.defaultPic;
					if (pics.toString() !== data.defaultPIC.toString()) {
						const closeParentPopup = function () { $scope.closeModal(false); };
						$uibModal.open({
							template: angular.element('#confirm-dutty-form').html(),
							backdrop: 'static',
							keyboard: false,
							size: 'sm',
							appendTo: $window.parent.parent.$('body'),
							windowClass: 'pic-assignment-insert-modal',
							controller: function ($scope) {
								$scope.confirm = function (isConfirmed) {
									$scope.$dismiss();
									if (isConfirmed) {
										closeParentPopup();
									}
								};
							}
						});
					} else {
						$scope.closeModal(false);
					}
				};

				$scope.closeModal = function (isReload = false) {
					if (!isReload) {
						angular.element($window.parent.document).find('#defaultPic').select2("close");
					}
					$uibModalInstance.close(isReload);
				};

				$scope.setDefaultPic = function () {
					if (!$scope.validateParams($scope.defaultPic)) {
						return;
					}
					$scope.isSubmit = true;
					$scope.BlockUI();
					var params = {
						headlineId: $scope.headline.headlineId,
						picIds: $scope.defaultPic.toString()
					};
					$picOfHeadlineService.setDefaultPic(params).then(function (data) {
						$scope.unblockUI();
						var result = data.Result;
						if (result.IsSuccessful) {
							$scope.messageResultOptions = new MessageResult('Set Default PIC successfully!', true, true);
							setInterval(function () {
								$scope.closeModal(true);
							}, 3000);
						} else {
							$scope.isSubmit = false;
							$scope.messageResultOptions = new MessageResult(result.Message, true, false);
						}
					});
				};

				$scope.removeDefaultPic = function () {
					$scope.BlockUI();
					var params = {
						headlineId: $scope.headline.headlineId
					};
					$picOfHeadlineService.removeDefaultPic(params).then(function (data) {
						$scope.unblockUI();
						var result = data.Result;
						if (result.IsSuccessful) {
							$scope.messageResultOptions = new MessageResult('Remove Default PIC successfully!', true, true);
							setInterval(function () {
								$scope.closeModal(true);
							}, 3000);
						} else {
							$scope.isSubmit = false;
							$scope.messageResultOptions = new MessageResult(result.Message, true, false);
						}
					});
				};

				$scope.validateParams = function (pics) {
					var result = true;
					var message = '';
					if (pics.length === 0) {
						message = viewResources.DefaultPIC + " " + viewResources.IsRequired;
						result = false;
					}

					$scope.messageResultOptions = new MessageResult(message, !result, result);
					return result;
				};

				$scope.unblockUI = function () {
					if (parent && parent.$.unblockUI) {
						parent.$.unblockUI();
					}
				};

				$scope.BlockUI = function () {
					if (parent && parent.BlockUI) {
						parent.BlockUI();
					}
				};

				$($window.parent.document).off('keydown').on('keydown', function (eventObject) {
					if (eventObject.ctrlKey) {
						if (eventObject.keyCode === 83) {  //s
							$scope.setDefaultPic();
							$scope.$apply();
							eventObject.preventDefault();
							return false;
						}

						if (eventObject.keyCode === 77 && $scope.defaultPic.length > 0) {  //m
							$scope.removeDefaultPic();
							$scope.$apply();
							eventObject.preventDefault();
							return false;
						}
					}
				});

				angular.element(document).ready(function () {
					$($window.parent).on("click", function ($e) {
						if (!$e.target.className.includes("select2")) {
							angular.element($window.parent.document).find('.pic-assignment-select2').select2("close");
						}
					});
					$picOfHeadlineService.GetPicList().then(function (result) {
						var pics = result.Result;
						$scope.picOptions = pics ? pics.map(pic => {
							return {
								id: pic.PicId,
								text: pic.Username + ' (' + pic.RoleName + ')',
								selected: data.defaultPIC !== null && data.defaultPIC.some(p => p.toString() === pic.PicId.toString())
							};
						}) : [];
					});
				});
			}]);
})();