(function () {
    "use strict";
    /* jshint unused: vars,
    camelcase: false */
    /* global hotKeyManager */
    /* exported registerHotkeyFunctions */
    function registerHotkeyFunctions($scope) {
        $scope.registerHotKey = function () {
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "Feedback_AddCustomerFeedback"
            });
        };
        $scope.invokeHotKey_CtrlS = function () {
            $scope.addFeedback();
            $scope.safeApply();
        };
        $scope.invokeHotKey_CtrlE = function () {
            if ($scope.addCustomerFeedbackParams.RelatedCustomerId === '' ||
                $scope.addCustomerFeedbackParams.RelatedCustomerId === null ||
                $scope.addCustomerFeedbackParams.RelatedCustomerId === undefined) {
                return;
            }

            $scope.verifyUserName();
        };
    }
})();