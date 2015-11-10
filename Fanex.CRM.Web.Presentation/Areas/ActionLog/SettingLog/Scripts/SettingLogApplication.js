/* global actionLogDirectiveManager */
(function () {
    "use strict";
    angular.module('settinglog-application', []);

    actionLogDirectiveManager.registerFromDateDirective(angular.module('settinglog-application'));
    actionLogDirectiveManager.registerToDateDirective(angular.module('settinglog-application'));
})();