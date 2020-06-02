(function () {
	"use strict";
    /* global crmConstants,
    viewResources,
    sharedScriptManager */
	/* jshint camelcase: false */
	angular.module('picofheadline-application').directive('selectWithSearchContainer', function () {
		function renderSelect(scope, element, options, multiple) {
			if (!options || options.length === 0) {
				return;
			}

			$(element).select2({
				allowClear: true,
				placeholder: 'Please select',
				data: options,
				multiple: multiple || false
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
					renderSelect(scope, element, newval, element.context.multiple);
				});
			}
		};
	});
})();