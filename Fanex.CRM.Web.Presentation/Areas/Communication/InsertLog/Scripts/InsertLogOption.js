(function () {
    "use strict";

    window.registerOptions = function ($scope) {
        $scope.productOptions = {
            DataList: [],
            OnChangeEvent: null
        };
        $scope.subProductOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedSubProduct
        };
    }
})();