editLogApplication.directive('reviewEditorContainer', function () {
    var editorId = "revieweditorbox";
    function renderEditor(scope, element, attrs) {
        var editorHTML =
        '<textarea id="' + editorId + '" name="' + editorId + '" class="remarkeditorbox">' +
            communicationLog.Review +
        '</textarea>';

        element.html(editorHTML);
        var editor = element.find("#" + editorId);
        editor.htmlarea({
            changeEvent: function () {
                scope.updateReview(editor.htmlarea("toHtmlString"));
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
                    text: viewResources.Inset_Edit_Block,
                    action: function (btn) {
                        /* jshint unused: vars */
                        var s = this.getSelectedHTML();
                        s = formatEditBlock(accname, s);
                        this.pasteHTML(s);
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
                renderEditor(scope, element, attrs);
            });
        }
    };
});