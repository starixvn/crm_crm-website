/* global sharedScriptManager, RegisterTemplate */
(function () {
    "use strict";
    angular.module('customer-informheadline-statistic', ['$uiBlockerApplication', 'alertBoxApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('customer-informheadline-statistic'));

    var informStatisticRegister = new RegisterTemplate(angular.module('customer-informheadline-statistic'));
    informStatisticRegister.registerDefaultFocus();
    informStatisticRegister.registerDownloadFile();
})();