(function () {
	"use strict";
    /* global viewResources,
    crmConstants,
    currentDate: true,
	registerHotkeyFunctions,
    moment,
    sharedScriptManager */
	/* jshint camelcase: false */
	angular.module('picofheadline-application').controller('picOfHeadlineController',
		['$scope', '$window', '$picOfHeadlineService', '$uibModal', '$timeout',
			function ($scope, $window, $picOfHeadlineService, $uibModal, $timeout) {
				$scope.validateFilterMessage = { isShowErrMsg: false, message: '' };
				$scope.siteOptions = null;
				$scope.statusOptions = null;
				$scope.positionOptions = null;
				$scope.currencyOptions = null;
				$scope.defaultFocusOptions = null;
				$scope.downloadFileOptions = null;
				$scope.removePic = [];
				$scope.picOfHeadlineOptions = {
					DataList: null,
					IsResize: false,
					IsUpdateGrid: false
				};
				$scope.searchCriteria = {
					Site: '',
					Currency: '',
					Status: '',
					HeadlineType: '',
					PicName: '',
					DirectAccount: '',
					GroupHeadline: '',
					Position: '',
					ExcludeAssignedAcc: false,
					ExcludeIgnoredAcc: true,
					ExcludeIsTest: true,
					PageIndex: 1,
					PageSize: 100,
					IsGetFromCached: false
				};
				$scope.lastSelectedParams = angular.copy($scope.searchCriteria);

				$scope.initController = function () {
					$scope.initPicker();
					$scope.getAllSetting();
					$scope.defaultFocusOptions = {
						IsSetFocus: false,
						OnChangeEvent: $scope.completeSetFocus
					};

					$scope.defaultFocusOptions.IsSetFocus = true;
					$scope.$apply();
				};

				$scope.initPicker = function () {
					$scope.siteOptions = {
						DataList: [],
						OnChangeEvent: $scope.updateSelectedSite
					};

					$scope.positionOptions = {
						DataList: [],
						OnChangeEvent: $scope.updateSelectedPosition,
						AllText: viewResources.All
					};

					$scope.currencyOptions = {
						DataList: [],
						OnChangeEvent: $scope.updateSelectedCurrency
					};

					$scope.statusOptions = {
						DataList: [
							{
								Id: 'Open',
								Name: 'Open'
							},
							{
								Id: 'Suspended',
								Name: 'Suspended'
							},
							{
								Id: 'Closed',
								Name: 'Closed'
							},
							{
								Id: 'Disabled',
								Name: 'Disabled'
							}
						],
						OnChangeEvent: $scope.updateSelectedStatus
					};
				};

				$scope.getAllSetting = function () {
					$scope.BlockUI();
					$scope.isLoadedAllSite = false;
					$picOfHeadlineService.getAllSetting()
						.then(function (data) {
							var allSetting = data.Result;
							$scope.siteOptions.DataList = allSetting.SiteList.map(site => {
								return {
									Id: site.Site,
									Name: site.Site
								};
							});
							$scope.positionOptions.DataList = allSetting.PositionList;
							$scope.currencyOptions.DataList = allSetting.CurrencyList;
							$scope.completedLoadFilterInfo();
						}, function () {
							$scope.completedLoadFilterInfo();
						});
				};

				$scope.completedLoadFilterInfo = function () {
					$scope.unblockUI();
				};

				$scope.updateSelectedSite = function (siteIdList) {
					$scope.searchCriteria.Site = siteIdList;

					if (!siteIdList) {
						$('#site--1').prop('checked', true);
						return;
					}
				};

				$scope.updateSelectedPosition = function (positionIdList) {
					$scope.searchCriteria.Position = positionIdList;
					if (!positionIdList) {
						$('#position--1').prop('checked', true);
						return;
					}
				};

				$scope.updateSelectedCurrency = function (currencyList) {
					$scope.searchCriteria.Currency = currencyList;
					if (!currencyList) {
						$('#currency--1').prop('checked', true);
						return;
					}
				};

				$scope.updateSelectedStatus = function (statusList) {
					$scope.searchCriteria.Status = statusList;
					if (!statusList) {
						$('#status--1').prop('checked', true);
						return;
					}
				};

				$scope.invokeHotKey_CtrlS = function () {
					$scope.getPICAssignment();
				};

				$scope.getPICAssignment = function () {
					$scope.searchCriteria.PageIndex = 1;
					const validation = $scope.validateFilterParams();
					$scope.validateFilterMessage = {
						isShowErrMsg: !validation.isValid,
						message: validation.errMsg
					};

					if (!validation.isValid) {
						return;
					}

					$scope.BlockUI();
					var getReportCriteria = {
						Site: $scope.searchCriteria.Site,
						Currency: $scope.searchCriteria.Currency,
						Status: $scope.searchCriteria.Status,
						HeadlineType: $scope.searchCriteria.HeadlineType,
						PicName: $scope.searchCriteria.PicName,
						DirectAccount: $scope.searchCriteria.DirectAccount,
						GroupHeadline: $scope.searchCriteria.GroupHeadline,
						Position: $scope.searchCriteria.Position,
						ExcludeIgnoredAcc: $scope.searchCriteria.ExcludeIgnoredAcc,
						ExcludeAssignedAcc: $scope.searchCriteria.ExcludeAssignedAcc,
						ExcludeIsTest: $scope.searchCriteria.ExcludeIsTest,
						PageIndex: $scope.searchCriteria.PageIndex,
						PageSize: $scope.searchCriteria.PageSize
					};

					$scope.lastSelectedParams = getReportCriteria;
					$picOfHeadlineService.getPICAssignment(getReportCriteria)
						.then(function (data) {
							$scope.picOfHeadlineOptions = {
								DataList: data.Result,
								IsUpdateGrid: true
							};
							$scope.removePic = [];
							$scope.unblockUI();
						}, function () {
							$scope.unblockUI();
						});
				};

				$scope.validateFilterParams = function () {
					var criteria = $scope.searchCriteria;

					if (criteria.Site === '') {
						return { isValid: false, errMsg: 'Please choose Site!' };
					}

					if (criteria.Currency === '') {
						return { isValid: false, errMsg: 'Please choose Currency!' };
					}

					if (criteria.Status === '') {
						return { isValid: false, errMsg: 'Please choose Status!' };
					}

					return { isValid: true, errMsg: '' };
				};

				$scope.pagingCustomerReport = function (pageIndex, pageSize) {
					$scope.BlockUI();
					$scope.searchCriteria.PageIndex = pageIndex;
					$scope.searchCriteria.PageSize = pageSize;
					$picOfHeadlineService.getPICAssignment($scope.searchCriteria)
						.then(function (data) {
							$scope.picOfHeadlineOptions = {
								DataList: data.Result,
								IsUpdateGrid: true
							};
							$scope.unblockUI();
						}, function () {
							$scope.unblockUI();
						});
				};

				$scope.exportReport = function () {
					$scope.downloadFileOptions = {
						URL: window.rootUrl + "PicOfHeadline/ExportExcelReport",
						Params: $scope.lastSelectedParams
					};

					$scope.$apply();
				};

				$scope.completeSetFocus = function () {
					$scope.defaultFocusOptions.IsSetFocus = false;
				};

				$scope.unblockUI = function () {
					if (parent && parent.$.unblockUI) {
						parent.$.unblockUI();
					}

					hotKeyManager.CachedData.isBlockingUI = false;
				};

				$scope.BlockUI = function () {
					hotKeyManager.CachedData.isBlockingUI = true;
					if (parent && parent.BlockUI) {
						parent.BlockUI();
					}
				};

				$scope.openAssignModal = function (headlineId = null, directAccounts = []) {
					const modalInstance = $uibModal.open({
						template: angular.element('#assign-pic-modal').html(),
						controller: 'assignPICController',
						backdrop: 'static',
						keyboard: false,
						size: 'lg',
						backdropClass: 'related-customer-modal-backdrop',
						windowClass: 'related-customer-modal',
						resolve: {
							initData: function () {
								return {
									headlineId: headlineId,
									directAccounts: directAccounts.length > 0 ?
										directAccounts.map((acc, index) => {
											return { index: index, username: acc, isValid: true };
										}) : []
								};
							}
						}
					});
					modalInstance.result.then(function (isReload) {
						if (isReload) {
							$scope.getPICAssignment();
						}
					});
				};

				$scope.onClickSingleCheckbox = function (assignment, isChecked) {
					if (isChecked) {
						$scope.removePic.push(assignment);
					} else {
						let index = $scope.removePic.findIndex(function (pic) {
							return pic.DirectAccountID === assignment.DirectAccountID &&
								pic.HeadlineID === assignment.HeadlineID;
						});
						$scope.removePic.splice(index, 1);
					}

					$scope.handleRemoveBtnState();
				};

				$scope.onClickSelectAllCheckbox = function (isChecked) {
					if (isChecked) {
						$scope.removePic = $scope.picOfHeadlineOptions.DataList.Items.filter(item => item.PicList !== null);
					} else {
						$scope.removePic = [];
					}
					$scope.handleRemoveBtnState();
				};

				$scope.handleRemoveBtnState = function () {
					if ($scope.removePic.length > 0) {
						angular.element(document).find('#remove-btn').removeAttr('disabled');
					} else {
						angular.element(document).find('#remove-btn').attr('disabled', 'disabled');
					}
					document.getElementById('select-all-chkbox').checked =
						$scope.removePic.length === $scope.picOfHeadlineOptions.DataList.Items.filter(item => item.PicList !== null).length;
				};

				$scope.onClickRemoveBtn = function () {
					const passedData = $scope.getRemoveAssignmentData();
					const modalInstance = $uibModal.open({
						template: angular.element('#remove-assignment-modal').html(),
						controller: 'removeAssignmentController',
						backdrop: 'static',
						keyboard: false,
						size: 'lg',
						backdropClass: 'related-customer-modal-backdrop',
						windowClass: 'remove-assignment-modal',
						resolve: {
							removeAssignmentData: function () {
								return passedData;
							}
						}
					});

					modalInstance.result.then(function (isReload) {
						if (isReload) {
							$scope.getPICAssignment();
						}
					});
				};

				$scope.getRemoveAssignmentData = function () {
					const removePic = $scope.removePic;
					const pics = [];
					const selectedPics = [];

					for (var i = 0; i < removePic.length; i++) {
						if (removePic[i].PicList !== null && removePic[i].PicList.length > 0) {
							removePic[i].PicList.map(p => {
								pics.push(p);

								selectedPics.push({
									HeadlineId: removePic[i].HeadlineID,
									CustId: removePic[i].DirectAccountID,
									PicName: p.PicName,
									PicId: p.PicId,
									IsSelected: true
								});
							});
						}
					}

					var distinctPics = Array.from(new Set(pics.map(pic => { return pic.PicId; })))
						.map(id => {
							return {
								PicId: id,
								PicName: pics.find(p => p.PicId === id).PicName,
								Role: pics.find(p => p.PicId === id).Role
							};
						});

					var removeAssignmentData = $scope.removePic.map(pic => {
						return {
							HeadlineId: pic.HeadlineID,
							HeadLineName: pic.HeadlineName,
							DirectAccountId: pic.DirectAccountID,
							DirectAccount: pic.DirectAccount,
							PicList: pic.PicList,
							PicAssignment: distinctPics.map(p => {
								return {
									PicId: p.PicId,
									PicName: p.PicName,
									IsSelectable: pic.PicList !== null && pic.PicList.some(v => v.PicId === p.PicId)
								};
							})
						};
					});

					return {
						distinctPics: distinctPics,
						selectedAssignment: selectedPics,
						data: removeAssignmentData
					};
				};

				$scope.onClickPlusBtn = function (headlineId, accounts) {
					$scope.openAssignModal(headlineId, accounts);
				};

				$(document).off('keydown').on('keydown', function (eventObject) {
					if (eventObject.keyCode === 13) // enter
					{
						$scope.getPICAssignment();
						eventObject.preventDefault();
						return false;
					}

					if (eventObject.ctrlKey) {
						switch (eventObject.keyCode) {
							case 83: // s
								$scope.getPICAssignment();
								eventObject.preventDefault();
								return false;

							case 73: // i
								$scope.openAssignModal();
								eventObject.preventDefault();
								return false;
						}
					}
				});

				angular.element(document).ready(function () {
					$scope.initController();
					const parentHead = $window.parent.$('head');
					const style = $('head link[href*="/Areas/PicAndHeadline/PicOfHeadline/Styles/PicOfHeadline.less"]').clone();

					parentHead.append(style);
				});
			}]);
})();