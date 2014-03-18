'use strict';
/* global Setting,
FeedbackData,
hotKeyManager,
head,
clientTime,
allowDeleteFeedback,
accId */
/* exported AddGuestFeedback,
invokeHotKey_CtrlS,
isShowCustinfoBox,
isShowDescriptionBox,
isShowFeedbackDetailBox,
isShowSolutionBox */
/* jshint camelcase: false */
var feedbackData;
var settingData;

var isShowCustinfoBox = false;
var isShowFeedbackDetailBox = false;
var isShowDescriptionBox = false;
var isShowSolutionBox = false;

function AddGuestFeedback() {
    feedbackData.addGuestFeedback(function () {
        parent.$('#isGuest').attr('checked', true);

        var identity = $('#txtidentity').val().trim();
        parent.leftPanelManager.loadLeftPanel(identity, true);
    });
}

var invokeHotKey_CtrlS = function () {
    this.AddGuestFeedback();
};

head.ready(function () {
    try { parent.BlockUI(); } catch (e) { }

    feedbackData = new FeedbackData(accId, 0, clientTime);
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