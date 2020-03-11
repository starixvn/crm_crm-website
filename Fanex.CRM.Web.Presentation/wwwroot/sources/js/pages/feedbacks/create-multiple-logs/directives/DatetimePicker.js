(function (angular) {
    "use strict";
    angular.module('createMultipleLogsModule').directive('datetimepicker', function () {
        return {
            restrict: 'A',
            scope: { onselect: '&' },
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                $(function () {
                    element.datetimepicker({
                        dateFormat: 'M dd, yy',
                        timeFormat: 'hh:mm TT',
                        stepHour: 1,
                        stepMinute: 1,
                        stepSecond: 1,
                        ampm: true,
                        changeMonth: true,
                        changeYear: true,
                        showOtherMonths: true,
                        selectOtherMonths: true,
                        yearRange: '-10:+0',
                        onSelect: function (date) {
                            scope.$apply(function () {
                                ngModelCtrl.$setViewValue(date);
                            });
                        }
                    });

                    if (scope.onselect && typeof (scope.onselect) === 'function') {
                        var id = attrs.id;
                        var hourSelect = $('#h_' + id);
                        var minuteSelect = $('#m_' + id);

                        $('#' + id)
                    .delegate(hourSelect, 'change', function () {
                        scope.onselect();
                    })
                    .delegate(minuteSelect, 'change', function () {
                        scope.onselect();
                    });
                    }
                });
            }
        };
    });
})(angular);