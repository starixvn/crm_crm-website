(function (angular) {
    'use strict';

    angular.module('createMultipleLogsModule').controller('RelatedCustomerController', relatedCustomerController);
    relatedCustomerController.$inject = ['$scope', '$window', '$uibModal', '$uibModalInstance', '$uiBlockerService', 'createMultipleLogsFactory',
        'headlines', 'relatedCustomerResults', 'isEdit'];

    function relatedCustomerController($scope, $window, $uibModal, $uibModalInstance, $uiBlockerService, createMultipleLogsFactory,
        headlines, relatedCustomerResults, isEdit) {
        let vm = this;
        vm.maxItems = 10000;
        vm.maximumRelatedCustomersPerFeedback = 2000;
        vm.relatedCustomerResults = relatedCustomerResults;
        vm.relatedCustomerInput = '';
        vm.relatedCustomers = [];
        vm.overRelatedCustomers = [];
        vm.resultMessages = [];
        vm.isSellectedAll = false;
        vm.isSuccessful = false;
        vm.isEdit = isEdit;
        vm.saved = false;

        vm.close = function () {
            $uibModalInstance.close(vm.saved ? vm.relatedCustomerResults : relatedCustomerResults);
        };

        vm.keydownHandler = function (event) {
            // ctrl + a
            if (event.ctrlKey && event.keyCode === 65 && vm.relatedCustomerInput === '') {
                vm.isSellectedAll = true;
                return;
            }

            switch (event.keyCode) {
                case 8: // backspace
                    if (vm.relatedCustomerInput === '') {
                        if (vm.isSellectedAll) {
                            vm.relatedCustomers = [];
                            vm.isSellectedAll = false;
                        } else {
                            vm.relatedCustomers.pop();
                        }

                        if (vm.relatedCustomers.length === 0) {
                            vm.resultMessages = [];
                        }

                        return;
                    }
                    break;
                case 13: // enter
                case 32: // space
                case 188: // semicolon
                    vm.relatedCustomerInput = vm.relatedCustomerInput.replace(',', '');
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

            if (vm.relatedCustomers.length === 0) {
                vm.resultMessages = [];
            }
        };

        vm.pasteHandler = function (event) {
            const items = event.originalEvent.clipboardData.getData('text/plain')
                .replace(/ /g, ',')
                .replace(/\r\n/g, ',')
                .replace(/\n/g, ',')
                .replace(/\t/g, ',')
                .split(',').filter(item => item !== '');

            const totalItems = vm.relatedCustomers.length + items.length;
            vm.resultMessages = [];
            if (totalItems > vm.maxItems) {
                setTimeout(() => {
                    vm.relatedCustomerInput = '';
                    $scope.$apply();
                }, 0);
                vm.isSuccessful = false;
                vm.resultMessages.push(`You are not allowed to input over ${vm.maxItems} usernames. Please check it!`);
                return;
            } else {
                vm.isSuccessful = true;
            }

            items.forEach(item => {
                vm.addRelatedCustomer(item);
            });
        };

        vm.focusoutHandler = function () {
            vm.isSellectedAll = false;
            vm.addRelatedCustomer(vm.relatedCustomerInput);
        };

        vm.verifyRelatedCustomer = function () {
            vm.resultMessages = [];
            if (vm.relatedCustomers.length === 0) {
                vm.isSuccessful = false;
                vm.resultMessages.push('Username is mandatory!');
                return;
            } else {
                vm.isSuccessful = true;
            }

            const headlineIds = headlines.map(x => x.CustId).join(','),
                username = vm.relatedCustomers.map(x => x.username).filter((value, index, self) => self.indexOf(value) === index).join(',');
            $uiBlockerService.blockUI();
            createMultipleLogsFactory.verifyRelatedCustomer({ headlineIds, username })
                .success(function (data) {
                    $uiBlockerService.unblockUI();

                    const relatedCustomerResults = headlines.map(item => {
                        return {
                            HeadlineId: item.HeadlineId,
                            Headline: item.Headline,
                            CustId: item.CustId,
                            Username: item.Username,
                            RelatedCustomers: data.filter(x => x.HeadlineId === item.CustId)
                        };
                    });
                    vm.overRelatedCustomers = relatedCustomerResults.filter(item => item.RelatedCustomers.length > vm.maximumRelatedCustomersPerFeedback);

                    vm.relatedCustomers.forEach(item => {
                        const index = data.findIndex(x => x.Username === item.username && x.ResultTypeId > 0),
                            overIndex = vm.overRelatedCustomers.findIndex(x => x.Username === item.username);
                        if (index > -1 || overIndex > -1) {
                            item.isValid = false;
                        }
                    });

                    let invalidRelatedCustomers = data.filter(item => item.ResultTypeId === 1);
                    if (invalidRelatedCustomers.length > 0) {
                        vm.resultMessages.push('Invalid usernames: ' + invalidRelatedCustomers.map(x => x.Username).join(', '));
                    }

                    invalidRelatedCustomers = data.filter(item => item.ResultTypeId === 2 || item.ResultTypeId === 3);
                    if (invalidRelatedCustomers.length > 0) {
                        vm.resultMessages.push('The system does not support to add related customer for deposit and guest customer. Please check usernames: '
                            + invalidRelatedCustomers.map(x => x.Username).join(', '));
                    }

                    invalidRelatedCustomers = data.filter(item => item.ResultTypeId === 4);
                    if (invalidRelatedCustomers.length > 0) {
                        vm.resultMessages.push('These usernames do not belong to any headline or direct accounts without group headline: '
                            + invalidRelatedCustomers.map(x => x.Username).join(', '));
                    }

                    invalidRelatedCustomers = data.filter(item => item.ResultTypeId === 5);
                    if (invalidRelatedCustomers.length > 0) {
                        vm.resultMessages.push('You do not allow to add related customers for itself. Please check usernames: '
                            + invalidRelatedCustomers.map(x => x.Username).join(', '));
                    }

                    if (vm.resultMessages.length > 0 || vm.overRelatedCustomers.length > 0) {
                        vm.isSuccessful = false;
                        vm.relatedCustomerResults = [];
                    } else {
                        vm.isSuccessful = true;
                        vm.relatedCustomerResults = relatedCustomerResults;
                    }
                }).error(function () {
                    $uiBlockerService.unblockUI();
                });
        };

        vm.viewRelatedCustomer = function (custId) {
            const index = vm.relatedCustomerResults.findIndex(item => item.CustId === custId);
            $uibModal.open({
                template: angular.element('#template-view-related-customers').html(),
                controller: 'ViewRelatedCustomerController',
                controllerAs: 'viewRelatedCustomerCtrl',
                backdrop: 'static',
                keyboard: false,
                size: 'lg',
                windowClass: 'view-related-customer-modal',
                appendTo: $window.parent.$('body'),
                resolve: {
                    relatedCustomers: () => vm.relatedCustomerResults[index].RelatedCustomers.map(x => x.Username).join(', ')
                }
            });
        };

        vm.viewOverRelatedCustomer = function (item) {
            $uibModal.open({
                template: angular.element('#template-view-related-customers').html(),
                controller: 'ViewRelatedCustomerController',
                controllerAs: 'viewRelatedCustomerCtrl',
                backdrop: 'static',
                keyboard: false,
                size: 'lg',
                windowClass: 'view-related-customer-modal',
                appendTo: $window.parent.$('body'),
                resolve: {
                    relatedCustomers: () => item.RelatedCustomers.map(x => x.Username).join(', ')
                }
            });
        };

        vm.save = function () {
            vm.saved = true;
            vm.close();
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