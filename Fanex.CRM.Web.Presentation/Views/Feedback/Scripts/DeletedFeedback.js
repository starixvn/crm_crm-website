"use strict";
/* global reRenderGrid,
replaceSpaceForIE9,
head,
addDays,
bindSetting
*/
/* exported loadSubItem,
openEditFeedback */
var deletedFeedback;

//Contain the template
var maxHeightOfPopUp = ($(window).height() + 50);
var maxWidthOfPopUp = 987;

// -- Calendar --- //
function SetupCalendar(arrDateValues, arrIds) {
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
                        $('<option value="0">All</option>').appendTo($("#" + target));
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

function openEditFeedback(url) {
    try {
        parent.OpenPopup(url, "Update Feedback", maxWidthOfPopUp, maxHeightOfPopUp, true, false, window, 2000, function () {
            deletedFeedback.loadDeletedFeedback(false);
        });
    } catch (e) { }
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

deletedFeedback = {
    totalRows: 0,
    pageIndex: 1,
    pageSize: 100,
    fDate: "",
    tDate: "",
    userName: "",
    isGuest: false,
    communicationId: 0,
    mainCategoryId: 0,
    subCategoryId: 0,
    statusId: 0,
    isReviewd: false,
    responseId: 0,
    gridHeight: 158,
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = deletedFeedback.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;
        $("#pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        /* jshint unused: vars */
        var max_page = Math.ceil(deletedFeedback.totalRows / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        deletedFeedback.pageIndex = page_index + 1;
        deletedFeedback.pageSize = _items_per_page;
        deletedFeedback.loadDeletedFeedback(true);
        return false;
    },
    loadDeletedFeedback: function (loadOldData) {
        if (!loadOldData) {
            deletedFeedback.pageIndex = 1;
        }

        deletedFeedback.fDate = $('#txtfromdate').val();
        deletedFeedback.tDate = $('#txttodate').val();
        deletedFeedback.userName = $('#txtusername').val().trim();
        deletedFeedback.isGuest = parseInt($("#ddlguest").val());
        deletedFeedback.communicationId = parseInt($('#ddlcommunication').val());
        deletedFeedback.mainCategoryId = parseInt($('#ddlcategory').val());
        deletedFeedback.subCategoryId = parseInt($('#ddlsubcategory').val());
        deletedFeedback.statusId = parseInt($('#ddlstatus').val());
        deletedFeedback.isReviewd = parseInt($('#ddlIsReviewed').val());
        deletedFeedback.responseId = parseInt($('#ddlCustomerReponse').val());

        var data =
        {
            'fromDate': deletedFeedback.fDate,
            'toDate': deletedFeedback.tDate,
            'userName': deletedFeedback.userName,
            'isGuest': deletedFeedback.isGuest,
            'communicationID': deletedFeedback.communicationId,
            'mainCategoryID': deletedFeedback.mainCategoryId,
            'subCategoryID': deletedFeedback.subCategoryId,
            'isReviewed': deletedFeedback.isReviewd,
            'pageIndex': deletedFeedback.pageIndex,
            'pageSize': deletedFeedback.pageSize,
            'fromCache': loadOldData,
            'responseId': deletedFeedback.responseId
        };
        try { parent.BlockUI(); } catch (e) { }
        var mainreport = $("#lastedfeedbackgrid");
        $('#errmsg').html('');
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Feedback/GetDeletedFeedback",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs != null && rs.rows != null && rs.rows.length > 0) {
                    var arr = [];
                    var arrLeft = [];
                    deletedFeedback.totalRows = parseInt(rs.total);
                    var css = 'bgodd';
                    for (var i = 0; i < rs.rows.length; i++) {
                        css = (i % 2) > 0 ? 'bgodd' : 'bgeven';
                        arr.push(deletedFeedback.renderFeedbackRow(rs.rows[i], css));
                        arrLeft.push(deletedFeedback.renderFeedbackLeftRow(rs.rows[i], css));
                    }

                    $("#latestfeedbackbody tbody").empty().append(arr.join(''));
                    $("#latestfeedbackbodyHeader tbody").empty().append(arrLeft.join(''));

                    var gridOpts = {
                        tblContentId: 'latestfeedbackbody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: deletedFeedback.gridHeight,
                        freezeWidth: 380,
                        numberFreezeCol: 1
                    };
                    mainreport.removeClass("hidden").gridScroller(gridOpts);

                    deletedFeedback.initPager(deletedFeedback.totalRows, deletedFeedback.pageSize, deletedFeedback.pageIndex - 1);
                    reRenderGrid('lastedfeedbackgrid', 'latestfeedbackbodyHeader', 'latestfeedbackbody', deletedFeedback.gridHeight, 380, 1, 380);
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
    renderFeedbackRow: function (row, css) {

        var html = '<tr class="' + css + '">\
                        <td>\
                            <div style="width:380px"><div class="lnksubject">\
                                <a class="desTooltip" href="JavaScript:void(0);"<b>' + row.Subject + '</b></a>\
                            </div>\
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Feedback date&nbsp;<b>' + row.FeedbackDate + '</b>\
                            <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;By&nbsp;<b>' + row.Creator + '</b>\
                            &nbsp;on&nbsp;' + row.CreateDate +
                            '<br>' + deletedFeedback.reviewerFormat(row.Reviewer, row.ReviewDate) +
                            '</div>\
                        </td>\
                        <td><div class="c" style="width:130px">&nbsp;' + row.Username + '</div></td>\
                        <td><div style="width:200px">' + row.MainCategory + '<br/>&nbsp;&nbsp;&nbsp;' + row.SubCategory + '</div></td>\
                        <td><div class="c" style="width:50px">&nbsp;' + row.Status + '</div></td>\
                        <td><div class="c" style="width:200px">&nbsp;' + row.RelatedUsername + '</div></td>\
                </tr>';
        return replaceSpaceForIE9(html);
    },
    renderFeedbackLeftRow: function (row, css) {

        var html = '<tr class="' + css + '">\
                       <td>\
                            <div style="width:380px">\
                                <div class="lnksubject">\
                                    <a class="desTooltip" href="JavaScript:void(0);" onclick="openEditFeedback(\'../Feedback/EditFeedback?id=' +
                                        row.ID + '&rep=2\', 1);" >\
                                        <b>' + row.Subject + '</b>\
                                    </a>\
                                </div>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Feedback date&nbsp;<b>' + row.FeedbackDate + '</b>\
                                        <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;By&nbsp;<b>' + row.Creator + '</b>\
                                        &nbsp;on&nbsp;' + row.CreateDate +
                                        '<br>' + deletedFeedback.reviewerFormat(row.Reviewer, row.ReviewDate) +
                            '</div>\
                        </td>\
                    </tr>';
        return html;
    },
    reviewerFormat: function (reviewer, reviewDate) {
        var html = '';
        if (reviewer !== "") {
            html = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Reviewed&nbsp;by&nbsp;<b>' + reviewer + '</b>&nbsp;on&nbsp;' + reviewDate + '<br>';
        }
        return html;
    }
};

head.ready(function () {
    var tdate = new Date(clientTime);
    var fdate = addDays(new Date(), -1);
    /* jshint newcap: false */
    SetupCalendar([fdate, tdate], ['txtfromdate', 'txttodate']);

    var gridOpts = {
        tblContentId: 'latestfeedbackbody',
        isShowPager: true
    };
    $("#lastedfeedbackgrid").initGrid(gridOpts);

//    try { topwnd = findMainWindow(window); } catch (e) { }
    bindSetting("ddlcommunication", "Communication", 0);
    bindSetting("ddlcategory", "MainCategory", 0);
    bindSetting("ddlCustomerReponse", "Response", 0);

    try { parent.UnBlockUI(); } catch (e) { }

    $('body').mousedown(function (e) {
        if (e.button === 1) {
            return false;
        }
    });
});