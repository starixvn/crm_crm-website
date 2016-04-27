(function () {
    "use strict";

    window.registerOptions = function ($scope) {
        $scope.productOptions = {
            DataList: [],
            SelectedId: $scope.communicationLog.Product.Id,
            OnChangeEvent: $scope.updateSelectedProduct
        };
        $scope.subProductOptions = {
            DataList: [],
            SelectedId: $scope.communicationLog.SubProduct.Id,
            OnChangeEvent: $scope.updateSelectedSubProduct
        };
    }
})();