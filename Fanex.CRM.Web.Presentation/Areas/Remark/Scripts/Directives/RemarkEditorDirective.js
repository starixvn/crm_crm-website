(function () {
    "use strict";
    /* jshint camelcase: false */
    /* jshint unused: vars */
    angular.module('remark-application').directive('remarkEditorDirective', function ($compile) {
        return {
            restrict: 'C',
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    /* jshint unused: false */
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
})();