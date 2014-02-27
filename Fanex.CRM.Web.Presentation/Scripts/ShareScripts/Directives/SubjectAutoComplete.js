"use strict";
/* global directiveManager */
/* jshint unused: vars */
/* jshint camelcase: false */
directiveManager = directiveManager || {};
directiveManager.registerSubjectAutoComplete = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        //        var defaultRegisterParams = {
        //            PlugInName: null,
        //            OptionName: null,
        //            PickerId: null
        //        };

        function isAutoSuggest(keyCode) {
            return keyCode !== keyboardManager.Enums.KeyCode.LeftArrow &&
                keyCode !== keyboardManager.Enums.KeyCode.UpArrow &&
                keyCode !== keyboardManager.Enums.KeyCode.RightArrow &&
                keyCode !== keyboardManager.Enums.KeyCode.DownArrow &&
                keyCode !== keyboardManager.Enums.KeyCode.Enter;
        }

        function bindingAutoSuggestionSubject(scope, element, attrs) {
            var options = scope[registerParams.OptionName];
            if (options === null) {
                return;
            }

            var suggestionItemNumber = options.NumberOfSuggestItems;
            element.keyup(function (e) {
                if (isAutoSuggest(e.keyCode)) {
                    var rows = [];
                    var inputValue = $(this).val();
                    if (inputValue !== "") {
                        rows = jlinq.from(options.SubjectList).starts("Subject", inputValue)
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
                if (options.OnChangeEvent &&
                    typeof (options.OnChangeEvent) === "function") {
                    options.OnChangeEvent(element.val());
                }
            });
        }

        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                bindingAutoSuggestionSubject(scope, element, attrs);
            }
        };
    });
};