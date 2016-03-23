//(function () {
//    "use strict";
//    angular.module('communicationlog-insert-application').directive('dropdownSubProduct', function () {
//        function renderDropdown(scope, element) {
//            if (scope.extensionList) {
//                var dropdown = element.find("#subProduct");
//                dropdown.empty();
//                var productListLength = scope.subProductList.length;
//                for (var i = 0; i < productListLength; i++) {
//                    var product = scope.subProductList[i];
//                    dropdown.append(
//                    '<option value="' + product.Id + '">' + product.Value + '</option>');
//                }

//                element.change(function () {
//                    scope.updateSelectedProduct(dropdown.find('option:selected').val());
//                });
//            }
//        }

//        return {
//            restrict: 'A',
//            scope: true,
//            link: function (scope, element, attrs) {
//                scope.$watch(attrs.ngModel, function (newval, oldval) {
//                    /* jshint unused: false */
//                    renderDropdown(scope, element);
//                });
//            }
//        };
//    });
//})();