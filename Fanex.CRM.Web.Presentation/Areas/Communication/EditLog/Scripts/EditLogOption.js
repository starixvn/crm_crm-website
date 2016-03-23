(function () {
    "use strict";

    window.registerOptions = function ($scope) {
        $scope.updateSelectedSubProduct = function (subProduct) {
            $scope.communicationLog.Product = subProduct;
            $scope.betTypeList = subProduct.ChildSettings;
            $scope.communicationLog.BetType = $scope.betTypeList.length > 0 ?
            $scope.betTypeList[0] :
                {
                    Id: crmConstants.Enums.BetType.NotApply
                };
            $scope.$apply();
            $scope.updateChangeFlag();
        };

        $scope.productOptions = {
            DataList: [],
            SelectedId: null,
            OnChangeEvent: null
        };
        $scope.subProductOptions = {
            DataList: [],
            SelectedId: null,
            OnChangeEvent: $scope.updateSelectedSubProduct
        };
    }
})();