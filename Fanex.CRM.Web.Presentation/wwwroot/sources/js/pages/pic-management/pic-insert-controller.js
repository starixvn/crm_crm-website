(function () {
	'use strict';

	angular.module('pic-management-application').controller('picInsertController', ['$scope', '$window', '$uibModal', '$picManagementFactory', '$uibModalInstance', function ($scope, $window, $uibModal, $picManagementFactory, $uibModalInstance) {
		let vm = this;
		vm.isDiabled = false;
		vm.username = '';
		vm.roles = [];
		vm.initSelectedRole = { Id: 0, Value: viewResources.Selected };
		vm.languageDropdownSettings = {
			idMember: 'Value',
			nameMember: 'Value',
			sourceData: [],
			checkedAll: false
		};

		vm.selectedCountry = { Id: 0, Name: viewResources.Selected };
		vm.remark = '';
		vm.communicationTools = [];
		vm.initialSelectedComm = { Id: -1, Value: viewResources.Selected };
		vm.messageResultOptions = new MessageResult('', false, false);

		// start muilti select
		vm.textSettings = {
			Filter: viewResources.Search,
			NoData: viewResources.NoData,
			NoItemChecked: viewResources.Selected,
			AllItemsSelected: viewResources.All,
			CheckAll: viewResources.All
		};

		vm.selectedLanguages = [];

		vm.languageChangedHandler = function () {
			vm.isSelectAllLanguage = vm.languageDropdownSettings.sourceData.length > 0
				&& vm.selectedLanguages.length === vm.languageDropdownSettings.sourceData.length;
		};

		vm.communicationToolItems = [
			{ selectedComm: vm.initialSelectedComm, Value: '' }
		];

		vm.getAllSetting = function () {
			vm.isDiabled = true;
			$picManagementFactory.getAllSetting()
				.then(function (data) {
					let allSetting = data.Result;
					vm.picTypes = allSetting.PicTypeList;
					vm.selectedPicType = vm.picTypes[0];
					vm.getRolesByPic(vm.selectedPicType.Id);
					vm.communicationTools = allSetting.CommunicationTools;
					vm.communicationTools.unshift(vm.initialSelectedComm);
					let languages = angular.copy(allSetting.Languages);

					vm.languageDropdownSettings = {
						idMember: 'Value',
						nameMember: 'Value',
						sourceData: languages,
						checkedAll: false
					};

					vm.countries = allSetting.Countries;
					vm.countries.unshift(vm.selectedCountry);
					vm.isDiabled = false;
				}, function () {
					vm.isDiabled = false;
				});
		};

		vm.getRolesByPic = function (selectedPicType) {
			vm.isDiabled = true;
			const param = { PicType: selectedPicType };
			$picManagementFactory.getRolesByPic(param)
				.then(function (data) {
					const roles = data.Result;
					vm.roles = roles;
					vm.selectedRole = vm.initSelectedRole
					vm.roles.unshift(vm.selectedRole);
					vm.isDiabled = false;
				}, function () {
					vm.isDiabled = false;
				});
		};

		vm.savePic = function () {
			vm.isDiabled = true;

			let communicationToolItems = [];
			for (let index = 0; index < vm.communicationToolItems.length; index++) {
				if (vm.communicationToolItems[index].selectedComm.Id > 0 && vm.communicationToolItems[index].Value.trim() !== '') {
					if (communicationToolItems.some(element => element.selectedComm.Id === vm.communicationToolItems[index].selectedComm.Id)) {
						let tempIndex = communicationToolItems.findIndex(element1 => element1.selectedComm.Id === vm.communicationToolItems[index].selectedComm.Id);
						communicationToolItems[tempIndex].Value = communicationToolItems[tempIndex].Value + ',' + vm.communicationToolItems[index].Value;
					}
					else {
						communicationToolItems.push(vm.communicationToolItems[index]);
					}
				}
			}

			let criteria = {
				Username: vm.username,
				PicType: vm.selectedPicType.Id,
				Role: vm.selectedRole.Id,
				Remark: vm.remark,
				CommunicationTools: communicationToolItems.map(commItem => {
                    const values = commItem.Value.split(',').map(value => value.trim()).filter(value => value !== '').toString();
                    return { Type: commItem.selectedComm.Id, Value: values }
                }),
				Language: vm.selectedLanguages.map(lang => lang.Id).join(','),
				Country: vm.selectedCountry.Id
			};

			let message = vm.validateParams(criteria);
			if (message === '') {
				vm.messageResultOptions = new MessageResult('', false, false);

				$picManagementFactory.insertPic(criteria)
					.then(function (data) {
						vm.showInsertResult(data.Result);
					}, function (error) {
                        vm.messageResultOptions = new MessageResult(error, true, false)
					});
			} else {
				vm.isDiabled = false;
				vm.messageResultOptions = new MessageResult(message, true, false)
			}
		};

		vm.validateParams = function (params) {
			let message = '';
			if (params.Username === '') {
				message = viewResources.Username + ' ' + viewResources.IsRequired;
			}
			else if (params.picType < 0) {
				message = viewResources.PicType + ' ' + viewResources.IsRequired;
			}
			else if (params.Role <= 0) {
				message = viewResources.Role + ' ' + viewResources.IsRequired;
			}
			else if (vm.communicationToolItems[0].selectedComm.Id < 0 || vm.communicationToolItems[0].Value.trim() === '') {
				message = viewResources.CommunicationTool + ' ' + viewResources.IsRequired;
			}

			return message;
		}

		vm.showInsertResult = function (result) {
			if (result.Code !== 0) {
				vm.isDiabled = false;
				vm.messageResultOptions = new MessageResult(result.Message, true, false);
				return;
			}

			vm.messageResultOptions = new MessageResult(result.Message, true, true);
			setTimeout(function () {
				vm.close()
			}, 1000);
		};

		vm.close = function () {
			if (vm.messageResultOptions.IsSuccess) {
				vm.messageResultOptions = new MessageResult('', false, false);
				$uibModalInstance.close({ isSuccess: true });
			}
			else {
				if (vm.isParamsChange()) {
					const modalConfirmInstance = $uibModal.open({
						template: angular.element('#template-pic-change-confirm').html(),
						controller: 'picChangeConfirmController',
						controllerAs: 'picChangeConfirmCtrl',
						backdrop: 'static',
						backdropClass: 'pic-change-confirm-modal-backdrop',
						keyboard: false,
						size: 'lg',
						windowClass: 'pic-change-confirm-modal',
						appendTo: $window.parent.$('body'),
					});

					modalConfirmInstance.result.then(function (isConfirmed) {
						if (isConfirmed) {
							$uibModalInstance.close();
						}
					});
				}
				else {
					$uibModalInstance.close();
				}
			}
		};

		vm.addCommunicationTool = function () {
			let item = { selectedComm: vm.initialSelectedComm, Value: '' };
			vm.communicationToolItems.push(item);
		};

		vm.removeCommunicationTool = function (item) {
			let index = vm.communicationToolItems.indexOf(item.x);
			if (index > -1) {
				vm.communicationToolItems.splice(index, 1);
			}
		};

		vm.isParamsChange = function () {
			let result = false;
			if (vm.username !== '' ||
				vm.selectedPicType.Id > 0 ||
				vm.selectedRole.Id > 0 ||
				vm.remark !== '' ||
				vm.selectedLanguages.length > 0 ||
				vm.selectedCountry.Id > 0 ||
				vm.communicationToolItems.length > 1 ||
				vm.communicationToolItems[0].selectedComm.Id > 0 ||
				vm.communicationToolItems[0].Value !== '') {
				result = true;
			}

			return result;
		};

		angular.element(document).ready(function () {
			vm.getAllSetting();
		});

		$($window.parent.document).off('keydown').on('keydown', function (eventObject) {
			if (!vm.isDiabled && eventObject.ctrlKey) {
				switch (eventObject.keyCode) {
					case 83: // s
						vm.savePic();
						$scope.$apply();
						eventObject.preventDefault();
						return false;
				}
			}
		});
	}]);
})();