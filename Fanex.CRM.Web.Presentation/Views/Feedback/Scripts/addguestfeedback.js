"use strict";
/* global bindingSetting,
bindingCountry,
bindingChildByParentId,
bindingSite,
allowDeleteFeedback,
feedbackObjectValidate,
showAlertMessage,
successMsg,
accId,
getRelatedCustomer,
head,
clientTime,
initHTMLArea,
errMsgFeedbackDateGreatter,
errMsgResolutionDateGreatter,
SetupCalendar,
inputMultipleRelatedNote,
jlinq
*/
/* exported settings,
ddls,
numberOfFinish,
orgLengt,
AddGuestFeedback,
bindingEvent,
isShowCustinfoBox,
isShowDescriptionBox,
isShowFeedbackDetailBox,
isShowReviewBox,
isShowSolutionBox */
var checkSpecialCharacters,
    DoAdd,
    getFeedbacktData,
    getNumOfFeedbackItems,
    appendListFeedbacksLasted,
    bindingEvent;

var settings = ["Title", "Status", "Station", "Response", "Communication", "Language"];
var ddls = ["ddltitle", "ddlstatus", "ddlcsextension", "ddlresponse", "ddlcommunication"];
var listObjToValidate = ["txtsubject", "txtidentity"];
var listEditorToValidate = ["desediter"];

var numberOfFinish = 0;
var AllSetting;
var listFeedbacksLasted = [];
var numberOfFeedbackItem = 0;
var orgLengt = 0;
var isShowCustinfoBox = false;
var isShowFeedbackDetailBox = false;
var isShowDescriptionBox = false;
var isShowSolutionBox = false;
var isShowReviewBox = false;

function bindingEvent() {
    $('#ddlcategory').change(function () {
        bindingChildByParentId('ddlsubcategory', AllSetting.Category, parseInt($('#ddlcategory').val()));
    });

    $('#ddlproduct').change(function () {
        bindingChildByParentId('ddlbettype', AllSetting.Product, parseInt($('#ddlproduct').val()));
    });
}

function loadFeedbackSetting() {
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/SelAllSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"Id":' + 0 + '}',
        success: function (setting) {
            AllSetting = setting;
            // Identity Information
            bindingSetting("ddltitle", setting.Title, 13000);
            bindingSite("ddlSiteInfo", setting.Site, 0);
            dropdownMultiSelect("ddlLanguageInfo", "ddlLanguageInfo", setting.Language);

            // Details
            bindingSetting("ddlcategory", setting.Category, 0);
            bindingSetting("ddlproduct", setting.Product, 0);
            bindingSetting("ddlstatus", setting.Status, 0);
            bindingSetting("ddlresponse", setting.Response, 0);
            bindingSetting("ddlcommunication", setting.Communication, 0);
            bindingSetting("ddlcsextension", setting.Extension, 0);
            bindingCountry("ddlcountry", setting.Country, 0);

            bindingChildByParentId('ddlsubcategory', setting.Category, (setting.Category ? setting.Category[0].Id : 3000), 0);
            bindingChildByParentId('ddlbettype', setting.Product, 0, 0);

            if (!allowDeleteFeedback) {
                $("select#ddlstatus option[value='11004']").remove();
            }

            bindingEvent();
        },
        error: function () { },
        complete: function () {
            try {
                parent.UnBlockUI();
            } catch (e) { }
            //get lasted feedbacks data
            getNumOfFeedbackItems();
            setInterval(function () {
                getNumOfFeedbackItems();
            }, 100000);
        }
    });
}

//insert by David 16-03-2012
$("#txtidentity").blur(function () {
    checkSpecialCharacters($(this).attr('id'));
});

checkSpecialCharacters = function (id) {
    var iChars = "!`#%^&*()=[]\\\';,/{}|\":<>?~";

    var data = $("#" + id).val();
    for (var i = 0; i < data.length; i++) {
        if (iChars.indexOf(data.charAt(i)) !== -1) {
            $(("#" + id).replace("ddl", "error")
                        .replace("txt", "error"))
                        .removeClass("hidden")
                        .html("Your Indentity has special characters. !`#%^&*()=[]\\\';,/{}|\":<>?~");
            return false;
        }
    }
    return true;
};

function AddGuestFeedback() {
    /* jshint maxstatements: 87 */
    $("#errmsg").empty();
    $("#lblverify").html('');

    if (!checkSpecialCharacters('txtidentity')) {
        return false;
    }

    if (feedbackObjectValidate(listEditorToValidate, listObjToValidate)) {
        var currentDate = new Date();
        var selectDate = Date.parse($("#txtdate").val());

        var serverTime = new Date(clientTime);
        serverTime.setMinutes(currentDate.getMinutes());
        serverTime.setSeconds(currentDate.getSeconds());

        if (selectDate > serverTime) {
            showAlertMessage("errmsg", errMsgFeedbackDateGreatter, 1);
            return false;
        }
        if (selectDate >= Date.parse($("#txtResolutionTime").val())) {
            showAlertMessage("errmsg", errMsgResolutionDateGreatter, 1);
            return false;
        }
        if ($('#ddlcommunication option:selected').text().toLowerCase() === 'n/a') {
            showAlertMessage("errmsg", "Please choose communication!", 1);
            return false;
        }
        if ($('#ddlproduct option:selected').val() === 0) {
            showAlertMessage("errmsg", "Please choose product!", 1);
            return false;
        }
        if ($('#hIsNotChild').val() === '1') {
            showAlertMessage("errmsg",
                "There is no Subcategory for Category [" +
                    $("#ddlcategory option:selected").text() +
                "]. Please add a Subcategory or choose another Category.", 1);
            return false;
        }
        //----------check length of data
        /*var identity = $('#txtidentity').val().trim();
        if (identity.length > 50) {
            $('#txtidentity').focus();
            showAlertMessage("errmsg", 'Max length of Username is 50', 1);
            return false;
        }
        var firstName = $('#firstNameInfo').val().trim();
        if (firstName.length > 50) {
            $('#firstNameInfo').focus();
            showAlertMessage("errmsg", 'Max length of First Name is 50', 1);
            return false;
        }
        var lastName = 
        if ($('#lastNameInfo').val().length > 50) {
            $('#lastNameInfo').focus();
            showAlertMessage("errmsg", 'Max length of Last Name is 50', 1);
            return false;
        }
        if ($('#txtphone').val().length > 50) {
            $('#txtphone').focus();
            showAlertMessage("errmsg", 'Max length of Phone is 50', 1);
            return false;
        }
        if ($('#txtmobilephone').val().length > 50) {
            $('#txtmobilephone').focus();
            showAlertMessage("errmsg", 'Max length of Mobile phone is 50', 1);
            return false;
        }
        if ($('#txtfax').val().length > 50) {
            $('#txtfax').focus();
            showAlertMessage("errmsg", 'Max length of Fax is 50', 1);
            return false;
        }
        if ($('#txtEmail').val().length > 254) {
            $('#txtEmail').focus();
            showAlertMessage("errmsg", 'Max length of Email is 254', 1);
            return false;
        }
        if ($('#txtSkype').val().length > 100) {
            $('#txtSkype').focus();
            showAlertMessage("errmsg", 'Max length of Skype is 100', 1);
            return false;
        }
        if ($('#txtMSN').val().length > 100) {
            $('#txtMSN').focus();
            showAlertMessage("errmsg", 'Max length of MSN is 100', 1);
            return false;
        }
        if ($('#txtsubject').val().length > 200) {
            $('#txtsubject').focus();
            showAlertMessage("errmsg", 'Max length of Subject is 200', 1);
            return false;
        }
        if ($('#txtPIC1').val().length > 100) {
            $('#txtPIC1').focus();
            //$('#txtfirstname').css({ color: 'red'});
            showAlertMessage("errmsg", 'Max length of PIC1 is 100', 1);
            return false;
        }
        if ($('#txtPIC2').val().length > 100) {
            $('#txtPIC2').focus();
            //$('#txtfirstname').css({ color: 'red'});
            showAlertMessage("errmsg", 'Max length of PIC2 is 100', 1);
            return false;
        }
        if ($('#txtPIC3').val().length > 100) {
            $('#txtPIC3').focus();
            //$('#txtfirstname').css({ color: 'red'});
            showAlertMessage("errmsg", 'Max length of PIC3 is 100', 1);
            return false;
        }
        if ($("#desediter").htmlarea("toHtmlString").length > 8000) {
            $('#desediter').focus();
            //$('#txtfirstname').css({ color: 'red'});
            showAlertMessage("errmsg", 'Max length of Description is 8000', 1);
            return false;
        }
        if ($("#solediter").htmlarea("toHtmlString").length > 8000) {
            $('#solediter').focus();
            //$('#txtfirstname').css({ color: 'red'});
            showAlertMessage("errmsg", 'Max length of Solution is 8000', 1);
            return false;
        }
        if ($("#revediter").htmlarea("toHtmlString").length > 8000) {
            $('#revediter').focus();
            //$('#txtfirstname').css({ color: 'red'});
            showAlertMessage("errmsg", 'Max length of Review is 8000', 1);
            return false;
        }*/
        //------------------------
        /* jshint newcap: false */
        DoAdd();
    }
}

DoAdd = function () {
    try { parent.BlockUI(); } catch (e) { }
    var newFeedback = getFeedbacktData();
    var feedback = { feedback: newFeedback };
    $.ajax({
        type: "POST",
        url: window.rootUrl + "Feedback/AddGuestFeedback",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(feedback),
        success: function (rs) {
            if (rs.ErrCode === 0) {
                showAlertMessage("errmsg", successMsg, 0);
                parent.$("#isGuest").prop("checked", true);
                parent.leftPanelManager.loadLeftPanel($("#txtidentity").val().trim(), true);
            }
            else {
                showAlertMessage("errmsg", rs.ErrMsg, 1);
            }
            setTimeout(function () {
                parent.UnBlockUI();
            }, 100);
        },
        complete: function () {
            try { parent.UnBlockUI(); } catch (e) { }
        }
    });
};

getFeedbacktData = function () {
    /* jshint maxstatements: 83 */
    var customerInfo = {
        Title: {
            Id: $("#ddltitle").val()
        },
        FirstName: $("#firstNameInfo").val().trim(),
        LastName: $("#lastNameInfo").val().trim(),
        ContactNumber1: {
            CountryCode: $("#contact1InfoCountry").val().trim(),
            AreaCode: $("#contact1InfoArea").val().trim(),
            PhoneNumber: $("#contact1InfoPhone").val().trim()
        },
        ContactNumber2: {
            CountryCode: $("#contact2InfoCountry").val().trim(),
            AreaCode: $("#contact2InfoArea").val().trim(),
            PhoneNumber: $("#contact2InfoPhone").val().trim()
        },
        ContactNumber3: {
            CountryCode: $("#contact3InfoCountry").val().trim(),
            AreaCode: $("#contact3InfoArea").val().trim(),
            PhoneNumber: $("#contact3InfoPhone").val().trim()
        },
        Fax: {
            CountryCode: $("#faxInfoCountry").val().trim(),
            AreaCode: $("#faxInfoArea").val().trim(),
            FaxNumber: $("#faxInfoFax").val().trim()
        },
        Mobile: {
            CountryCode: $("#mobileInfoCountry").val().trim(),
            AreaCode: $("#mobileInfoArea").val().trim(),
            PhoneNumber: $("#mobileInfoPhone").val().trim()
        },
        PIC1: $("#pic1Info").val().trim(),
        PIC2: $("#pic2Info").val().trim(),
        PIC3: $("#pic3Info").val().trim(),
        PIC4: $("#pic4Info").val().trim(),
        Skype: $("#skypeInfo").val().trim(),
        Email: $("#emailInfo").val().trim(),
        MSN: $("#msnInfo").val().trim(),
        PassportNumber: $("#passportInfo").val().trim(),
        Birthday: $("#birthdayInfo").datepicker("getDate"),
        Site: $("#ddlSiteInfo").val(),
        Country: $("#ddlcountry").val(),
        Languages: $("#ddlLanguageInfo").fanexMultiSelect('getItems', true).join(","),
        BettingCode: $("#bettingCodeInfo").val().trim(),
        RefCodes2: $("#refCodes2Info").val().trim(),
        IsVIP: $("#isVIP").prop("checked")
    }

    // related customers
    var feedback = {
        Creator: {
            Id: accId
        },
        Customer: {
            Id: 0,
            UserName: $("#txtidentity").val().trim()
        },
        CustomerInfo: customerInfo,
        FeedbackDate: $("#txtdate").val(),
        ResolutionTime: $('#txtResolutionTime').val(),
        CsExtension: {
            Id: $("#ddlcsextension").val()
        },
        Communication: {
            Id: $("#ddlcommunication").val()
        },
        Category: {
            Id: $("#ddlcategory").val()
        },
        SubCategory: {
            Id: $("#ddlsubcategory").val()
        },
        Product: {
            Id: $("#ddlproduct").val()
        },
        BetType: {
            Id: $("#ddlbettype").val()
        },
        Status: {
            Id: $("#ddlstatus").val()
        },
        Role: {
            Id: 7004 // $("#ddlrole").val()
        },
        Response: {
            Id: $("#ddlresponse").val()
        },
        RelatedCustomer: {
            Id: 0,
            UserName: $("#chkisrelated").prop("checked") ? getRelatedCustomer() : ""
        },
        Subject: $("#txtsubject").val().trim(),
        Description: $("#desediter").htmlarea("toHtmlString").trim(),
        Solution: $("#solediter").htmlarea("toHtmlString").trim()
    };

    // reviewed
    var IsUpdateReviewed = $("#chkisreview").prop("checked");
    var Reviewer = 0;
    var Review = "";
    if (IsUpdateReviewed) {
        Reviewer = accId;
        Review = $("#revediter").htmlarea("toHtmlString");
    }

    feedback.IsReviewed = IsUpdateReviewed;
    feedback.Reviewer = {
        Id: Reviewer
    };
    feedback.Review = Review.trim();

    return feedback;
};

//mark because it may conflict with other function 
getNumOfFeedbackItems = function () {
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetNumOfFeedbackSubjectTemplateItem",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function (rs) {
            numberOfFeedbackItem = rs;
            listFeedbacksLasted = [];
            var numOfRange = Math.round((numberOfFeedbackItem / 100000) + 0.5);
            for (var i = 1; i <= numOfRange; i++) {
                appendListFeedbacksLasted(i);
            }
        },
        error: function () {
        },
        complete: function () {
        }
    });
};

appendListFeedbacksLasted = function (range) {
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetFeedbackSubjectTemplateItemsRange",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{ "range": "' + range.toString() + '" }',
        success: function (rs) {
            if (rs != null && rs.length > 0) {
                listFeedbacksLasted = listFeedbacksLasted.concat(rs);
            }
        },
        error: function () {
        },
        complete: function () {
        }
    });
};

head.ready(function () {
    initHTMLArea('editboxdes', 'desediter', '');
    initHTMLArea('editboxsol', 'solediter', '');
    initHTMLArea('editboxrev', 'revediter', '');

    try { parent.BlockUI(); } catch (e) { }
    loadFeedbackSetting();

    var feedbackdate = new Date(clientTime);
    var resolutiondate = new Date(clientTime);
    resolutiondate.setMinutes(feedbackdate.getMinutes() + 1);

    var arrDateValues = [feedbackdate.format("m/d/Y H:i"), resolutiondate.format("m/d/Y H:i")];
    var arrIds = ['txtdate', 'txtResolutionTime'];
    /* jshint newcap: false */
    SetupCalendar(arrDateValues, arrIds);
    $("#birthdayInfo").datepicker({ dateFormat: "mm/dd/yy" });

    $('#btnRelatedHelp').tipsy({
        gravity: 'nw',
        title: function () {
            return inputMultipleRelatedNote;
        }
    });

    $("#txtrelatedcustomer").fcbkcomplete({
        /* jshint camelcase: false */
        json_url: window.rootUrl + 'Customer/RelatedCustomerAutoSuggestion',
        addontab: true,
        maxitems: 30,
        input_min_size: 1,
        height: 10,
        cache: false,
        newel: true
    });
    if ($("#chkisrelated").prop('checked')) {
        var typeCust = $("#typeCust");
        typeCust.val("");
        typeCust.removeAttr("disabled");
    }
    $("#btnsubmit").removeAttr("disabled");
    $("#btnsubmitbottom").removeAttr("disabled");
    
    //add input listen for auto complete Subject name input text box
    $("#txtsubject").keyup(function (e) {
        var keyCode = e.keyCode;
        if (keyCode === 37 || keyCode === 38 || keyCode === 39 || keyCode === 40) {
            return true;
        }
        else if (keyCode !== 13) {
            var rows = [];
            if ($(this).val() !== "") {
                rows = jlinq.from(listFeedbacksLasted).starts("Subject", $(this).val()).sort("Subject").select();
                rows = jlinq.from(rows).skip(0).take(15);
                if (rows.length > 0) {
                    $("#txtsubject").autoComplete(rows, "Subject");
                }
                else {
                    $(this).removeAutoComplete();
                }
            }
            else {
                $(this).removeAutoComplete();
            }
        }
    });

    $("#txtidentity").focus();
});