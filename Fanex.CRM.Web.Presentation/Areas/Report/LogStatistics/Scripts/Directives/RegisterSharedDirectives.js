/* global RegisterTemplate */
(function () {
    "use strict";
    function registerPickerForLogStatistic() {
        var logStatisticRegister = new RegisterTemplate(angular.module('log-statistics-application'));

        logStatisticRegister.registerDatePicker();
        logStatisticRegister.registerDownloadFile();
        logStatisticRegister.registerProductMultiSelectPicker();
        logStatisticRegister.registerCommunicationToolPicker(true);
        logStatisticRegister.registerCategoryMultiSelectPicker();
    }

    registerPickerForLogStatistic();
})();