"use strict";
/* global insertLogApplication */
insertLogApplication.directive('dropdownSubCategory', function () {
    function renderDropdown(scope, element, attrs) {
        /* jshint unused: vars */
        if (scope.subCategoryList) {
            var dropdown = element.find("#subCategory");
            dropdown.empty();
            /* jshint camelcase: false */
            var subCategoryListLength = scope.subCategoryList.length;
            for (var i = 0; i < subCategoryListLength; i++) {
                var subCategory = scope.subCategoryList[i];
                dropdown.append(
                    '<option value="' + subCategory.Id + '"' +
                        (scope.communicationLog.SubCategory.Name !== '' && subCategory.Name === scope.communicationLog.SubCategory.Name ?
                            ' selected="selected"' :
                            '') +
                        '>' + subCategory.Name + '</option>');
            }

            element.change(function () {
                scope.updateSelectedSubCategory(dropdown.find('option:selected').val());
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