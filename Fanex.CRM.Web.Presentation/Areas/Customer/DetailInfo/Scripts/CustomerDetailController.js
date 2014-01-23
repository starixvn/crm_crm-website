
customerDetailApplication.controller('customerDetailController', function ($scope,
    $http,
    $customerDetailService) {
    $scope.customerDetail = customerDetail;

    $scope.getCustomerInfoDetailById = function () {
        //getCustomerInfoDetailById
    };

    $scope.updateCustomerInfoDetail = function () {
        //updatedCustomerInfoDetailParams
    };

    angular.element(document).ready(function () {
        $scope.getCustomerInfoDetailById();
    });
});
