"use strict";
/* global directiveManager */
/* exported RegisterTemplate */
function RegisterTemplate(application) {
    var self = this;
    self.application = application;
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

        directiveManager.registerDropdownContainer(self.application, reportTypeRegisterParams);
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
        directiveManager.registerDatePickerContainer(self.application, fromDateRegisterParams);
        directiveManager.registerDatePickerContainer(self.application, toDateRegisterParams);
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

        directiveManager.registerWeekPickerContainer(self.application, fromWeekRegisterParams);
        directiveManager.registerWeekPickerContainer(self.application, toWeekRegisterParams);
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

        directiveManager.registerMonthPickerContainer(self.application, fromMonthRegisterParams);
        directiveManager.registerMonthPickerContainer(self.application, toMonthRegisterParams);
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

        directiveManager.registerYearPickerContainer(self.application, fromYearRegisterParams);
        directiveManager.registerYearPickerContainer(self.application, toYearRegisterParams);
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

        directiveManager.registerDropdownContainer(self.application, productRegisterParams);
    };
    self.registerSitePicker = function () {
        var siteRegisterParams = {
            PlugInName: 'sitePickerContainer',
            OptionName: 'siteOptions',
            PickerId: 'site',
            RestrictType: 'C'
        };

        directiveManager.registerDropdownSiteContainer(self.application, siteRegisterParams);
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

        directiveManager.registerDropdownContainer(self.application, statusRegisterParams);
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

        directiveManager.registerDropdownContainer(self.application, responseRegisterParams);
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

        directiveManager.registerDropdownContainer(self.application, roleRegisterParams);
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

        directiveManager.registerDropdownContainer(self.application, communicationToolRegisterParams);
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

        directiveManager.registerDropdownContainer(self.application, mainCategoryRegisterParams);
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

        directiveManager.registerDropdownContainer(self.application, subCategoryRegisterParams);
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

        directiveManager.registerDropdownContainer(self.application, extensionRegisterParams);
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

        directiveManager.registerDefaultFocus(self.application, defaultFocusRegisterParams);
    };
    self.registerDownloadFile = function () {
        var downloadFileRegisterParams = {
            PlugInName: 'downloadFileContainer',
            OptionName: 'downloadFileOptions',
            RestrictType: 'C'
        };

        directiveManager.registerDownloadFile(self.application, downloadFileRegisterParams);
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

        directiveManager.registerDropdownContainer(self.application, titleRegisterParams);
    };
    self.registerCustomerTypePicker = function () {
        var customerTypeRegisterParams = {
            PlugInName: 'customerTypePickerContainer',
            OptionName: 'customerTypeOptions',
            PickerId: 'customertype',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.application, customerTypeRegisterParams);
    };
    self.registerVIPTypePicker = function () {
        var vipTypeRegisterParams = {
            PlugInName: 'vipTypePickerContainer',
            OptionName: 'vipTypeOptions',
            PickerId: 'viptype',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(self.application, vipTypeRegisterParams);
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

        directiveManager.registerMultiSelectContainer(self.application, languageRegisterParams);
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

        directiveManager.registerMultiSelectContainer(self.application, siteRegisterParams);
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

        directiveManager.registerMultiSelectContainer(self.application, countryRegisterParams);
    };

    self.registerToggleElement = function () {
        var countryRegisterParams = {
            PlugInName: 'toggleElement',
            OptionName: 'toggleElementOptions',
            PickerId: null,
            RestrictType: 'C'
        };

        directiveManager.registerToggleElement(self.application, countryRegisterParams);
    };

    self.registerSubjectAutoComplete = function () {
        var subjectAutoCompleteRegisterParams = {
            PlugInName: 'subjectAutoCompleteContainer',
            OptionName: 'subjectOptions',
            PickerId: "subject",
            RestrictType: 'C'
        };

        directiveManager.registerSubjectAutoComplete(self.application, subjectAutoCompleteRegisterParams);
    };
}



