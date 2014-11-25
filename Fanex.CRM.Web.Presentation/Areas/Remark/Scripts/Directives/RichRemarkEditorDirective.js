'use strict';
/* global remarkApplication,
sharedScriptManager,
hotKeyManager,
keyboardManager,
remarkResources */
/* jshint camelcase: false */
/* jshint unused: vars */
remarkApplication.directive('richRemarkEditorDirective', function () {
    var isAlt = false;
    var ctrlKeyCode = keyboardManager.Enums.KeyCode.Ctrl,
        enterKeyCode = keyboardManager.Enums.KeyCode.Enter;

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
            keyUpEvent: function (eventObject) {
                if (eventObject.which === ctrlKeyCode) {
                    isAlt = false;
                }

                hotKeyManager.keyUp(eventObject);
            },
            keyDownEvent: function (eventObject) {
                var keyCode = eventObject.which;
                if (keyCode === ctrlKeyCode) {
                    isAlt = true;
                }
                else if (keyCode === enterKeyCode && isAlt) {
                    scope.$apply(function () {
                        if (isSpecialRemark && remarkIndex === scope.remarks.SpecialRemarks.length) {
                            scope.insertSpecialRemark();
                        } else if (!isSpecialRemark && remarkIndex === scope.remarks.NormalRemarks.length) {
                            scope.insertRemark();
                        }

                        $('textarea').val("").htmlarea('updateHtmlArea');
                    });
                }

                var isBubble = hotKeyManager.documentKeyDown(eventObject);
                if (isBubble) {
                    parent.hotKeyManager.CachedData.KeyPress = hotKeyManager.CachedData.KeyPress;
                    isBubble = parent.hotKeyManager.documentKeyDown(eventObject);
                }

                return isBubble;
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

        $('.jHtmlArea').find('iframe').css('height', '170px');
        $('.remarkeditorbox').css('height', '194px');
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

    return {
        restrict: 'C',
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (newval, oldval) {
                renderRemarkEditor(scope, element, attrs);
            });
        }
    };
});