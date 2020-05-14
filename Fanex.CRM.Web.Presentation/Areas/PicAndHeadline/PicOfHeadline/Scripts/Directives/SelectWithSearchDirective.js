(function () {
    "use strict";
    /* global crmConstants,
    viewResources,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('picofheadline-application').directive('selectWithSearchContainer', function () {

        function renderSelect(scope, element, options) {
            if (!options || options.length === 0) {
                return;
            }

            $(element).select2({
                allowClear: true,
                placeholder: '',
                data: options
            });
        }

        return {
            restrict: 'C',
            require: 'ngModel',
            scope: {
                dataSources: '=ngModel',
                options: '=options'
            },
            link: function (scope, element) {
                scope.$watch('options', function (newval, oldval) {
                    renderSelect(scope, element, newval);
                });
            }
        };
    });
})();