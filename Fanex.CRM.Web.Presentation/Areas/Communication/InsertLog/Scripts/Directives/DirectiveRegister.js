(function () {
    function registerDirectives() {
        addParentProductDirective();
    }

    function addParentProductDirective() {
        var parentProductRegisterOptions = {
            PlugInName: 'dropdownParentProduct',
            OptionName: 'parentProductOptions',
            PickerId: 'parentProduct',
            RestrictType: 'A',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(angular.module('communicationlog-insert-application'), parentProductRegisterOptions);
    }

    registerDirectives();
})();