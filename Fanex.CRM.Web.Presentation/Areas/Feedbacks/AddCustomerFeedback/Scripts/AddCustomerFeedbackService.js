(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('feedbacks-addcustomerfeedback-application').factory('addCustomerFeedbackService',
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

                serviceInstance.checkFeedbackDuplication = function (params) {
                    var url = window.rootUrl + "Feedbacks/CheckFeedbackDuplication";
                    return sharedScriptManager.registerAngularService($http, $q, url, params);
                };

                serviceInstance.recheckVerifyUserName = function (params) {
                    var url = window.rootUrl + "CustomerSetting/VerifyUsername";
                    return sharedScriptManager.registerAngularService($http, $q, url, params);
                };

                return serviceInstance;
            }]);
})();