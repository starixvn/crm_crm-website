(function () {
    "use strict";
    angular.module('communicationlog-insert-application').directive('dropdownMainCategory', function () {
        function renderDropdown(scope, element) {
            /* jshint unused: vars */
            if (scope.extensionList) {
                var dropdown = element.find("#mainCategory");
                dropdown.empty();
                /* jshint camelcase: false */
                var mainCategoryListLength = scope.mainCategoryList.length;
                for (var i = 0; i < mainCategoryListLength; i++) {
                    var mainCategory = scope.mainCategoryList[i];
                    dropdown.append(
                    '<option value="' + mainCategory.Id + '">' + mainCategory.Name + '</option>');
                }

                element.change(function () {
                    scope.updateSelectedMainCategory(dropdown.find('option:selected').val());
                });
            }
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    /* jshint unused: false */
                    renderDropdown(scope, element);
                });
            }
        };
    });
})();