(function () {
    "use strict";
    /* global sharedScriptManager */
    angular.module('changepicstate-application')
    .factory('changePicStateService', ['$http', '$q', function ($http, $q) {
        var serviceInstance = {};
        serviceInstance.changePicState = function (params) {
            var url = window.rootUrl + "PicManagement/ChangePicState";
            return sharedScriptManager.registerAngularService($http, $q, url, params);
        };

        return serviceInstance;
    } ]);
})();
