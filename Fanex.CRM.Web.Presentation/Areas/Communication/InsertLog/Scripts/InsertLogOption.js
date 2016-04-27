(function () {
    "use strict";

    window.registerOptions = function ($scope) {
        $scope.productOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedProduct
        };
        $scope.subProductOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedSubProduct
        };        
    }
})();