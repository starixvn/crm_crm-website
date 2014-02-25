"use strict";
/* global dynamicReportApplication, 
directiveManager */

function dynamicReportRegisterPicker() {
    function registerReportTypePicker() {
        var reportTypeRegisterParams = {
            PlugInName: 'reportTypeContainer',
            OptionName: 'reportTypeOptions',
            TargetPlugInId: 'reporttype',
            RestrictType: 'C'
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

    registerReportTypePicker();
    registerDatePicker();
    registerWeekPicker();
    registerMonthPicker();
    registerYearPicker();
}

dynamicReportRegisterPicker();