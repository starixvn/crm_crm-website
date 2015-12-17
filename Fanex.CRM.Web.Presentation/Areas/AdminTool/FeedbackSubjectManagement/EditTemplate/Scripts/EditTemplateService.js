(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('addmintool-feedbacksubjectmanagement-edittemplate').factory('editTemplateService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.editTemplate = function (params) {
            var url = window.rootUrl + "FeedbackSubjectManagement/EditTemplate";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.loadTemplate = function (params) {
            var url = window.rootUrl + "FeedbackSubjectManagement/FeedbackSubjectTemplateById";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();
