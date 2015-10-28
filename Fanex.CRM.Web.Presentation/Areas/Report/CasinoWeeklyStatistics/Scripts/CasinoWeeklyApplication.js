/* global RegisterTemplate */
(function () {
    "use strict";
    angular.module('report-casinoweekly-application', ['$uiBlockerApplication']);

    var casinoWeeklyRegister = new RegisterTemplate(angular.module('report-casinoweekly-application'));
    casinoWeeklyRegister.registerDownloadFile();
})();