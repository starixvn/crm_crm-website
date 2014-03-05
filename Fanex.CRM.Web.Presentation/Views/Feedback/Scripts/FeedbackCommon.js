"use strict";
/* global showAlertMessage, 
AllSetting, 
desvalue: true,
solvalue: true,
revvalue: true,
accname 
*/
/* exported trim, 
getRelatedCustomer, 
validateSetting,
EnableUpdateReview,
bindSetting, 
bindingCountry,
bindingManageChildByParentId, 
bindCountry, 
loadSubItem,
getSubCategory, 
initHTMLArea, 
SetupCalendar,
keyup, 
checkIsRelated, 
verifyUserName,
deleteAllInValidCustomers, 
DeleteRelatedCust,
feedbackObjectValidate, 
bindingSite, 
showPopup,
getClientDateByTimeZone, 
getAutoFeedbackSubject,
downup,
showHideDetailBox,
dropdownMultiSelect
*/

var arr = [];
var arrCust = [];
var bindingChildByParentId,
    renderRelated,
    checkDuplicateRelated,
    trimSpace,
    trimSpacesOnEditor,
    checkContainsValue,
    checkMultiSelect;

function bindingSetting(id, data, currValue) {    
    if (id === 'ddlproduct') {
        var defaultValue = { "Id": 0, "Type": 'Product', "Value": "N/A", "DisplayOrder": 0, "Child": null, "IsSub": false, "Parent": null };
        $("#tmplOption").tmpl(defaultValue).appendTo("#" + id);
    }
    
    if (data && data.length > 0) {
        $("#tmplOption").tmpl(data).appendTo("#" + id);
        
        if (currValue != null) {
            $("#" + id + " option[value='" + currValue + "']").attr("selected", "selected");
        }

        if (id === 'ddlsubcategory') {
            $('#hIsNotChild').val('0');
        }
    }
    else {
        if (id === 'ddlsubcategory') {
            showAlertMessage("errmsg",
                "There is no Subcategory for Category [" +
                    $("#ddlcategory option:selected").text() +
                "]. Please add a Subcategory or choose another Category.",
                1);
            $('#ddlcategory').val(AllSetting.Category[0].Id);
            bindingChildByParentId('ddlsubcategory', AllSetting.Category, AllSetting.Category[0].Id, 0);
            $('#hIsNotChild').val('1');
            return false;
        }
        else if (id !== 'ddlbettype') {
            $('<option value="0">N/A</option>').appendTo($("#" + id));
        }
    }
}

bindingChildByParentId = function (id, data, currValue, selectedId) {
    if (data) {
        var totalItems = data.length;
        $('#' + id).empty();
        if (id === 'ddlbettype' || data.length === 0) {
            $('<option value="0">N/A</option>').appendTo($("#" + id));
        }

        if (totalItems > 0) {
            for (var i = 0; i < totalItems; i++) {
                if (data[i].Child && data[i].Id === currValue) {
                    bindingSetting(id, data[i].Child, selectedId);
                }
            }
        }
    }
};

function formatEditBlock(user, str) {
    var currdate = new Date();
    var strreturn;
    if (str === "") {
        str = "&nbsp;";
    }

    strreturn = '<div>\
                    <div style="background-color: #FFFFD0; padding-left: 10px;border-left: 1px solid #CCCCCC">\
                        <div style="font-size: 9px;">by <span style="color: #105289; font-weight: bold; font-size: 11px;">';
    strreturn += user;
    strreturn += '</span> on ' + currdate.toLocaleString() + '</div><div style="border-top: 1px solid #CCCCCC; padding: 5px 0pt 0pt;">';
    strreturn += str + '</div></div><br></div>';
    return strreturn;
}

function toggleClass(reviewid, editid) {
    $("#" + reviewid).toggleClass("hidden");
    $("#" + editid).toggleClass("hidden");
}

function initEdit(type) {
    switch (type) {
        case 'des':
            toggleClass("btndesreview", "btndesedit");
            $("#previewboxdes").addClass("hidden");
            $("#editboxdes").removeClass("hidden");
            break;
        case 'sol':
            toggleClass("btnsolreview", "btnsoledit");
            $("#previewboxsol").addClass("hidden");
            $("#editboxsol").removeClass("hidden");
            break;
        default:
            toggleClass("btnrevreview", "btnrevedit");
            $("#previewboxrev").addClass("hidden");
            $("#editboxrev").removeClass("hidden");
    }
}

function initReview(type) {

    switch (type) {
        case 'des':
            toggleClass("btndesreview", "btndesedit");
            if ($("#desediter").length) {
                desvalue = $("#desediter").htmlarea("toHtmlString");
            }
            $("#editboxdes").addClass("hidden");
            $("#previewboxdes").html(desvalue).removeClass("hidden");
            break;
        case 'sol':
            toggleClass("btnsolreview", "btnsoledit");
            if ($("#solediter").length) {
                solvalue = $("#solediter").htmlarea("toHtmlString");
            }
            $("#editboxsol").addClass("hidden");
            $("#previewboxsol").html(solvalue).removeClass("hidden");
            break;
        default:
            toggleClass("btnrevreview", "btnrevedit");
            if ($("#revediter").length) {
                revvalue = $("#revediter").htmlarea("toHtmlString");
            }
            $("#editboxrev").addClass("hidden");
            $("#previewboxrev").html(revvalue).removeClass("hidden");
    }
}

function trim(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

function getRelatedCustomer() {
    var arrUser = [];
    var userName = "";
    $('#related td').each(function (i, td) {
        /* jshint unused: vars */
        arrUser[i] = this.id;
    });

    if (arrUser != null) {
        if (arrUser.length === 0) {
            userName = "_";
            return userName;
        }

        for (var i = 0; i < arrUser.length; i++) {
            if (arrUser[i] !== 0) {
                if (i === 0) {
                    userName = userName + arrUser[i];
                }
                else {
                    userName = userName + "," + arrUser[i];
                }
            }
            else {
                continue;
            }
        }
    }

    return userName;
}

function validateSetting(arrObj) {
    var result = true;
    $(".errormsg").each(function () {
        if ($(this).hasClass("hidden") === false) {
            $(this).addClass("hidden");
        }
    });
    for (var i = 0; i < arrObj.length; i++) {
        if ($("#" + arrObj[i]).val() === "") {
            $("#" + arrObj[i].replace("ddl", "error").replace("txt", "error")).removeClass("hidden").html("This information is required");
            $("#" + arrObj[i]).focus();
            result = false;
        }
    }
    return result;
}

function EnableUpdateReview(obj) {
    if (obj.checked) {
        $("#editreview").css("display", "");
        initEdit("rev");
    }
    else {
        $("#editreview").css("display", "none");
        initReview("rev");
    }
}

//----deleted feedbac/
function bindSetting(id, settingType, currValue) {
    var data = {
        type: settingType,
        excludeItem: ''
    };

    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/SelSettingByType",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data),
        success: function (rs) {
            if (rs != null && rs.length > 0) {
                $("#tmplOption").tmpl(rs).appendTo("#" + id);
                if (currValue != null) {
                    $("#" + id + " option[value='" + currValue + "']").attr("selected", "selected");
                }
            }
        }
    });
}

function bindingCountry(id, data, currValue) {
    if (data) {
        var defaultValue = { "Id": 0, "CountryCode": 1, "CountryName": "N/A", "DisplayOrder": 0, "IsActive": true };
        $("#tmplCountry").tmpl(defaultValue).appendTo("#" + id);

        if (data != null && data.length > 0) {
            $("#tmplCountry").tmpl(data).appendTo("#" + id);
            if (currValue != null) {
                $("#" + id + " option[value='" + currValue + "']").attr("selected", "selected");
            }
        }
        else {
            $('<option value="0">N/A</option>').appendTo($("#" + id));
        }
    }
}

function bindingManageChildByParentId(id, data, currValue, selectedId) {
    if (data) {
        $('#' + id).empty();
        if (id === 'ddlbettype' || data.length === 0) {
            $('<option value="0">N/A</option>').appendTo($("#" + id));
        }
        $('<option value="0">All</option>').appendTo($("#" + id));

        if (data.length > 0) {
            for (var i = 0; i < data.length; i++) {
                if (data[i].Id === currValue) {
                    bindingSetting(id, data[i].Child, selectedId);
                }
            }
        }
    }
}

function bindCountry(currValue) {
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetAllCountry",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '',
        success: function (rs) {
            if (rs != null && rs.length > 0) {
                $("#tmplCountry").tmpl(rs).appendTo("#ddlcountry");
                if (currValue != null) {
                    $("#ddlcountry option[value='" + currValue + "']").attr("selected", "selected");
                }
            }

        }
    });
}

function loadSubItem(obj, target, currValue) {
    if ($(obj).val() !== "") {
        if ($(obj).val() !== "0") {
            $.ajax({
                type: "POST",
                url: window.rootUrl + "AdminTool/GetSettingByParent",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                data: '{"parentId":' + $(obj).val() + '}',
                success: function (rs) {
                    $("#" + target).children().remove().end();
                    if (rs != null && rs.length > 0) {
                        $("#tmplOption").tmpl(rs).appendTo("#" + target);
                        if (currValue != null) {
                            $("#" + target + " option[value='" + currValue + "']").attr("selected", "selected");
                        }
                    }
                    else {
                        $('<option value="0">N/A</option>').appendTo($("#" + target));
                    }
                }
            });
        }
        else {
            $("#" + target).children().remove().end();
            $('<option value="0">N/A</option>').appendTo($("#" + target));
        }
    }
}

function getSubCategory(currValue, parentValue) {
    var parentId;
    if (parentValue != null) {
        parentId = parentValue;
    }
    else {
        parentId = $("#ddlcategory").val();
    }

    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetSettingByParent",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"parentId":' + parentId + '}',
        success: function (rs) {
            $("#ddlsubcategory").children().remove().end();
            if (rs != null && rs.length > 0) {
                $("#tmplOption").tmpl(rs).appendTo("#ddlsubcategory");
                if (currValue != null) {
                    $("#ddlsubcategory option[value='" + currValue + "']").attr("selected", "selected");
                }
            }
            else {
                $('<option value="0">N/A</option>').appendTo($("#ddlsubcategory"));
            }
        }
    });
}

// --- Html area --- //

function initHTMLArea(wrapperid, editerid, strvalue) {
    document.getElementById(wrapperid).innerHTML =
        '<textarea id="' + editerid +
            '" name="' + editerid +
            '" style="width:auto; height:100px; min-width:710px;width:710px;">' +
            strvalue +
        '</textarea>';
    $("#" + editerid).htmlarea({
        toolbar: [
                ["html"], ["bold", "forecolor", "italic", "underline", "strikethrough", "|", "subscript", "superscript"],
                ["increasefontsize", "decreasefontsize"],
                ["orderedlist", "unorderedlist"],
                ["indent", "outdent"],
                ["justifyleft", "justifycenter", "justifyright"],
                ["link", "unlink"], ["image", "horizontalrule"],
                ["p", "h1", "h2", "h3", "h4", "yh5", "h6"],
                ["cut", "copy", "paste"],
                 [{
                     css: "edit",
                     text: "Inset Edit Block",
                     action: function (btn) {
                         /* jshint unused: vars */
                         var s = this.getSelectedHTML();
                         s = formatEditBlock(accname, s);
                         this.pasteHTML(s);
                     }
                 }]
                  ]
    });
}

// --- /Html area --- //

// -- Calendar --- //
function SetupCalendar(arrDateValues, arrIds) {
    for (var i = 0; i < arrIds.length; i++) {
        var index = arrIds[i];
        var valueIndex = arrDateValues[i];

        $('#' + index).datetimepicker(
        {
            timeFormat: 'hh:mm TT',
            stepHour: 1,
            stepMinute: 1,
            stepSecond: 1,
            //showSecond: true,
            ampm: true
        });

        if (valueIndex != null && valueIndex !== "") {
            $('#' + index).datepicker('setDate', (new Date(valueIndex)));
        }
    }
}

function keyup(event) {
    /* jshint unused: vars */
    if ($("#txtrelatedcustomer").val() !== "") {
        $("#btnverify").removeAttr("disabled");
    }
    else {
        $("#btnverify").attr("disabled", "disabled");
    }
}

function checkIsRelated(obj) {
    if (obj.checked) {
        $("#typeCust").removeAttr("disabled");

        var arrUser;
        $('#txtrelatedcustomer').each(function () {
            arrUser = $(this).val();
        });

        if (arrUser != null && arrUser.length > 0) {
            $("#btnverify").removeAttr("disabled");
            $("#btnsubmit").attr("disabled", "disabled");
            $("#btnsubmitbottom").attr("disabled", "disabled");
        }
        $("#lblverify").removeClass("hidden");
    }
    else {
        $("#btnsubmit").removeAttr("disabled");
        $("#btnsubmitbottom").removeAttr("disabled");
        $("#typeCust").attr("disabled", "disabled");
        $("#btnverify").attr("disabled", "disabled");
        $("#lblverify").addClass("hidden");
        $("#ColorGuide").addClass("hidden");
        $("#trRelatedInfo").addClass("hidden");
    }
}

function verifyUserName() {
    var arrUser = [];
    var userName = "";
    var i;
    $('#txtrelatedcustomer option').each(function (i, option) {
        arrUser[i] = $(option).text();
    });

    if (arrUser != null) {
        for (i = 0; i < arrUser.length; i++) {
            if (i === 0) {
                userName = userName + arrUser[i];
            }
            else {
                userName = userName + "," + arrUser[i];
            }
        }
    }

    if ($("#chkisrelated").attr("checked") && userName !== "") {
        $("#lblverify").addClass("smallloading").removeClass("hidden");
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Customer/VerifyUserName",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"UserName":"' + userName + '"}',
            success: function (rs) {
                if (rs != null && rs.length > 0) {
                    var userId = '';
                    for (i = 0; i < rs.length; i++) {
                        if (parseInt(rs[i].CustId) !== 0) {
                            userId += (rs[i].CustId + (i === rs.length - 1 ? '' : ","));
                        }
                    }
                }

                $('#txtrelatedcustomer option').each(function (i, option) {
                    $(option).remove();
                });
                $('#holder li').each(function (i, option) {
                    if (this.id !== 'txtrelatedcustomer_annoninput') {
                        $(option).remove();
                    }
                });

                var dup = checkDuplicateRelated();
                if (dup && dup.length > 0) {
                    $("#divVerifyDuplicate").html(
                        '<span style="font-weight:bold; font-size: 8pt;">Exist UserName(' +
                            dup +
                        ') is both IBCCustomer and Guest<br/>Please choose the UserName you want update.</span> ');
                    $("#divVerifyDuplicate").dialog({ width: 500, height: 120, zIndex: 900, resizable: false, modal: true,
                        title: 'Warning',
                        buttons: [{
                            text: "Ok",
                            click: function () {
                                $(this).dialog("close");
                            }
                        }, ]
                    });

                }
                $("#btnsubmit").removeAttr("disabled");
                $("#btnsubmitbottom").removeAttr("disabled");
                for (i = 0; i < rs.length; i++) {
                    arr.push(renderRelated(rs[i]));
                    arrCust.push(rs[i].CustId);
                    $("#related").html(arr.join(''));
                }

                if ($("#related td").length === 0) {
                    $("#ColorGuide").addClass("hidden");
                    $("#trRelatedInfo").addClass("hidden");
                }
                else {
                    $("#ColorGuide").removeClass("hidden");
                    $("#trRelatedInfo").removeClass("hidden");
                }

            },
            error: function () { },
            complete: function () {
                $("#lblverify").removeClass("smallloading").removeClass("hidden");
            }
        });
    }
}

renderRelated = function (row) {
    var userName = "";
    $('#related td').each(function (i, td) {
        /* jshint unused: vars */
        arrCust[i] = this.id;
    });

    if (arrCust != null && arrCust.length > 0) {
        for (var i = 0; i < arrCust.length; i++) {
            if (row.CustId === arrCust[i] && row.CustId !== 0) {
                userName = userName + arrCust[i];
                return null;
            }
        }
    }

    var html;
    if (row.CustId !== 0) {
        if (row.CustId > 0) {
            html = '<tr>\
                        <td id =' + row.CustId + ' value= ' + row.UserName + ' style="border: 0px;">\
                                <a rel="friend" class="txt-customer">\
                                    <span onclick="DeleteRelatedCust(' + row.CustId + ');" class="icon-del"></span>' + row.UserName + '</a>\
                         </td>\
                     </tr>';
        }
        else {
            html = '<tr>\
                        <td id =' + row.CustId + ' value= ' + row.UserName + ' style="border: 0px;">\
                                <a rel="friend" class="txt-guest">\
                                    <span onclick="DeleteRelatedCust(' + row.CustId + ');" class="icon-del"></span>' + row.UserName + '</a>\
                        </td>\
                    </tr>';
        }
    }
    else {
        html = '<tr class="invalidcustomers">\
                    <td id =' + row.CustId + ' value= ' + row.UserName + ' style="border: 0px;">\
                            <a rel="friend" class="txt-invalid">\
                                <span onclick="deleteAllInValidCustomers();" class="icon-del"></span>' + row.UserName + '</a>\
                    </td>\
                </tr>';
    }

    return html;
};

function deleteAllInValidCustomers() {
    var length = arrCust.length;
    var arrCustPos = [];
    var arrHtmlPos = [];

    $('.invalidcustomers').remove();
    for (var i = 0; i < length; i++) {
        if (arrCust[i] !== 0) {
            arrCustPos.push(arrCust[i]);
            arrHtmlPos.push(arr[i]);
        }
    }

    arr = [];
    arr = arrHtmlPos;
    arrCust = [];
    arrCust = arrCustPos;
}

function DeleteRelatedCust(custId) {

    $("#" + custId).parent().remove();

}
function feedbackObjectValidate(editorObj, otherObj) {
    var result = true;

    $(".errormsg").each(function () {
        if ($(this).hasClass("hidden") === false) {
            $(this).addClass("hidden");
        }
    });
    //---editor

    for (var i = 0; i < editorObj.length; i++) {
        if (trimSpace(trimSpacesOnEditor($("#" + editorObj[i]).htmlarea("toHtmlString"))) === "") {
            $("#" + editorObj[i].replace("editer", "editererror")).removeClass("hidden").html("This information is required");
            $("#" + editorObj[i]).focus();
            result = false;
        }
    }
    //----other
    for (var j = 0; j < otherObj.length; j++) {
        if ($("#" + otherObj[j]).val() === "") {
            $("#" + otherObj[j].replace("ddl", "error").replace("txt", "error")).removeClass("hidden").html("This information is required");
            $("#" + otherObj[j]).focus();
            result = false;
        }
    }
    //-------------
    return result;
}

function bindingSite(id, rs, currentValue) {
    var creditArr = [];
    var licenseeArr = [];

    creditArr.push('<optgroup label="Credit">');
    licenseeArr.push('<optgroup label="Licensee">');
    if (rs) {
        for (var i = 0; i < rs.length; i++) {
            var currentSite = rs[i];
            if (currentSite != null) {
                if (currentSite.SiteType === 0) {
                    creditArr.push('<option value="' + currentSite.SiteId + '">' + currentSite.SiteName + '</option>');
                }
                else if (currentSite.SiteType === 1) {
                    licenseeArr.push('<option value="' + currentSite.SiteId + '">' + currentSite.SiteName + '</option>');
                }
            }
        }
    }

    creditArr.push('</optgroup>');
    licenseeArr.push('</optgroup>');

    var siteArr = [];

    siteArr.push('<option value="0">Select</option>');
    siteArr.push(creditArr);
    siteArr.push(licenseeArr);
    $('#' + id).append(siteArr.join(''));

    $("#" + id + " option[value='" + currentValue + "']").attr("selected", "selected");
}

function dropdownMultiSelect(prefixId, elementId, result, defaultValues) {
    /* jshint unused: vars */
    var dropdownLanguage = [];
    var optionLength = result ? result.length : 0;
    if (optionLength > 0) {
        for (var i = 0; i < optionLength; i++) {
            var info = result[i];
            dropdownLanguage.push({
                id: prefixId + info.Id,
                value: info.Id,
                text: info.Value,
                checked: checkMultiSelect(info.Id, defaultValues),
                enable: true,
                cssClass: "",
                items: []
            });
        }
    }

    /* jshint camelcase: false */
    var dropdown = $('#' + elementId);
    dropdown.fanexMultiSelect(
        {
            minWidth: 171,
            maxHeight: 450,
            shadow: true,
            showHeader: true,
            isCheckAll: false,
            showFooter: false,
            showTooltip: false,
            checkAllText: 'All Languages',
            isUsedCustomCheckAllText: true,
            customCheckAllText: 'All Languages',
            noItemText: 'N/A',
            noneSelectedText: 'N/A',
            selectedList: 3,
            listData: dropdownLanguage
        });

    //element.find("#customerinfodetail-languageoptions .fanex-multiselect-state-default").attr("tabindex", 29);
}

checkMultiSelect = function (id, defaultValues) {
    var length = defaultValues ? defaultValues.length : 0;
    for (var i = 0; i < length; i++) {
        if (id === defaultValues[i]) {
            return true;
        }
    }

    return false;
};

function showPopup(obj, content) {

    obj.ShowBubblePopup({
        innerHtml: content,
        position: 'bottom',
        align: 'left',
        zIndex: 100,
        width: 'auto',
        tail: { align: 'left' },
        themeName: 'black',
        themePath: '../../Scripts/Bubblepopup/images/jquerybubblepopup-theme',
        alwaysVisible: false,
        closingDelay: 10
    });
}

checkDuplicateRelated = function () {
    var arrUser = [];
    var userName = "";
    $('#txtrelatedcustomer option').each(function (i, option) {
        arrUser[i] = $(option).text();
    });

    if (arrUser != null) {
        for (var i = 0; i < arrUser.length; i++) {
            if (i === 0) {
                userName = userName + arrUser[i];
            }
            else {
                userName = userName + "," + arrUser[i];
            }

        }
    }
    var isRelated = $('#chkisrelated');
    var relatedVal = arrUser;

    var dups = [];
    var items = [];

    if (isRelated && isRelated.attr("checked") && relatedVal) {

        for (var j = 0; j < relatedVal.length; j++) {
            if (!checkContainsValue(items, relatedVal[j])) {
                items.push(relatedVal[j]);
            }
            else {
                dups.push(relatedVal[j]);
            }
        }
        return dups;
    }
};

trimSpace = function (str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
};

trimSpacesOnEditor = function (str) {
    var newContent = str.replace(/(&nbsp;)|(<[^<>]+>)/g, "");
    return newContent;
};

checkContainsValue = function (a, e) {
    for (var j = 0; j < a.length; j++) {
        if (trimSpace(a[j].toLowerCase()) === trimSpace(e.toLowerCase())) {
            return true;
        }
    }

    return false;
};

function getClientDateByTimeZone(clientTimeZone) {
    var myDate = new Date(); // Set this to your date in whichever timezone.
    var utcDate = myDate.toUTCString();
    return new Date(utcDate + (3600000 * clientTimeZone));
}

function getAutoFeedbackSubject() {

    var category = $("#ddlcategory option:selected").text();
    var subcategory = $("#ddlsubcategory option:selected").text();
    $('#txtsubject').val(category + ' - ' + subcategory);

}

function showHideDetailBox(isShow, detailBoxid) {
    if (isShow) {
        $('.' + detailBoxid).removeClass('hidden');
    }
    else {
        $('.' + detailBoxid).addClass('hidden');
    }
}