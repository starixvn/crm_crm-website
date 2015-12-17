(function () {
    "use strict";
    /* global viewResources */
    /* exported registerOptions */
    window.registerOptions = function ($scope) {
        $scope.messageResultOptions = null;
        $scope.mainSettingOptions = null;
        $scope.isSetFocus = false;

        $scope.initDefaultOption = function () {
            $scope.mainSettingOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedMainSetting,
                AllText: viewResources.All
            };
        };
    };
})();