(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('addmintool-feedbacksubjectmanagement-deletetemplate')
    .factory('deleteTemplateService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.deleteTemplate = function (params) {
            var url = window.rootUrl + "FeedbackSubjectManagement/DeleteTemplate";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();
