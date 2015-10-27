/* global RegisterTemplate */
(function () {
    "use strict";
    var casinoWeeklyApplication = angular.module('report-casinoweekly-application', ['$uiBlockerApplication']);

    var casinoWeeklyRegister = new RegisterTemplate(casinoWeeklyApplication);
    casinoWeeklyRegister.registerDownloadFile();
})();