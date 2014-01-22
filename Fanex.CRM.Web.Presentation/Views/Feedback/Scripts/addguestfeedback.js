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
/* exported feedbackNew, 
settings,
ddls,
numberOfFinish,
orgLengt,
AddGuestFeedback */
var checkSpecialCharacters,
    DoAdd,
    getFeedbacktData,
    getNumOfFeedbackItems,
    appendListFeedbacksLasted,
    feedbackNew;

var settings = ["Title", "Status", "Station", "Response", "Communication", "Language"];
var ddls = ["ddltitle", "ddlstatus", "ddlcsextension", "ddlresponse", "ddlcommunication", "ddllanguage"];
var listObjToValidate = ["txtsubject", "txtidentity"];
var listEditorToValidate = ["desediter"];

var numberOfFinish = 0;
var AllSetting;
var listFeedbacksLasted = [];
var numberOfFeedbackItem = 0;
var orgLengt = 0;

function loadFeedbackSetting() {
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/SelAllSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"Id":' + 0 + '}',
        success: function (setting) {
            AllSetting = setting;

            bindingSetting("ddltitle", setting.Title, 13000);
            bindingSetting("ddlcategory", setting.Category, 0);
            bindingSetting("ddlproduct", setting.Product, 0);
            bindingSetting("ddlrole", setting.Role, 7004);
            bindingSetting("ddlstatus", setting.Status, 0);
            bindingSetting("ddlresponse", setting.Response, 0);
            bindingSetting("ddlcommunication", setting.Communication, 0);
            bindingSetting("ddllanguage", setting.Language, 0);
            bindingSetting("ddlcsextension", setting.Extension, 0);
            bindingCountry("ddlcountry", setting.Country, 0);

            bindingChildByParentId('ddlsubcategory', setting.Category, (setting.Category ? setting.Category[0].Id : 3000), 0);
            bindingChildByParentId('ddlbettype', setting.Product, 0, 0);
            bindingSite('ddlsite', setting.Site, 0);

            if (!allowDeleteFeedback) {
                $("select#ddlstatus option[value='11004']").remove();
            }
            //getAutoFeedbackSubject();
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

        var serverTime = new Date($('#hServerTime').val());
        serverTime.setMinutes(currentDate.getMinutes());
        serverTime.setSeconds(currentDate.getSeconds());

        if (selectDate > serverTime) {
            showAlertMessage("errmsg", errMsgFeedbackDateGreatter, 1);
            return false;
        }
        if (Date.parse($("#txtdate").val()) >= Date.parse($("#txtResolutionTime").val())) {
            showAlertMessage("errmsg", errMsgResolutionDateGreatter, 1);
            return false;
        }
        if ($('#ddlcommunication option:selected').text().toLowerCase() === 'n/a') {
            showAlertMessage("errmsg", "Please choose communication!", 1);
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
        if ($('#txtidentity').val().length > 50) {
            $('#txtidentity').focus();
            //$('#txtidentity').css('color:red');
            showAlertMessage("errmsg", 'Max length of Username is 50', 1);
            return false;
        }
        if ($('#txtfirstname').val().length > 50) {
            $('#txtfirstname').focus();
            //$('#txtfirstname').css({ color: 'red'});
            showAlertMessage("errmsg", 'Max length of First Name is 50', 1);
            return false;
        }
        if ($('#txtlastname').val().length > 50) {
            $('#txtlastname').focus();
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
        }
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
                parent.$("#isGuest").attr("checked", true);
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
    var UserName = $("#txtidentity").val().trim();
    var Title = $("#ddltitle").val();
    var FirstName = $("#txtfirstname").val().trim();
    var LastName = $("#txtlastname").val().trim();
    var Phone = $("#txtphone").val().trim();
    var MobilePhone = $("#txtmobilephone").val().trim();
    var Fax = $("#txtfax").val().trim();
    var Email = $("#txtEmail").val().trim();
    var LanguageID = $("#ddllanguage").val();
    var CountryID = $("#ddlcountry").val();
    var FeedBackDate = $("#txtdate").val();
    var Creator = accId;
    var CommunitationID = $("#ddlcommunication").val();
    var StationID = $("#ddlcsextension").val();
    var MainCategoryID = $("#ddlcategory").val();
    var SubCategoryID = $("#ddlsubcategory").val();
    var SiteID = $("#ddlsite").val();
    //    var RoleID = $("#ddlrole").val();
    var ProductID = $("#ddlproduct").val();
    var BettypeID = $("#ddlbettype").val();
    var ResponseID = $("#ddlresponse").val();
    var StatusID = $("#ddlstatus").val();
    var RelatedCustID = "";
    if ($("#chkisrelated").attr("checked")) {
        RelatedCustID = getRelatedCustomer();
    }
    var Subject = $("#txtsubject").val().trim();
    var Description = $("#desediter").htmlarea("toHtmlString");
    var Solution = $("#solediter").htmlarea("toHtmlString");
    var IsUpdateReviewed = $("#chkisreview").attr("checked");
    var Reviewer;
    var Review;
    if (IsUpdateReviewed) {
        Reviewer = accId;
        Review = $("#revediter").htmlarea("toHtmlString");
    }
    else {
        Reviewer = 0;
        Review = "";
    }
    var skype = $('#txtSkype').val().trim();
    var msn = $('#txtMSN').val().trim();
    var resolutionTitme = $('#txtResolutionTime').val();

    // insert by David.Tran 07-03-2012
    var pic1 = $("#txtPIC1").val().trim();
    var pic2 = $("#txtPIC2").val().trim();
    var pic3 = $("#txtPIC3").val().trim();
    //


    feedbackNew.Customer.Id = 0;
    feedbackNew.Customer.UserName = UserName;
    feedbackNew.CustomerInfo.Title.Id = Title;
    feedbackNew.CustomerInfo.FirstName = FirstName;
    feedbackNew.CustomerInfo.LastName = LastName;
    feedbackNew.CustomerInfo.Phone = Phone;
    feedbackNew.CustomerInfo.MobilePhone = MobilePhone;
    feedbackNew.CustomerInfo.Fax = Fax;
    feedbackNew.CustomerInfo.Email = Email;
    feedbackNew.CustomerInfo.Skype = skype;
    feedbackNew.CustomerInfo.MSN = msn;
    feedbackNew.FeedbackDate = FeedBackDate;
    feedbackNew.Role.Id = 7004;
    feedbackNew.Language.Id = LanguageID;
    feedbackNew.Country.Id = CountryID;
    feedbackNew.Creator.Id = Creator;
    feedbackNew.Communication.Id = CommunitationID;
    feedbackNew.CsExtension.Id = StationID;
    feedbackNew.Category.Id = MainCategoryID;
    feedbackNew.SubCategory.Id = SubCategoryID;
    feedbackNew.Site.Id = SiteID;
    feedbackNew.Product.Id = ProductID;
    feedbackNew.BetType.Id = BettypeID;
    feedbackNew.Response.Id = ResponseID;
    feedbackNew.Status.Id = StatusID;
    feedbackNew.RelatedCustomer.Id = 0;
    feedbackNew.RelatedCustomer.UserName = RelatedCustID;
    feedbackNew.Subject = Subject.trim();
    feedbackNew.Description = Description.trim();
    feedbackNew.Solution = Solution.trim();

    // insert by David 08-03-2012
    feedbackNew.Customer.Pic1 = pic1;
    feedbackNew.Customer.Pic2 = pic2;
    feedbackNew.Customer.Pic3 = pic3;
    //
    if (IsUpdateReviewed) {
        feedbackNew.IsReviewed = true;
        feedbackNew.Reviewer.Id = Reviewer;
        feedbackNew.Review = Review.trim();
    }
    else {
        feedbackNew.IsReviewed = false;
        feedbackNew.Reviewer.Id = 0;
    }

    feedbackNew.ResolutionTime = resolutionTitme;
    return feedbackNew;
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
    $('#hServerTime').val(feedbackdate);

    var arrDateValues = [feedbackdate.format("m/d/Y H:i"), resolutiondate.format("m/d/Y H:i")];
    var arrIds = ['txtdate', 'txtResolutionTime'];
    /* jshint newcap: false */
    SetupCalendar(arrDateValues, arrIds);

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
        //                    select_all_text: "select",
    });
    if ($("#chkisrelated").attr('checked')) {
        $("#typeCust").val("");
        $("#typeCust").removeAttr("disabled");
    }
    $("#btnsubmit").removeAttr("disabled");
    $("#btnsubmitbottom").removeAttr("disabled");
    //add input listen for auto complete Subject name input text box
    $("#txtsubject").keyup(function (e) {
        if (e.keyCode === 37 ||
        e.keyCode === 38 ||
        e.keyCode === 39 ||
         e.keyCode === 40) {
            return true;
        }
        else if (e.keyCode !== 13) {
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
});

feedbackNew = {
    "Id": 0,
    "BetType": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Category": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Communication": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Country": {
        "Id": 0,
        "CountryCode": 0,
        "CountryName": null,
        "DisplayOrder": 0,
        "IsActive": false
    },
    "CreateDate": null,
    "Creator": {
        "Id": 0,
        "IsAllowUpdateReview": false,
        "Password": null,
        "RoleId": 0,
        "Username": ""
    },
    "CsExtension": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Customer": {
        "Id": 0,
        "Answer": null,
        "BankInfo": null,
        "BettingCode": null,
        "Country": 0,
        "CountryName": null,
        "CreatedDate": null,
        "Creator": null,
        "CustomerInfo": null,
        "IsVip": false,
        "Language": 0,
        "NickName": null,
        "Pic1": null,
        "Pic2": null,
        "Pic3": null,
        "Question": null,
        "RefCode2": null,
        "RelatedCustomer": null,
        "Remark": null,
        "RoleId": 0,
        "RoleName": "",
        "Site": null,
        "SpecialRemark": null,
        "UserName": ""
    },
    "CustomerInfo": {
        "BirthDay": null,
        "Email": " ",
        "Fax": " ",
        "FirstName": " ",
        "LastName": " ",
        "MSN": " ",
        "MobilePhone": " ",
        "Phone": " ",
        "Skype": " ",
        "Title": {
            "Id": 0,
            "Child": null,
            "DisplayOrder": 0,
            "IsSub": false,
            "Parent": null,
            "Type": null,
            "Value": null
        }
    },
    "Description": "",
    "FeedbackDate": "/Date(1317709320000+0700)/",
    "IsReviewed": false,
    "Language": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Priority": null,
    "Product": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "RelatedCustomer": {
        "Id": 0,
        "Answer": null,
        "BankInfo": null,
        "BettingCode": null,
        "Country": 0,
        "CountryName": null,
        "CreatedDate": null,
        "Creator": null,
        "CustomerInfo": null,
        "IsVip": false,
        "Language": 0,
        "NickName": null,
        "Pic1": null,
        "Pic2": null,
        "Pic3": null,
        "Question": null,
        "RefCode2": null,
        "RelatedCustomer": null,
        "Remark": null,
        "RoleId": 0,
        "RoleName": "",
        "Site": null,
        "SpecialRemark": null,
        "UserName": ""
    },
    "ResolutionTime": null,
    "Response": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Review": "",
    "Reviewer": {
        "Id": 0,
        "IsAllowUpdateReview": false,
        "Password": null,
        "RoleId": 0,
        "Username": ""
    },
    "Role": {
        "Id": 7004,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Site": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Solution": " ",
    "SolvedDate": null,
    "Status": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "StrFeedbackDate": "",
    "SubCategory": {
        "Id": 0,
        "Child": null,
        "DisplayOrder": 0,
        "IsSub": false,
        "Parent": null,
        "Type": null,
        "Value": null
    },
    "Subject": "",
    "UpdateReviewMode": 0
};
