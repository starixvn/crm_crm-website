"use strict";
/* global hotKeyManager */
angular.module('$uiBlockerApplication', [])
    .service('$uiBlockerService', ['$rootScope', function ($rootScope) {
        /* jshint unused: false */
        this.blockUI = function () {
            try {
                if (window.hotKeyManager) {
                    hotKeyManager.CachedData.isBlockingUI = true;
                }

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

                if (window.hotKeyManager) {
                    hotKeyManager.CachedData.isBlockingUI = false;
                }
            } catch (e) {
            }
        };
    } ]);