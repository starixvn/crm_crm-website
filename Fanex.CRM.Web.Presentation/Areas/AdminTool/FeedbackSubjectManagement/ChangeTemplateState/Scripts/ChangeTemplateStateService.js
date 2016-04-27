(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('addmintool-feedbacksubjectmanagement-changeTemplateState')
    .factory('changeTemplateStateService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.changeTemplateState = function (params) {
            var url = window.rootUrl + "FeedbackSubjectManagement/ChangeTemplateState";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();
