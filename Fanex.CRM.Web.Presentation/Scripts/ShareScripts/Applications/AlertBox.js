'use strict';
/* jshint unused: vars */
/* global crmConstants */
angular.module('alertBoxApplication', [])
    .service('alertBoxService', ['$rootScope', function ($rootScope) {
        this.alertResult = {
            type: crmConstants.Enums.MessageType.Success,
            message: ''
        };

        this.show = function (alertResult) {
            this.alertResult = alertResult;
            $rootScope.$broadcast('alertResult-show');
        };

        this.hide = function () {
            this.alertResult = {
                type: crmConstants.Enums.MessageType.Success,
                message: ''
            };
            $rootScope.$broadcast('alertResult-hide');
        };
    } ])
    .directive('alertBoxContainer', [
        'alertBoxService',
        '$timeout',
        function (alertBoxService, $timeout) {
            function show(element) {
                var type = alertBoxService.alertResult.type;
                var message = alertBoxService.alertResult.message;

                var typeClass = (type === crmConstants.Enums.MessageType.Success) ? 'alertboxscc' : 'alertboxerr';
                var alertElement = element.find('#alertBox');
                alertElement.removeClass()
                    .addClass(typeClass)
                    .empty()
                    .html('<div>' + message + '</div>');
                element.removeClass('display-none');

                if (type === crmConstants.Enums.MessageType.Success) {
                    $timeout(function () { hide(element); }, 3000);
                }
            }

            function hide(element) {
                element.addClass('display-none');
            }

            return {
                restrict: 'A',
                scope: true,
                template: '<div id="alertBox"></div>',
                link: function (scope, element, attrs) {
                    scope.$on('alertResult-show', function () {
                        show(element);
                    });

                    scope.$on('alertResult-hide', function () {
                        hide(element);
                    });
                }
            };
        } ]);