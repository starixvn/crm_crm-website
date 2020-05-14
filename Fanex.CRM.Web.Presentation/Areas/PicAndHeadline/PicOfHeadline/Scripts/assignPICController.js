(function () {
    'use strict';

    angular.module('picofheadline-application').controller('assignPICController',
        ['$scope', '$window', '$uibModal', '$uibModalInstance',
            function ($scope, $window, $uibModal, $uibModalInstance) {
                $scope.maxItems = 10000;
                $scope.relatedCustomers = [];
                $scope.resultMessages = [];
                $scope.isSellectedAll = false;
                $scope.isSuccessful = false;

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
                                    $scope.relatedCustomers = [];
                                    $scope.isSellectedAll = false;
                                } else {
                                    $scope.relatedCustomers.pop();
                                }

                                if ($scope.relatedCustomers.length === 0) {
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
                    const totalItems = $scope.relatedCustomers.length;
                    if (username !== undefined && username !== '' && totalItems < $scope.maxItems) {
                        username = username.length > MAX_CHARS ? username.substring(0, MAX_CHARS) + '...' : username;
                        $scope.relatedCustomers.push({ index: totalItems + 1, username, isValid: true });
                    }

                    setTimeout(() => {
                        $scope.relatedCustomerInput = '';
                        $scope.$apply();
                    }, 0);
                };

                $scope.removeRelatedCustomer = function (relatedCustomer) {
                    const index = $scope.relatedCustomers.indexOf(relatedCustomer);
                    if (index > -1) {
                        $scope.relatedCustomers.splice(index, 1);
                    }

                    if ($scope.relatedCustomers.length === 0) {
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

                    const totalItems = $scope.relatedCustomers.length + items.length;
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
                }

                angular.element(document).ready(function () {
                    $scope.headlines = [
                        {
                            "id": 1,
                            "text": "One"
                        }, {
                            "id": 2,
                            "text": "Two"
                        }, {
                            "id": 3,
                            "text": "Three"
                        }
                    ];
                });
        }]);
})();