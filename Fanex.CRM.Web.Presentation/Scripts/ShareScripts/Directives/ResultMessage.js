"use strict";
/* global crmConstants */
angular.module('$resultMessageApplication', [])
    .service('$resultMessageService', ['$rootScope', function ($rootScope) {
        this.resultMessageOptions = {
            messageType: crmConstants.Enums.MessageType.Success,
            message: ''
        };

        this.showMessage = function (resultMessageOptions) {
            this.resultMessageOptions = resultMessageOptions;
            $rootScope.$broadcast('resultMessage-showMessage');
        };

        this.closeMessage = function () {
            $rootScope.$broadcast('resultMessage-closeMessage');
        };
    } ])
    .directive('resultMessageContainer', [
        '$resultMessageService',
        '$timeout',
        function ($resultMessageService, $timeout) {
            function showMessage(scope, element, attrs) {
                var messageType = $resultMessageService.resultMessageOptions.messageType;
                var message = $resultMessageService.resultMessageOptions.message;
                element.removeClass("error success warning")
                    .empty()
                    .text(message)
                    .removeClass("display-none");
                var bodyWith = $(window).width();
                var popUpWidth = element.width();
                var leftPosition = Math.round((bodyWith - popUpWidth) / 2);
                element.css("left", leftPosition);

                switch (messageType) {
                    case crmConstants.Enums.MessageType.Success:
                        element.addClass("success");
                        $timeout(function () {
                            closeMessage(scope, element, attrs);
                        }, 3000);
                        break;
                    case crmConstants.Enums.MessageType.Error:
                        var buttonClose = '<a id="button-closemessage" title="Close message"></a>';
                        element.addClass("error")
                            .append(buttonClose);

                        element.find("#button-closemessage").click(function () {
                            closeMessage(scope, element, attrs);
                        });
                        break;
                    case crmConstants.Enums.MessageType.Warning:
                        element.addClass("warning");
                        break;
                }
            }

            function closeMessage(scope, element, attrs) {
                element.addClass("display-none");
            }

            return {
                restrict: "C",
                scope: true,
                link: function (scope, element, attrs) {
                    scope.$on('resultMessage-showMessage', function () {
                        showMessage(scope, element, attrs);
                    });

                    scope.$on('resultMessage-closeMessage', function () {
                        closeMessage(scope, element, attrs);
                    });
                }
            };
        } ]);