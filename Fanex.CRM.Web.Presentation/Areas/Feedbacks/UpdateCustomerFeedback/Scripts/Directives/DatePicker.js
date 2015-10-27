(function () {
    "use strict";
    /* global currentDate */
    angular.module('feedbacks-updatecustomerfeedback-application').directive('datepicker', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs, ngModelCtrl) {
                $(function () {
                    element.datepicker({
                        dateFormat: 'M dd, yy',
                        maxDate: new Date(currentDate),
                        changeMonth: true,
                        changeYear: true,
                        showOtherMonths: true,
                        selectOtherMonths: true,
                        yearRange: '-100:+0',
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
})();