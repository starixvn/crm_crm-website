(function () {
    "use strict";
    angular.module('changes-log-application').controller('changesLogController', ['$scope',
        function ($scope) {
            let vm = this;
            $scope.isShowFilter = true;
            vm.errorMessage = '';
            vm.isPicReport = false;
            vm.changeHeadlineDATab = function () {
                vm.isPicReport = false;
            };

            vm.changePICTab = function () {
                vm.isPicReport = true;
            };

            angular.element(document).ready(function () {
                $("#alertboxerr").addClass("alertboxerr");
            });
        }]);
})();