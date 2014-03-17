"use strict";
/* global directiveManager */
/* exported RegisterTemplate */
function RegisterTemplate(aplication) {
    var self = this;
    self.aplication = aplication;
    self.registerReportTypePicker = function () {
        var reportTypeRegisterParams = {
            PlugInName: 'reportTypeContainer',
            OptionName: 'reportTypeOptions',
            PickerId: 'reporttype',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerDropdownContainer(self.aplication, reportTypeRegisterParams);
    };

    self.registerDatePicker = function () {
        var fromDateRegisterParams = {
            PlugInName: 'datePickerFromDateContainer',
            OptionName: 'fromDateOptions',
            PickerId: 'fromDate',
            RestrictType: 'C'
        };
        var toDateRegisterParams = {
            PlugInName: 'datePickerToDateContainer',
            OptionName: 'toDateOptions',
            PickerId: 'toDate',
            RestrictType: 'C'
        };
        directiveManager.registerDatePickerContainer(self.aplication, fromDateRegisterParams);
        directiveManager.registerDatePickerContainer(self.aplication, toDateRegisterParams);
    };
    self.registerWeekPicker = function () {
        var fromWeekRegisterParams = {
            PlugInName: 'weekPickerFromWeekContainer',
            OptionName: 'fromWeekOptions',
            PickerId: 'fromweek',
            RestrictType: 'C'
        };
        var toWeekRegisterParams = {
            PlugInName: 'weekPickerToWeekContainer',
            OptionName: 'toWeekOptions',
            PickerId: 'toweek',
            RestrictType: 'C'
        };
        directiveManager.registerWeekPickerContainer(self.aplication, fromWeekRegisterParams);
        directiveManager.registerWeekPickerContainer(self.aplication, toWeekRegisterParams);
    };
    self.registerMonthPicker = function () {
        var fromMonthRegisterParams = {
            PlugInName: 'monthPickerFromMonthContainer',
            OptionName: 'fromMonthOptions',
            PickerId: {
                Month: 'frommonth_month',
                Year: 'frommonth_year'
            },
            RestrictType: 'C'
        };

        var toMonthRegisterParams = {
            PlugInName: 'monthPickerToMonthContainer',
            OptionName: 'toMonthOptions',
            PickerId: {
                Month: 'tomonth_month',
                Year: 'tomonth_year'
            },
            RestrictType: 'C'
        };

        directiveManager.registerMonthPickerContainer(self.aplication, fromMonthRegisterParams);
        directiveManager.registerMonthPickerContainer(self.aplication, toMonthRegisterParams);
    };
    self.registerYearPicker = function () {
        var fromYearRegisterParams = {
            PlugInName: 'yearPickerFromDateContainer',
            OptionName: 'fromYearOptions',
            PickerId: 'fromdate_year',
            RestrictType: 'C'
        };

        var toYearRegisterParams = {
            PlugInName: 'yearPickerToDateContainer',
            OptionName: 'toYearOptions',
            PickerId: 'todate_year',
            RestrictType: 'C'
        };

        directiveManager.registerYearPickerContainer(self.aplication, fromYearRegisterParams);
        directiveManager.registerYearPickerContainer(self.aplication, toYearRegisterParams);
    };
    self.registerProductPicker = function () {
        var productRegisterParams = {
            PlugInName: 'productPickerContainer',
            OptionName: 'productOptions',
            PickerId: 'product',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.aplication, productRegisterParams);
    };
    self.registerSitePicker = function () {
        var siteRegisterParams = {
            PlugInName: 'sitePickerContainer',
            OptionName: 'siteOptions',
            PickerId: 'site',
            RestrictType: 'C'
        };

        directiveManager.registerDropdownSiteContainer(self.aplication, siteRegisterParams);
    };
    self.registerStatusPicker = function () {
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

        directiveManager.registerDropdownContainer(self.aplication, statusRegisterParams);
    };
    self.registerResponsePicker = function () {
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

        directiveManager.registerDropdownContainer(self.aplication, responseRegisterParams);
    };
    self.registerRolePicker = function () {
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

        directiveManager.registerDropdownContainer(self.aplication, roleRegisterParams);
    };
    self.registerCommunicationToolPicker = function () {
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

        directiveManager.registerDropdownContainer(self.aplication, communicationToolRegisterParams);
    };
    self.registerMainCategoryPicker = function () {
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

        directiveManager.registerDropdownContainer(self.aplication, mainCategoryRegisterParams);
    };
    self.registerSubCategoryPicker = function () {
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

        directiveManager.registerDropdownContainer(self.aplication, subCategoryRegisterParams);
    };
    self.registerExtensionPicker = function () {
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

        directiveManager.registerDropdownContainer(self.aplication, extensionRegisterParams);
    };
    self.registerDefaultFocus = function () {
        var defaultFocusRegisterParams = {
            PlugInName: 'defaultFocus',
            OptionName: 'defaultFocusOptions',
            PickerId: null,
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDefaultFocus(self.aplication, defaultFocusRegisterParams);
    };
    self.registerDownloadFile = function () {
        var downloadFileRegisterParams = {
            PlugInName: 'downloadFileContainer',
            OptionName: 'downloadFileOptions',
            RestrictType: 'C'
        };

        directiveManager.registerDownloadFile(self.aplication, downloadFileRegisterParams);
    };

    self.registerTitlePicker = function () {
        var titleRegisterParams = {
            PlugInName: 'titlePickerContainer',
            OptionName: 'titleOptions',
            PickerId: 'title',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.aplication, titleRegisterParams);
    };
    self.registerCustomerTypePicker = function () {
        var customerTypeRegisterParams = {
            PlugInName: 'customerTypePickerContainer',
            OptionName: 'customerTypeOptions',
            PickerId: 'customerType',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.aplication, customerTypeRegisterParams);
    };
    self.registerVIPTypePicker = function () {
        var vipTypeRegisterParams = {
            PlugInName: 'vipTypePickerContainer',
            OptionName: 'vipTypeOptions',
            PickerId: 'vipType',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.aplication, vipTypeRegisterParams);
    };

    self.registerLanguageMultiSelectPicker = function () {
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

        directiveManager.registerMultiSelectContainer(self.aplication, languageRegisterParams);
    };
    self.registerSiteMultiSelectPicker = function () {
        var siteRegisterParams = {
            PlugInName: 'sitePickerContainer',
            OptionName: 'siteOptions',
            PickerId: 'site',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Name"
            }
        };

        directiveManager.registerMultiSelectContainer(self.aplication, siteRegisterParams);
    };
    self.registerCountryMultiSelectPicker = function () {
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

        directiveManager.registerMultiSelectContainer(self.aplication, countryRegisterParams);
    };

    self.registerToggleElement = function () {
        var countryRegisterParams = {
            PlugInName: 'toggleElement',
            OptionName: 'toggleElementOptions',
            PickerId: null,
            RestrictType: 'C'
        };

        directiveManager.registerToggleElement(self.aplication, countryRegisterParams);
    };

    self.registerSubjectAutoComplete = function () {
        var subjectAutoCompleteRegisterParams = {
            PlugInName: 'subjectAutoCompleteContainer',
            OptionName: 'subjectOptions',
            PickerId: "subject",
            RestrictType: 'C'
        };

        directiveManager.registerSubjectAutoComplete(self.aplication, subjectAutoCompleteRegisterParams);
    }
}



