(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('addmintool-feedbacksubjectmanagement-deletetemplate')
    .factory('deleteTemplateService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.changeStateTemplate = function (params) {
            var url = window.rootUrl + "FeedbackSubjectManagement/ChangeStateTemplate";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();
