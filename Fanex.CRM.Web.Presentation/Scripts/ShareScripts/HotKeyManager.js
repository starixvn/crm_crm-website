var HotKeyManager = function () {
    var self = this;

    self.CachedData = {
        KeyPress: [],
        ActiveDocumentManager: []
    };

    self.initPage = function () {
        self.bindEvent();
    };

    self.bindEvent = function () {
        $(document).keydown(function (eventObject) {
            var isCustomHotKey = self.keyDown(eventObject);
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
        var keyCode = parseInt(eventObject.keyCode);
        self.CachedData.KeyPress[keyCode.toString()] = true;
        var isCustomHotKey = false;
        // Use follow line to test working flow
        //        console.log(self.CachedData.KeyPress);
        if (self.CachedData.ActiveDocumentManager.length == 0) {
            return isCustomHotKey;
        }

        isCustomHotKey = self.invokeHotKey();
        return isCustomHotKey;
    };
    self.keyUp = function (eventObject) {
        if (self.CachedData.ActiveDocumentManager.length == 0) {
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

        return false;
    };

    self.invokeHotKey_CtrlU = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i];
            if (typeof (activeDocumentManager.invokeHotKey_CtrlU) === "function") {
                activeDocumentManager.invokeHotKey_CtrlU();
            }
        }
    };
    self.invokeHotKey_CtrlG = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i];
            if (typeof (activeDocumentManager.invokeHotKey_CtrlG) === "function") {
                activeDocumentManager.invokeHotKey_CtrlG();
            }
        }
    };
    self.invokeHotKey_CtrlS = function () {
        var activeDocumentLength = self.CachedData.ActiveDocumentManager.length;
        for (var i = 0; i < activeDocumentLength; i++) {
            var activeDocumentManager = self.CachedData.ActiveDocumentManager[i];
            if (typeof (activeDocumentManager.invokeHotKey_CtrlS) === "function") {
                activeDocumentManager.invokeHotKey_CtrlS();
            }
        }
    };
};


var hotKeyManager = new HotKeyManager();
$(document).ready(function () {
    hotKeyManager.initPage();
});