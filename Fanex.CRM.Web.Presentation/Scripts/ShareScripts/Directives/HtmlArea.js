"use strict";
/* global directiveManager */
/* jshint unused: vars */
/* jshint camelcase: false */
directiveManager = directiveManager || {};
directiveManager.registerHtmlArea = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        //        var defaultRegisterParams = {
        //            PlugInName: null,
        //            OptionName: null,
        //            PickerId: null
        //        };
        
        var editorId = registerParams.PickerId;
        function renderRemarkEditor(scope, element, attrs) {
            var options = scope[registerParams.OptionName];
            if (options === null) {
                return;
            }

            var selectedText = options.SelectedText;
            if (selectedText && selectedText !== "") {
                selectedText = selectedText.indexOf(" ", selectedText.length - 1) !== -1 ?
                    selectedText.substring(0, selectedText.length - 1) + "&nbsp;" :
                    selectedText;
            }

            var editorHTML =
                '<textarea id="' + editorId + '" name="' + editorId + '" class="remarkeditorbox">' +
                    selectedText +
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
                toolbar: [
                    ["html"], ["bold", "forecolor", "italic", "underline", "strikethrough", "|", "subscript", "superscript"],
                    ["increasefontsize", "decreasefontsize"],
                    ["orderedlist", "unorderedlist"],
                    ["indent", "outdent"],
                    ["justifyleft", "justifycenter", "justifyright"],
                    ["link", "unlink"], ["image", "horizontalrule"],
                    ["p", "h1", "h2", "h3", "h4", "yh5", "h6"],
                    ["cut", "copy", "paste"],
                    [{
                        css: "edit",
                        text: options.EditIconText,
                        action: function (btn) {
                            var s = this.getSelectedHTML();
                            this.pasteHTML(s);
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