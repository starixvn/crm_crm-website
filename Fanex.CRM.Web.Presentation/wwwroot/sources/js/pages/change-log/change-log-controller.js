(function () {
    "use strict";
    angular.module('change-log-application').controller('changeLogController', ['$scope', '$window', '$uiBlockerService',
        function ($scope, $window, $uiBlockerService) {
            let vm = this;
            $scope.isShowFilter = true;
            vm.isPicReport = false;
            vm.errorMessage = '';
            angular.element(document).ready(function () {
                $("#alertboxerr").addClass("alertboxerr");
            });

            //$(document).off('keydown').on('keydown', function (eventObject) {
            //    if (eventObject.keyCode === 13) // enter
            //    {
            //        vm.getReport();
            //        eventObject.preventDefault();
            //        return false;
            //    }

            //    if (eventObject.ctrlKey) {
            //        switch (eventObject.keyCode) {
            //            case 83: // s
            //                vm.getReport();
            //                eventObject.preventDefault();
            //                return false;

            //            case 73: // i
            //                vm.insertPic();
            //                eventObject.preventDefault();
            //                return false;
            //        }
            //    }
            //});
        }]);
})();