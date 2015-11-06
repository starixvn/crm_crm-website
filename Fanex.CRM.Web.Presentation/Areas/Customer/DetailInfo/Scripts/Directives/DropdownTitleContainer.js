(function () {
    "use strict";
    angular.module('customerdetail-application').directive('dropdownTitleContainer', function () {
        function renderDropdown(scope, element) {
            /* jshint unused: vars */
            if (scope.titleList) {
                var dropdown = element.find("#customerinfodetail-titleoptions");
                dropdown.empty();
                var optionHtml = '';
                var titleLength = scope.titleList.length;
                for (var i = 0; i < titleLength; i++) {
                    var title = scope.titleList[i];
                    optionHtml += '<option value="' + title.Id + '"' +
                        (scope.customerDetail.Title.Id === title.Id ?
                        ' selected="selected"' :
                        '') + '>' + title.Value + '</option>';
                }

                dropdown.append(optionHtml);
                element.change(function () {
                    scope.updateSelectedTitle(dropdown.find('option:selected').val());
                });
            }
        }
        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    //jshint unused:false
                    renderDropdown(scope, element);
                });
            }
        };
    });
})();