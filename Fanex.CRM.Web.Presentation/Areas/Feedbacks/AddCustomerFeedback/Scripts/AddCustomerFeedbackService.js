'use strict';
/* global addCustomerFeedbackApplication,
sharedScriptManager */
addCustomerFeedbackApplication.factory('addCustomerFeedbackService',
    ['$http', '$q',
    function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.getAllSetting = function (params) {
            var url = window.rootUrl + "AddCustomerFeedback/AllSettingToAddCustomerFeedback";
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
            var url = window.rootUrl + "AddCustomerFeedback/AddCustomerFeedback";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
