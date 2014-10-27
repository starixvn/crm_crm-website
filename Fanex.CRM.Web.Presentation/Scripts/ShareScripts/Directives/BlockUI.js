"use strict";
/* global hotKeyManager */
/* jshint unused: vars */
angular.module('$uiBlockerApplication', [])
    .service('$uiBlockerService', ['$rootScope', function ($rootScope) {
        this.blockUI = function () {
            if (hotKeyManager) {
                hotKeyManager.CachedData.isBlockingUI = true;
            }

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

            if (hotKeyManager) {
                hotKeyManager.CachedData.isBlockingUI = false;
            }
        };
    } ]);