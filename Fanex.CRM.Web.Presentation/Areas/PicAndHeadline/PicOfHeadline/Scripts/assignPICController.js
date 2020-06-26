(function () {
	'use strict';
	/* global hotKeyManager,
	viewResources,
	MessageResult,
	crmConstants,
	currentDate: true,
	moment,
	sharedScriptManager */
	/* jshint camelcase: false */
	angular.module('picofheadline-application').controller('assignPICController',
		['$scope', '$window', '$uibModal', '$uibModalInstance', '$picOfHeadlineService', 'initData',
			function ($scope, $window, $uibModal, $uibModalInstance, $picOfHeadlineService, initData) {
				$scope.maxItems = 10000;
				$scope.resultMessages = [];
				$scope.isSellectedAll = false;
				$scope.isSuccessful = false;
				$scope.headlineOptions = [];
				$scope.picOptions = [];
				$scope.sites = [];
				$scope.assignPICModel = {
					headlineId: null,
					pics: '',
					directAccounts: JSON.parse(JSON.stringify(initData.directAccounts))
				};

				$scope.onClickCancelBtn = function () {
					const model = $scope.assignPICModel;
					if (model.headlineId !== initData.headlineId ||
						model.pics !== '' ||
						JSON.stringify(model.directAccounts) !== JSON.stringify(initData.directAccounts)) {
						const closeParentPopup = function () { $scope.closeModal(false); };
						$uibModal.open({
							template: angular.element('#confirm-dutty-form').html(),
							backdrop: 'static',
							keyboard: false,
							size: 'sm',
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

				$scope.keydownHandler = function (event) {
					// ctrl + a
					if (event.ctrlKey && event.keyCode === 65 && $scope.relatedCustomerInput === '') {
						$scope.isSellectedAll = true;
						return;
					}

					switch (event.keyCode) {
						case 8: // backspace
							if ($scope.relatedCustomerInput === '') {
								if ($scope.isSellectedAll) {
									$scope.assignPICModel.directAccounts = [];
									$scope.isSellectedAll = false;
								} else {
									$scope.assignPICModel.directAccounts.pop();
								}

								if ($scope.assignPICModel.directAccounts.length === 0) {
									$scope.resultMessages = [];
								}

								return;
							}
							break;
						case 13: // enter
						case 32: // space
						case 188: // semicolon
							$scope.relatedCustomerInput = $scope.relatedCustomerInput.replace(',', '');
							$scope.addRelatedCustomer($scope.relatedCustomerInput);
							break;
						default:
							break;
					}
				};

				$scope.addRelatedCustomer = function (username) {
					const MAX_CHARS = 30;
					const totalItems = $scope.assignPICModel.directAccounts.length;
					if (username !== undefined && username !== '' && totalItems < $scope.maxItems) {
						username = username.length > MAX_CHARS ? username.substring(0, MAX_CHARS) + '...' : username;
						$scope.assignPICModel.directAccounts.push({ index: totalItems + 1, username, isValid: true });
					}

					setTimeout(() => {
						$scope.relatedCustomerInput = '';
						$scope.$apply();
					}, 0);
				};

				$scope.removeRelatedCustomer = function (relatedCustomer) {
					const index = $scope.assignPICModel.directAccounts.indexOf(relatedCustomer);
					if (index > -1) {
						$scope.assignPICModel.directAccounts.splice(index, 1);
					}

					if ($scope.assignPICModel.directAccounts.length === 0) {
						$scope.resultMessages = [];
					}
				};

				$scope.pasteHandler = function (event) {
					const items = event.originalEvent.clipboardData.getData('text/plain')
						.replace(/ /g, ',')
						.replace(/\r\n/g, ',')
						.replace(/\n/g, ',')
						.replace(/\t/g, ',')
						.split(',').filter(item => item !== '');

					const totalItems = $scope.assignPICModel.directAccounts.length + items.length;
					$scope.resultMessages = [];
					if (totalItems > $scope.maxItems) {
						setTimeout(() => {
							$scope.relatedCustomerInput = '';
							$scope.$apply();
						}, 0);
						$scope.isSuccessful = false;
						$scope.resultMessages.push(`You are not allowed to input over ${$scope.maxItems} usernames. Please check it!`);
						return;
					} else {
						$scope.isSuccessful = true;
					}

					items.forEach(item => {
						$scope.addRelatedCustomer(item);
					});
				};

				$scope.focusoutHandler = function () {
					$scope.isSellectedAll = false;
					$scope.addRelatedCustomer($scope.relatedCustomerInput);
				};

				$scope.closeModal = function (isReload = false) {
					$uibModalInstance.close(isReload);
				};

				$scope.validatePicAssignment = function () {
					if (!$scope.validateParams($scope.assignPICModel)) {
						return;
					}

					var params = {
						headlineId: parseInt($scope.assignPICModel.headlineId, 10),
						picIds: $scope.assignPICModel.pics.toString(),
						directAccountIds: $scope.assignPICModel.directAccounts.map(function (acc) { return acc.username; }).toString()
					};

					$picOfHeadlineService.validatePicAssignment(params).then(function (data) {
						var result = data.Result;
						if (result.IsSuccessful && result.AssignedDirectAccounts.length === 0) {
							$scope.assignPIC();
							return;
						}

						if (result.IsSuccessful && result.AssignedDirectAccounts.length > 0) {
							$scope.assignedDirectAccounts = result.AssignedDirectAccounts;
							$scope.openAssignedDirectAccountModal();
						}

						if (!result.IsSuccessful && result.AssignedDirectAccounts.length === 0) {
							$scope.showErrorDirectAccount(result.Message);
							$scope.messageResultOptions = new MessageResult(result.Message, !result.IsSuccessful, result.IsSuccessful);
							return;
						}
					});
				};

				$scope.showErrorDirectAccount = function (message) {
					if (message.indexOf(':') !== -1) {
						const invalidDirectAccountString = message.substring(message.indexOf(':') + 1, message.length).trim();
						const invalidListDirectAccounts = invalidDirectAccountString.split(',');
						const assignPICDirectAccounts = [];
						const map = new Map();
						for (const item of $scope.assignPICModel.directAccounts) {
							if (!map.has(item.username)) {
								map.set(item.username, true);    // set any value to Map
								assignPICDirectAccounts.push({
									index: item.index,
									username: item.username,
									isValid: item.isValid
								});
							}
						}

						assignPICDirectAccounts.map(function (account) {
							if (invalidListDirectAccounts.some(acc => acc.trim() === account.username.trim())) {
								account.isValid = false;
							}
						});

						$scope.assignPICModel.directAccounts = [...new Set(assignPICDirectAccounts)];
					}
				};

				$scope.openAssignedDirectAccountModal = function () {
					var assignPIC = function (isKeepAssign) {
						$scope.assignPIC(isKeepAssign);
					};
					var assignedDirectAccounts = $scope.assignedDirectAccounts;
					$uibModal.open({
						template: angular.element('#list-assigned-direct-account').html(),
						backdrop: 'static',
						keyboard: false,
						backdropClass: 'pic-assignment-modal-backdrop',
						windowClass: 'pic-assignment-modal',
						size: 'lg',
						appendTo: $window.parent.$('body'),
						controller: function ($scope) {
							$scope.assignPIC = function (isKeepAssign) {
								$scope.$dismiss();
								assignPIC(isKeepAssign);
							};
							$scope.assignedDirectAccounts = assignedDirectAccounts;

							$(document).off('keydown').on('keydown', function (eventObject) {
								if (eventObject.ctrlKey) {
									if (eventObject.keyCode === 77) {
										$scope.$dismiss();
										assignPIC(false);
										eventObject.preventDefault();
										return false;
									}

									if (eventObject.keyCode === 75) {
										$scope.$dismiss();
										assignPIC(true);
										eventObject.preventDefault();
										return false;
									}
								}
							});
						}
					});
				};

				$scope.assignPIC = function (isKeepAssign = true) {
					$scope.BlockUI();
					var params = {
						headlineId: $scope.assignPICModel.headlineId,
						picIds: $scope.assignPICModel.pics.toString(),
						directAccountIds: $scope.assignPICModel.directAccounts.map(function (acc) { return acc.username; }).toString(),
						isKeepAssign: isKeepAssign
					};
					$picOfHeadlineService.assignPIC(params).then(function (data) {
						$scope.unblockUI();
						var result = data.Result;
						if (result.IsSuccessful) {
							$scope.messageResultOptions = new MessageResult('Assign PIC successfully!', true, true);
							setInterval(function () {
								$scope.closeModal(true);
							}, 3000);
						} else {
							$scope.messageResultOptions = new MessageResult(result.Message, true, false);
						}
					});
				};

				$scope.validateParams = function (params) {
					var result = true;
					var message = '';
					if (params.headlineId === null || parseInt(params.headlineId, 10) < 0) {
						message = viewResources.GroupHeadline + " " + viewResources.IsRequired;
						result = false;
					}
					else if (params.pics.length === 0) {
						message = viewResources.PIC + " " + viewResources.IsRequired;
						result = false;
					}
					else if (params.directAccounts.length === 0) {
						message = viewResources.DirectAccount + " " + viewResources.IsRequired;
						result = false;
					}

					$scope.messageResultOptions = new MessageResult(message, !result, result);
					return result;
				};

				$scope.unblockUI = function () {
					if (parent && parent.$.unblockUI) {
						parent.$.unblockUI();

						hotKeyManager.CachedData.isBlockingUI = false;
					}
				};

				$scope.BlockUI = function () {
					hotKeyManager.CachedData.isBlockingUI = true;
					if (parent && parent.BlockUI) {
						parent.BlockUI();
					}
				};

				$(document).off('keydown').on('keydown', function (eventObject) {
					if (eventObject.ctrlKey) {
						if (eventObject.keyCode === 83) {  //s
							$scope.validatePicAssignment();
							eventObject.preventDefault();
							return false;
						}
					}
				});

				angular.element(document).ready(function () {
					$picOfHeadlineService.GetAssignPICSettings().then(function (data) {
						var settings = data.Result;
						$scope.headlineOptions = settings.groupHeadlines ? settings.groupHeadlines.map(headline => {
							return {
								id: headline.HeadLineID,
								text: headline.HeadlineName,
								selected: initData.headlineId !== null ? initData.headlineId === headline.HeadLineID : false
							};
						}) : [];
						$scope.picOptions = settings.pics ? settings.pics.map(pic => {
							return {
								id: pic.PicId,
								text: pic.Username + ' (' + pic.RoleName + ')'
							};
						}) : [];

						$scope.assignPICModel.headlineId = $scope.headlineOptions.some(o => o.selected) ? initData.headlineId : null;
						$scope.sites = settings.sites;
					});
				});
			}]);
})();