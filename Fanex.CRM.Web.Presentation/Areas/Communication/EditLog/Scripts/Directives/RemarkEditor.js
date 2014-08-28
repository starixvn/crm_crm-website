"use strict";
/* global editLogApplication, 
communicationLog,
viewResources,
hotKeyManager,
sharedScriptManager,
currentUsername */
/* jshint unused: vars */
/* jshint camelcase: false */
editLogApplication.directive('remarkEditorContainer', function () {
    var editorId = "remarkeditorbox";

    function renderRemarkEditor(scope, element, attrs) {
        var remark = communicationLog.Remark.indexOf(" ", communicationLog.Remark.length - 1) !== -1 ?
            communicationLog.Remark.substring(0, communicationLog.Remark.length - 1) + "&nbsp;" :
            communicationLog.Remark;

        var editorHTML =
            '<textarea id="' + editorId + '" name="' + editorId + '" class="remarkeditorbox">' +
                sharedScriptManager.encodeHtml(remark) +
            '</textarea>';

        element.html(editorHTML);
        var editor = element.find("#" + editorId);
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
            blurEvent: function () {
                return hotKeyManager.documentBlur();
            },
            clickEvent: function () {
                $("#startTime").datepicker("hide");
                $("#endTime").datepicker("hide");
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
                    text: viewResources.Insert_Edit_Block,
                    action: function (btn) {
                        var editText = this.getSelectedHTML();
                        editText = sharedScriptManager.insertEditDescriptionBlock(currentUsername, editText, parent.homeManager.currentTime);
                        this.pasteHTML(editText);
                        scope.updateRemark(editor.htmlarea("toHtmlString"));
                    }
                }]
           ]
        });

    }

    return {
        restrict: 'A',
        scope: true,
        link: function (scope, element, attrs) {
            scope.$watch(attrs.ngModel, function (newval, oldval) {
                renderRemarkEditor(scope, element, attrs);
            });
        }
    };
});