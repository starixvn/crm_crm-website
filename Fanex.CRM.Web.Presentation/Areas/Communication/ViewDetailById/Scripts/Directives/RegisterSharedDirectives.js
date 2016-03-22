(function () {
    "use strict";
    /* global directiveManager, RegisterTemplate, crmConstants */

    function viewDetailByIdApplicationRegisterPicker() {
        function registerProductPicker() {
            var productRegisterParams = {
                PlugInName: 'productPickerContainer',
                OptionName: 'productOptions',
                PickerId: 'subProduct',
                RestrictType: 'A',

                DataPropertyName: {
                    Id: "Id",
                    Name: "Value"
                }
            };

            directiveManager.registerDropdownContainer(angular.module('communicationlog-viewdetailbyid-application'), productRegisterParams);
        }
        function registerSitePicker() {
            var siteRegisterParams = {
                PlugInName: 'sitePickerContainer',
                OptionName: 'siteOptions',
                PickerId: 'site',
                RestrictType: 'C',
            };

            directiveManager.registerDropdownSiteContainer(angular.module('communicationlog-viewdetailbyid-application'), siteRegisterParams);
        }
        function registerStatusPicker() {
            var statusRegisterParams = {
                PlugInName: 'statusPickerContainer',
                OptionName: 'statusOptions',
                PickerId: 'status',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Id",
                    Name: "Name"
                }
            };

            directiveManager.registerDropdownContainer(angular.module('communicationlog-viewdetailbyid-application'), statusRegisterParams);
        }
        function registerResponsePicker() {
            var responseRegisterParams = {
                PlugInName: 'responsePickerContainer',
                OptionName: 'responseOptions',
                PickerId: 'response',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Id",
                    Name: "Value"
                }
            };

            directiveManager.registerDropdownContainer(angular.module('communicationlog-viewdetailbyid-application'), responseRegisterParams);
        }
        function registerRolePicker() {
            var roleRegisterParams = {
                PlugInName: 'rolePickerContainer',
                OptionName: 'roleOptions',
                PickerId: 'role',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Id",
                    Name: "Value"
                }
            };

            directiveManager.registerDropdownContainer(angular.module('communicationlog-viewdetailbyid-application'), roleRegisterParams);
        }
        function registerCommunicationToolPicker() {
            var communicationToolRegisterParams = {
                PlugInName: 'communicationToolPickerContainer',
                OptionName: 'communicationToolOptions',
                PickerId: 'communication-tool',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Id",
                    Name: "Name"
                }
            };

            directiveManager.registerDropdownContainer(angular.module('communicationlog-viewdetailbyid-application'), communicationToolRegisterParams);
        }
        function registerMainCategoryPicker() {
            var mainCategoryRegisterParams = {
                PlugInName: 'mainCategoryPickerContainer',
                OptionName: 'mainCategoryOptions',
                PickerId: 'main-category',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Id",
                    Name: "Name"
                }
            };

            directiveManager.registerDropdownContainer(angular.module('communicationlog-viewdetailbyid-application'), mainCategoryRegisterParams);
        }
        function registerSubCategoryPicker() {
            var subCategoryRegisterParams = {
                PlugInName: 'subCategoryPickerContainer',
                OptionName: 'subCategoryOptions',
                PickerId: 'sub-category',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Id",
                    Name: "Name"
                }
            };

            directiveManager.registerDropdownContainer(angular.module('communicationlog-viewdetailbyid-application'), subCategoryRegisterParams);
        }
        function registerExtensionPicker() {
            var extensionRegisterParams = {
                PlugInName: 'extensionPickerContainer',
                OptionName: 'extensionOptions',
                PickerId: 'extension',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Id",
                    Name: "Value"
                }
            };

            directiveManager.registerDropdownContainer(angular.module('communicationlog-viewdetailbyid-application'), extensionRegisterParams);
        }
        function registerCountryPicker() {
            var countryRegisterParams = {
                PlugInName: 'countryPickerContainer',
                OptionName: 'countryOptions',
                PickerId: 'country',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Id",
                    Name: "Name"
                }
            };

            directiveManager.registerDropdownContainer(angular.module('communicationlog-viewdetailbyid-application'), countryRegisterParams);
        }
        function registerLanguagePicker() {
            var languageRegisterParams = {
                PlugInName: 'languagePickerContainer',
                OptionName: 'languageOptions',
                PickerId: 'language',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Id",
                    Name: "Name"
                }
            };

            directiveManager.registerDropdownContainer(angular.module('communicationlog-viewdetailbyid-application'), languageRegisterParams);
        }
        function registerBetTypePicker() {
            var betTypeRegisterParams = {
                PlugInName: 'betTypePickerContainer',
                OptionName: 'betTypeOptions',
                PickerId: 'bet-type',
                RestrictType: 'C',

                DataPropertyName: {
                    Id: "Id",
                    Name: "Name"
                }
            };

            directiveManager.registerDropdownContainer(angular.module('communicationlog-viewdetailbyid-application'), betTypeRegisterParams);
        }
        function registerRemarkEditorPicker() {
            var remarkRegisterParams = {
                PlugInName: 'remarkEditorContainer',
                OptionName: 'remarkOptions',
                PickerId: 'remarkeditorbox',
                RestrictType: 'C'
            };

            directiveManager.registerHtmlArea(angular.module('communicationlog-viewdetailbyid-application'), remarkRegisterParams);
        }
        function registerSolutionEditorPicker() {
            var solutionRegisterParams = {
                PlugInName: 'solutionEditorContainer',
                OptionName: 'solutionOptions',
                PickerId: 'solutioneditorbox',
                RestrictType: 'C'
            };

            directiveManager.registerHtmlArea(angular.module('communicationlog-viewdetailbyid-application'), solutionRegisterParams);
        }
        function registerReviewEditorPicker() {
            var reviewRegisterParams = {
                PlugInName: 'reviewEditorContainer',
                OptionName: 'reviewOptions',
                PickerId: 'revieweditorbox',
                RestrictType: 'C'
            };

            directiveManager.registerHtmlArea(angular.module('communicationlog-viewdetailbyid-application'), reviewRegisterParams);
        }
        function registerSubjectAutoComplete() {
            var subjectAutoCompleteRegisterParams = {
                PlugInName: 'subjectAutoComplete',
                OptionName: 'subjectOptions',
                PickerId: 'revieweditorbox',
                RestrictType: 'C'
            };

            directiveManager.registerHtmlArea(angular.module('communicationlog-viewdetailbyid-application'), subjectAutoCompleteRegisterParams);
        }
        function registerStartTimePicker() {
            var startTimeRegisterParams = {
                PlugInName: 'timePickerStartTimeContainer',
                OptionName: 'startTimeOptions',
                PickerId: 'startTime',
                RestrictType: 'C',
                PickerType: crmConstants.Enums.CalendarPickerType.FromDate
            };

            directiveManager.registerDateTimePicker(angular.module('communicationlog-viewdetailbyid-application'), startTimeRegisterParams);
        }
        function registerEndTimePicker() {
            var endTimeRegisterParams = {
                PlugInName: 'timePickerEndTimeContainer',
                OptionName: 'endTimeOptions',
                PickerId: 'endTime',
                RestrictType: 'C',
                PickerType: crmConstants.Enums.CalendarPickerType.ToDate
            };

            directiveManager.registerDateTimePicker(angular.module('communicationlog-viewdetailbyid-application'), endTimeRegisterParams);
        }

        registerProductPicker();
        registerSitePicker();
        registerStatusPicker();
        registerResponsePicker();
        registerRolePicker();
        registerCommunicationToolPicker();
        registerMainCategoryPicker();
        registerSubCategoryPicker();
        registerExtensionPicker();
        registerCountryPicker();
        registerLanguagePicker();
        registerBetTypePicker();
        registerRemarkEditorPicker();
        registerSolutionEditorPicker();
        registerReviewEditorPicker();
        registerSubjectAutoComplete();
        registerStartTimePicker();
        registerEndTimePicker();

        var viewDetailByIdRegister = new RegisterTemplate(angular.module('communicationlog-viewdetailbyid-application'));
        viewDetailByIdRegister.registerLicenseeTypePicker();
    }

    viewDetailByIdApplicationRegisterPicker();
})();