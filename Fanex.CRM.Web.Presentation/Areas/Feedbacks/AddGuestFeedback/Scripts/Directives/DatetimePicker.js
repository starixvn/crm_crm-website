'use strict';
/* global addGuestFeedbackApplication */
addGuestFeedbackApplication.directive('datetimepicker', function () {
    return {
        restrict: 'A',
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
            });
        }
    };
});