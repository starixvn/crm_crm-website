"use strict";
/* global dynamicReportApplication, 
directiveManager */

function dynamicReportRegisterPicker() {
    function registerReportTypePicker() {
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
        directiveManager.registerDropdownContainer(dynamicReportApplication, reportTypeRegisterParams);
    }
    function registerDatePicker() {
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
        directiveManager.registerDatePickerContainer(dynamicReportApplication, fromDateRegisterParams);
        directiveManager.registerDatePickerContainer(dynamicReportApplication, toDateRegisterParams);
    }
    function registerWeekPicker() {
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
        directiveManager.registerWeekPickerContainer(dynamicReportApplication, fromWeekRegisterParams);
        directiveManager.registerWeekPickerContainer(dynamicReportApplication, toWeekRegisterParams);
    }
    function registerMonthPicker() {
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

        directiveManager.registerMonthPickerContainer(dynamicReportApplication, fromMonthRegisterParams);
        directiveManager.registerMonthPickerContainer(dynamicReportApplication, toMonthRegisterParams);
    }
    function registerYearPicker() {
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

        directiveManager.registerYearPickerContainer(dynamicReportApplication, fromYearRegisterParams);
        directiveManager.registerYearPickerContainer(dynamicReportApplication, toYearRegisterParams);
    }
    function registerProductPicker() {
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

        directiveManager.registerDropdownContainer(dynamicReportApplication, productRegisterParams);
    }
    function registerSitePicker() {
        var siteRegisterParams = {
            PlugInName: 'sitePickerContainer',
            OptionName: 'siteOptions',
            PickerId: 'site',
            RestrictType: 'C',
        };

        directiveManager.registerDropdownSiteContainer(dynamicReportApplication, siteRegisterParams);
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

        directiveManager.registerDropdownContainer(dynamicReportApplication, statusRegisterParams);
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

        directiveManager.registerDropdownContainer(dynamicReportApplication, responseRegisterParams);
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

        directiveManager.registerDropdownContainer(dynamicReportApplication, roleRegisterParams);
    }
    function registerCommunicationToolPicker() {
        var communicationToolRegisterParams = {
            PlugInName: 'communicationToolPickerContainer',
            OptionName: 'communicationToolOptions',
            PickerId: 'communication-tool',
            RestrictType: 'C',

            DataPropertyName: {
                Id: "Id",
                Name: "Value"
            }
        };

        directiveManager.registerDropdownContainer(dynamicReportApplication, communicationToolRegisterParams);
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

        directiveManager.registerDropdownContainer(dynamicReportApplication, mainCategoryRegisterParams);
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

        directiveManager.registerDropdownContainer(dynamicReportApplication, subCategoryRegisterParams);
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

        directiveManager.registerDropdownContainer(dynamicReportApplication, extensionRegisterParams);
    }
    function registerDefaultFocus() {
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

        directiveManager.registerDefaultFocus(dynamicReportApplication, defaultFocusRegisterParams);
    }

    registerReportTypePicker();
    registerDatePicker();
    registerWeekPicker();
    registerMonthPicker();
    registerYearPicker();
    registerProductPicker();
    registerSitePicker();
    registerStatusPicker();
    registerResponsePicker();
    registerRolePicker();
    registerCommunicationToolPicker();
    registerMainCategoryPicker();
    registerSubCategoryPicker();
    registerExtensionPicker();
    registerDefaultFocus();
}

dynamicReportRegisterPicker();