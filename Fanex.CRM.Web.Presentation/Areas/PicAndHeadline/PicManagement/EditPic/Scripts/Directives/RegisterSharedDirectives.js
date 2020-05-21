(function () {
    "use strict";
    /* global RegisterTemplate,
    directiveManager */
    function customerReportRegisterPicker() {
        var customerReportRegister = new RegisterTemplate(angular.module('editpic-application'));
        customerReportRegister.registerDefaultFocus = function () {
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

            directiveManager.registerDefaultFocus(angular.module('editpic-application'), defaultFocusRegisterParams);
        };
        customerReportRegister.registerCommunicationTool = function () {
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

            directiveManager.registerSingleSelectDirectiveContainer(angular.module('editpic-application'), communicationToolRegisterParams);
        }
        customerReportRegister.registerRoleSelect = function () {
            var communicationToolRegisterParams = {
                PlugInName: 'rolePickerContainer',
                OptionName: 'roleOptions',
                PickerId: 'role',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Value",
                    Type: "SettingType",
                    Value: "Value"
                }
            };

            directiveManager.registerSingleSelectDirectiveContainer(angular.module('editpic-application'), communicationToolRegisterParams);
        }
        
        customerReportRegister.registerLanguageSelect = function () {
            var languageRegisterParams = {
                PlugInName: 'languagePickerContainer',
                OptionName: 'languageOptions',
                PickerId: 'language',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Name",
                    Type: "SettingType",
                    Value: "Name"
                }
            };

            directiveManager.registerSingleSelectDirectiveContainer(angular.module('editpic-application'), languageRegisterParams);
        }
        
        customerReportRegister.registerCountrySelect = function () {
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

            directiveManager.registerSingleSelectDirectiveContainer(angular.module('editpic-application'), countryRegisterParams);
        }
        
        customerReportRegister.registerDefaultFocus();
        customerReportRegister.registerCommunicationTool();
        customerReportRegister.registerRoleSelect();
        customerReportRegister.registerLanguageSelect();
        customerReportRegister.registerCountrySelect();
    }

    customerReportRegisterPicker();
})();