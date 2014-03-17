"use strict";
/* global editLogApplication, 
communicationLog,
viewResources,
hotKeyManager */
editLogApplication.directive('reviewEditorContainer', function () {
    var editorId = "revieweditorbox";
    function renderEditor(scope, element, attrs) {
        /* jshint unused: vars */
        var editorHTML =
        '<textarea id="' + editorId + '" name="' + editorId + '" class="remarkeditorbox">' +
            communicationLog.Review +
        '</textarea>';

        element.html(editorHTML);
        var editor = element.find("#" + editorId);
        /* jshint camelcase: false */
        editor.htmlarea({
            changeEvent: function () {
                scope.updateReview(editor.htmlarea("toHtmlString"));
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
                ["p", "h1", "h2", "h3", "h4", "yh5", "h6"],
                ["cut", "copy", "paste"],
                [{
                    css: "edit",
                    text: viewResources.Insert_Edit_Block,
                    action: function (btn) {
                        /* jshint unused: vars */
                        var s = this.getSelectedHTML();
                        this.pasteHTML(s);
                    }
                }]
           ]
        });

        if (!scope.hasReviewPermission) {
            element.find(".jHtmlArea").attr("disabled", "true");
        }
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (newval, oldval) {
                /* jshint unused: vars */
                renderEditor(scope, element, attrs);
            });
        }
    };
});