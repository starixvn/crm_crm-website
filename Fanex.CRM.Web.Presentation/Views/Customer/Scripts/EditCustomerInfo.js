"use strict";
/*global 
showAlertMessage, head,
convertUnitToLocalDatetime, accId, 
custId, successMsg */

/* exported 
updateCustomerInfo, verifyUserName, 
checkEditIndentityInfo, changeUpDownButton,
downup, keyup */

var custUserName = "";
var showBankInfo = false;

function keyup(event) {
    /*jshint unused: vars */
    if ($("#txtrelatedcustomer").val() !== "") {
        $("#btnOtherUserVerify").removeAttr("disabled");
    }
    else {
        $("#btnOtherUserVerify").attr("disabled", "disabled");
    }
}

$.datepicker._setTime = function (inst, date) {
    /*jshint unused: vars */
};

// -- Calendar --- //
function setupCalendar(strdatetime) {

    $('#txtBirthDay').datepicker({
        dateFormat: 'mm/dd/yy',
        yearRange: '-100:+0',
        changeYear: true
    });
    if (strdatetime != null && strdatetime !== "") {
        $('#txtBirthDay').val(strdatetime).datepicker('setDate', (new Date(strdatetime)));
    }
}

// --/Calendar --- //
function checkSpecialCharacters(id) {
    var iChars = "!`#%^&*()=[]\\\';,/{}|\":<>?~";
    var data = $("#" + id).val();

    for (var i = 0; i < data.length; i++) {
        if (iChars.indexOf(data.charAt(i)) !== -1) {
            showAlertMessage("errmsg", 'Your Indentity has special characters. !`#%^&*()=[]\\\';,/{}|\":<>?~', 1);
            return false;
        }
    }
    return true;
}

function ddlBinding(id, settingType, parrentId, currentValue) {
    var inputData = '{"settingName":"' + settingType + '", "parentId":' + parrentId + '}';
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetSettingRelationByName",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: inputData,
        success: function (rs) {
            if (rs != null && rs.length > 0) {
                $("#tmplOption").tmpl(rs).appendTo("#" + id);
                if (currentValue != null) {
                    $("#" + id + " option[value='" + currentValue + "']").attr("selected", "selected");
                }
            }
        }
    });
}

function assignDataToDdl(rs, currentValue) {
    var creditArr = [];
    var licenseeArr = [];

    creditArr.push('<optgroup label="Credit">');
    licenseeArr.push('<optgroup label="Licensee">');

    for (var i = 0; i < rs.length; i++) {
        var currentSite = rs[i];
        if (currentSite != null) {
            if (parseInt(currentSite.SiteType) === 0) {
                creditArr.push('<option value="' + currentSite.SiteId + '">' + currentSite.SiteName + '</option>');
            }
            else if (parseInt(currentSite.SiteType) === 1) {
                licenseeArr.push('<option value="' + currentSite.SiteId + '">' + currentSite.SiteName + '</option>');
            }
        }
    }

    creditArr.push('</optgroup>');
    licenseeArr.push('</optgroup>');

    var siteArr = [];

    siteArr.push('<option value="0">Select</option>');
    siteArr.push(creditArr);
    siteArr.push(licenseeArr);
    $('#ddlRelatedProduct').append(siteArr.join(''));

    $("#ddlRelatedProduct option[value='" + currentValue + "']").attr("selected", "selected");
}

//---Verify username---
function selGroupSite(currentValue) {
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/SelAllSite",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function (rs) {
            assignDataToDdl(rs, currentValue);
        }
    });
}

//----Get Country-----
function getAllCountries(id, currentValue) {
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetAllCountry",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function (rs) {
            if (rs != null && rs.length > 0) {
                $("#tmplCountry").tmpl(rs).appendTo("#" + id);
                if (currentValue != null) {
                    $("#" + id + " option[value='" + currentValue + "']").attr("selected", "selected");
                }
            }
        }
    });
}
//----/Get Country-----

// --- Load CRM info --- //
function getCRMInfoById(custId) {
    try { parent.BlockUI(); } catch (e) { }

    $.ajax({
        type: "POST",
        url: window.rootUrl + "Customer/GetCustomerInfoById",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"custId":' + custId + '}',
        success: function (rs) {

            $("#tmplCRMInfo").tmpl(rs).appendTo("#tbodyCRMInfo");
            if (rs.RelatedCustomer != null && rs.RelatedCustomer.Id != null) {
                $("#lblverify").attr("value", rs.RelatedCustomer.Id);
            }

            custUserName = rs.UserName;
            //            desvalue = rs.Remark;
            ddlBinding('ddlLanguage', 'language', 0, rs.Language);
            selGroupSite(rs.Site.Id);

            getAllCountries("ddlCountry", parseInt(rs.Country));

            $("#hiddenCRMId").val(rs.Id);
            $("#hiddenCreatorUserName").val(rs.Creator.Username);
            if (rs.IsVip === true) {
                $("#chkVIP").attr("checked", "checked");
            }

            if (rs.CustomerInfo.BirthDay != null) {
                setupCalendar(convertUnitToLocalDatetime(rs.CustomerInfo.BirthDay, 'm/d/Y'));
            }
            else {
                var currdate = new Date();
                setupCalendar(currdate.format('m/d/Y'));
            }
            if (rs.RelatedCustomer != null && rs.RelatedCustomer.UserName === "") {
                $("#btnOtherUserVerify").attr("disabled", "disabled");
            }
        }, complete: function () {
            try {
                parent.UnBlockUI();
            } catch (e) {
            }
        }
    });
}
// ---/ Load CRM info --- //

//----Update CRM info----
function getCurrentData() {
    //user that is login
    var userId = accId;
    //custid is customerid
    var curCustId = custId;
    //user name is the name of cutomer
    var firstName = $("#txtFirstName").val();
    var lastName = $("#txtLastName").val();
    var titleId = 0;
    var birthDay = $("#txtBirthDay").val();
    var langueId = $("#ddlLanguage").val();
    var phone = $("#txtPhone").val();
    var mobilePhone = $("#txtMobile").val();
    var fax = $("#txtFax").val();
    var email = $("#txtEmail").val();
    var bettingCode = $("#txtBettingCode").val();
    var refCode2 = $("#RefCode2").val();
    var pic1 = $("#txtPIC1").val();
    var pic2 = $("#txtPIC2").val();
    var pic3 = $("#txtPIC3").val();
    var question = '';
    var answer = '';
    var countryId = $("#ddlCountry").val();
    var isVip = $("#chkVIP").attr("checked");
    if ($("#txtrelatedcustomer").val() === "") {
        $("#lblverify").attr("value", "0");
        $("#lblverify").removeClass("noneverifycustomer").addClass("hidden");
    }

    //var realatedId = $("#lblverify").attr("value");
    var siteId = $("#ddlRelatedProduct").val();
    var bankName = $("#txtBank").val();
    var bankAdd = $("#txtBankAddress").val();
    var bankAccName = $("#txtAccountNo").val();
    var bankAccNo = $("#txtAccountNo").val();
    var bankSwiftCode = $("#txtIBANS_WIFT").val();
    var remark = $("#remarkeditor").val();
    var bankAccHolderName = $('#txtAccountHolderName').val();
    var specialRemark = $('#specialremarkeditor').val();
    var userName = '';
    var isUpdateUserName = false;
    if ($("#chkIsUpdateIndentity") !== undefined &&
        $("#chkIsUpdateIndentity").attr("checked") &&
        $("#txtUserName").val() !== '') {
        isUpdateUserName = true;
        userName = $("#txtUserName").val();
    } else {
        userName = $("#lblUserName").html();
    }
    custUserName = userName;
    var skype = $('#txtSkype').val();
    var msn = $('#txtMSN').val();

    var creatorId = accId;

    var data = {
        "userId": userId,
        "custId": curCustId,
        "userName": userName.trim(),
        "firstName": firstName.trim(),
        "lastName": lastName.trim(),
        "titleId": titleId,
        "birthDay": birthDay,
        "languageId": langueId,
        "phone": phone.trim(),
        "mobilePhone": mobilePhone.trim(),
        "fax": fax.trim(),
        "email": email.trim(),
        "bettingCode": bettingCode.trim(),
        "refCode2": refCode2.trim(),
        "pic1": pic1.trim(),
        "pic2": pic2.trim(),
        "pic3": pic3.trim(),
        "question": question.trim(),
        "answer": answer.trim(),
        "countryId": countryId,
        "isVip": isVip,
        "relatedCustId": 0,
        "siteId": siteId,
        "bankName": bankName.trim(),
        "bankAddress": bankAdd.trim(),
        "bankAccountName": bankAccName.trim(),
        "bankAccountNo": bankAccNo.trim(),
        "bankSwiftCode": bankSwiftCode.trim(),
        "remark": remark.trim(),
        "creatorId": creatorId,
        "bankHolderName": bankAccHolderName.trim(),
        "specialRemark": specialRemark.trim(),
        "isUpdateUserName": isUpdateUserName,
        "skype": skype.trim(),
        "msn": msn.trim()
    };
    return data;
}

function doUpdate() {

    //Update CustInfo
    var currentData = JSON.stringify(getCurrentData());
    try { parent.BlockUI(); } catch (e) { }

    $.ajax({
        type: "POST",
        url: window.rootUrl + "Customer/UpdateCustomerInfo",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: currentData,
        success: function (rs) {

            if (rs.ErrCode === 0) {
                showAlertMessage("errmsg", successMsg, 0);
                parent.leftPanelManager.loadLeftPanel(custUserName, true);
            }
            else {
                showAlertMessage("errmsg", rs.ErrMsg, 1);
            }

            try { parent.UnBlockUI(); } catch (e) { }
        },
        error: function () {
            showAlertMessage("errmsg", 'Update fail!', 1);
            try { parent.UnBlockUI(); } catch (e) { }
        }
    });
}

function updateCustomerInfo() {
    /*jshint maxstatements: 100 */
    if (!checkSpecialCharacters('txtUserName')) {
        return false;
    }

    $("#errmsg").empty();
    if ($('#txtUserName').val().length > 50) {
        $('#txtUserName').focus();
        showAlertMessage("errmsg", 'Max length of Username is 50', 1);
        return false;
    }

    if ($('#txtFirstName').val().length > 50) {
        $('#txtFirstName').focus();
        showAlertMessage("errmsg", 'Max length of First Name is 50', 1);
        return false;
    }

    if ($('#txtLastName').val().length > 50) {
        $('#txtLastName').focus();
        showAlertMessage("errmsg", 'Max length of Last Name is 50', 1);
        return false;
    }

    if ($('#txtPhone').val().length > 50) {
        $('#txtPhone').focus();
        showAlertMessage("errmsg", 'Max length of Phone is 50', 1);
        return false;
    }

    if ($('#txtMobile').val().length > 50) {
        $('#txtMobile').focus();
        showAlertMessage("errmsg", 'Max length of Mobile phone is 50', 1);
        return false;
    }

    if ($('#txtFax').val().length > 50) {
        $('#txtFax').focus();
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

    if ($('#txtPIC1').val().length > 100) {
        $('#txtPIC1').focus();
        showAlertMessage("errmsg", 'Max length of PIC1 is 100', 1);
        return false;
    }

    if ($('#txtPIC2').val().length > 100) {
        $('#txtPIC2').focus();
        showAlertMessage("errmsg", 'Max length of PIC2 is 100', 1);
        return false;
    }

    if ($('#txtPIC3').val().length > 100) {
        $('#txtPIC3').focus();
        showAlertMessage("errmsg", 'Max length of PIC3 is 100', 1);
        return false;
    }

    if ($('#txtBettingCode').val().length > 50) {
        $('#txtBettingCode').focus();
        showAlertMessage("errmsg", 'Max length of Betting Code is 50', 1);
        return false;
    }

    if ($('#RefCode2').val().length > 50) {
        $('#RefCode2').focus();
        showAlertMessage("errmsg", 'Max length of Ref Code 2 is 50', 1);
        return false;
    }

    if ($('#txtBank').val().length > 100) {
        $('#txtBank').focus();
        showAlertMessage("errmsg", 'Max length of Bank is 100', 1);
        return false;
    }

    if ($('#txtAccountHolderName').val().length > 100) {
        $('#txtAccountHolderName').focus();
        showAlertMessage("errmsg", 'Max length of Account Holder Name is 100', 1);
        return false;
    }

    if ($('#txtAccountNo').val().length > 50) {
        $('#txtAccountNo').focus();
        showAlertMessage("errmsg", 'Max length of Account No. is 50', 1);
        return false;
    }

    if ($('#txtBankAddress').val().length > 200) {
        $('#txtBankAddress').focus();
        showAlertMessage("errmsg", 'Max length of Bank Address is 200', 1);
        return false;
    }

    if ($('#txtIBANS_WIFT').val().length > 200) {
        $('#txtIBANS_WIFT').focus();
        showAlertMessage("errmsg", 'Max length of IBAN/SWIFT is 200', 1);
        return false;
    }

    if ($("#remarkeditor").val().length > 1000) {
        showAlertMessage("errmsg", 'Max length of Remark is 1000', 1);
        return false;
    }

    if ($("#specialremarkeditor").val().length > 1000) {
        showAlertMessage("errmsg", 'Max length of Special Remark is 1000', 1);
        return false;
    }

    doUpdate();
}

//---Verify username---
function verifyUserName() {

    var userName = $("#txtrelatedcustomer").val();
    if (userName !== "") {
        if (userName.toLowerCase() === custUserName.toLowerCase()) {
            $("#lblverify").removeClass("smallloading")
                           .removeClass("verifycustomer")
                           .addClass("noneverifycustomer")
                           .removeClass("hidden")
                           .html('Invalid Related Customer');
        }
        else {
            $("#lblverify").addClass("smallloading");
            $.ajax({
                type: "POST",
                url: window.rootUrl + "Customer/VerifyUserName",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{"UserName":"' + userName + '"}',
                success: function (rs) {
                    if (rs > 0) {
                        $("#lblverify").removeClass("smallloading")
                                        .removeClass("noneverifycustomer")
                                        .addClass("verifycustomer")
                                        .removeClass("hidden")
                                        .html(userName)
                                        .attr("value", rs);
                    }
                    else {
                        $("#lblverify").removeClass("smallloading")
                                        .removeClass("verifycustomer")
                                        .addClass("noneverifycustomer")
                                        .removeClass("hidden")
                                        .html('Invalid Related Customer')
                                        .attr("value", -1);
                    }
                }
            });
        }
    }
}

function checkEditIndentityInfo() {
    if ($("#chkIsUpdateIndentity").attr("checked")) {
        $("#lblEditIndentity").removeClass("hidden");
        $("#lblUserName").addClass("hidden");
    } else {
        $("#lblEditIndentity").addClass("hidden");
        $("#lblUserName").removeClass("hidden");
    }
}

function downup() {
    var bankinfoHidden = $('.bankinfo');
    if (showBankInfo) {
        bankinfoHidden.addClass('hidden');

        $('#btndownup').addClass('downup');
        $('#btndownup').removeClass('updown');
        showBankInfo = false;
    }
    else {
        bankinfoHidden.removeClass('hidden');

        $('#btndownup').addClass('updown');
        $('#btndownup').removeClass('downup');
        showBankInfo = true;
    }
}

function changeUpDownButton() {
    $("#btndownup").mouseover(function () {
        $(this).attr('class', $(this).attr('class').replace("updown", "updown_over").replace("downup", "downup_over"));
    });
    $("#btndownup").mouseout(function () {
        $(this).attr('class', '');
        if (showBankInfo) {
            $(this).addClass("updown");
        }
        else {
            $(this).addClass("downup");
        }
    });
}

head.ready(
    function () {
        getCRMInfoById(custId);
    }
);
