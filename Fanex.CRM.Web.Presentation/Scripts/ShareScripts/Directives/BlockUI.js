"use strict";
/* global crmConstants */
angular.module('$uiBlockerApplication', [])
    .service('$uiBlockerService', ['$rootScope', function ($rootScope) {
        this.blockUI = function () {
            try {
                parent.BlockUI();
            } catch (e) {
            }
        };

        this.unblockUI = function () {
            try {
                parent.$.unblockUI();
            } catch (e) {
            }
        };
    } ]);