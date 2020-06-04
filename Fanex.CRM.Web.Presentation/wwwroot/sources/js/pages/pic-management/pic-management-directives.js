(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function picRegisterPicker() {
        var picRegister = new RegisterTemplate(angular.module('pic-management-application'));
        picRegister.registerDefaultFocus = function () {
            var defaultFocusRegisterParams = {
                PlugInName: 'defaultFocus',
                OptionName: 'defaultFocusOptions',
                PickerId: "firstname",
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Id",
                    Name: "Value"
                }
            };

            directiveManager.registerDefaultFocus(angular.module('pic-management-application'), defaultFocusRegisterParams);
        };

        var picRegister = new RegisterTemplate(angular.module('pic-management-application'));
        picRegister.registerCommunicationTool = function () {
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

        picRegister.registerRoleSelect = function () {
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

        picRegister.registerCountrySelect = function () {
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

        picRegister.registerCommunicationTool();
        picRegister.registerRoleSelect();
        picRegister.registerCountrySelect();
        picRegister.registerRolePicker();
        picRegister.registerDefaultFocus();
        picRegister.registerDownloadFile();
    }

    picRegisterPicker();
})();