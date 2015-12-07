(function () {
    function registerPickerForCasinoDailyEnhancement() {
        var casinoDailyEnhancementRegister = new RegisterTemplate(angular.module('report-casinodaily-enhancement-application'));

        casinoDailyEnhancementRegister.registerDatePicker();
        casinoDailyEnhancementRegister.registerDownloadFile();
        casinoDailyEnhancementRegister.registerProductMultiSelectPicker();
        casinoDailyEnhancementRegister.registerCommunicationToolPicker(true);
        casinoDailyEnhancementRegister.registerCategoryMultiSelectPicker();
    }

    registerPickerForCasinoDailyEnhancement();
})();