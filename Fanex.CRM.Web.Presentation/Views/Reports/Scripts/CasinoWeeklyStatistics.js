"use strict";
/* global replaceSpecialHtmlTag, replaceSpaceForIE9, emailListValidate, 
head, fromDate, toDate */
/* exported arrWidth,
maxHeightOfPopUp */
/* jshint unused: vars */

var arrWidth = [];
var maxHeightOfPopUp = ($(window).height() + 50);
var casinoWeeklyDetail;
var crmChart;

var casinoWeeklySummary = {
    title: '',
    firstDate: '',
    selectDate: '',
    defaultTotalCols: 6,
    columnWidthDefault: 90,
    totalWidth: 0,
    gridHeight: 100,
    chartSetting: { categories: [], series: [] },
    getWeeklySummary: function () {
        var mainreport = $("#weeklysummarygrid");
        mainreport.addClass('hidden');
        $("#weeklydetailgrid").addClass('hidden');
        $('#err').html('');

        casinoWeeklySummary.firstDate = $('#txtfromdate').val();
        casinoWeeklySummary.selectDate = $('#txttodate').val();

        try { parent.BlockUI(); } catch (e) { }
        var data = { 'fromDate': casinoWeeklySummary.firstDate, 'toDate': casinoWeeklySummary.selectDate };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/GetCasinoWeeklySummaryReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                casinoWeeklySummary.title = 'Weekly Summary Report From ' + casinoWeeklySummary.firstDate + ' To ' + casinoWeeklySummary.selectDate;
                $('#gridtitle').html(casinoWeeklySummary.title);

                if (rs != null && rs.DailySummaries && rs.DailySummaries.length > 0) {
                    casinoWeeklySummary.assignChartData(rs);

                    //push header
                    var reportHeader = (casinoWeeklySummary.renderHeaderReport(rs.GrandTotal.Cells));
                    mainreport.find(".col_header_content thead").empty().html(reportHeader);
                    //--------------
                    var arrFrozen = [];
                    var arrScroll = [];
                    var totalMainCategories = rs.DailySummaries.length;
                    for (var i = 0; i < totalMainCategories; i++) {
                        var arrNewData = rs.DailySummaries[i];

                        if (arrNewData != null && arrNewData.DailySummaries.length > 0) {
                            var mainId = arrNewData.Total.MainCategoryId;
                            //group title
                            var css = (i % 2 === 0 ? "bgeven" : "bgodd");
                            arrFrozen.push(casinoWeeklySummary.renderFrozenGroup(mainId, arrNewData.Total.MainCategory, css));
                            arrScroll.push(casinoWeeklySummary.renderScrollGroup(arrNewData.Total,
                                mainId,
                                css,
                                'style="font-weight: bold;"',
                                arrNewData.Total.MainCategory));
                        }
                    }
                    //------------------
                    $("#weeklysummarybody tbody").empty().append(arrScroll.join(''));
                    $("#weeklysummarybg tbody").empty().append(arrFrozen.join(''));
                    //-----grand total
                    var arrFooterScroll = [];
                    arrFooterScroll.push(casinoWeeklySummary.renderFooter(rs.GrandTotal));
                    mainreport.find(".row_footer_content thead").empty().append(
                        '<tr>\
                            <th>\
                                <div style="width:300px;height: 17px;" class="table-cell l">Grand Total</div>\
                            </th>\
                         </tr>');
                    mainreport.find(".col_footer_content thead").empty().empty().html(arrFooterScroll.join(''));
                    //------------------------------

                    mainreport.removeClass('hidden');
                    var gridOpts = {
                        tblContentId: 'weeklysummarybody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: casinoWeeklySummary.gridHeight,
                        freezeWidth: 300,
                        numberFreezeCol: 1
                    };
                    mainreport.gridScroller(gridOpts);


                }
                else {
                    $('<div id="errmsg" class="errmsg"><div class="alertboxerr"><div>There is no information.</div></div></div>').appendTo($("#err"));
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
            html = '<tr>';
            html += '<th><div class="c table-cell" style="width: 300px; height:30px;">Category</div></th>';
            var totalCells = row.length;
            for (var i = 0; i < totalCells; i++) {
                html += '<th>\
                        <div class="table-cell c" style="width: ' + casinoWeeklySummary.columnWidthDefault +
                            'px; height:30px;">' + row[i].Title.replace('_', ' ') + '</div>\
                     </th>';
            }
            html += '<th><div class="table-cell c" style="width: ' + casinoWeeklySummary.columnWidthDefault + 'px; height:30px;">Grand Total</div></th>';
            html += '</tr>';
        }
        return html;
    },
    renderFrozenGroup: function (id, title, css) {
        var html = '<tr class="' + css + '">\
                        <td><div class="l b" style="width:300px;">' + title + '</div></td>\
                    </tr>';
        return html;
    },
    renderScrollGroup: function (row, id, css, style, title) {
        var html = '';
        if (row) {
            html = '<tr class="' + css + '">';
            html += '<td><div class="l b" style="width:300px;">' + title + '</div></td>';
            var totalCells = row.Cells.length;
            for (var i = 0; i < totalCells; i++) {
                html += '<td ' + style + '>\
                        <div class="c ' + css + '" style="width:' + casinoWeeklySummary.columnWidthDefault + 'px;">' + row.Cells[i].Value + '</div>\
                     </td>';
            }
            html += '<td ' + style + '><div class="c ' + css + '" style="width:' + casinoWeeklySummary.columnWidthDefault + 'px;">' + row.Total + '</div></td>';
            html += '</tr>';
        }

        return html;
    },
    renderFooter: function (row) {
        var html = '';
        if (row) {
            html = '<tr>';
            html += '<th><div class="c table-cell" style="width:300px; height:17px;">' + row.Total + '</div></th>';
            var totalCells = row.Cells.length;
            for (var i = 0; i < totalCells; i++) {
                html += '<th>\
                        <div class="table-cell c" style="width:' + casinoWeeklySummary.columnWidthDefault + 'px; height:17px;">' + row.Cells[i].Value + '</div>\
                     </th>';
            }
            html += '<th><div class="c table-cell" style="width:' + casinoWeeklySummary.columnWidthDefault + 'px; height:17px;">' + row.Total + '</div></th>';
            html += '</tr>';
        }

        return html;
    },
    exportExcel: function () {
        var url = "CasinoWeeklyStatistics?export=true&type=1&fdate=" + casinoWeeklySummary.firstDate +
            "&tdate=" + casinoWeeklySummary.selectDate +
            "&title=" + casinoWeeklySummary.title;
        $("#frm").attr("src", url);
    },
    viewDetail: function () {
        casinoWeeklyDetail.getDailyDetailReport(false);
    },
    assignChartData: function (rs) {
        var categories = [];
        var series = [];

        if (rs && rs.DailySummaries) {
            //            var items = [];
            var totalMainCategories = rs.DailySummaries.length;
            var totalCols = rs.GrandTotal.Cells.length;

            for (var i = 0; i < totalMainCategories; i++) {
                var mainCategoryDetail = rs.DailySummaries[i].Total;
                var categoryName = mainCategoryDetail.MainCategory;
                var detailData = [];

                for (var k = 0; k < totalCols; k++) {
                    detailData.push(mainCategoryDetail.Cells[k].Value);

                    if (i === totalMainCategories - 1) {
                        categories.push(mainCategoryDetail.Cells[k].Title);
                    }
                }

                series.push({ name: categoryName, data: detailData });
            }

            this.chartSetting.categories = categories;
            this.chartSetting.series = series;
        }
    },
    viewChart: function () {
        var chartOptions = {
            xAxis: {
                categories: this.chartSetting.categories
            },
            series: this.chartSetting.series,
            tooltip: {
                valueSuffix: ' Feedback(s)'
            }
        };

        crmChart.title = $('#gridtitle').html();

        crmChart.viewChart(chartOptions);
    }
};

var pagerOpts = {
    /* jshint camelcase: false */
    callback: null,
    start_per_page: 50,
    step_per_page: 50,
    max_items_per_page: 200,
    items_per_page: 50,
    current_page: 1
};

casinoWeeklyDetail = {
    title: '',
    totalRows: 0,
    pageIndex: 1,
    pageSize: 50,
    fromDate: '',
    toDate: '',
    gridHeight: 100,
    freeWidth: 420,
    freeCols: 5,
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = casinoWeeklyDetail.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;
        $("#pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
        /* jshint unused: vars */
        var max_page = Math.ceil(casinoWeeklyDetail.totalRows / _items_per_page);
        page_index = page_index < max_page ? page_index : max_page - 1;
        casinoWeeklyDetail.pageIndex = page_index + 1;
        casinoWeeklyDetail.pageSize = _items_per_page;
        casinoWeeklyDetail.getDailyDetailReport(true);
        return false;
    },
    getDailyDetailReport: function (loadOldData) {
        $('#weeklysummarygrid').addClass('hidden');
        var mainreport = $("#weeklydetailgrid");

        $('#err').html('');
        try { parent.BlockUI(); } catch (e) { }
        if (!loadOldData) {
            mainreport.addClass('hidden');
            casinoWeeklyDetail.pageIndex = 1;
        }
        casinoWeeklyDetail.fromDate = casinoWeeklySummary.firstDate;
        casinoWeeklyDetail.toDate = casinoWeeklySummary.selectDate;
        var data = { 'fromDate': casinoWeeklyDetail.fromDate,
            'toDate': casinoWeeklyDetail.toDate,
            'fromCache': loadOldData,
            'itemPerPage': casinoWeeklyDetail.pageSize,
            'currentPage': casinoWeeklyDetail.pageIndex
        };
        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/GetCasinoDailyDetailReport",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                casinoWeeklyDetail.title = 'Weekly Casino - Related Customer Inquiries Record From ' + casinoWeeklyDetail.fromDate +
                    ' To ' + casinoWeeklyDetail.toDate;
                $('#detailTitle').html(casinoWeeklyDetail.title);

                if (rs && rs.DailyComplaints && rs.DailyComplaints.length > 0) {
                    var arrScroll = [];
                    var arrFrozen = [];
                    var totalMainCategories = rs.DailyComplaints.length;
                    for (var i = 0; i < totalMainCategories; i++) {
                        casinoWeeklyDetail.totalRows = parseInt(rs.total);
                        var row = rs.DailyComplaints[i];
                        var css = (i % 2 === 0 ? "bgeven" : "bgodd");
                        arrScroll.push(casinoWeeklyDetail.renderScrollRow(row, css));
                        arrFrozen.push(casinoWeeklyDetail.renderFrozenRow(row, css));
                    }
                    $('#btnExportDetail');

                    $("#weeklydetailgridbody tbody").empty().append(arrScroll.join(''));
                    $("#weeklydetailfirstcol tbody").empty().append(arrFrozen.join(''));
                    var gridOpts = {
                        tblContentId: 'weeklydetailgridbody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: casinoWeeklyDetail.gridHeight,
                        freezeWidth: casinoWeeklyDetail.freeWidth,
                        numberFreezeCol: casinoWeeklyDetail.freeCols
                    };
                    mainreport.removeClass('hidden').gridScroller(gridOpts);
                    casinoWeeklyDetail.initPager(casinoWeeklyDetail.totalRows, casinoWeeklyDetail.pageSize, casinoWeeklyDetail.pageIndex - 1);
                    casinoWeeklyDetail.reRenderGrid();

                    mainreport.find(".y-scrollbar").scrollTop(0);
                }
                else {
                    casinoWeeklyDetail.renderNoDataRow();
                }
            },
            error: function () {
                casinoWeeklyDetail.renderNoDataRow();
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
        $('#weeklysummarygrid').removeClass('hidden');
        $('#weeklydetailgrid').addClass('hidden');
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
                                'fromDate': casinoWeeklyDetail.fromDate,
                                'toDate': casinoWeeklyDetail.toDate,
                                'reportTitle': casinoWeeklyDetail.title,
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

        $.ajax({
            type: "POST",
            url: window.rootUrl + "Reports/AllowExportCasinoStatisticDetail",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: '',
            success: function (rs) {
                if (rs) {
                    if (rs.ErrCode === 0) {
                        var url = "CasinoWeeklyStatistics?export=true&type=2&fdate=" + casinoWeeklyDetail.fromDate +
                            "&tdate=" + casinoWeeklyDetail.toDate +
                            "&title=" + casinoWeeklyDetail.title;
                        $("#frm").attr("src", url);
                    }
                    else {
                        casinoWeeklyDetail.openConfirmPopup();
                    }
                }
                else {
                    casinoWeeklyDetail.openConfirmPopup();
                }
            },
            error: function () {

            },
            complete: function () {

            }
        });
    },
    reRenderGrid: function () {
        var trFrozen = $('#weeklydetailfirstcol tbody tr');
        var totalRows = trFrozen.length;

        for (var i = 0; i < totalRows; i++) {
            var frozenHeight = $('#weeklydetailfirstcol tbody tr:eq(' + i + ') td:eq(0)').height();
            var scrollHeight = $('#weeklydetailgridbody tbody tr:eq(' + i + ') td:eq(0)').height();

            var maxHeight = 18;
            if (frozenHeight > scrollHeight) {
                maxHeight = frozenHeight + 1;
            }
            else if (scrollHeight > frozenHeight) {
                maxHeight = scrollHeight + 1;
            }

            $('#weeklydetailgridbody tbody tr:eq(' + i + ') td:eq(0) div').css('height', maxHeight).append(' ');
            $('#weeklydetailfirstcol tbody tr:eq(' + i + ') td:eq(0) div').attr('style',
                ' display:table-cell; vertical-align: middle; height:' + maxHeight + 'px');
        }

        var mainreport = $("#weeklydetailgrid");
        var gridOpts = {
            tblContentId: 'weeklydetailgridbody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: casinoWeeklyDetail.gridHeight,
            freezeWidth: casinoWeeklyDetail.freeWidth,
            numberFreezeCol: casinoWeeklyDetail.freeCols
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

        var mainreport = $('#weeklydetailgrid');

        $("#weeklydetailfirstcol tbody").empty().html('<tr>\
            <td style="border-right:0px none;"><div class="c" style="width:80px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:140px; height:20px;">&nbsp;</div></td></tr>'
            );

        $("#weeklydetailgridbody tbody").empty().html(html);
        var gridOpts = {
            tblContentId: 'weeklydetailgridbody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: casinoWeeklyDetail.gridHeight,
            freezeWidth: casinoWeeklyDetail.freeWidth,
            numberFreezeCol: casinoWeeklyDetail.freeCols
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

    $('#txtfromdate').val((new Date(fromDate)).format("M d, Y"));
    $('#txttodate').val((new Date(toDate)).format("M d, Y"));

    //    try { topwnd = findMainWindow(window); topwnd.UnBlockUI(); } catch (e) { }

    var gridOpts = {
        tblContentId: 'weeklysummarybody',
        isShowPager: false
    };
    $("#weeklysummarygrid").initGrid(gridOpts);

    var gridOptions = {
        tblContentId: 'weeklydetailgridbody',
        isShowPager: true
    };

    $("#weeklydetailgrid").initGrid(gridOptions);

    crmChart = parent.crmChart;
});
