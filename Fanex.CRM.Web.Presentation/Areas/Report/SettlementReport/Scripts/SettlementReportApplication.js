/* global sharedScriptManager */
(function () {
    "use strict";
    angular.module('report-settlementreport-application', ['$uiBlockerApplication', '$resultMessageApplication']);
    sharedScriptManager.setNoCachingHttpRequest(angular.module('report-settlementreport-application'));
    sharedScriptManager.setCommonHeaderHttpRequest(angular.module('report-settlementreport-application'));
})();