(function () {
    "use strict";

    window.registerOptions = function ($scope) {
        $scope.productOptions = {
            DataList: [],
            SelectedId: null,
            OnChangeEvent: $scope.updateSelectedProduct
        };
        $scope.subProductOptions = {
            DataList: [],
            SelectedId: null,
            OnChangeEvent: $scope.updateSelectedSubProduct
        };
    }
})();