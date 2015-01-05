'use strict';
/* global remarkApplication,
sharedScriptManager,
hotKeyManager,
remarkResources */
/* jshint camelcase: false */
/* jshint unused: vars */
remarkApplication.directive('richRemarkEditorDirective', function () {
    function renderRemarkEditor(scope, element, attrs) {
        var textArea = element.find(".textarea-remark");
        var remarkIndex = parseInt(element.attr("data-remarkindex"));
        var isSpecialRemark = parseInt(element.attr("data-isspecialremark")) ? true : false;
        var remark = remarkText(isSpecialRemark, remarkIndex, scope);

        var editorHTML =
            '<textarea name="rich-remark-editor" class="remarkeditorbox">' +
                sharedScriptManager.encodeHtml(remark) +
            '</textarea>';
        textArea.remove();
        element.append(editorHTML);
        var editor = element.find("textarea");

        editor.htmlarea({
            changeEvent: function () {
                remarkIndex = parseInt(element.attr("data-remarkindex"));
                scope.updateRemark(
                    editor.htmlarea("toHtmlString"),
                    isSpecialRemark,
                    remarkIndex);
            },
            blurEvent: function () {
                if (editor.htmlarea("toHtmlString") === '' ||
                    editor.htmlarea("toHtmlString") === '<br>') {
                    editor.val(remarkResources.RemarkPlaceHolder).htmlarea('updateHtmlArea');
                }

                return hotKeyManager.documentBlur();
            },
            clickEvent: function () {
                if (editor.htmlarea("toHtmlString") === remarkResources.RemarkPlaceHolder) {
                    editor.val('').htmlarea('updateHtmlArea');
                }
            },
            toolbar: [
                ["html"], ["bold", "forecolor", "italic", "underline", "strikethrough", "|", "subscript", "superscript"],
                ["increasefontsize", "decreasefontsize"],
                ["orderedlist", "unorderedlist"],
                ["indent", "outdent"],
                ["justifyleft", "justifycenter", "justifyright"]
            ]
        });

        var editorcss = { 'color': '#666', 'font-family': 'inherit', 'font-size': '14px', 'padding': '2px 5px' };
        $('.jHtmlArea').find('iframe').css('height', '170px')
            .contents().find('body').css(editorcss);
        $('.remarkeditorbox').css('height', '170px');
    }

    function remarkText(isSpecialRemark, remarkIndex, scope) {
        if (isSpecialRemark) {
            return !scope.remarks || remarkIndex === scope.remarks.SpecialRemarks.length ?
                remarkResources.RemarkPlaceHolder :
                scope.remarks.SpecialRemarks[remarkIndex];
        }

        return !scope.remarks || remarkIndex === scope.remarks.NormalRemarks.length ?
            remarkResources.RemarkPlaceHolder :
            scope.remarks.NormalRemarks[remarkIndex];
    }

    function updateRemarkText(isSpecial, index, remark) {
        var editor = $('.rich-remark-editor-directive' +
                        '[data-remarkindex="' + index + '"]' +
                        '[data-isspecialremark="' + isSpecial + '"] textarea');
        if (editor.length && remark !== editor.htmlarea("toHtmlString")) {
            editor.val(remark).htmlarea('updateHtmlArea');
        }
    }

    return {
        restrict: 'C',
        link: function (scope, element, attrs) {
            renderRemarkEditor(scope, element, attrs);
            scope.$watch(function ($scope) {
                if (!$scope.remarks) {
                    return;
                }

                var remarkLength = $scope.remarks.SpecialRemarks.length;
                var isSpecial = 1;
                for (var i = 0; i < remarkLength; i++) {
                    updateRemarkText(isSpecial, i, $scope.remarks.SpecialRemarks[i]);
                }

                remarkLength = $scope.remarks.NormalRemarks.length;
                isSpecial = 0;
                for (var i = 0; i < remarkLength; i++) {
                    updateRemarkText(isSpecial, i, $scope.remarks.NormalRemarks[i]);
                }
            });
        }
    };
});