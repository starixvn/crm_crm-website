/* global dynamicReportApplication, 
directiveManager,
crmConstants */

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
var periodTypeRegisterParams = {
    PlugInName: 'periodTypeContainer',
    OptionName: 'periodTypeOptions',
    TargetPlugInId: 'periodtype',
    RestrictType: 'C'
};
var fromWeekRegisterParams = {
    PlugInName: 'weekPickerFromWeekContainer',
    OptionName: 'fromWeekOptions',
    PickerId: 'fromweek',
    RestrictType: 'C',
    PickerType: crmConstants.Enums.CalendarPickerType.FromDate
};
var toWeekRegisterParams = {
    PlugInName: 'weekPickerToWeekContainer',
    OptionName: 'toWeekOptions',
    PickerId: 'toweek',
    RestrictType: 'C',
    PickerType: crmConstants.Enums.CalendarPickerType.ToDate
};

var fromMonthRegisterParams = {
    PlugInName: 'monthPickerFromMonthContainer',
    OptionName: 'fromMonthOptions',
    PickerId: {
        Month: 'frommonth_month',
        Year: 'frommonth_year'
    },
    RestrictType: 'C',
    PickerType: crmConstants.Enums.CalendarPickerType.FromDate
};
var toMonthRegisterParams = {
    PlugInName: 'monthPickerToMonthContainer',
    OptionName: 'toMonthOptions',
    PickerId: {
        Month: 'tomonth_month',
        Year: 'tomonth_year'
    },
    RestrictType: 'C',
    PickerType: crmConstants.Enums.CalendarPickerType.ToDate
};

directiveManager.registerDatePickerContainer(dynamicReportApplication, fromDateRegisterParams);
directiveManager.registerDatePickerContainer(dynamicReportApplication, toDateRegisterParams);
directiveManager.registerDropdownContainer(dynamicReportApplication, periodTypeRegisterParams);
directiveManager.registerWeekPickerContainer(dynamicReportApplication, fromWeekRegisterParams);
directiveManager.registerWeekPickerContainer(dynamicReportApplication, toWeekRegisterParams);
directiveManager.registerMonthPickerContainer(dynamicReportApplication, fromMonthRegisterParams);
directiveManager.registerMonthPickerContainer(dynamicReportApplication, toMonthRegisterParams);
//directiveManager.registerDatePicker(dynamicReportApplication);