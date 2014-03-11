"use strict";
/* global editLogApplication, viewResources */
editLogApplication.directive('remarkEditorContainer', function () {
    var editorId = "remarkeditorbox";
    function renderRemarkEditor(scope, element, attrs) {
        /* jshint unused: vars */
        var editorHTML =
        '<textarea id="' + editorId + '" name="' + editorId + '" class="remarkeditorbox">' +
            scope.redemptionLog.Remark +
        '</textarea>';

        element.html(editorHTML);
        var editor = element.find("#" + editorId);
        /* jshint camelcase: false */
        editor.htmlarea({
            changeEvent: function () {
                scope.updateRemark(editor.htmlarea("toHtmlString"));
            },
            keyUpEvent: function (eventObject) {
                hotKeyManager.keyUp(eventObject);
            },
            keyDownEvent: function (eventObject) {
                return hotKeyManager.documentKeyDown(eventObject);
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

        $('.jHtmlArea').find('iframe').css('height', '170px');
        $('#remarkeditorbox').css('height', '194px');
    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (newval, oldval) {
                /* jshint unused: vars */
                renderRemarkEditor(scope, element, attrs);
            });
        }
    };
});