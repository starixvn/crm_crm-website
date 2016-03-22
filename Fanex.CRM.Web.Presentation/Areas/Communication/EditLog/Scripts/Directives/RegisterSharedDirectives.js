(function () {
    "use strict";
    /* global RegisterTemplate */
    function editCommunicationLogRegisterPicker() {
        var editCommunicationLogRegister = new RegisterTemplate(angular.module('communicationlog-edit-application'));
        editCommunicationLogRegister.registerLicenseeTypePicker();

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

        directiveManager.registerDropdownContainer(angular.module('communicationlog-edit-application'), parentProductRegisterOptions);
    }

    editCommunicationLogRegisterPicker();
})();