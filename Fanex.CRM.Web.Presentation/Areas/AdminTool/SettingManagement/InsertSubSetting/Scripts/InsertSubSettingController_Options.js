(function () {
    "use strict";
    /* global viewResources */
    /* exported registerOptions */
    window.registerOptions = function ($scope) {
        $scope.messageResultOptions = null;
        $scope.parentSettingOptions = null;
        $scope.isSetFocus = false;

        $scope.initDefaultOption = function () {
            $scope.parentSettingOptions = {
                DataList: [],
                OnChangeEvent: $scope.updateSelectedParentSetting,
                AllText: viewResources.All
            };
        };
    };
})();