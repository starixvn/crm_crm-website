(function () {
	"use strict";
    /* global crmConstants,
    viewResources,
    sharedScriptManager */
	/* jshint camelcase: false */
	angular.module('picofheadline-application').directive('selectWithSearchContainer', ['$document', '$window', function ($document, $window) {
		function renderSelect(scope, element, options) {
			if (!options || options.length === 0) {
				return;
			}

			$(element).select2({
				allowClear: true,
				placeholder: 'Please select',
				data: options,
				multiple: element.context.multiple || false,
				dropdownParent: $window.parent.$('body')
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
	}]);
})();