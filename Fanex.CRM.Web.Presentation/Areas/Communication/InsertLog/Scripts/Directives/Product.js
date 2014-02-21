"use strict";
/* global insertLogApplication */
insertLogApplication.directive('dropdownProduct', function () {
    function renderDropdown(scope, element, attrs) {
        /* jshint unused: vars */
        if (scope.extensionList) {
            var dropdown = element.find("#product");
            dropdown.empty();
            /* jshint camelcase: false */
            var productListLength = scope.productList.length;
            for (var i = 0; i < productListLength; i++) {
                var product = scope.productList[i];
                dropdown.append(
                    '<option value="' + product.Id + '">' + product.Value + '</option>');
            }

            element.change(function () {
                scope.updateSelectedProduct(dropdown.find('option:selected').val());
            });
        }
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (newval, oldval) {
                /* jshint unused: vars */
                renderDropdown(scope, element, attrs);
            });
        }
    };
});