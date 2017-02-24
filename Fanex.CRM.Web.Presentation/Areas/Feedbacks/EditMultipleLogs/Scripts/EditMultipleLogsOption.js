(function () {
    "use strict";

    window.registerOptions = function ($scope) {
        $scope.productOptions = {
            DataList: [],
            SelectedId: $scope.multipleLogs.Product.Id,
            OnChangeEvent: $scope.updateSelectedProduct
        };
        $scope.subProductOptions = {
            DataList: [],
            SelectedId: $scope.multipleLogs.SubProduct.Id,
            OnChangeEvent: $scope.updateSelectedSubProduct
        };
    }
})();