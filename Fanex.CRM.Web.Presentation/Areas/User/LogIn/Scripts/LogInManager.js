"use strict";
/* global sharedScriptManager */
var logInManager, msg, username, password, btnSubmit, innerMsg;

var LogInManager = function () {
    var self = this;

    self.checkBrowser = function () {
        var msieVersion = sharedScriptManager.getIeVersion();
        if (msieVersion !== -1 && msieVersion < 8.0) {
            var warnPnl = document.getElementById('not-supported-browser-panel');
            if (warnPnl.style.display === 'none') {
                warnPnl.style.display = 'block';
            }
        }
    };
    self.trimString = function (stringValue) {
        return stringValue.replace(/^\s*/, "").replace(/\s*$/, "");
    };

    self.scrollScreen = function (target, times, duration) {
        for (var i = times, j = 0; i >= 0; i--) {
            setTimeout(target + ".style.left = '-" + i + "0px'", j++ * duration);
            setTimeout(target + ".style.left = '" + i + "0px'", j++ * duration);
        }
    };
    self.submitLogIn = function () {
        msg.style.display = 'none';
        var isValid = true;
        username.value = logInManager.trimString(username.value);

        if (username.value.length === 0) {
            username.classList.add('err');
            isValid = false;
            logInManager.scrollScreen('username', 0, 100);
        } else {
            username.classList.remove('err');
        }

        if (password.value.length === 0) {
            password.classList.add('err');
            isValid = false;
            logInManager.scrollScreen('password', 0, 100);
        } else {
            password.classList.remove('err');
        }

        return isValid;
    };
    self.initPage = function () {
        setInterval(function () {
            window.history.forward();
        }, 0);

        msg = document.getElementById('msg');
        innerMsg = document.getElementById('inner-msg');
        username = document.getElementById('username');
        password = document.getElementById('password');
        btnSubmit = document.getElementById('submit');

        logInManager.checkBrowser(); // check if the client's browser is supported
        username.focus();
        if (innerMsg.innerHTML.length > 0) {
            msg.style.display = 'block';
            logInManager.scrollScreen('msg', 0, 100);
        }
        else {
            msg.style.display = 'none';
        }
    };
};

logInManager = new LogInManager();

window.onload = function () {
    logInManager.initPage();
};

document.forms[0].onsubmit = function () {
    return logInManager.submitLogIn();
};