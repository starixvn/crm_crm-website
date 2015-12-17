"use strict";
/* global hotKeyManager */
angular.module('$uiBlockerApplication', [])
    .service('$uiBlockerService', ['$rootScope', function ($rootScope) {
        /* jshint unused: false */
        this.blockUI = function () {
            if (window.hotKeyManager) {
                hotKeyManager.CachedData.isBlockingUI = true;
            }

            if (parent && parent.BlockUI) {
                parent.BlockUI();
            }
        };

        this.unblockUI = function () {
            if (parent && parent.$.unblockUI) {
                parent.$.unblockUI();
            }

            if (window.hotKeyManager) {
                hotKeyManager.CachedData.isBlockingUI = false;
            }
        };
    } ]);