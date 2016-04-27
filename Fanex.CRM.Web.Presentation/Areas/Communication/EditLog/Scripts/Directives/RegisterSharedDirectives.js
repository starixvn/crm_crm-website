(function () {
    "use strict";
    /* global RegisterTemplate */
    function editCommunicationLogRegisterPicker() {
        var editCommunicationLogRegister = new RegisterTemplate(angular.module('communicationlog-edit-application'));
        editCommunicationLogRegister.registerLicenseeTypePicker();

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

        directiveManager.registerDropdownContainer(angular.module('communicationlog-edit-application'), productRegisterOptions);
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

        directiveManager.registerDropdownContainer(angular.module('communicationlog-edit-application'), subProductRegisterOptions);
    }

    editCommunicationLogRegisterPicker();
})();