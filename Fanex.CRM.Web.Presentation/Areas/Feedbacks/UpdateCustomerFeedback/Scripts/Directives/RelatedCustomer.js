"use strict";
/* exported orgLengt, trim */
/* jshint unused: vars */
/* jshint camelcase: false */
(function () {
    angular.module('feedbacks-updatecustomerfeedback-application').directive('relatedcustomer', function () {
        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, element, attrs) {
                $(function () {
                    element.fcbkcomplete({
                        json_url: window.rootUrl + 'CustomerSetting/RelatedCustomerAutoSuggestion',
                        addontab: true,
                        maxitems: 500,
                        input_min_size: 1,
                        height: 10,
                        cache: false,
                        newel: true
                    });
                });

                scope.$watch(attrs.ngModel, function (newVal, oldVal) {
                    /* jshint unused: false */
                    var typecust = $('#typeCust').prop('disabled', !newVal);
                    if (!newVal) {
                        typecust.val('');
                    }
                });
            }
        };
    });
})();

var orgLengt = 0;
function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}