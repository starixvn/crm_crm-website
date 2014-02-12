var HotKeyManager = function () {
    var self = this;

    self.CachedData = {
        KeyPress: [],
        ActiveDocumentManager: [],
        isInvokeHotKey: false
    };

    self.initPage = function () {
        self.bindEvent();
    };

    self.bindEvent = function () {
        $(document).keydown(function (eventObject) {
            var isCustomHotKey = self.keyDown(eventObject);
            if (!self.CachedData.isInvokeHotKey) {
                parent.hotKeyManager.CachedData.KeyPress = hotKeyManager.CachedData.KeyPress;
                isCustomHotKey = parent.hotKeyManager.invokeHotKey();
            }

            if (isCustomHotKey) {
                if (eventObject.preventDefault) {
                    eventObject.stopPropagation();
                    eventObject.preventDefault();
                } else {
                    eventObject.returnValue = false;
                    window.event.returnValue = false;
                    window.event.keyCode = 0;
                }

                return false;
            }
        });

        $(document).keyup(function (eventObject) {
            var isCustomHotKey = self.keyUp(eventObject);
        });

        $(window).blur(function () {
            self.CachedData.KeyPress = [];
        });
    };
    self.keyDown = function (eventObject) {
        if (!self.CachedData.isInvokeHotKey) {
            var keyCode = parseInt(eventObject.keyCode);
            self.CachedData.KeyPress[keyCode.toString()] = true;
            var isCustomHotKey = false;
            self.CachedData.isInvokeHotKey = false;
            // Use follow line to test working flow
            // console.log(self.CachedData.KeyPress);
            if (self.CachedData.ActiveDocumentManager.length === 0) {
                return isCustomHotKey;
            }

            isCustomHotKey = self.invokeHotKey();
            if (self.CachedData.isInvokeHotKey) {
                self.delayInvokeHotKey();
            }

            return isCustomHotKey;
        }
    };

    self.delayInvokeHotKey = function () {
        setTimeout(function () {
            self.CachedData.isInvokeHotKey = false;
        }, 100);
    };
    self.keyUp = function (eventObject) {
        if (self.CachedData.ActiveDocumentManager.length === 0) {
            return;
        }

        var keyCode = parseInt(eventObject.keyCode);
        self.CachedData.KeyPress[keyCode.toString()] = false;
    };
    self.invokeHotKey = function () {
        var isCustomHotKey = false;
        var numberOfKeyPress = self.getNumberOfKeyPress();
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

        if (self.CachedData.KeyPress[keyCodeEnums.Character_Comma]) {
            self.invokeHotKey_CtrlComma();
            return true;
        }

        if (self.CachedData.KeyPress[keyCodeEnums.Character_Period]) {
            self.invokeHotKey_CtrlPeriod();
            return true;
        }

        return false;
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
            console.log(self.CachedData.ActiveDocumentManager[i].Id);
            if (typeof (activeDocumentManager.invokeHotKey_CtrlD) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlD();
            }
        }
    };
    self.invokeHotKey_CtrlC = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i].Value;
            if (typeof (activeDocumentManager.invokeHotKey_CtrlC) === "function") {
                self.CachedData.isInvokeHotKey = true;
                activeDocumentManager.invokeHotKey_CtrlC();
            }
        }
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
};

var hotKeyManager = new HotKeyManager();
$(document).ready(function () {
    hotKeyManager.initPage();
});