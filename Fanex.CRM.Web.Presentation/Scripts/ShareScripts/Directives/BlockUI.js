"use strict";
/* global hotKeyManager */
angular.module('$uiBlockerApplication', [])
    .service('$uiBlockerService', ['$rootScope', function ($rootScope) {
        this.blockUI = function () {
            if (hotKeyManager) {
                hotKeyManager.CachedData.isBlockingUI = true;
            }

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

            if (hotKeyManager) {
                hotKeyManager.CachedData.isBlockingUI = false;
            }
        };
    } ]);