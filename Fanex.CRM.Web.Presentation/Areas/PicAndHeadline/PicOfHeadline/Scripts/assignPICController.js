(function () {
	'use strict';
	/* global hotKeyManager,
	viewResources,
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
					directAccounts: initData.directAccounts
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

				$scope.closeModal = function () {
					$uibModalInstance.close();
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
						const assignPICDirectAccounts = $scope.assignPICModel.directAccounts;
						assignPICDirectAccounts.map(function (account) {
							if (invalidListDirectAccounts.some(acc => acc.trim() === account.username.trim())) {
								account.isValid = false;
							}
						});

						$scope.assignPIC.directAccounts = assignPICDirectAccounts;
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
						size: 'sm',
						controller: function ($scope) {
							$scope.assignPIC = function (isKeepAssign) {
								$scope.$dismiss();
								assignPIC(isKeepAssign);
							};
							$scope.assignedDirectAccounts = assignedDirectAccounts;
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
								$scope.closeModal();
							}, 3000);
						} else {
							$scope.messageResultOptions = new MessageResult(result.Message, true, false);
						}
					});
				};

				$scope.validateParams = function (params) {
					var result = true;
					var message = '';
					if (!params.headlineId || parseInt(params.headlineId, 10) <= 0) {
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

				angular.element(document).ready(function () {
					$picOfHeadlineService.GetAssignPICSettings().then(function (data) {
						var settings = data.Result;
						$scope.headlineOptions = settings.groupHeadlines ? settings.groupHeadlines.map(headline => {
							return {
								id: headline.HeadLineID,
								text: headline.HeadlineName,
								selected: initData.headlineId ? initData.headlineId === headline.HeadLineID : false
							};
						}) : [];
						$scope.picOptions = settings.pics ? settings.pics.map(pic => {
							return {
								id: pic.PicId,
								text: pic.Username + ' (' + pic.Role + ')'
							};
						}) : [];
						$scope.assignPICModel.headlineId = $scope.headlineOptions.some(o => o.selected) ? initData.headlineId : null;
						$scope.sites = settings.sites;
					});
				});
			}]);
})();