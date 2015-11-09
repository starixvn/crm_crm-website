(function () {
    "use strict";
    /* global jlinq,
    keyboardManager */
    angular.module('communicationlog-edit-application').directive('subjectAutoComplete', function () {
        var suggestionItemNumber = 15;
        function bindingAutoSuggestionSubject(scope, element) {
            element.keyup(function (e) {
                if (isAutoSuggest(e.keyCode)) {
                    var rows = [];
                    var inputValue = $(this).val();
                    if (inputValue !== "") {
                        rows = jlinq.from(scope.feedbackSubjectTemplateList).starts("Subject", inputValue)
                        .sort("Subject")
                        .select();
                        rows = jlinq.from(rows).skip(0).take(suggestionItemNumber);
                        if (rows.length > 0) {
                            $(this).autoComplete(rows, "Subject");
                        }
                        else {
                            $(this).removeAutoComplete();
                        }
                    }
                    else {
                        $(this).removeAutoComplete();
                    }
                }
            });

            element.change(function () {
                scope.updateSelectedSubject(element.val());
            });
        }

        function isAutoSuggest(keyCode) {
            return keyCode !== keyboardManager.Enums.KeyCode.LeftArrow &&
            keyCode !== keyboardManager.Enums.KeyCode.UpArrow &&
            keyCode !== keyboardManager.Enums.KeyCode.RightArrow &&
            keyCode !== keyboardManager.Enums.KeyCode.DownArrow &&
            keyCode !== keyboardManager.Enums.KeyCode.Enter;
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element) {
                bindingAutoSuggestionSubject(scope, element);
            }
        };
    });
})();