(function () {
	"use strict";
	/* global sharedScriptManager */
	/* jshint camelcase: false */
	angular.module('picofheadline-application').directive('movableModal', ['$document', '$window',
		function ($document, $window) {
			return {
				restrict: 'A',
				link: function (scope, iElement, iAttrs) {
					var startX = 0,
						startY = 0,
						x = 0,
						y = 0;

					var dialogWrapper = iElement.parent();

					dialogWrapper.css({
						position: 'relative'
					});

					iElement.on('mousedown', function (event) {
						// Prevent default dragging of selected content
						event.preventDefault();
						startX = event.pageX - x;
						startY = event.pageY - y;
						$($window.parent.document).on('mousemove', mousemove);
						$($window.parent.document).on('mouseup', mouseup);
					});

					function mousemove(event) {
						y = event.pageY - startY;
						x = event.pageX - startX;
						dialogWrapper.css({
							top: y >= 0 ? y : 0 + 'px',
							left: x + 'px'
						});
					}

					function mouseup() {
						$($window.parent.document).unbind('mousemove', mousemove);
						$($window.parent.document).unbind('mouseup', mouseup);
					}
				}
			};
		}
	]);
})();