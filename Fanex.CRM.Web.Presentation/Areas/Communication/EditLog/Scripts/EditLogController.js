editLogApplication.controller('editLogController', function ($scope,
    $http,
    $editLogService) {
    $scope.communicationLog = communicationLog;
    $scope.extensionList = null;
    $scope.countryList = null;
    $scope.communicationToolList = null;
    $scope.languageList = null;
    $scope.mainCategoryList = null;

    $scope.isUpdatingTime = false;
    $scope.isLoadedAllExtension = false;
    $scope.isLoadedAllCountry = false;
    $scope.isLoadedAllCommunicationTool = false;
    $scope.isLoadedAllLanguage = false;
    $scope.isLoadedAllMainCategory = false;

    $scope.initController = function () {
        $scope.getAllExtension();
        $scope.getAllCountry();
        $scope.getCommunicationTool();
        $scope.getAllLanguage();
        $scope.getAllMainCategory();
    };
    $scope.getAllExtension = function () {
        $scope.BlockUI();
        $scope.isLoadedAllSite = false;
        $editLogService.getAllExtension()
            .then(function (data) {
                $scope.extensionList = data.Result;
                $scope.isLoadedAllExtension = true;
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                $scope.isLoadedAllExtension = true;
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getAllCountry = function () {
        $scope.BlockUI();
        $scope.isLoadedAllCountry = false;
        $editLogService.getAllCountry()
            .then(function (data) {
                $scope.countryList = data.Result;
                $scope.isLoadedAllCountry = true;
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                $scope.isLoadedAllCountry = true;
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getCommunicationTool = function () {
        $scope.BlockUI();
        $scope.isLoadedAllCountry = false;
        $editLogService.getAllCommunicationTool()
            .then(function (data) {
                $scope.communicationToolList = data.Result;
                $scope.isLoadedAllCommunicationTool = true;
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                $scope.isLoadedAllCountry = true;
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getAllLanguage = function () {
        $scope.BlockUI();
        $scope.isLoadedAllCountry = false;
        $editLogService.getAllLanguage()
            .then(function (data) {
                $scope.languageList = data.Result;
                $scope.isLoadedAllLanguage = true;
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                $scope.isLoadedAllLanguage = true;
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.getAllMainCategory = function () {
        $scope.BlockUI();
        $scope.isLoadedAllCountry = false;
        $editLogService.getAllMainCategory()
            .then(function (data) {
                $scope.mainCategoryList = data.Result;
                $scope.isLoadedAllMainCategory = true;
                $scope.completedLoadFilterInfo();
            },
            function (data) {
                $scope.isLoadedAllMainCategory = true;
                $scope.completedLoadFilterInfo();
            });
    };
    $scope.completedLoadFilterInfo = function () {
        if ($scope.isLoadedAllExtension &&
            $scope.isLoadedAllCountry &&
            $scope.isLoadedAllCommunicationTool &&
            $scope.isLoadedAllLanguage &&
            $scope.isLoadedAllMainCategory) {
            $scope.unblockUI();
        }
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
            $scope.communicationLog.EndTime = new Date(selectedTime);
            $scope.communicationLog.EndTime.setMinutes(selectedTime.getMinutes() + 1);
        }
    };
    $scope.updateEndTime = function (selectedTime) {
        $scope.communicationLog.EndTime = selectedTime;
        if (selectedTime < new Date($scope.communicationLog.StartTime)) {
            $scope.communicationLog.StartTime = new Date(selectedTime);
            $scope.communicationLog.StartTime.setMinutes(selectedTime.getMinutes() - 1);
        }
    };
    $scope.updateSelectedExtension = function (extention) {
        $scope.communicationLog.Extension = extention;
    };
    $scope.updateSelectedCountry = function (country) {
        $scope.communicationLog.Country = country;
    };
    $scope.updateSelectedCommunicationTool = function (communicationTool) {
        $scope.communicationLog.CommunicationTool = communicationTool;
    };
    $scope.updateSelectedLanguage = function (languageId) {
        $scope.communicationLog.LanguageId = languageId;
    };
    $scope.updateSelectedMainCategory = function (mainCategory) {
        $scope.communicationLog.MainCategory = mainCategory;
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