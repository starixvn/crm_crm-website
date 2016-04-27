/* global RegisterTemplate */
(function () {
    "use strict";
    function registerPickerForDeletedFeedbackStatistic() {
        var deletedFeedbackStatisticRegister = new RegisterTemplate(angular.module('deleted-feedback-statistics-application'));

        deletedFeedbackStatisticRegister.registerDatePicker();
        deletedFeedbackStatisticRegister.registerDownloadFile();
    }

    registerPickerForDeletedFeedbackStatistic();
})();