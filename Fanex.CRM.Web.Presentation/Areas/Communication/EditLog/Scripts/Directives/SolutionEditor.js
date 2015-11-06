(function () {
    "use strict";
    /* global communicationLog,
    viewResources,
    hotKeyManager,
    sharedScriptManager,
    currentUsername */
    /* jshint camelcase: false */
    /* jshint unused: vars */
    angular.module('communicationlog-edit-application').directive('solutionEditorContainer', function () {
        var editorId = "solutioneditorbox";
        function renderEditor(scope, element) {
            var solution = communicationLog.Solution;
            var editorHTML =
        '<textarea id="' + editorId + '" name="' + editorId + '" class="remarkeditorbox">' +
             sharedScriptManager.encodeHtml(solution) +
        '</textarea>';

            element.html(editorHTML);
            var editor = element.find("#" + editorId);
            editor.htmlarea({
                changeEvent: function () {
                    scope.updateSolution(editor.htmlarea("toHtmlString"));
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
                        //jshint unused:false
                        var editText = this.getSelectedHTML();
                        editText = sharedScriptManager.insertEditDescriptionBlock(currentUsername, editText, parent.homeManager.currentTime);
                        this.pasteHTML(editText);
                        scope.updateSolution(editor.htmlarea("toHtmlString"));
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
                    //jshint unused:false
                    renderEditor(scope, element);
                });
            }
        };
    });
})();