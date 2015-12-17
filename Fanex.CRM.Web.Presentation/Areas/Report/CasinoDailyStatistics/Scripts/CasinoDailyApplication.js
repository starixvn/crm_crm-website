/* global RegisterTemplate */
(function () {
    "use strict";
    angular.module('report-casinodaily-application', ['$uiBlockerApplication']);

    var casinoDailyRegister = new RegisterTemplate(angular.module('report-casinodaily-application'));
    casinoDailyRegister.registerDatePicker();
    casinoDailyRegister.registerDownloadFile();
})();