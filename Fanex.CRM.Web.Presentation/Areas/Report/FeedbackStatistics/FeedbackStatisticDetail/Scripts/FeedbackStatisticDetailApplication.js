/* global RegisterTemplate */
(function () {
    "use strict";
    angular.module('feedback-statistic-detail-application', ['$uiBlockerApplication']);

    var fbStatisticRegister = new RegisterTemplate(angular.module('feedback-statistic-detail-application'));
    fbStatisticRegister.registerDownloadFile();
})();