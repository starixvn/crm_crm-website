(function (angular) {
    'use strict';

    angular.module('commToolSettingModule').directive('tipsy', tipsyDirective);
    tipsyDirective.$inject = [];
    function tipsyDirective() {
        return {
            restrict: 'A',
            scope: {
                gravity: '=?'
            },
            link: function (scope, element) {
                element.tipsy({
                    gravity: scope.gravity ? scope.gravity : 'nw'
                });
            }
        };
    }
})(angular);