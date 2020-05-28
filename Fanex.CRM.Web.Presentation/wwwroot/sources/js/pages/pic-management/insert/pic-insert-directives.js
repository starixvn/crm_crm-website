(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function picInsertRegisterPicker() {
        var picInsertRegister = new RegisterTemplate(angular.module('pic-management-application'));
        picInsertRegister.registerCommunicationTool = function () {
            var communicationToolRegisterParams = {
                PlugInName: 'communicationToolPickerContainer',
                OptionName: 'communicationToolOptions',
                PickerId: 'communication-tool',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Id",
                    Type: "SettingType",
                    Value: "Name"
                }
            };

            directiveManager.registerSingleSelectDirectiveContainer(angular.module('pic-management-application'), communicationToolRegisterParams);
        }

        picInsertRegister.registerRoleSelect = function () {
            var communicationToolRegisterParams = {
                PlugInName: 'roleInsertPickerContainer',
                OptionName: 'roleOptions',
                PickerId: 'role',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Value",
                    Type: "SettingType",
                    Value: "Value"
                }
            };

            directiveManager.registerSingleSelectDirectiveContainer(angular.module('pic-management-application'), communicationToolRegisterParams);
        }

        picInsertRegister.registerCountrySelect = function () {
            var countryRegisterParams = {
                PlugInName: 'countryPickerContainer',
                OptionName: 'countryOptions',
                PickerId: 'country',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Name",
                    Type: "SettingType",
                    Value: "Name"
                }
            };

            directiveManager.registerSingleSelectDirectiveContainer(angular.module('pic-management-application'), countryRegisterParams);
        }

        picInsertRegister.registerCommunicationTool();
        picInsertRegister.registerRoleSelect();
        picInsertRegister.registerCountrySelect();
    }

    picInsertRegisterPicker();
})();