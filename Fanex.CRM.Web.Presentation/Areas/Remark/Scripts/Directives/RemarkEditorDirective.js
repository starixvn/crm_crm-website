'use strict';
/* global remarkApplication, remarkResources */
/* jshint camelcase: false */
/* jshint unused: vars */
remarkApplication.directive('remarkEditorDirective', function ($compile) {
    function hideEditorBox(scope, element, attrs) {
        var editor = $(element);
        if (editor.hasClass("rich-remark-editor-directive")) {
            var remark = element.find("textarea").htmlarea("toHtmlString");
            element.find(".jHtmlArea").remove();

            var remarkIndex = parseInt(element.attr("data-remarkindex"));
            var isSpecialRemark = parseInt(element.attr("data-isspecialremark")) ? true : false;

            var editorHTML = remarkArea(remarkIndex, isSpecialRemark, scope, remark);
            element.append(editorHTML);

            editor.removeClass("rich-remark-editor-directive");
            $compile(element)(scope);

            if ((isSpecialRemark && remarkIndex === scope.remarks.SpecialRemarks.length) ||
                (!isSpecialRemark && remarkIndex === scope.remarks.NormalRemarks.length)) {
                element.find(".textarea-remark").html(
                    remark === "" || remark === "<br>" ?
                    remarkResources.RemarkPlaceHolder :
                    remark);
            }
        }
    }

    function remarkArea(remarkIndex, isSpecialRemark, scope, remark) {
        return '<div class="textarea-remark" rows="4">' + remark + '</div>';
    }

    return {
        restrict: 'C',
        link: function (scope, element, attrs) {
            element.bind("click", function () {
                var editor = $(element);
                if (!editor.hasClass("rich-remark-editor-directive")) {
                    editor.addClass("rich-remark-editor-directive");
                    $compile(element)(scope);
                    element.find(".jHtmlArea iframe")[0].contentWindow.focus();
                }
            });

            element.mouseleave(function () {
                hideEditorBox(scope, element, attrs);
            });

            $compile(element.find(".textarea-remark"))(scope);
        }
    };
});