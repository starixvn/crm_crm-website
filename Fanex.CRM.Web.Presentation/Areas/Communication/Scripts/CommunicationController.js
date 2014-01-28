communicationApplication.controller('communicationController', function ($scope,
    $http,
    $communicationService) {
    $scope.logs = communicationLogs;

    $scope.initController = function () {
    };

    $scope.invokeHotKey_CtrlI = function () {
    };
    $scope.editCommunicationLog = function (logIndex) {
        parent.OpenPopup(
            window.rootUrl + "CommunicationLog/OpenEditView?" + $.param({
                logRunningNumber: $scope.logs[logIndex].RunningNumber
            }),
            viewResources.Edit_Communication_Log,
            $(parent.document).width() - 200,
            $(parent.document).height() - 70,
            true,
            false,
            parent.window,
            3000,
            null,
            null);
    };
    angular.element(document).ready(function () {
        $scope.initController();
        hotKeyManager.CachedData.ActiveDocumentManager.push({
            Value: $scope,
            Id: "CustomerCommunicationLogs"
        });
    });
});