'use strict';
/* global Setting,
FeedbackData,
hotKeyManager,
head,
clientTime,
allowDeleteFeedback,
accId,
accname,
verifyUserName */
/* exported AddGuestFeedback,
invokeHotKey_CtrlS,
isShowCustinfoBox,
isShowDescriptionBox,
isShowFeedbackDetailBox,
isShowSolutionBox,
orgLengt,
invokeHotKey_CtrlE */
/* jshint camelcase: false */
var feedbackData;
var settingData;

var isShowCustinfoBox = false;
var isShowFeedbackDetailBox = false;
var isShowDescriptionBox = false;
var isShowSolutionBox = false;
var orgLengt = 0;

function AddGuestFeedback() {
    feedbackData.addGuestFeedback(function () {
        parent.$('#isGuest').attr('checked', true);

        var identity = $('#txtidentity').val().trim();
        parent.$('#txtuserName').val(identity);
        parent.leftPanelManager.loadLeftPanel(identity, true);
    });
}

var invokeHotKey_CtrlS = function () {
    this.AddGuestFeedback();
};

var invokeHotKey_CtrlE = function () {
    verifyUserName();
};

head.ready(function () {
    try { parent.BlockUI(); } catch (e) { }

    feedbackData = new FeedbackData(accId, accname, 0, clientTime);
    feedbackData.initialize();

    settingData = new Setting(allowDeleteFeedback);
    settingData.getSettings(function () {
        feedbackData.getNumOfFeedbackItems();
        setInterval(function () {
            feedbackData.getNumOfFeedbackItems();
        }, 100000);

        try { parent.UnBlockUI(); } catch (e) { }
    });

    /* register hot key */
    hotKeyManager.CachedData.ActiveDocumentManager.push({
        Value: window,
        Id: 'AddGuestFeedback'
    });

    $('#txtidentity').focus();
});