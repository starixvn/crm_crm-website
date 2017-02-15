(function () {
    "use strict";

    function registerDirectives() {
        registerProductDirective();
        registerSubProductDirective();
    }

    function registerProductDirective() {
        var productRegisterOptions = {
            PlugInName: 'dropdownProduct',
            OptionName: 'productOptions',
            PickerId: 'product',
            RestrictType: 'A',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(angular.module('edit-multiple-logs-application'), productRegisterOptions);
    }

    function registerSubProductDirective() {
        var subProductRegisterOptions = {
            PlugInName: 'dropdownSubProduct',
            OptionName: 'subProductOptions',
            PickerId: 'subProduct',
            RestrictType: 'A',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(angular.module('edit-multiple-logs-application'), subProductRegisterOptions);
    }

    registerDirectives();
})();