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

    registerReportTypePicker();
    registerDatePicker();
    registerWeekPicker();
    registerMonthPicker();
    registerYearPicker();
    registerProductPicker();
    registerSitePicker();
    registerStatusPicker();
    registerResponsePicker();
}

dynamicReportRegisterPicker();