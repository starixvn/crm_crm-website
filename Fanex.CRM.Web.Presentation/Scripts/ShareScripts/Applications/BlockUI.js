"use strict";
/* global crmConstants */
angular.module('$uiBlockerApplication', [])
    .service('$uiBlockerService', ['$rootScope', function ($rootScope) {
        this.blockUI = function () {
            try {
                if (parent && parent.BlockUI) {
                    parent.BlockUI();
                }
            } catch (e) {
            }
        };

        this.unblockUI = function () {
            try {
                if (parent && parent.$.unblockUI) {
                    parent.$.unblockUI();
                }
            } catch (e) {
            }
        };
    } ]);