(function (angular) {
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

        directiveManager.registerDropdownContainer(angular.module('createMultipleLogsModule'), productRegisterOptions);
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

        directiveManager.registerDropdownContainer(angular.module('createMultipleLogsModule'), subProductRegisterOptions);
    }

    registerDirectives();
})(angular);