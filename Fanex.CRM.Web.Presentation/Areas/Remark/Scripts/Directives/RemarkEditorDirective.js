'use strict';
/* global remarkApplication */
/* jshint camelcase: false */
/* jshint unused: vars */
remarkApplication.directive('remarkEditorDirective', function ($compile) {       
    return {
        restrict: 'C',
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (newval, oldval) {
                $compile(element.find(".textarea-remark"))(scope);
                var editor = $(element);
                if (!editor.hasClass("rich-remark-editor-directive")) {
                    editor.addClass("rich-remark-editor-directive");
                    $compile(element)(scope);
                }
            });
        }
    };
});