(function () {
    "use strict";
    /* jshint unused: vars,
    camelcase: false */
    /* global viewResources */
    /* exported registerOptions */
    function registerOptions($scope) {
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
    }
})();