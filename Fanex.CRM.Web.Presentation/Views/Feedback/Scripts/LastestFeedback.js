"use strict";
/* global OpenPopup, reRenderGrid, head */
/* exported _pageIndex, _pageSize, totalItems,
HideTooltip, openEditFeedback */

//Contain the template
var _pageIndex = 1;
var _pageSize = 20;
var totalItems = 0;
var maxHeightOfPopUp = ($(window).height() + 50);
var maxWidthOfPopUp = 987;

var topFeedback;

function tooltip(i) {
    var id = "tooltip" + i;
    var desc = "Description" + i;
    $("#" + id).tipsy(
        { fade: true, gravity: "nw", html: true, title: function () { return document.getElementById(desc).innerHTML; } });

}
function HideTooltip(i) {
    var tooltip = "tooltip" + i;
    $("#" + tooltip).tipsy("hide");
}

function openEditFeedback(url, type) {
    /* jshint unused: vars */
    try {
        $('.desTooltip').tipsy('hide');

        parent.OpenPopup(url, "Update Feedback", maxWidthOfPopUp, maxHeightOfPopUp, true, false, window, 1000, function () {
            topFeedback.loadLastedFeedback(false);
        }, function () {
        });
    }
    catch (e) {
        /* jshint newcap: false */
        OpenPopup(url, "Update Feedback", maxWidthOfPopUp, maxHeightOfPopUp, true, false, window, 1000, function () {
            topFeedback.loadLastedFeedback(false);
        }, function () {
        });
    }
}

topFeedback = {
    totalRows: 0,
    pageIndex: 1,
    pageSize: 100,
    selectTop: 10,
    loadLastedFeedback: function (loadOldData) {
        if (!loadOldData) {
            topFeedback.pageIndex = 1;
        }
        topFeedback.selectTop = $('#selectTopFeedback').val();
        $('#divDescriptions').empty();

        var data =
        {
            'pageIndex': topFeedback.pageIndex,
            'pageSize': topFeedback.pageSize,
            'fromCache': loadOldData,
            'selectTop': topFeedback.selectTop
        };

        try { parent.BlockUI(); } catch (e) { }
        var mainreport = $("#lastedfeedbackgrid");
        $('#errmsg').html('');

        $('#gridtitle').html('Top ' + topFeedback.selectTop + ' Feedback');
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Feedback/SelTopFeedback",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs != null && rs.rows != null && rs.rows.length > 0) {
                    var arr = [];
                    var arrLeft = [];
                    topFeedback.totalRows = parseInt(rs.total);
                    for (var i = 0; i < rs.rows.length; i++) {
                        arr.push(topFeedback.renderFeedbackRow(rs.rows[i], i));
                        arrLeft.push(topFeedback.renderFeedbackLeftRow(rs.rows[i], i));
                    }

                    $("#latestfeedbackbody tbody").empty().append(arr.join(''));
                    $('#latestfeedbackbodyHeader tbody').empty().append(arrLeft.join(''));

                    var gridOpts = {
                        tblContentId: 'latestfeedbackbody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: 55,
                        freezeWidth: 400,
                        numberFreezeCol: 1
                    };
                    mainreport.removeClass("hidden").gridScroller(gridOpts);
                    reRenderGrid('lastedfeedbackgrid', 'latestfeedbackbodyHeader', 'latestfeedbackbody', 55, 400, 1, 400);

                    var getSmallestDate = new Date(topFeedback.getMinDate(rs.rows));
                    $('#txtfromdate').datepicker("setDate", getSmallestDate);
                    //----------------------------------------

                    $('#latestfeedbackbody tbody tr').each(function () {
                        tooltip($(this).find("td:first a").attr('id').replace('tooltip', ''));
                    });
                    //----------------------------------------

                }
                else {
                    mainreport.addClass('hidden');
                    $('#errmsg').html('<div class="alertboxerr"><div>There is no information.</div></div>');
                }

            },
            error: function () {
                mainreport.addClass('hidden');
                $('#errmsg').html('<div class="alertboxerr"><div>There is no information.</div></div>');
            },
            complete: function () {

                try { parent.UnBlockUI(); } catch (e) { }
            }
        });
    },
    renderFeedbackRow: function (row, i) {
        var trStyle = '';
        if (row.Status != null) {
            trStyle = row.Status.toLowerCase() === 'pending' ? 'pending' : row.Status.toLowerCase() === 'closed' ? 'closed' : '';
        }

        var html = '<tr class="' + trStyle + '">\
                       <td><div style="width:400px">\
                                <div class="lnksubject"><a class="desTooltip" id = "tooltip' + i + '"  href="JavaScript:void(0);"\
                                onclick="openEditFeedback(\'../Feedback/EditFeedback?id=' + row.ID + '&rep=2\', 1);HideTooltip(' + i + ');" >\
                                <b>' + row.Subject + '</b></a></div>\
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Feedback date <b>' + row.FeedbackDate + '</b><br>\
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;By <b>' + row.Creator + '</b> on ' + row.CreateDate + '<br>\
                                ' + topFeedback.durationTimeFormat(row.DurationTime) + '<br>\
                                ' + topFeedback.reviewerFormat(row.Reviewer, row.ReviewDate) + '\
                            </div></td>\
                        <td><div style="width:220px">' + row.MainCategory + '<br/>&nbsp;&nbsp;&nbsp;' + row.SubCategory + '</div></td>\
                        <td><div class="c" style="width:50px">&nbsp;' + row.Status + '</div></td>\
                        <td><div class="c" style="width:260px">&nbsp;' + row.RelatedUsername + '</div></td>\
                </tr>';
        return html;
    },
    renderFeedbackLeftRow: function (row, i) {
        var trStyle = '';
        if (row.Status != null) {
            trStyle = row.Status.toLowerCase() === 'pending' ? 'pending' : row.Status.toLowerCase() === 'closed' ? 'closed' : '';
        }

        $('#divDescriptions').append('<div id = "Description' + i + '" class="hidden des">' + row.Description + '</div>');

        var html = '<tr class="' + trStyle + '">\
                       <td><div style="width:400px">\
                                <div class="lnksubject"><a class="desTooltip" id = "tooltip' + i + '"  href="JavaScript:void(0);"\
                                onclick="openEditFeedback(\'../Feedback/EditFeedback?id=' + row.ID + '&rep=2\', 1);HideTooltip(' + i + ');" >\
                                <b>' + row.Subject + '</b></a></div>\
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Feedback date <b>' + row.FeedbackDate + '</b><br>\
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;By <b>' + row.Creator + '</b> on ' + row.CreateDate + '<br>\
                                ' + topFeedback.durationTimeFormat(row.DurationTime) + '<br>\
                                ' + topFeedback.reviewerFormat(row.Reviewer, row.ReviewDate) + '\
                            </div></td>\
                </tr>';
        return html;
    },
    reviewerFormat: function (reviewer, reviewDate) {
        var html = '';
        if (reviewer !== "") {
            html = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Reviewed by <b>' + reviewer + '</b> on ' + reviewDate + '<br>';
        }
        return html;
    },
    durationTimeFormat: function (time) {
        var html = '';
        if (time) {
            html = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Solving Duration: " + time;
        }
        return html;
    },
    getMinDate: function (arr) {

        if (arr != null) {
            var minDate = new Date(arr[0].FeedbackDate);
            for (var i = 1; i < arr.length; i++) {
                var currentDate = new Date(arr[i].FeedbackDate);
                if (currentDate < minDate) {
                    minDate = currentDate;
                }
            }

            return minDate;
        }

        return new Date();
    }
};

head.ready(function () {
    var gridOpts = {
        tblContentId: 'latestfeedbackbody',
        isShowPager: false
    };
    $("#lastedfeedbackgrid").initGrid(gridOpts);

    //try { topwnd = findMainWindow(window); } catch (e) { }
    topFeedback.loadLastedFeedback(false);

    $('body').mousedown(function (e) {
        if (e.button === 1) {
            return false;
        }
    });
});