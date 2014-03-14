'use strict';
var feedbackData = undefined;
var settingData = undefined;

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

/* jshint camelcase: false */
var invokeHotKey_CtrlS = function () {
    AddGuestFeedback();
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