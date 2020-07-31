/*
 * Global variable: logActions
 */
(function () {
    "use strict";
    angular.module('changes-log-application').controller('changesLogController', ['$scope', '$changesLogFactory',
        function ($scope, $changesLogFactory) {
            let vm = this;
            $scope.errorMessage = '';
            $scope.isError = false;
            $scope.focused = true;
            $scope.logActions = '';
            vm.isPicReport = false;
            vm.changeHeadlineDATab = function () {
                $scope.isError = false;
                $scope.errorMessage = '';
                vm.isPicReport = false;
            };

            vm.changePicTab = function () {
                $scope.isError = false;
                $scope.errorMessage = '';
                vm.isPicReport = true;
            };

            angular.element(document).ready(function () {
                $('#changes-log-application .btn-group').removeClass('display-none');

                $changesLogFactory.getLogActions()
                    .then(function (data) {
                        logActions = JSON.parse(data.Result);
                    }, function () {
                    });
            });
        }]);
})();