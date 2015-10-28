(function () {
    "use strict";
    /* global RegisterTemplate */
    angular.module('feedback-statistic-application', ['$uiBlockerApplication', 'alertBoxApplication']);

    var fbStatisticRegister = new RegisterTemplate(angular.module('feedback-statistic-application'));
    fbStatisticRegister.registerDatePicker();
    fbStatisticRegister.registerDownloadFile();
})();