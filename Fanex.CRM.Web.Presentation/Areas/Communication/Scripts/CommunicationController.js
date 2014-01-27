communicationApplication.controller('communicationController', function ($scope,
    $http,
    $communicationService) {
    $scope.logs = communicationLogs;

    $scope.initController = function () {
    };

    $scope.invokeHotKey_CtrlI = function () {
    };
    $scope.editCommunicationLog = function (logIndex) {
        alert($scope.logs[logIndex].RunningNumber);
    };
    angular.element(document).ready(function () {
        $scope.initController();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "CustomerCommunicationLogs"
        });
    });
});