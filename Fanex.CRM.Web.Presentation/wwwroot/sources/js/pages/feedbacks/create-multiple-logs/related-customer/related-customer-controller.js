(function (angular) {
    'use strict';

    angular.module('createMultipleLogsModule').controller('RelatedCustomerController', relatedCustomerController);
    relatedCustomerController.$inject = ['$scope', '$window', '$uibModal', '$uibModalInstance', '$uiBlockerService', 'createMultipleLogsFactory',
        'headlines', 'relatedCustomerResults', 'isEdit'];

    function relatedCustomerController($scope, $window, $uibModal, $uibModalInstance, $uiBlockerService, createMultipleLogsFactory,
        headlines, relatedCustomerResults, isEdit) {
        let vm = this;
        vm.maxItems = 10000;
        vm.relatedCustomerResults = relatedCustomerResults;
        vm.relatedCustomerInput = '';
        vm.relatedCustomers = [];
        vm.resultMessages = [];
        vm.isSuccessful = false;
        vm.isEdit = isEdit;
        vm.saved = false;

        vm.close = function () {
            $uibModalInstance.close(vm.saved ? vm.relatedCustomerResults : relatedCustomerResults);
        };

        vm.keydownHandler = function (event) {
            switch (event.keyCode) {
                case 8: // backspace
                    if (vm.relatedCustomerInput === '') {
                        vm.relatedCustomers.pop();
                        return;
                    }
                    break;
                case 13: // enter
                case 32: // space
                case 188: // semicolon
                    if (vm.relatedCustomerInput === '') {
                        return;
                    }

                    vm.addRelatedCustomer(vm.relatedCustomerInput);
                    break;
                default:
                    break;
            }
        };

        vm.addRelatedCustomer = function (username) {
            const MAX_CHARS = 30;
            const totalItems = vm.relatedCustomers.length;
            if (username !== undefined && username !== '' && totalItems < vm.maxItems) {
                username = username.length > MAX_CHARS ? username.substring(0, MAX_CHARS) + '...' : username;
                vm.relatedCustomers.push({ index: totalItems + 1, username, isValid: true });
            }

            setTimeout(() => {
                vm.relatedCustomerInput = '';
                $scope.$apply();
            }, 0);
        };

        vm.removeRelatedCustomer = function (relatedCustomer) {
            const index = vm.relatedCustomers.indexOf(relatedCustomer);
            if (index > -1) {
                vm.relatedCustomers.splice(index, 1);
            }
        };

        vm.pasteHandler = function (event) {
            const items = event.originalEvent.clipboardData.getData('text/plain')
                .replace(/ /g, ',')
                .replace(/\r\n/g, ',')
                .replace(/\n/g, ',')
                .replace(/\t/g, ',')
                .split(',');

            if (items.length > vm.maxItems) {
                vm.isSuccessful = false;
                vm.resultMessages.push('The total number of related customers of feedback is over 10,000.');
                return;
            } else {
                vm.isSuccessful = true;
                vm.resultMessages = [];
            }

            items.forEach(item => {
                vm.addRelatedCustomer(item);
            });
        };

        vm.focusoutHandler = function () {
            vm.addRelatedCustomer(vm.relatedCustomerInput);
        };

        vm.verifyRelatedCustomer = function () {
            if (vm.relatedCustomers.length === 0) {
                vm.isSuccessful = false;
                vm.resultMessages.push('Username is mandatory!');
                return;
            } else {
                vm.isSuccessful = true;
                vm.resultMessages = [];
            }

            const headlineIds = headlines.map(x => x.CustId).join(','),
                username = vm.relatedCustomers.map(x => x.username).filter((value, index, self) => self.indexOf(value) === index).join(',');
            $uiBlockerService.blockUI();
            createMultipleLogsFactory.verifyRelatedCustomer({ headlineIds, username })
                .success(function (data) {
                    $uiBlockerService.unblockUI();

                    vm.relatedCustomers.forEach(item => {
                        const index = data.findIndex(x => x.Username === item.username && x.ResultTypeId > 0);
                        if (index > -1) {
                            item.isValid = false;
                        }
                    });

                    let invalidRelatedCustomers = data.filter(item => item.ResultTypeId === 1);
                    if (invalidRelatedCustomers.length > 0) {
                        vm.resultMessages.push('Invalid usernames: ' + invalidRelatedCustomers.map(x => x.Username).join(', '));
                    }

                    invalidRelatedCustomers = data.filter(item => item.ResultTypeId === 2 || item.ResultTypeId === 3);
                    if (invalidRelatedCustomers.length > 0) {
                        vm.resultMessages.push('The system does not support to add related customer for deposit customer/guests account. Please check usernames: '
                            + invalidRelatedCustomers.map(x => x.Username).join(', '));
                    }

                    invalidRelatedCustomers = data.filter(item => item.ResultTypeId === 4);
                    if (invalidRelatedCustomers.length > 0) {
                        vm.resultMessages.push('These usernames do not belong to any headline: '
                            + invalidRelatedCustomers.map(x => x.Username).join(', '));
                    }

                    invalidRelatedCustomers = data.filter(item => item.ResultTypeId === 5);
                    if (invalidRelatedCustomers.length > 0) {
                        vm.resultMessages.push('You do not allow to add related customers for itself. Please check usernames: '
                            + invalidRelatedCustomers.map(x => x.Username).join(', '));
                    }

                    if (vm.resultMessages.length > 0) {
                        vm.isSuccessful = false;
                        vm.relatedCustomerResults = [];
                    } else {
                        vm.isSuccessful = true;
                        vm.relatedCustomerResults = headlines.map(item => {
                            return {
                                HeadlineId: item.CustId,
                                Headline: item.Username,
                                RelatedCustomers: data.filter(x => x.HeadlineId === item.CustId)
                            };
                        });
                    }
                }).error(function () {
                    $uiBlockerService.unblockUI();
                });
        };

        vm.save = function () {
            vm.saved = true;
            vm.close();
        };

        vm.delete = function () {
            const modalInstance = $uibModal.open({
                template: angular.element('#template-delete-realted-customers').html(),
                controller: 'DeleteRelatedCustomerController',
                controllerAs: 'deleteRelatedCustomerCtrl',
                backdrop: 'static',
                keyboard: false,
                windowClass: 'delete-related-customer-modal',
                appendTo: $window.parent.$('body')
            });
            modalInstance.result.then(function (deleted) {
                if (deleted) {
                    vm.saved = true;
                    vm.relatedCustomerResults = [];
                    vm.close();
                }
            });
        };

        function initialize() {
            relatedCustomerResults.forEach(item => {
                item.RelatedCustomers.forEach(child => {
                    const index = vm.relatedCustomers.map(x => x.username).indexOf(child.Username);
                    if (index === -1) {
                        const totalItems = vm.relatedCustomers.length;
                        vm.relatedCustomers.push({ index: totalItems + 1, username: child.Username, isValid: true });
                    }
                });
            });
        }

        initialize();
    }
})(angular);