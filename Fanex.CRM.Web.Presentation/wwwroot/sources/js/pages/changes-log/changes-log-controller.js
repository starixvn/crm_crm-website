(function () {
    "use strict";
    angular.module('changes-log-application').controller('changesLogController', ['$scope',
        function ($scope) {
            let vm = this;
            $scope.errorMessage = '';
            $scope.isError = false;
            vm.isPicReport = false;
            vm.changeHeadlineDATab = function () {
                vm.isPicReport = false;
            };

            vm.changePicTab = function () {
                $scope.isError = false;
                $scope.errorMessage = '';
                vm.isPicReport = true;
            };

            angular.element(document).ready(function () {
                $('#changes-log-application .btn-group').removeClass('display-none');
            });
        }]);
})();