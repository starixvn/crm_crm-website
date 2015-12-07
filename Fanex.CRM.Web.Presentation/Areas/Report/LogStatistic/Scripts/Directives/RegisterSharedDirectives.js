(function () {
    function registerPickerForLogStatistic() {
        var logStatisticRegister = new RegisterTemplate(angular.module('log-statistic-application'));

        logStatisticRegister.registerDatePicker();
        logStatisticRegister.registerDownloadFile();
        logStatisticRegister.registerProductMultiSelectPicker();
        logStatisticRegister.registerCommunicationToolPicker(true);
        logStatisticRegister.registerCategoryMultiSelectPicker();
    }

    registerPickerForLogStatistic();
})();