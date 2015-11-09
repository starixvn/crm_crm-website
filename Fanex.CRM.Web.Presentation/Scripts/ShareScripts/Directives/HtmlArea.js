"use strict";
/* global directiveManager: true,
sharedScriptManager,
hotKeyManager,
currentUsername */
/* jshint unused: false */
/* jshint camelcase: false */
directiveManager = directiveManager || {};
directiveManager.registerHtmlArea = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        var editorId = registerParams.PickerId;
        function renderRemarkEditor(scope, element, attrs) {
            var options = scope[registerParams.OptionName];
            if (options == null) {
                return;
            }

            var selectedText = options.SelectedText;
            if (selectedText !== "") {
                selectedText = selectedText.indexOf(" ", selectedText.length - 1) !== -1 ?
                    selectedText.substring(0, selectedText.length - 1) + "&nbsp;" :
                    selectedText;
            }

            var editorHTML =
                '<textarea id="' + editorId + '" name="' + editorId + '" class="remarkeditorbox">' +
                   sharedScriptManager.encodeHtml(selectedText) +
                '</textarea>';

            element.html(editorHTML);
            var editor = element.find("#" + editorId);
            editor.htmlarea({
                changeEvent: function () {
                    if (options.OnChangeEvent &&
                        typeof (options.OnChangeEvent) === "function") {
                        options.OnChangeEvent(editor.htmlarea("toHtmlString"));
                    }
                },
                keyUpEvent: function (eventObject) {
                    hotKeyManager.keyUp(eventObject);
                },
                keyDownEvent: function (eventObject) {
                    return hotKeyManager.documentKeyDown(eventObject);
                },
                blurEvent: function () {
                    return hotKeyManager.documentBlur();
                },
                toolbar: [
                    ["html"], ["bold", "forecolor", "italic", "underline", "strikethrough", "|", "subscript", "superscript"],
                    ["increasefontsize", "decreasefontsize"],
                    ["orderedlist", "unorderedlist"],
                    ["indent", "outdent"],
                    ["justifyleft", "justifycenter", "justifyright"],
                    ["link", "unlink"], ["image", "horizontalrule"],
                    ["p", "h1", "h2", "h3", "h4", "h5", "h6"],
                    ["cut", "copy", "paste"],
                    [{
                        css: "edit",
                        text: options.EditIconText,
                        action: function (btn) {
                            var editText = this.getSelectedHTML();
                            editText = sharedScriptManager.insertEditDescriptionBlock(currentUsername, editText, parent.homeManager.currentTime);
                            this.pasteHTML(editText);
                            if (options.OnChangeEvent &&
                                typeof (options.OnChangeEvent) === "function") {
                                options.OnChangeEvent(editor.htmlarea("toHtmlString"));
                            }
                        }
                    }]
               ]
            });

        }

        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newval, oldval) {
                    renderRemarkEditor(scope, element, attrs);
                });
            }
        };
    });
};