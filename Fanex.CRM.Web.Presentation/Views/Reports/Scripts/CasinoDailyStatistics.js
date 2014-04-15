"use strict";
/* global replaceSpecialHtmlTag, replaceSpaceForIE9, emailListValidate, head,
fromDate, toDate */
/* exported arrWidth, maxHeightOfPopUp, maxWidthOfPopUp,
addDays */
/* jshint unused: vars */

var arrWidth = [];
var maxHeightOfPopUp = ($(window).height() + 50);
var maxWidthOfPopUp = ($(window).width());
var casinoDailyStatistics, casinoDailyDetail;
var crmChart;

//add n number of days
function addDays(startDate, n) {
    var endDate = new Date();
    endDate.setTime(startDate.getTime() + n * 86400000);
    return endDate;
}

var pagerOpts = {
    /*jshint camelcase: false */
    callback: null,
    start_per_page: 50,
    step_per_page: 50,
    max_items_per_page: 200,
    items_per_page: 50,
    current_page: 1
};

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
        $('#' + arrIds[i]).datepicker(config);
        if (arrDateValues[i] != null && arrDateValues[i] !== "") {
            $('#' + arrIds[i]).val(arrDateValues[i]).datepicker('setDate', (new Date(arrDateValues[i])));
        }
    }
}

casinoDailyStatistics = {
    fromDate: '',
    toDate: '',
    columnWidthDefault: 70,
    title: '',
    gridHeight: 100,
    chartData: [],
    getDailyReport: function () {
        var mainreport = $("#dailysummarygrid");
        mainreport.addClass('hidden');
        $('#dailydetailgrid').addClass('hidden');
        $('#err').html('');

        try { parent.BlockUI(); } catch (e) { }
        casinoDailyStatistics.fromDate = $('#txtfromdate').val();
        casinoDailyStatistics.toDate = $('#txttodate').val();
        var data = { 'fromDate': casinoDailyStatistics.fromDate, 'toDate': casinoDailyStatistics.toDate };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/GetCasinoDailySummaryReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                casinoDailyStatistics.title = 'Daily Summary On Customer Inquiries From ' + casinoDailyStatistics.fromDate +
                    ' To ' + casinoDailyStatistics.toDate;
                $('#gridtitle').html(casinoDailyStatistics.title);

                if (rs != null && rs.DailySummary && rs.DailySummary.length > 0) {
                    casinoDailyStatistics.assignChartData(rs);

                    var totalChild = ((rs.GrandTotal.SystemSummary.length) * (rs.GrandTotal.SystemSummary[0].Cells.length)) + 1;
                    var maxChild = totalChild > casinoDailyStatistics.defaultTotalCols ? casinoDailyStatistics.defaultTotalCols : totalChild;
                    casinoDailyStatistics.totalWidth = ((casinoDailyStatistics.columnWidthDefault * maxChild)) + (maxChild) + ((maxChild) * 2);

                    //push header
                    var reportHeader = casinoDailyStatistics.renderHeaderReport(rs.GrandTotal);
                    mainreport.find(".col_header_content thead").empty().html(reportHeader);
                    //--------------
                    var arrFrozen = [];
                    var arrScroll = [];
                    var totalMainCategories = rs.DailySummary.length;
                    for (var i = 0; i < totalMainCategories; i++) {
                        var arrNewData = rs.DailySummary[i];
                        if (arrNewData != null && arrNewData.SystemSummary.length > 0) {
                            var css = (i % 2 === 0 ? "bgeven" : "bgodd");
                            arrFrozen.push(casinoDailyStatistics.renderFrozenGroup(arrNewData.MainCategory, css));
                            arrScroll.push(casinoDailyStatistics.renderScrollGroup(arrNewData.SystemSummary,
                                css,
                                'style="font-weight: bold;"',
                                arrNewData.MainCategory));
                        }
                    }
                    //------------------
                    $("#dailysummarybody tbody").empty().append(arrScroll.join(''));
                    $("#dailysummarybg tbody").empty().append(arrFrozen.join(''));
                    //-----grand total
                    var arrFooterScroll = [];
                    arrFooterScroll.push(casinoDailyStatistics.renderFooterGroup(rs.SumAllFeedbacks, rs.GrandTotal.SystemSummary));
                    mainreport.find(".row_footer_content thead").empty().append(
                        '<tr>\
                            <th rowspan="2">\
                                <div style="width:300px;height: 39px;" class="table-cell l">Grand Total</div>\
                            </th>\
                         </tr>');
                    mainreport.find(".col_footer_content thead").empty().empty().html(arrFooterScroll.join(''));
                    //------------------------------

                    mainreport.removeClass('hidden');
                    var gridOpts = {
                        tblContentId: 'dailysummarybody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: casinoDailyStatistics.gridHeight,
                        freezeWidth: 300,
                        numberFreezeCol: 1
                    };
                    mainreport.gridScroller(gridOpts);
                }
                else {
                    $('<div id="errmsg" class="errmsg"><div class="alertboxerr"><div>There is no information.</div></div></div>').appendTo($("#err"));
                    mainreport.addClass('hidden');
                }
            },
            error: function () { },
            complete: function () {
                try { parent.UnBlockUI(); } catch (e) { }
            }
        });
    },
    renderHeaderReport: function (row) {
        var html = '';
        if (row) {
            var totalGroupCels = row.SystemSummary[0].Cells.length;
            var totalWidth = (casinoDailyStatistics.columnWidthDefault * totalGroupCels);
            var totalCels = row.SystemSummary.length;

            var summary;

            //--------------------------
            html += '<tr>';
            html += '<th rowspan="2"><div class="table-cell c" style="width:300px; heigth:39px;">Category</div></th>';
            for (var i = 0; i < totalCels; i++) {
                summary = row.SystemSummary[i];
                html += '<th colspan="' + totalGroupCels + '"><div class="c table-cell" style="width: ' + totalWidth + 'px">' + summary.Sytem + '</div></th>';
            }
            html += '</tr>';
            //--------------------------
            html += '<tr>';
            for (var k = 0; k < totalCels; k++) {
                summary = row.SystemSummary[k];
                for (var j = 0; j < totalGroupCels; j++) {
                    html += '<th>\
                            <div style="width:' + casinoDailyStatistics.columnWidthDefault +
                                'px" class="c table-cell">' + summary.Cells[j].Title + '</div>\
                         </th>';
                }
            }
            html += '</tr>';
            //--------------------------
        }
        return html;
    },
    renderFrozenGroup: function (title, css) {
        var html = '<tr class="' + css + '">\
                        <td><div class="l b" style="width:300px;">' + title + '</div></td>\
                    </tr>';
        return html;
    },
    renderScrollGroup: function (row, css, style, title) {
        var html = '';
        if (row) {
            html = '<tr class="' + css + '">';
            html += '<td><div class="l b" style="width:300px; ">' + title + '</div></td>';
            var totalCells = row.length;
            for (var i = 0; i < totalCells; i++) {
                var summary = row[i];
                for (var j = 0; j < summary.Cells.length; j++) {
                    html += '<td ' + style + '>\
                            <div class="c ' + css +
                                '" style="width:' + casinoDailyStatistics.columnWidthDefault + 'px;">' +
                                    summary.Cells[j].Value +
                            '</div>\
                         </td>';
                }
            }
            html += '</tr>';
        }

        return html;
    },
    renderFooterGroup: function (sum, detail) {
        var html = '';
        if (sum && detail) {
            //first row
            html += '<tr>';
            html += '<th rowspan="2"><div class="l b" style="width:300px; height: 39px;">Grand Total</div></th>';
            var i;
            for (i = 0; i < detail.length; i++) {
                var summary = detail[i];
                var totalCells = summary.Cells.length;
                for (var j = 0; j < totalCells; j++) {
                    html += '<th><div class="c " style="width:' + casinoDailyStatistics.columnWidthDefault + 'px;">' + summary.Cells[j].Value + '</div></th>';
                }
            }

            html += '</tr>';
            //----------------------
            var cols = detail[0].Cells.length;
            var totalWidth = casinoDailyStatistics.columnWidthDefault * cols;
            html += '<tr>';
            for (i = 0; i < sum.length; i++) {
                html += '<th style="border-bottom:none; width: ' + totalWidth + 'px " colspan="' + cols + '"><div class="c">' + sum[i] + '</div></th>';
            }

            html += '</tr>';
        }
        return html;
    },
    exportExcel: function () {
        var url = "CasinoDailyStatistics?export=true&type=1&fdate=" + casinoDailyStatistics.fromDate +
            "&tdate=" + casinoDailyStatistics.toDate +
            "&title=" + casinoDailyStatistics.title;
        $("#frm").attr("src", url);
    },
    viewDetail: function () {
        casinoDailyDetail.getDailyDetailReport(false);
    },
    assignChartData: function (rs) {
        var mainCategories = [];
        this.chartData = [];

        if (rs && rs.GrandTotal) {
            var feedbackSummary = rs.GrandTotal;
            var totalSubcategories = feedbackSummary.SystemSummary.length;
            var groupIndex = 0;
            var totalFeedbacks;

            for (var i = 0; i < totalSubcategories; i++) {
                var categoryDetail = feedbackSummary.SystemSummary[i];
                var categoryName = categoryDetail.Sytem;
                totalFeedbacks = rs.SumAllFeedbacks[i];
                if (totalFeedbacks === 0) {
                    continue;
                }

                var sumAllFeedbacksLength = rs.SumAllFeedbacks.length;
                var sumAllFeedbacksAmount = 0;
                for (var l = 0; l < sumAllFeedbacksLength; l++) {
                    sumAllFeedbacksAmount += parseFloat(rs.SumAllFeedbacks[l]);
                }

                var getBackgroundColor = crmChart.getBackGroundColor(i);
                totalFeedbacks = rs.SumAllFeedbacks[i];
                mainCategories.push({
                    title: categoryName,
                    value: totalFeedbacks,
                    color: getBackgroundColor
                });

                groupIndex++;
            }

            this.chartData = mainCategories;
        }
    },
    viewChart: function () {
        if (this.chartData == null ||
            this.chartData.length === 0) {
            return false;
        }

        var chartOption = $.extend({}, crmChart.newopts, {
            yAxisUnit: "Feedback(s)",
            chartType: crmChart.chartType.pie
        });

        crmChart.title = $('#gridtitle').html();
        crmChart.viewChart(this.chartData, chartOption);
    }
};

casinoDailyDetail = {
    title: '',
    totalRows: 0,
    pageIndex: 1,
    pageSize: 50,
    fromDate: '',
    toDate: '',
    gridHeight: 100,
    freeWidth: 420,
    freeCols: 5,
    /*jshint camelcase: false */
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = casinoDailyDetail.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;
        $("#pagination").pagination(total_items, pagerOpts);
    },

    pageSelectCallback: function (page_index, _items_per_page, jq) {
        /* jshint unused: vars */
        var max_page = Math.ceil(casinoDailyDetail.totalRows / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        casinoDailyDetail.pageIndex = page_index + 1;
        casinoDailyDetail.pageSize = _items_per_page;
        casinoDailyDetail.getDailyDetailReport(true);
        return false;
    },
    getDailyDetailReport: function (loadOldData) {
        $('#dailysummarygrid').addClass('hidden');
        var mainreport = $("#dailydetailgrid");
        $('#err').html('');

        try { parent.BlockUI(); } catch (e) { }
        if (!loadOldData) {
            mainreport.addClass('hidden');
            casinoDailyDetail.pageIndex = 1;
        }
        casinoDailyDetail.fromDate = casinoDailyStatistics.fromDate;
        casinoDailyDetail.toDate = casinoDailyStatistics.toDate;
        var data = { 'fromDate': casinoDailyDetail.fromDate,
            'toDate': casinoDailyDetail.toDate,
            'fromCache': loadOldData,
            'itemPerPage': casinoDailyDetail.pageSize,
            'currentPage': casinoDailyDetail.pageIndex
        };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/GetCasinoDailyDetailReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                casinoDailyDetail.title = 'Daily Casino - Related Customer Inquiries Record From ' + casinoDailyDetail.fromDate +
                    ' To ' + casinoDailyDetail.toDate;
                $('#detailTitle').html(casinoDailyDetail.title);

                if (rs && rs.DailyComplaints && rs.DailyComplaints.length > 0) {
                    var arrScroll = [];
                    var arrFrozen = [];
                    var totalMainCategories = rs.DailyComplaints.length;
                    for (var i = 0; i < totalMainCategories; i++) {
                        casinoDailyDetail.totalRows = parseInt(rs.total);
                        var row = rs.DailyComplaints[i];
                        var css = (i % 2 === 0 ? "bgeven" : "bgodd");
                        arrScroll.push(casinoDailyDetail.renderScrollRow(row, css));
                        arrFrozen.push(casinoDailyDetail.renderFrozenRow(row, css));
                    }

                    $('#btnExportDetail').removeClass('hidden');
                    $("#dailydetailgridbody tbody").empty().append(arrScroll.join(''));
                    $("#dailydetailfirstcol tbody").empty().append(arrFrozen.join(''));

                    var gridOpts = {
                        tblContentId: 'dailydetailgridbody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: casinoDailyDetail.gridHeight,
                        freezeWidth: casinoDailyDetail.freeWidth,
                        numberFreezeCol: casinoDailyDetail.freeCols
                    };
                    mainreport.removeClass("hidden").gridScroller(gridOpts);
                    casinoDailyDetail.initPager(casinoDailyDetail.totalRows, casinoDailyDetail.pageSize, casinoDailyDetail.pageIndex - 1);
                    casinoDailyDetail.reRenderGrid();

                    mainreport.find(".y-scrollbar").scrollTop(0);
                }
                else {
                    casinoDailyDetail.renderNoDataRow();
                }
            },
            error: function () {
                casinoDailyDetail.renderNoDataRow();
            },
            complete: function () {
                try { parent.UnBlockUI(); } catch (e) { }
            }
        });
    },
    renderScrollRow: function (row, css) {
        var html = '<tr class="' + css + '">\
            <td><div class="c" style="width: 80px;">&nbsp;</div></td>\
            <td><div class="c" style="width: 60px;">&nbsp;</div></td>\
            <td><div class="c" style="width: 60px;">&nbsp;</div></td>\
            <td><div class="c" style="width: 60px;">&nbsp;</div></td>\
            <td><div class="c" style="width: 140px;">&nbsp;' + row.CustomerName + '</div></td>\
            <td><div class="c" style="width: 100px;">&nbsp;' + row.Licensee + '</div></td>\
            <td><div class="c" style="width: 100px;">&nbsp;' + row.CommunicationTool.ContactBy + '</div></td>\
            <td><div class="c" style="width: 100px;">&nbsp;' + row.ProductName + '</div></td>\
            <td><div class="c" style="width: 60px;">&nbsp;' + row.ExtNo + '</div></td>\
            <td><div class="l" style="width: 220px;">&nbsp;' + row.CommunicationTool.Address + '</div></td>\
            <td><div class="c" style="width: 100px;">&nbsp;' + (row.Country ? row.Country : '&nbsp;') + '</div></td>\
            <td><div class="c" style="width: 180px;">&nbsp;' + row.Category + '</div></td>\
            <td><div class="l" style="width: 480px;">&nbsp;' + replaceSpecialHtmlTag(row.Complaints) + '</div></td>\
            <td><div class="l" style="width: 480px;">&nbsp;' + replaceSpecialHtmlTag(row.Solution) + '</div></td>\
            <td><div class="c" style="width: 60px;">&nbsp;' + row.Status + '</div></td>\
            <td><div class="c" style="width: 60px;">&nbsp;' + row.Response + '</div></td>\
            <td><div class="c" style="width: 100px;">&nbsp;' + row.HandlyBy + '</div></td>\
            <td><div class="c" style="width: 80px;">&nbsp;' + row.CreationTime + '</div></td>\
        </tr>';

        return replaceSpaceForIE9(html);
    },
    renderFrozenRow: function (row, css) {
        var html = '<tr class="' + css + '">\
            <td><div class="c" style="width: 80px;">' + row.FeedbackDate + '</div></td>\
            <td><div class="c" style="width: 60px;">' + row.FeedbackTime + '</div></td>\
            <td><div class="c" style="width: 60px;">' + (new Date(row.ResolutionDate).format("h:i A")) + '</div></td>\
            <td><div class="c" style="width: 60px;">' + row.Duration + '</div></td>\
            <td><div class="c" style="width: 140px;">' + row.CustomerName + '</div></td>\
        </tr>';

        return html;
    },
    goBack: function () {
        $('#dailysummarygrid').removeClass('hidden');
        $('#dailydetailgrid').addClass('hidden');
    },
    openConfirmPopup: function () {
        $('#divEmailError').addClass('hidden').html('');
        $("#divPopup").dialog({ width: 350, height: 200, zIndex: 500, resizable: false,
            close: function (event, ui) {
                /* jshint unused: vars */
            },
            buttons: [
                    {
                        text: "Ok",
                        click: function () {
                            var emailAddress = $('#txtEmail').val().trim();
                            var validateCode = emailListValidate(emailAddress);

                            if (validateCode === 1) {
                                $('#divEmailError').removeClass('hidden').html('<div>Email cannot empty.</div>');
                                return false;
                            }
                            else if (validateCode === 2) {
                                $('#divEmailError').removeClass('hidden').html('<div>Email is invalid format.</div>');
                                return false;
                            }

                            var p = {
                                'fromDate': casinoDailyDetail.fromDate,
                                'toDate': casinoDailyDetail.toDate,
                                'reportTitle': casinoDailyDetail.title,
                                'isSendMail': true,
                                'email': emailAddress
                            };

                            $.ajax({
                                type: "POST",
                                url: window.rootUrl + "Reports/ExportCasinoDetailReport",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: JSON.stringify(p),
                                success: function (rs) {
                                    /* jshint unused: vars */
                                },
                                error: function () {
                                },
                                complete: function () {
                                }
                            });
                            $(this).dialog("close");
                        }
                    }
                ]
        });
    },
    exportExcel: function () {
        try { parent.BlockUI(); } catch (e) { }
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/AllowExportCasinoStatisticDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                if (rs) {
                    if (rs.ErrCode === 0) {
                        var url = "CasinoDailyStatistics?export=true&type=2&fdate=" + casinoDailyDetail.fromDate +
                             "&tdate=" + casinoDailyDetail.toDate +
                             "&title=" + casinoDailyDetail.title;
                        $("#frm").attr("src", url);
                    }
                    else {
                        casinoDailyDetail.openConfirmPopup();
                    }
                }
                else {
                    casinoDailyDetail.openConfirmPopup();
                }
            },
            error: function () {
            },
            complete: function () {
                try {
                    parent.UnBlockUI();
                } catch (e) {
                }
            }
        });
    },
    reRenderGrid: function () {
        var trFrozen = $('#dailydetailfirstcol tbody tr');
        var totalRows = trFrozen.length;

        for (var i = 0; i < totalRows; i++) {
            var frozenHeight = $('#dailydetailfirstcol tbody tr:eq(' + i + ') td:eq(0)').height();
            var scrollHeight = $('#dailydetailgridbody tbody tr:eq(' + i + ') td:eq(0)').height();

            var maxHeight = 18;
            if (frozenHeight > scrollHeight) {
                maxHeight = frozenHeight + 1;
            }
            else if (scrollHeight > frozenHeight) {
                maxHeight = scrollHeight + 1;
            }

            $('#dailydetailgridbody tbody tr:eq(' + i + ') td:eq(0) div').css('height', maxHeight).append(' ');
            $('#dailydetailfirstcol tbody tr:eq(' + i + ') td:eq(0) div').attr('style',
                ' display:table-cell; vertical-align: middle; height:' + maxHeight + 'px');
        }
        var mainreport = $("#dailydetailgrid");
        var gridOpts = {
            tblContentId: 'dailydetailgridbody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: casinoDailyDetail.gridHeight,
            freezeWidth: casinoDailyDetail.freeWidth,
            numberFreezeCol: casinoDailyDetail.freeCols
        };
        mainreport.gridScroller(gridOpts);
    },
    renderNoDataRow: function () {
        $('#pagination').html('&nbsp;');
        $('#btnExportDetail').addClass('hidden');

        var html = '<tr>\
            <td style="border-right:0px none;"><div class="c" style="width:80px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:140px; height:20px;">&nbsp;</div></td>\
            <td colspan="13"><div class="c" style="width:2080px; height:20px;"><div class="noInfomation">There is no information.</div></div></td>\
        </tr>';

        var mainreport = $('#dailydetailgrid');

        $("#dailydetailfirstcol tbody").empty().html('<tr>\
            <td style="border-right:0px none;"><div class="c" style="width:80px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:140px; height:20px;">&nbsp;</div></td></tr>'
            );

        $("#dailydetailgridbody tbody").empty().html(html);
        var gridOpts = {
            tblContentId: 'dailydetailgridbody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: casinoDailyDetail.gridHeight,
            freezeWidth: casinoDailyDetail.freeWidth,
            numberFreezeCol: casinoDailyDetail.freeCols
        };
        mainreport.removeClass("hidden").gridScroller(gridOpts);
    }
};

head.ready(function () {
    try {
        parent.BlockUI();
        parent.$.unblockUI();
    } catch (e) {
    }

    var arrDateValues = [(new Date(fromDate)).format("M d, Y"), (new Date(toDate)).format("M d, Y")];
    var arrIds = ['txtfromdate', 'txttodate'];
    setupCalendar(arrDateValues, arrIds);

    //    try { topwnd = findMainWindow(window); topwnd.UnBlockUI(); } catch (e) { }

    var gridOpts = {
        tblContentId: 'dailysummarybody',
        isShowPager: false
    };

    $("#dailysummarygrid").initGrid(gridOpts);
    gridOpts = {
        tblContentId: 'dailydetailgridbody',
        isShowPager: true
    };

    $("#dailydetailgrid").initGrid(gridOpts);

    crmChart = parent.crmChart;
});