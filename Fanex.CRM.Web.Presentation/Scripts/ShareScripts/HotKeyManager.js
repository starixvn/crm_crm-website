"use strict";
/* global sharedScriptManager,
keyboardManager */
/* jshint unused: false,
camelcase: false,
maxstatements: false */
/* Hotkey notes:
- Don't invoke hot key so fast 
(including: keep holding Ctrl + Key,
or make next combine in short time after previous combination)
*/
var hotKeyManager;
var HotKeyManager = function () {
    var self = this;

    self.CachedData = {
        KeyPress: [],
        BeforeBlurKeyPress: [],
        ActiveDocumentManager: [],
        isInvokeHotKey: false,
        isSupportedBrowser: false,
        isIgnoreKey: false,
        isBubbleToParent: true,
        isBlockingUI: false
    };

    self.initPage = function () {
        self.CachedData.isSupportedBrowser = $.browser.mozilla || $.browser.msie;
        if (!self.CachedData.isSupportedBrowser) {
            return;
        }

        self.CachedData.KeyPress = [];
        self.updateKeyPressValue(false);
        self.bindEvent();

        if (parent && parent.hotKeyManager) {
            self.CachedData.KeyPress = self.copyArray(parent.hotKeyManager.CachedData.BeforeBlurKeyPress);
            self.CachedData.BeforeBlurKeyPress = self.CachedData.KeyPress;
        }
    };

    self.updateKeyPressValue = function (isUpdateToParent) {
        self.CachedData.BeforeBlurKeyPress = self.copyArray(self.CachedData.KeyPress);
        if (isUpdateToParent && parent && parent.hotKeyManager) {
            parent.hotKeyManager.CachedData.KeyPress = self.copyArray(self.CachedData.KeyPress);
            parent.hotKeyManager.CachedData.BeforeBlurKeyPress = self.copyArray(self.CachedData.BeforeBlurKeyPress);
        }
    };

    self.bindEvent = function () {
        $(document).keydown(function (eventObject) {
            if (self.CachedData.isBlockingUI || self.CachedData.isIgnoreKey || parent.hotKeyManager.CachedData.isIgnoreKey) {              
                self.preventBubbleEvent(eventObject);
                return false;
            }

            self.CachedData.isIgnoreKey = true;
            parent.hotKeyManager.CachedData.isIgnoreKey = true;
            var keyCode = parseInt(eventObject.keyCode);
            self.CachedData.KeyPress[keyCode.toString()] = true;
            self.updateKeyPressValue(true);
            var isCustomHotKey = self.keyDown(eventObject);

            if (!self.CachedData.isInvokeHotKey && self.CachedData.isBubbleToParent) {
                isCustomHotKey = parent.hotKeyManager.invokeHotKey();
                if (parent.hotKeyManager.CachedData.isInvokeHotKey) {
                    self.CachedData.isInvokeHotKey = true;
                    parent.hotKeyManager.delayInvokeHotKey();
                    self.delayInvokeHotKey();
                }
            } else {
                self.delayInvokeHotKey();
            }

            if (isCustomHotKey || self.CachedData.isInvokeHotKey) {
                self.CachedData.isInvokeHotKey = false;
                self.preventBubbleEvent(eventObject);
                self.unblockKeyDownEvent(true);
                return false;
            }

            self.unblockKeyDownEvent(true);
        });

        $(document).keyup(function (eventObject) {
            self.keyUp(eventObject);
        });

        $(document).blur(function () {
            self.documentBlur();
        });

        var ieVersion = sharedScriptManager.getIeVersion();
        if (ieVersion !== -1 && ieVersion >= 10) {
            $(window).blur(function () {
                self.documentBlur();
            });
        }
    };
    self.unblockKeyDownEvent = function (isBubbleParent) {
        self.CachedData.isIgnoreKey = false;
        if (isBubbleParent) {
            parent.hotKeyManager.CachedData.isIgnoreKey = false;
        }
    };
    self.documentKeyDown = function (eventObject) {
        if (self.CachedData.isBlockingUI||
            self.CachedData.isIgnoreKey ||
            parent.hotKeyManager.CachedData.isIgnoreKey) {
            self.preventBubbleEvent(eventObject);
            return false;
        }

        self.CachedData.isIgnoreKey = true;
        var keyCode = parseInt(eventObject.keyCode);
        self.CachedData.KeyPress[keyCode.toString()] = true;
        self.updateKeyPressValue(true);
        var isCustomHotKey = self.keyDown(eventObject);
        if (self.CachedData.isInvokeHotKey) {
            self.delayInvokeHotKey();
        }

        if (isCustomHotKey || self.CachedData.isInvokeHotKey) {
            self.CachedData.isInvokeHotKey = false;
            self.preventBubbleEvent(eventObject);
            self.unblockKeyDownEvent(false);
            return false;
        }

        self.unblockKeyDownEvent(false);
        return true;
    };
    self.documentBlur = function () {
        self.CachedData.BeforeBlurKeyPress = self.copyArray(self.CachedData.KeyPress);
        self.CachedData.KeyPress = [];
    };

    self.keyDown = function (eventObject) {
        if (!self.CachedData.isInvokeHotKey) {
            var isCustomHotKey = false;
            self.CachedData.isInvokeHotKey = false;
            // Use follow line to test working flow
            if (self.CachedData.ActiveDocumentManager.length === 0) {
                return isCustomHotKey;
            }

            isCustomHotKey = self.invokeHotKey();
            return isCustomHotKey;
        }

        return false;
    };
    self.preventBubbleEvent = function (eventObject) {
        if ($.browser.msie) {
            eventObject.returnValue = false;
            window.event.returnValue = false;
            window.event.keyCode = 0;
        }

        if (eventObject.preventDefault && eventObject.stopPropagation) {
            eventObject.stopPropagation();
            eventObject.preventDefault();
        } else {
            eventObject.returnValue = false;
            window.event.returnValue = false;
            window.event.keyCode = 0;
        }
    };
    self.delayInvokeHotKey = function () {
        //        setTimeout(function () {
        //            self.CachedData.isInvokeHotKey = false;
        //        }, 100);
        self.CachedData.isInvokeHotKey = false;
    };
    self.keyUp = function (eventObject) {
        if (self.CachedData.ActiveDocumentManager.length === 0) {
            return;
        }

        var keyCode = parseInt(eventObject.keyCode);
        self.CachedData.KeyPress[keyCode.toString()] = false;
        self.updateKeyPressValue(true);
    };
    self.invokeHotKey = function () {
        var isCustomHotKey = false;
        var numberOfKeyPress = self.getNumberOfKeyPress();
        do {
            switch (numberOfKeyPress) {
                case 2:
                    isCustomHotKey = self.handle2KeyHotKey();
                    break;
                case 3:
                    isCustomHotKey = self.handle3KeyHotKey();
                    break;
                default:
                    isCustomHotKey = false;
                    break;
            }

            numberOfKeyPress--;
        } while (numberOfKeyPress > 1 && !isCustomHotKey);

        return isCustomHotKey;
    };
    self.getNumberOfKeyPress = function () {
        var numberOfKeyPress = 0;
        var keyPressLength = self.CachedData.KeyPress.length;
        for (var i = 0; i < keyPressLength; i++) {
            if (self.CachedData.KeyPress[i]) {
                numberOfKeyPress++;
            }
        }

        return numberOfKeyPress;
    };
    self.handle3KeyHotKey = function () {
        return false;
    };
    self.handle2KeyHotKey = function () {
        var keyCodeEnums = keyboardManager.Enums.KeyCode;
        if (self.CachedData.KeyPress[keyCodeEnums.Ctrl]) {
            return self.handle2KeyHotKey_CtrlBinding();
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Alt]) {
            return self.handle2KeyHotKey_AltBinding();
        }
    };
    self.handle2KeyHotKey_CtrlBinding = function () {
        var keyCodeEnums = keyboardManager.Enums.KeyCode;

        if (self.CachedData.KeyPress[keyCodeEnums.Character_u]) {
            self.invokeHotKey_CtrlU();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_g]) {
            self.invokeHotKey_CtrlG();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_s]) {
            self.invokeHotKey_CtrlS();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_d]) {
            self.invokeHotKey_CtrlD();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_i]) {
            self.invokeHotKey_CtrlI();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_v]) {
            return self.invokeHotKey_CtrlV();
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_e]) {
            return self.invokeHotKey_CtrlE();
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_r]) {
            self.invokeHotKey_CtrlR();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_l]) {
            self.invokeHotKey_CtrlL();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_k]) {
            self.invokeHotKey_CtrlK();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_h]) {
            self.invokeHotKey_CtrlH();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_m]) {
            self.invokeHotKey_CtrlM();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_y]) {
            self.invokeHotKey_CtrlY();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_Comma]) {
            self.invokeHotKey_CtrlComma();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_Period]) {
            self.invokeHotKey_CtrlPeriod();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_t]) {
            self.invokeHotKey_CtrlT();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_p]) {
            self.invokeHotKey_CtrlP();
            return true;
        }

        return false;
    };
    self.handle2KeyHotKey_AltBinding = function () {
        var keyCodeEnums = keyboardManager.Enums.KeyCode;
        if (self.CachedData.KeyPress[keyCodeEnums.Enter]) {
            self.invokeHotKey_AltEnter();
            return true;
        }
    };

    self.invokeHotKey_CtrlU = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        self.CachedData.isInvokeHotKey = false;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlU) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlU();
            }
        }
    };
    self.invokeHotKey_CtrlG = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlG) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlG();
            }
        }
    };
    self.invokeHotKey_CtrlS = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlS) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlS();
            }
        }
    };
    self.invokeHotKey_CtrlD = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlD) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlD();
            }
        }

        return self.CachedData.isInvokeHotKey;
    };

    self.invokeHotKey_CtrlI = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlI) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlI();
            }
        }

        return self.CachedData.isInvokeHotKey;
    };
    self.invokeHotKey_CtrlV = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlV) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlV();
            }
        }

        return self.CachedData.isInvokeHotKey;
    };
    self.invokeHotKey_CtrlE = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlE) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlE();
            }
        }

        return self.CachedData.isInvokeHotKey;
    };
    self.invokeHotKey_CtrlR = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlR) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlR();
            }
        }
    };
    self.invokeHotKey_CtrlL = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlL) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlL();
            }
        }
    };
    self.invokeHotKey_CtrlK = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlK) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlK();
            }
        }
    };
    self.invokeHotKey_CtrlH = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlH) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlH();
            }
        }

        return self.CachedData.isInvokeHotKey;
    };
    self.invokeHotKey_CtrlM = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlM) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlM();
            }
        }

        return self.CachedData.isInvokeHotKey;
    };
    self.invokeHotKey_CtrlY = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlY) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlY();
            }
        }

        return self.CachedData.isInvokeHotKey;
    };
    self.invokeHotKey_CtrlP = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlP) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlP();
            }
        }

        return self.CachedData.isInvokeHotKey;
    };
    self.invokeHotKey_CtrlComma = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlComma) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlComma();
            }
        }
    };
    self.invokeHotKey_CtrlPeriod = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlPeriod) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlPeriod();
            }
        }
    };
    self.invokeHotKey_CtrlT = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlT) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlT();
            }
        }
    };
    self.invokeHotKey_AltEnter = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_AltEnter) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_AltEnter();
            }
        }

        return self.CachedData.isInvokeHotKey;
    };
    self.copyArray = function (array) {
        try {
            if (array === null ||
                array === undefined) {
                return array;
            }

            return array.slice(0);
        } catch (e) {
        }

        return [];
    };
};

hotKeyManager = new HotKeyManager();

$(document).ready(function () {
    hotKeyManager.initPage();
    hotKeyManager.unblockKeyDownEvent(false);
});