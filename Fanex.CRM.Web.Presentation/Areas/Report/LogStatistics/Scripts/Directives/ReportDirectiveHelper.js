(function () {
    "use strict";
    /* global
    viewResources,
    crmConstants,
    numeral */
    /* jshint camelcase: false,
    unused: vars */
    /* exported reportDirectiveHelper */

    var ReportDirectiveHelper = function () {
        var self = this;
        self.getGridOptions = function (report, isExpand) {
            var groupHeaders = [
            {
                HeaderText: viewResources.Complaints_Enquiries,
                Name: "ColComplaints_Enquiries",
                MergeRowNumber: 2
            }
        ];
            var headers = [
            {
                HeaderText: viewResources.Complaints_Enquiries,
                Width: 300,
                Name: "ColComplaints_Enquiries",
                HeaderAlign: "center",
                CellAlign: "left",
                ShowCollapseExpand: true
            }
        ];

            var horizontalButtonClassName = isExpand ? 'horizontal-collapse' : 'horizontal-expand';
            var horizontalExpansion = '<input class="btnHorizontal ' + horizontalButtonClassName + '" type="button" title="' + viewResources.ProductCollapseExpand + '" />';
            var viewChartIcon = '<input class="btnCharting " type="button" title="' + viewResources.ViewChart + '" />';
            var viewDetailIcon = '<input class="btnViewDetail " type="button" title="' + viewResources.ViewDetail + '" />';

            var productLength = report.ProductColumns.length;
            var customButtons = productLength > 1 ?
                                    horizontalExpansion + viewChartIcon + viewDetailIcon :
                                    viewChartIcon + viewDetailIcon;

            var gridOptions = {
                customizeHeader: true,
                showTitle: true,
                titleInfo: {
                    text: report ? report.Name : "",
                    showExcelButton: true,
                    exportExcelFunction: null,
                    showCollapseExpandButton: true,
                    customButtons: customButtons
                },
                numberOfFrozenColumn: 1,
                fullWidth: true,
                fullHeight: false,
                gridExpandHeight: 0,
                showDistinctRow: false,
                showHighlightRow: false,
                headerColumnHeight: 29,
                bodyRowHeight: 29,
                showTree: true,
                treeParentPaddingLeft: 5,

                showFooter: true
            };

            if (isExpand || productLength == 1) {
                self.getGridOptionWhenExpanding(report, gridOptions, groupHeaders, headers);
            } else {
                self.getGridOptionWhenCollapsing(report, gridOptions, groupHeaders, headers);
            }

            return gridOptions;
        };

        self.getGridOptionWhenExpanding = function (report, gridOptions, groupHeaders, headers) {
            self.createReportHeader(groupHeaders, headers, report.ProductColumns, report.CommunicationColumns);

            var bodyData = [];
            self.createReportBodyWhenExpanding(report, bodyData);

            var footerData = [];
            self.createReportFooterWhenExpanding(report, footerData);

            self.setDataForGridOptions(gridOptions, groupHeaders, headers, bodyData, footerData);
        };
        self.createReportBodyWhenExpanding = function (report, bodyData) {
            var mainCategories = report.MainCategories;
            for (var i = 0; i < mainCategories.length; i++) {
                var mainCategory = mainCategories[i];

                var subCategories = mainCategory.SubCategories;
                var subCategoryRows = [];
                for (var j = 0; j < subCategories.length; j++) {
                    subCategoryRows.push({ Columns: self.getRowValueWhenExpanding(subCategories[j], report.ProductColumns, report.CommunicationColumns) });
                }

                bodyData.push({
                    Columns: self.getRowValueWhenExpanding(mainCategory, report.ProductColumns, report.CommunicationColumns),
                    SubRows: subCategoryRows
                });
            }
        };
        self.createReportFooterWhenExpanding = function (report, footerData) {
            var grandTotal = {
                Name: viewResources.Grand_Total,
                ListOfCommunicationCount: report.GrandTotalOfCommunicationCount,
                ListOfCommunicationSumByProduct: report.GrandTotalOfCommunicationSumByProduct
            };
            footerData.push({
                Columns: self.getRowValueWhenExpanding(grandTotal, report.ProductColumns, report.CommunicationColumns)
            });
        };
        self.getRowValueWhenExpanding = function (currentRow, productColumns, communicationColumns) {
            var dataDetail = [{ Name: "ColComplaints_Enquiries", Value: currentRow.Name}];

            var currentIndex = 0;
            for (var i = 0; i < productColumns.length; i++) {
                var product = productColumns[i];
                var numberOfCommunication = currentRow.ListOfCommunicationCount;

                for (var j = 0; j < communicationColumns.length; j++) {
                    dataDetail.push({
                        Name: product.Id + "_ColCommunicationTool_" + j,
                        Value: numeral(numberOfCommunication[currentIndex]).format(crmConstants.Enums.FormatConstant.IntNumber)
                    });
                    currentIndex++;
                }

                dataDetail.push({
                    Name: product.Id + "_ColCommunicationTool_Total",
                    Value: numeral(currentRow.ListOfCommunicationSumByProduct[i]).format(crmConstants.Enums.FormatConstant.IntNumber),
                    TdClass: 'product-total'
                });
            }

            return dataDetail;
        };

        self.getGridOptionWhenCollapsing = function (report, gridOptions, groupHeaders, headers) {
            var productHeaderGroup = [{ Id: 0, Name: viewResources.All_Products.toUpperCase()}];
            self.createReportHeader(groupHeaders, headers, productHeaderGroup, report.CommunicationColumns);

            var bodyData = [];
            self.createReportBodyWhenCollapsing(report, bodyData);

            var footerData = [];
            self.createReportFooterWhenCollapsing(report, footerData);

            self.setDataForGridOptions(gridOptions, groupHeaders, headers, bodyData, footerData);
        };
        self.createReportBodyWhenCollapsing = function (report, bodyData) {
            var mainCategories = report.MainCategories;
            for (var i = 0; i < mainCategories.length; i++) {
                var mainCategory = mainCategories[i];

                var subCategories = mainCategory.SubCategories;
                var subCategoryRows = [];
                for (var j = 0; j < subCategories.length; j++) {
                    subCategoryRows.push({ Columns: self.getRowValueWhenCollapse(subCategories[j], report.ProductColumns, report.CommunicationColumns) });
                }

                bodyData.push({
                    Columns: self.getRowValueWhenCollapse(mainCategory, report.ProductColumns, report.CommunicationColumns),
                    SubRows: subCategoryRows
                });
            }
        };
        self.createReportFooterWhenCollapsing = function (report, footerData) {
            var grandTotal = {
                Name: viewResources.Grand_Total,
                ListOfCommunicationCount: report.GrandTotalOfCommunicationCount,
                ListOfCommunicationSumByProduct: report.GrandTotalOfCommunicationSumByProduct
            };
            footerData.push({
                Columns: self.getRowValueWhenCollapse(grandTotal, report.ProductColumns, report.CommunicationColumns)
            });
        };
        self.getRowValueWhenCollapse = function (currentRow, productColumns, communicationColumns) {
            var dataDetail = [{ Name: "ColComplaints_Enquiries", Value: currentRow.Name}];

            var listOfcommunicationSum = [];
            var grandTotal = 0;

            var communicationColumnCount = communicationColumns.length;

            var listOfCommunicationCount = currentRow.ListOfCommunicationCount;

            for (var i = 0; i < communicationColumnCount; i++) {
                listOfcommunicationSum[i] = 0;
                for (var j = i; j < listOfCommunicationCount.length; j = j + communicationColumnCount) {
                    listOfcommunicationSum[i] += listOfCommunicationCount[j];
                }

                grandTotal += listOfcommunicationSum[i];

                dataDetail.push({
                    Name: "0_ColCommunicationTool_" + i,
                    Value: numeral(listOfcommunicationSum[i]).format(crmConstants.Enums.FormatConstant.IntNumber)
                });
            }

            dataDetail.push({
                Name: "0_ColCommunicationTool_Total",
                Value: numeral(grandTotal).format(crmConstants.Enums.FormatConstant.IntNumber),
                TdClass: 'product-total'
            });

            return dataDetail;
        };

        self.createReportHeader = function (groupHeaders, headers, productColumns, communicationColumns) {
            for (var i = 0; i < productColumns.length; i++) {
                var product = productColumns[i];
                groupHeaders.push({
                    HeaderText: productColumns[i].Name,
                    Name: product.Id + "_ColCommunicationTool_0",
                    MergeColNumber: communicationColumns.length + 1
                });

                self.createCommunicationHeader(headers, product, communicationColumns);

                headers.push({
                    HeaderText: viewResources.Total,
                    Width: 120,
                    Name: product.Id + "_ColCommunicationTool_Total",
                    HeaderAlign: "center",
                    CellAlign: "center"
                });
            }
        };
        self.createCommunicationHeader = function (headers, product, communicationColumns) {
            for (var j = 0; j < communicationColumns.length; j++) {
                headers.push({
                    HeaderText: communicationColumns[j].Name,
                    Width: 120,
                    Name: product.Id + "_ColCommunicationTool_" + j,
                    HeaderAlign: "center",
                    CellAlign: "center"
                });
            }
        }
        self.setDataForGridOptions = function (gridOptions, groupHeaders, headers, bodyData, footerData) {
            gridOptions.groupHeaderRows = [{ Columns: groupHeaders}];
            gridOptions.columns = headers;
            gridOptions.bodyRows = bodyData;
            gridOptions.footerRows = footerData;
        };

        self.viewChart = function (scope, report) {
            var crmChart = parent.crmChart;
            crmChart.title = report ? report.Name : "";
            var chartInfo = report;
            var chartData = {
                labels: self.getChartLabels(chartInfo, crmChart),
                datasets: self.getChartDatasets(chartInfo, crmChart)
            };

            var chartOption = angular.copy(crmChart.newopts);
            chartOption = $.extend(chartOption, {
                chartType: crmChart.chartType.bar,
                xAxisLabel: "",
                yAxisLabel: "",
                yAxisUnit: viewResources.DailyStatisticsReport_ChartSuffix
            });

            crmChart.viewChart(chartData, chartOption);
            parent.$(".ui-dialog-titlebar-close").click(function () {
                //scope.setFocus();
            });
        };

        self.getChartLabels = function (chartInfo) {
            var xAxisCategories = [];
            var communicationColumnsCount = chartInfo ? chartInfo.CommunicationColumns.length : 0;
            for (var i = 0; i < communicationColumnsCount; i++) {
                var communication = chartInfo.CommunicationColumns[i];
                xAxisCategories.push(communication.Name);
            }

            return xAxisCategories;
        };
        self.getChartDatasets = function (chartInfo, crmChart) {
            var chartSeries = [];
            var chartSerie = null;
            var mainCategoriesCount = chartInfo ? chartInfo.MainCategories.length : 0;
            for (var xIndex = 0; xIndex < mainCategoriesCount; xIndex++) {
                var mainCategory = chartInfo.MainCategories[xIndex];
                var backgroundColor = crmChart.getBackGroundColor(xIndex);
                chartSerie = {
                    fillColor: backgroundColor,
                    strokeColor: backgroundColor,
                    data: mainCategory.ListOfCommunicationCount,
                    title: mainCategory.Name
                };
                chartSeries.push(chartSerie);
            }

            return chartSeries;
        };
    };

    window.reportDirectiveHelper = new ReportDirectiveHelper();
})();