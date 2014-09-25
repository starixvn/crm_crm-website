'use strict';
/* global insertTemplateApplication,
sharedScriptManager */
insertTemplateApplication.factory('insertTemplateService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.insertTemplate = function (params) {
            var url = window.rootUrl + "FeedbackSubjectManagement/InsertTemplate";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
