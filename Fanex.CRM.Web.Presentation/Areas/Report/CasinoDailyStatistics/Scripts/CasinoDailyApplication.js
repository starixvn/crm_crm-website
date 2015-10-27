/* global RegisterTemplate */
(function () {
    "use strict";
    var casinoDailyApplication = angular.module('report-casinodaily-application', ['$uiBlockerApplication']);

    var casinoDailyRegister = new RegisterTemplate(casinoDailyApplication);
    casinoDailyRegister.registerDatePicker();
    casinoDailyRegister.registerDownloadFile();
})();