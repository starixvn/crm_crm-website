'use strict';
remarkApplication.directive('alertBox', function () {
    var templateHtml = '<div data-ng-hide="dialogIsHidden()" class="row row-item">\
        <div class="col-xs-12 col-sm-12 col-md-12">\
        <div ng-class="{alertboxscc: alertResult.IsSuccess, alertboxerr: !alertResult.IsSuccess}">\
        <div data-ng-bind="alertResult.Message"></div>\
        </div></div></div>';

    return {
        restrict: 'A',
        replace: true,
        template: templateHtml,
        scope: {
            alertResult: '='
        },
        controller: ['$scope', function ($scope) {
            $scope.dialogIsHidden = function () {
                return $scope.alertResult.Message === '';
            };
        }],
        link: function (scope, element, attrs) {
        }
    };
});