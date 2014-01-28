editLogApplication.controller('editLogController', function ($scope,
    $http,
    $editLogService) {
    $scope.communicationLog = communicationLog;
    $scope.isUpdatingTime = false;

    $scope.initController = function () {
    };
    $scope.invokeHotKey_CtrlS = function () {
    };

    $scope.invokeHotKey_CtrlV = function () {
    };

    $scope.editLog = function () {
        //        $scope.BlockUI();
        //        $editLogService.editLog()
        //            .then(function (data) {
        //                $scope.communicationLog = data.Result;
        //                $scope.unblockUI();
        //            },
        //            function (data) {
        //                $scope.unblockUI();
        //            });
    };

    $scope.updateStartTime = function (selectedTime) {
        $scope.communicationLog.StartTime = selectedTime;
        if (new Date($scope.communicationLog.EndTime) < selectedTime) {
            $scope.communicationLog.EndTime = selectedTime;
        }
    };
    $scope.updateEndTime = function (selectedTime) {
        $scope.communicationLog.EndTime = selectedTime;
        if (selectedTime < new Date($scope.communicationLog.StartTime)) {
            $scope.communicationLog.StartTime = selectedTime;
        }
    };

    $scope.unblockUI = function () {
        try {
            parent.$.unblockUI();
        } catch (e) {
        }
    };
    $scope.BlockUI = function () {
        try {
            parent.BlockUI();
        } catch (e) {
        }
    };
    angular.element(document).ready(function () {
        $scope.initController();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "CustomerCommunicationLogs_Edit"
        });
    });
});