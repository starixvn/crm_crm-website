(function () {
    "use strict";
    function changesLogRegisterPicker() {
        var changesLogRegister = new RegisterTemplate(angular.module('changes-log-application'));
        changesLogRegister.registerDatePicker();

        let fromDateRegisterParams = {
            PlugInName: 'datePickerPicFromDateContainer',
            OptionName: 'picFromDateOptions',
            PickerId: 'picFromDate',
            RestrictType: 'C'
        };

        let toDateRegisterParams = {
            PlugInName: 'datePickerPicToDateContainer',
            OptionName: 'picToDateOptions',
            PickerId: 'picToDate',
            RestrictType: 'C'
        };
        
        directiveManager.registerDatePickerContainer(angular.module('changes-log-application'), fromDateRegisterParams);
        directiveManager.registerDatePickerContainer(angular.module('changes-log-application'), toDateRegisterParams);
    }

    changesLogRegisterPicker();
})();