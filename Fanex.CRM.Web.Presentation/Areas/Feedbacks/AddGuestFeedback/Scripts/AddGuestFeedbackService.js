'use strict';
/* global addGuestFeedbackApplication,
sharedScriptManager */
addGuestFeedbackApplication.factory('addGuestFeedbackService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getAllSetting = function (params) {
            var url = window.rootUrl + "AddGuestFeedback/AllSetting";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.getCustomerInfoById = function (params) {
            var url = window.rootUrl + "CustomerSettingDetail/GetCustomerInfoDetailById";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };
        serviceInstance.getNumberOfFeedbackSubjectTemplate = function () {
            var url = window.rootUrl + "Setting/NumberOfFeedbackSubjectTemplate";
            return sharedScriptManager.registerAngularService($http, $q, url, null);
        };
        serviceInstance.getFeedbackSubjectTemplateByPagingIndex = function (pagingInfo) {
            var url = window.rootUrl + "Setting/GetFeedbackSubjectTemplateByPagingIndex";
            return sharedScriptManager.registerAngularService($http, $q, url, pagingInfo);
        };
        serviceInstance.addFeedback = function (params) {
            var url = window.rootUrl + "AddGuestFeedback/AddGuestFeedback";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
