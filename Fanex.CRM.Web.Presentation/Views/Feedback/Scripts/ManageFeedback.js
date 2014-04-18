"use strict";
/* global OpenPopup, reRenderGrid, replaceSpaceForIE9, bindingSetting,
gup, head, addDays,
bindingManageChildByParentId, clientTime */
/* exported loadSubItem, openEditFeedback, isNumberKey */

//Contain the template
var AllSetting;
var maxHeightOfPopUp = ($(window).height() + 50);
var maxWidthOfPopUp = 987;
var lastedFeedback;

// -- Calendar --- //
function setupCalendar(arrDateValues, arrIds) {
    var config = {
        dateFormat: 'M dd, yy',
        changeMonth: true,
        changeYear: true,
        showOtherMonths: true,
        selectOtherMonths: true
    };

    for (var i = 0; i < arrIds.length; i++) {
        if (arrDateValues[i]) {
            var datepickerElement = $('#' + arrIds[i]);
            datepickerElement.datepicker(config);
            datepickerElement.datepicker('setDate', arrDateValues[i]);
        }
    }
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
                        $('<option value="0">All</option>').appendTo($("#" + target));
                        $("#tmplOption").tmpl(rs).appendTo("#" + target);
                        if (currValue != null) {
                            $("#" + target + " option[value='" + currValue + "']").attr("selected", "selected");
                        }
                    }
                    else {
                        $('<option value="0">All</option>').appendTo();
                    }
                },
                error: function () { }
            });
        }
        else {
            $("#" + target).children().remove().end();
            $('<option value="0">All</option>').appendTo($("#" + target));
        }
    }
}

function bindingEvent() {
    $('#ddlcategory').change(function () {
        bindingManageChildByParentId('ddlsubcategory', AllSetting.Category, parseInt($('#ddlcategory').val()));
    });
}

function openEditFeedback(url) {
    try {
        parent.OpenPopup(url, "Update Feedback", maxWidthOfPopUp, maxHeightOfPopUp, true, false, window, 2000, function () {
            lastedFeedback.loadLastedFeedback(false);
        });
    }
    catch (e) {
        /* jshint newcap: false */
        OpenPopup(url, "Update Feedback", maxWidthOfPopUp, maxHeightOfPopUp, true, false, window, 2000, function () {
            lastedFeedback.loadLastedFeedback(false);
        });
    }
}

function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if ((charCode >= 48 && charCode <= 57) || 
        (charCode === 8 || charCode === 35 || charCode === 36 || charCode === 37 || charCode === 39 || charCode === 46)) {
        return true;
    }
    
    return false;
}

var pagerOpts = {
    /* jshint camelcase: false */
    callback: null,
    start_per_page: 100,
    step_per_page: 100,
    max_items_per_page: 500,
    items_per_page: 100,
    current_page: 1
};

lastedFeedback = {
    totalRows: 0,
    pageIndex: 1,
    pageSize: 100,
    fDate: "",
    tDate: "",
    runningNumber: "",
    userName: "",
    isGuest: false,
    communicationId: 0,
    mainCategoryId: 0,
    subCategoryId: 0,
    statusId: 0,
    isReviewd: false,
    responseId: 0,
    gridHeight: 195,
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = lastedFeedback.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;
        $("#pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        /* jshint unused: vars */
        var max_page = Math.ceil(lastedFeedback.totalRows / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        lastedFeedback.pageIndex = page_index + 1;
        lastedFeedback.pageSize = _items_per_page;
        lastedFeedback.loadLastedFeedback(true);
        return false;
    },
    loadLastedFeedback: function (loadOldData) {
        if (!loadOldData) {
            lastedFeedback.pageIndex = 1;
        }

        lastedFeedback.fDate = $('#txtfromdate').val();
        lastedFeedback.tDate = $('#txttodate').val();
        lastedFeedback.runningNumber = $('#runningnumber').val().trim();
        lastedFeedback.userName = $('#txtusername').val().trim();
        lastedFeedback.isGuest = parseInt($("#ddlguest").val());
        lastedFeedback.communicationId = parseInt($('#ddlcommunication').val());
        lastedFeedback.mainCategoryId = parseInt($('#ddlcategory').val());
        lastedFeedback.subCategoryId = parseInt($('#ddlsubcategory').val());
        lastedFeedback.statusId = parseInt($('#ddlstatus').val());
        lastedFeedback.isReviewd = parseInt($('#ddlIsReviewed').val());
        lastedFeedback.responseId = parseInt($('#ddlCustomerReponse').val());

        var data =
        {
            'fromDate': lastedFeedback.fDate,
            'toDate': lastedFeedback.tDate,
            'runningNumber': lastedFeedback.runningNumber,
            'UserName': lastedFeedback.userName,
            'IsGuest': lastedFeedback.isGuest,
            'CommunicationID': lastedFeedback.communicationId,
            'MainCategoryID': lastedFeedback.mainCategoryId,
            'SubCategoryID': lastedFeedback.subCategoryId,
            'StatusID': lastedFeedback.statusId,
            'IsReviewed': lastedFeedback.isReviewd,
            'pageIndex': lastedFeedback.pageIndex,
            'pageSize': lastedFeedback.pageSize,
            'fromCache': loadOldData,
            'responseId': lastedFeedback.responseId
        };

        try { parent.BlockUI(); } catch (e) { }
        var mainreport = $("#lastedfeedbackgrid");
        $('#errmsg').html('');

        $.ajax({
            type: "POST",
            url: window.rootUrl + "Feedback/GetFeedback",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs != null && rs.rows != null && rs.rows.length > 0) {
                    var arr = [];
                    var arrLeft = [];
                    lastedFeedback.totalRows = parseInt(rs.total);
                    for (var i = 0; i < rs.rows.length; i++) {
                        arr.push(lastedFeedback.renderFeedbackRow(rs.rows[i]));
                        arrLeft.push(lastedFeedback.renderFeedbackLeftRow(rs.rows[i]));
                    }

                    $("#latestfeedbackbody tbody").empty().append(arr.join(''));
                    $("#latestfeedbackbodyHeader tbody").empty().append(arrLeft.join(''));

                    var gridOpts = {
                        tblContentId: 'latestfeedbackbody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: lastedFeedback.gridHeight,
                        freezeWidth: 380,
                        numberFreezeCol: 1
                    };
                    mainreport.removeClass("hidden").gridScroller(gridOpts);

                    lastedFeedback.initPager(lastedFeedback.totalRows, lastedFeedback.pageSize, lastedFeedback.pageIndex - 1);
                    reRenderGrid('lastedfeedbackgrid', 'latestfeedbackbodyHeader', 'latestfeedbackbody', lastedFeedback.gridHeight, 380, 1, 380);
                }
                else {
                    mainreport.addClass('hidden');
                    $('#errmsg').html('<div class="alertboxerr"><div>There is no information.</div></div>');
                    $('#pagination').html('&nbsp;');
                }
            },
            error: function () {
                mainreport.addClass('hidden');
                $('#errmsg').html('<div class="alertboxerr"><div>There is no information.</div></div>');
                $('#pagination').html('&nbsp;');
            },
            complete: function () {
                try { parent.UnBlockUI(); } catch (e) { }
            }
        });
    },
    renderFeedbackRow: function (row) {
        var trStyle = '';
        if (row.Status != null) {
            trStyle = row.Status.toLowerCase() === 'pending' ? 'pending' : row.Status.toLowerCase() === 'closed' ? 'closed' : '';
        }
        var html = '<tr class="' + trStyle + '">\
                        <td>\
                            <div style="width:380px">\
                                <div class="lnksubject">\
                                    <a class="desTooltip" href="JavaScript:void(0);">\
                                        <b>' + row.Subject + '</b>\
                                    </a>\
                                </div>\
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Feedback date&nbsp;<b>' + row.FeedbackDate + '</b>\
                            <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;By&nbsp;<b>' + row.Creator + '</b>\
                            &nbsp;on&nbsp;' + row.CreateDate +
                            '<br>' + lastedFeedback.durationTimeFormat(row.DurationTime) +
                            '<br>' + lastedFeedback.reviewerFormat(row.Reviewer, row.ReviewDate) +
                            '</div>\
                        </td>\
                        <td><div style="width:130px">' + row.Username + '</div></td>\
                        <td><div style="width:270px"><div class="main-category">' + row.MainCategory +
                            '</div><span class="sub-category">' + row.SubCategory + '</span></div></td>\
                        <td><div class="c" style="width:50px">' + row.Status + '</div></td>\
                        <td><div style="width:130px">' + row.RelatedUsername + '</div></td>\
                </tr>';
        return replaceSpaceForIE9(html);
    },
    renderFeedbackLeftRow: function (row) {
        var trStyle = '';
        if (row.Status != null) {
            trStyle = row.Status.toLowerCase() === 'pending' ? 'pending' : row.Status.toLowerCase() === 'closed' ? 'closed' : '';
        }
        var html = '<tr class="' + trStyle + '">\
                       <td>\
                            <div style="width:380px">\
                                <div class="lnksubject">\
                                    <a class="desTooltip" href="JavaScript:void(0);" onclick="openEditFeedback(\'../Feedback/EditFeedback?id=' +
                                        row.ID + '&rep=2\', 1);" >\
                                        <b>' + row.Subject + '</b>\
                                    </a>\
                                </div>\
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Feedback date&nbsp;<b>' + row.FeedbackDate + '</b>\
                                <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;By&nbsp;<b>' + row.Creator + '</b> on ' + row.CreateDate +
                                '<br>' + lastedFeedback.durationTimeFormat(row.DurationTime) +
                                '<br>' + lastedFeedback.reviewerFormat(row.Reviewer, row.ReviewDate) +
                            '</div>\
                        </td>\
                </tr>';
        return html;
    },
    reviewerFormat: function (reviewer, reviewDate) {
        var html = '';
        if (reviewer !== "") {
            html = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Reviewed by&nbsp;<b>' + reviewer + '</b>&nbsp;on&nbsp;' + reviewDate + '<br>';
        }
        return html;
    },
    durationTimeFormat: function (time) {
        var html = '';
        if (time) {
            html = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Solving Duration:&nbsp;" + time;
        }
        return html;
    },
    loadManageFeedbackSettings: function () {
        $.ajax({
            type: "POST",
            url: window.rootUrl + "AdminTool/SelAllSetting",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '{"Id":' + 0 + '}',
            success: function (setting) {
                AllSetting = setting;
                bindingSetting("ddlcommunication", setting.Communication, 0);
                bindingSetting("ddlcategory", setting.Category, 0);
                bindingSetting("ddlstatus", setting.Status, 0);
                bindingSetting("ddlCustomerReponse", setting.Response, 0);

                $("select#ddlstatus option[value='11004']").remove();

                bindingEvent();

            },
            error: function () { }
        });
    }
};

head.ready(
    function () {
        try {
            parent.BlockUI();
            parent.UnBlockUI();
        } catch (e) { }

        var tdate = new Date(clientTime);
        var fdate = addDays(tdate, -1);
        setupCalendar([fdate, tdate], ['txtfromdate', 'txttodate']);

        var gridOpts = {
            tblContentId: 'latestfeedbackbody',
            isShowPager: true
        };
        $("#lastedfeedbackgrid").initGrid(gridOpts);

        // try { topwnd = findMainWindow(window); } catch (e) { }
        lastedFeedback.loadManageFeedbackSettings();

        var username = $.URLDecode(gup("custName"));
        var $txtUsername = $('#txtusername');
        var $ddlGuest = $('#ddlguest');
        if (username && username.trim().length !== 0) {
            $txtUsername.val(username);
            $ddlGuest.val($.URLDecode(gup("isGuest")));
        }
        else {
            $txtUsername.removeAttr('disabled');
            $ddlGuest.removeAttr('disabled');
        }

        try { parent.UnBlockUI(); } catch (e) { }
        $('body').mousedown(function (e) {
            if (e.button === 1) {
                return false;
            }
        });
    }
);
