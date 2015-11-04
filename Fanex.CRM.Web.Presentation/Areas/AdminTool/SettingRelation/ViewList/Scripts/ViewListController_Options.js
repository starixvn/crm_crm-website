(function () {
    "use strict";
    /* jshint unused: vars,
    camelcase: false */
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