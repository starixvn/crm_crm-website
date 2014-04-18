"use strict";
/* global directiveManager: true,
sharedScriptManager */
/* jshint unused: vars */
var directiveManager = directiveManager || {};
directiveManager.registerDownloadFile = function (angularApplication, registerParams) {
    angularApplication.directive(registerParams.PlugInName, function () {
        function downloadFile(scope, element, attrs, newVal, oldVal) {
            var options = scope[registerParams.OptionName];
            if (options === undefined ||
                options === null) {
                return;
            }

            if (options.URL && options.Params) {
                // options.Params can be string of parameters or array/object
                var data = typeof options.Params === 'string' ?
                    options.Params :
                    $.param(options.Params);
                var downloadUrl = options.URL + "?" + data;
                $.fileDownload(downloadUrl).done(function () { });
            }
        }

        return {
            restrict: registerParams.RestrictType,
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (newVal, oldVal) {
                    downloadFile(scope, element, attrs, newVal, oldVal);
                });
            }
        };
    })
};