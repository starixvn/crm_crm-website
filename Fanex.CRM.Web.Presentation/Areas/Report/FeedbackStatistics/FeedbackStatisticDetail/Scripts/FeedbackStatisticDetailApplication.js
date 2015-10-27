/* global RegisterTemplate */
(function () {
    "use strict";
    var fbStatisticDetailApplication = angular.module('feedback-statistic-detail-application', ['$uiBlockerApplication']);

    var fbStatisticRegister = new RegisterTemplate(fbStatisticDetailApplication);
    fbStatisticRegister.registerDownloadFile();
})();