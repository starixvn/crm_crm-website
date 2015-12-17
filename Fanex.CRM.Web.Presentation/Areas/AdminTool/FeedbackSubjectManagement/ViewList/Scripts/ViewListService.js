(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('addmintool-feedbacksubjectmanagement-application').factory('feedbackSubjectManagementService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getNumberOfFeedbackSubjectTemplate = function () {
            var url = window.rootUrl + "Setting/NumberOfFeedbackSubjectTemplate";
            return sharedScriptManager.registerAngularService($http, $q, url, null);
        };
        serviceInstance.getFeedbackSubjectTemplateByPagingIndex = function (pagingInfo) {
            var url = window.rootUrl + "Setting/GetFeedbackSubjectTemplateByPagingIndex";
            return sharedScriptManager.registerAngularService($http, $q, url, pagingInfo);
        };
        serviceInstance.searchTemplate = function (params) {
            var url = window.rootUrl + "FeedbackSubjectManagement/SearchFeedbackSubjectTemplate";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();
