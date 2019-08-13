(function (angular) {
    'use strict';
    /* global sharedScriptManager */

    angular.module('informHeadlineModule').factory('informHeadlineFactory', informHeadlineFactory);
    informHeadlineFactory.$inject = ['$http', '$q'];

    function informHeadlineFactory($http, $q) {
        // common
        function getPositions() {
            return [
                { "Id": 4, "Name": "Super" },
                { "Id": 3, "Name": "Master" },
                { "Id": 2, "Name": "Agent" },
                { "Id": 1, "Name": "Member" },
                { "Id": 0, "Name": "Guest" }
            ];
        }

        function getVips() {
            return [
                { "Id": 2, "Name": "All" },
                { "Id": 1, "Name": "Yes" },
                { "Id": 0, "Name": "No" }
            ];
        }

        function getTitleDetail(params) {
            const url = window.rootUrl + "InformHeadline/GetTitleDetail";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        function getAllSettings() {
            const url = window.rootUrl + 'InformHeadline/AllSettings';
            return sharedScriptManager.registerAngularService($http, $q, url);
        }

        function updateInformStatus(params) {
            const url = window.rootUrl + "InformHeadline/UpdateInformStatus";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        function getInforms(params) {
            const url = window.rootUrl + "InformHeadline/GetInforms";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        // create
        function getNewInforms(params) {
            const url = window.rootUrl + "InformHeadline/GetNewInforms";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        function insertInforms(params) {
            const url = window.rootUrl + 'InformHeadline/InsertInforms';
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        // update
        function getUpdateInforms(params) {
            const url = window.rootUrl + "InformHeadline/GetUpdateInforms";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        function updateInforms(params) {
            const url = window.rootUrl + 'InformHeadline/UpdateInforms';
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        // logs
        function getRemovedAccountLogs(params) {
            const url = window.rootUrl + 'InformHeadline/GetRemovedAccountLogs';
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        function getRemovedAccountLogDetails(params) {
            const url = window.rootUrl + 'InformHeadline/GetRemovedAccountLogDetails';
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        }

        return {
            getPositions: getPositions,
            getVips: getVips,
            getAllSettings: getAllSettings,
            getTitleDetail: getTitleDetail,
            updateInformStatus: updateInformStatus,
            getInforms: getInforms,
            // create
            getNewInforms: getNewInforms,
            insertInforms: insertInforms,
            // update
            getUpdateInforms: getUpdateInforms,
            updateInforms: updateInforms,
            // logs
            getRemovedAccountLogs: getRemovedAccountLogs,
            getRemovedAccountLogDetails: getRemovedAccountLogDetails
        };
    }
})(angular);