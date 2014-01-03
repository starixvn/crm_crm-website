var topwnd;
var arrWidth = new Array();
var maxHeightOfPopUp = ($(window).height() + 50)

var casinoWeeklySummary = {
    title: '',
    firstDate: '',
    selectDate: '',
    defaultTotalCols: 6,
    columnWidthDefault: 90,
    totalWidth: 0,
    gridHeight: 80,
    getWeeklySummary: function () {
        var mainreport = $("#weeklysummarygrid");
        mainreport.addClass('hidden');
        $("#weeklydetailgrid").addClass('hidden');
        $('#err').html('');

        casinoWeeklySummary.firstDate = $('#txtfromdate').val();
        casinoWeeklySummary.selectDate = $('#txttodate').val();

        try { topwnd.BlockUI(); } catch (e) { }
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

                    for (var i = 0; i < rs.DailySummaries.length; i++) {
                        var arrNewData = rs.DailySummaries[i];

                        if (arrNewData != null && arrNewData.DailySummaries.length > 0) {
                            var mainId = arrNewData.Total.MainCategoryId;
                            //group title
                            var css = (i % 2 == 0 ? "bgeven" : "bgodd");
                            arrFrozen.push(casinoWeeklySummary.renderFrozenGroup(mainId, arrNewData.Total.MainCategory, css));
                            arrScroll.push(casinoWeeklySummary.renderScrollGroup(arrNewData.Total, mainId, css, 'style="font-weight: bold;"', arrNewData.Total.MainCategory));
                        }
                    }
                    //------------------
                    $("#weeklysummarybody tbody").empty().append(arrScroll.join(''));
                    $("#weeklysummarybg tbody").empty().append(arrFrozen.join(''));
                    //-----grand total
                    var arrFooterScroll = [];
                    arrFooterScroll.push(casinoWeeklySummary.renderFooter(rs.GrandTotal));
                    mainreport.find(".row_footer_content thead").empty().append('<tr><th><div style="width:300px;height: 17px;" class="table-cell l">Grand Total</div></th></tr>');
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
            complete: function () { try { topwnd.UnBlockUI(); } catch (e) { } }
        });
    },
    renderHeaderReport: function (row) {
        var html = '<tr>';
        html += '<th><div class="c table-cell" style="width: 300px; height:30px;">Category</div></th>';
        for (var i = 0; i < row.length; i++) {
            html += '<th><div class="table-cell c" style="width: ' + casinoWeeklySummary.columnWidthDefault + 'px; height:30px;">' + row[i].Title.replace('_', ' ') + '</div></th>';
        }
        html += '<th><div class="table-cell c" style="width: ' + casinoWeeklySummary.columnWidthDefault + 'px; height:30px;">Grand Total</div></th>';
        html += '</tr>';

        return html;
    },
    renderFrozenGroup: function (id, title, css) {
        var html = '<tr class="' + css + '">\
                        <td><div class="l b" style="width:300px;">' + title + '</div></td>\
                    </tr>';
        return html;
    },
    renderScrollGroup: function (row, id, css, style, title) {
        var html = '<tr class="' + css + '">';
        html += '<td><div class="l b" style="width:300px;">' + title + '</div></td>';
        for (var i = 0; i < row.Cells.length; i++) {
            html += '<td ' + style + '><div class="c ' + css + '" style="width:' + casinoWeeklySummary.columnWidthDefault + 'px;">' + row.Cells[i].Value + '</div></td>';
        }
        html += '<td ' + style + '><div class="c ' + css + '" style="width:' + casinoWeeklySummary.columnWidthDefault + 'px;">' + row.Total + '</div></td>';
        html += '</tr>';

        return html;
    },
    renderFooter: function (row) {
        var html = '<tr>';
        html += '<th><div class="c table-cell" style="width:300px; height:17px;">' + row.Total + '</div></th>';
        for (var i = 0; i < row.Cells.length; i++) {
            html += '<th><div class="table-cell c" style="width:' + casinoWeeklySummary.columnWidthDefault + 'px; height:17px;">' + row.Cells[i].Value + '</div></th>';
        }
        html += '<th><div class="c table-cell" style="width:' + casinoWeeklySummary.columnWidthDefault + 'px; height:17px;">' + row.Total + '</div></th>';
        html += '</tr>';

        return html;
    },
    exportExcel: function () {
        var url = "CasinoWeeklyStatistics?export=true&type=1&fdate=" + casinoWeeklySummary.firstDate + "&tdate=" + casinoWeeklySummary.selectDate + "&title=" + casinoWeeklySummary.title;
        $("#frm").attr("src", url);
    },
    viewDetail: function () {
        casinoWeeklyDetail.getDailyDetailReport(false);
    },
    assignChartData: function (rs) {
        var items = [];
        for (var i = 0; i < rs.DailySummaries.length; i++) {
            var item = rs.DailySummaries[i];
            var chartInfo = {};
            eval("chartInfo.Name='" + item.Total.MainCategory + "'");
            for (var j = 0; j < item.Total.Cells.length; j++) {
                var cell = item.Total.Cells[j];
                eval("chartInfo." + cell.Title + "=" + cell.Value);
            }
            items.push(chartInfo);
        }
        casinoWeeklySummary.chart.setData(items);
    }
};

var pagerOpts = {
    callback: null,
    start_per_page: 50,
    step_per_page: 50,
    max_items_per_page: 200,
    items_per_page: 50,
    current_page: 1
};
var casinoWeeklyDetail = {
    title: '',
    totalRows: 0,
    pageIndex: 1,
    pageSize: 50,
    fromDate: '',
    toDate: '',
    gridHeight: 80,
    initPager: function (total_items, items_per_page, current_page) {
        pagerOpts.callback = casinoWeeklyDetail.pageSelectCallback;
        pagerOpts.items_per_page = items_per_page;
        pagerOpts.current_page = current_page;
        $("#pagination").pagination(total_items, pagerOpts);
    },
    pageSelectCallback: function (page_index, _items_per_page, jq) {
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
        try { topwnd.BlockUI(); } catch (e) { }
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
                casinoWeeklyDetail.title = 'Weekly Casino - Related Customer Inquiries Record From ' + casinoWeeklyDetail.fromDate + ' To ' + casinoWeeklyDetail.toDate;
                $('#detailTitle').html(casinoWeeklyDetail.title);

                if (rs && rs.DailyComplaints && rs.DailyComplaints.length > 0) {
                    var arrScroll = [];
                    var arrFrozen = [];
                    for (var i = 0; i < rs.DailyComplaints.length; i++) {
                        casinoWeeklyDetail.totalRows = parseInt(rs.total);
                        var row = rs.DailyComplaints[i];
                        var css = (i % 2 == 0 ? "bgeven" : "bgodd"); ;
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
                        freezeWidth: 270,
                        numberFreezeCol: 3
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
            complete: function () { try { topwnd.UnBlockUI(); } catch (e) { } }
        });
    },
    renderScrollRow: function (row, css) {
        var compaints = replaceSpecialHtmlTag(row.Complaints);
        var solutions = replaceSpecialHtmlTag(row.Solution);
        var html = '<tr class="' + css + '">\
            <td><div class="c" style="width: 60px;">&nbsp;' + row.FeedbackDate + '</div></td>\
            <td><div class="c" style="width: 60px;">&nbsp;' + row.FeedbackTime + '</div></td>\
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
            <td><div class="c" style="width: 60px;">' + row.FeedbackDate + '</div></td>\
            <td><div class="c" style="width: 60px;">' + row.FeedbackTime + '</div></td>\
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
            close: function (event, ui) { },
            buttons: [
                    {
                        text: "Ok",
                        click: function () {
                            var emailAddress = $('#txtEmail').val().trim();
                            var validateCode = emailListValidate(emailAddress);

                            if (validateCode == 1) {
                                $('#divEmailError').removeClass('hidden').html('<div>Email cannot empty.</div>');
                                return false;
                            }
                            else if (validateCode == 2) {
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
                    if (rs.ErrCode == 0) {
                        var url = "CasinoWeeklyStatistics?export=true&type=2&fdate=" + casinoWeeklyDetail.fromDate + "&tdate=" + casinoWeeklyDetail.toDate + "&title=" + casinoWeeklyDetail.title;
                        $("#frm").attr("src", url);
                    }
                    else
                        casinoWeeklyDetail.openConfirmPopup();
                }
                else
                    casinoWeeklyDetail.openConfirmPopup();
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
            if (frozenHeight > scrollHeight)
                maxHeight = frozenHeight + 1;
            else if (scrollHeight > frozenHeight)
                maxHeight = scrollHeight + 1;

            $('#weeklydetailgridbody tbody tr:eq(' + i + ') td:eq(0) div').css('height', maxHeight).append(' ');
            $('#weeklydetailfirstcol tbody tr:eq(' + i + ') td:eq(0) div').attr('style', ' display:table-cell; vertical-align: middle; height:' + maxHeight + 'px');
        }

        var mainreport = $("#weeklydetailgrid");
        var gridOpts = {
            tblContentId: 'weeklydetailgridbody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: casinoWeeklyDetail.gridHeight,
            freezeWidth: 270,
            numberFreezeCol: 3
        };
        mainreport.gridScroller(gridOpts);
    },
    renderNoDataRow: function () {
        $('#pagination').html('&nbsp;');
        $('#btnExportDetail').addClass('hidden');

        var html = '<tr>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:60px; height:20px;">&nbsp;</div></td>\
            <td style="border-right:0px none;"><div class="c" style="width:140px; height:20px;">&nbsp;</div></td>\
            <td colspan="13"><div class="c" style="width:2080px; height:20px;"><div class="alertboxerr"><div>There is no information.</div></div></td>\
        </tr>';

        var mainreport = $('#weeklydetailgrid');

        $("#weeklydetailfirstcol tbody").empty().html('<tr>\
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
            freezeWidth: 270,
            numberFreezeCol: 3
        };
        mainreport.removeClass("hidden").gridScroller(gridOpts);
    }
};

head.ready(
    function() {
        $('#txtfromdate').val((new Date(fromDate)).format("m/d/Y"));
        $('#txttodate').val((new Date(toDate)).format("m/d/Y"));

        try { topwnd = findMainWindow(window); topwnd.UnBlockUI(); } catch (e) { }

        var gridOpts = {
            tblContentId: 'weeklysummarybody',
            isShowPager: false
        };
        $("#weeklysummarygrid").initGrid(gridOpts);

        var gridOpts = {
            tblContentId: 'weeklydetailgridbody',
            isShowPager: true
        };
        $("#weeklydetailgrid").initGrid(gridOpts);
    }
    );
$.extend(casinoWeeklySummary, { chart: {
    data: null,
    categories: [],
    setData: function (_data) {
        this.data = _data;
        if (_data) {
            var val = _data[0];
            var objIndex = 0;
            var newCategories = [];
            jQuery.each(val, function (name, value) {
                if (objIndex != 0) {
                    var item = { field: name, label: name, enable: true };
                    newCategories.push(item);
                }
                objIndex++;
            });
            this.categories = newCategories;
        }
    },
    viewByTotal: function () {
        if (this.data != null) {
            var data_items = this.data;
            var serial_data = []; var categories = [];
            for (var i = 0, len = data_items.length; i < len; i++) {
                var item = data_items[i];
                var toData = [];
                $.each(this.categories, function (ind, cate) {
                    if (cate.enable) {
                        var val = eval("item." + cate.field + "");
                        toData.push(casinoWeeklySummary.chart.toNumber(val));
                        if (i == 0) { categories.push(cate.label); }
                    }
                });
                serial_data.push({
                    name: item.Name,
                    data: toData,
                    cursor: 'pointer'
                });
            }
            // init chart total
            var opts = {
                container: 'chart_content',
                type: 'spline',
                title: $('#gridtitle').html(),
                width: 1000,
                height: (maxHeightOfPopUp-100),
                categories: categories,
                series: serial_data,
                legendY: (maxHeightOfPopUp - 210)
            };
            if ($('#chart_content').length == 0) {
                this.initViewChart(opts);
            }
            accChart.renderSVG(opts);
        }
    },
    initViewChart: function (opts) {
        $('<div />').attr('id', 'chart_shadow').addClass('chart_shadow').appendTo('body').show();
        $('<div />').attr('id', 'chart_popup').addClass('chart_popup')
            .css({ width: 1000 })
            .appendTo('body');
        $('<div />').attr('id', 'chart_setting').addClass('chart_setting').appendTo('#chart_popup');
        $('<div />').attr('id', 'chart_title')
             .addClass('chart_title')
             .append('<div class="setting" title="Setting">&nbsp;</div><div class="title">' + opts.title + '</div><div class="close" title="Close">&nbsp;</div>')
             .appendTo('#chart_popup');
        $('<div />').attr('id', 'chart_content').addClass('chart_content')
             .css({ width: opts.width, height: opts.height })
             .appendTo('#chart_popup');
        $('#chart_popup').css("left", 10);
        $('#chart_popup').css("top", 10);
        $('#chart_popup').show();
        $('#chart_popup').draggable({
            handle: '#chart_title'
        });
        $('div.setting').bind('click', function (e) {
            if ($('#chart_setting').is(':hidden')) {
                $('#chart_setting').slideDown();
                $(this).attr('title', 'Hide setting');
            } else {
                $('#chart_setting').slideUp();
                $(this).attr('title', 'Display setting');
            }
        });
        $('div.close').bind('click', function (e) {
            casinoWeeklySummary.chart.close();
        });
        this.renderSetting();
    },
    renderSetting: function () {
        $('#chart_setting').html('<ul id="ul_setting"></ul>');
        var tpl_cate = '{{each categories}}<li><input class="setcheck" type="checkbox" {{if $value.enable}}checked="${$value.enable}"{{/if}} value="${$index}"/>${$value.enable}&nbsp;${$value.label}</li>{{/each}}';
        //$('<div />').append(tpl_cate).tmpl(this).appendTo("#ul_setting");
        $.each(this.categories, function (ind, cate) {
            var is_checked = (cate.enable == true) ? 'checked' : '';
            var _itemTpl = '<li><input class="setcheck" ' + is_checked + ' type="checkbox" value="' + ind + '"/>&nbsp;' + cate.label + '</li>';
            $("#ul_setting").append(_itemTpl);
        });
        $('#chart_setting').css({ width: 185, 'margin-top': 27 });
        if ($.browser.msie) {
            if ($.browser.version.indexOf('7') != -1) {
                $('#chart_setting').css({ 'margin-left': -1000 });
                $('#ul_setting').css({ 'margin-left': 10 });
            }
        }
        window.setTimeout(function () {
            $('input.setcheck').bind('click', function (e) {
                var checked = $(this).attr("checked");
                var index = $(this).val();
                casinoWeeklySummary.chart.categories[index].enable = checked;
                casinoWeeklySummary.chart.viewByTotal();
            });
        });
    },
    close: function () {
        $('#chart_shadow').fadeOut();
        $('#chart_popup').fadeOut();
        window.setTimeout(function () {
            $('#chart_shadow').remove();
            $('#chart_popup').remove();
        }, 1000);
    },
    toNumber: function (numText) {
        numText = "".concat(numText);
        return parseFloat(numText.replace(/,/gi, ''));
    }
}
});
