/* global dynamicReportApplication, directiveManager */

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

directiveManager.registerDatePickerContainer(dynamicReportApplication, fromDateRegisterParams);
directiveManager.registerDatePickerContainer(dynamicReportApplication, toDateRegisterParams);
directiveManager.registerDropdownContainer(dynamicReportApplication, periodTypeRegisterParams);
//directiveManager.registerDatePicker(dynamicReportApplication);