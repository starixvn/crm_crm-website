(function () {
    "use strict";
    /* global
    casinoDailyEnhancementResources,
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
                HeaderText: casinoDailyEnhancementResources.Complaints_Enquiries,
                Name: "ColComplaints_Enquiries",
                MergeRowNumber: 2
            }
        ];
            var headers = [
            {
                HeaderText: casinoDailyEnhancementResources.Complaints_Enquiries,
                Width: 300,
                Name: "ColComplaints_Enquiries",
                HeaderAlign: "center",
                CellAlign: "left",
                ShowCollapseExpand: true
            }
        ];

            var horizontalExpansion = '<input class="btnExpansion " type="button" title="' + "Expand / Collapse Horizontally" + '" />';
            var viewChartIcon = '<input class="btnCharting " type="button" title="' + casinoDailyEnhancementResources.ViewChart + '" />';
            var viewDetailIcon = '<input class="btnViewDetail " type="button" title="' + casinoDailyEnhancementResources.ViewDetail + '" />';

            var gridOptions = {
                customizeHeader: true,
                showTitle: true,
                titleInfo: {
                    text: report ? report.Name : "",
                    showExcelButton: true,
                    exportExcelFunction: null,
                    showCollapseExpandButton: true,
                    customButtons: horizontalExpansion + viewChartIcon + viewDetailIcon
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

            if (isExpand) {
                self.getGridOptionWhenExpanding(report, gridOptions, groupHeaders, headers);
            } else {
                self.getGridOptionWhenCollapsing(report, gridOptions, groupHeaders, headers);
            }

            return gridOptions;
        };

        self.getGridOptionWhenExpanding = function (report, gridOptions, groupHeaders, headers) {
            self.createReportHeaderWhenExpanding(groupHeaders, headers, report.ProductColumns, report.CommunicationColumns);
            groupHeaders.push({
                HeaderText: "Grand Total",
                Name: "GrandTotal",
                MergeRowNumber: 2
            });
            headers.push({
                HeaderText: "Grand Total",
                Width: 100,
                Name: "GrandTotal",
                HeaderAlign: "center",
                CellAlign: "center"
            });

            var bodyData = [];
            self.createReportBodyWhenExpanding(report, bodyData);

            var footerData = [];
            self.createReportFooterWhenExpanding(report, footerData);

            self.setDataForGridOptions(gridOptions, groupHeaders, headers, bodyData, footerData);
        };
        self.createReportHeaderWhenExpanding = function (groupHeaders, headers, productColumns, communicationColumns) {
            for (var i = 0; i < productColumns.length; i++) {
                var product = productColumns[i];
                groupHeaders.push({
                    HeaderText: productColumns[i].Name,
                    Name: product.Id + "_ColCommunicationTool_0",
                    MergeColNumber: communicationColumns.length + 1
                });

                self.createCommunicationHeader(headers, product, communicationColumns);

                headers.push({
                    HeaderText: casinoDailyEnhancementResources.Grand_Total,
                    Width: 120,
                    Name: product.Id + "_ColCommunicationTool_GrandTotal",
                    HeaderAlign: "center",
                    CellAlign: "center"
                });
            }
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
                Name: "Grand Total",
                ListOfCommunicationCount: report.GrandTotalOfCommunicationCount,
                ListOfCommunicationSumByProduct: report.GrandTotalOfCommunicationSumByProduct,
                TotalCommunications: report.GrandTotalAllCommunications
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
                    Name: product.Id + "_ColCommunicationTool_GrandTotal",
                    Value: numeral(currentRow.ListOfCommunicationSumByProduct[i]).format(crmConstants.Enums.FormatConstant.IntNumber)
                });
            }

            dataDetail.push({
                Name: "GrandTotal",
                Value: numeral(currentRow.TotalCommunications).format(crmConstants.Enums.FormatConstant.IntNumber)
            });

            return dataDetail;
        };

        self.getGridOptionWhenCollapsing = function (report, gridOptions, groupHeaders, headers) {
            var productHeaderGroup = { Id: 0, Name: "ALL PRODUCTS" };
            self.createReportHeaderWhenCollapsing(groupHeaders, headers, productHeaderGroup, report.CommunicationColumns);

            var bodyData = [];
            self.createReportBodyWhenCollapsing(report, bodyData);

            var footerData = [];
            self.createReportFooterWhenCollapsing(report, footerData);

            self.setDataForGridOptions(gridOptions, groupHeaders, headers, bodyData, footerData);
        };
        self.createReportHeaderWhenCollapsing = function (groupHeaders, headers, productHeaderGroup, communicationColumns) {
            groupHeaders.push({
                HeaderText: productHeaderGroup.Name,
                Name: productHeaderGroup.Id + "_ColCommunicationTool_0",
                MergeColNumber: communicationColumns.length
            });

            self.createCommunicationHeader(headers, productHeaderGroup, communicationColumns);

            groupHeaders.push({
                HeaderText: casinoDailyEnhancementResources.Grand_Total,
                Name: productHeaderGroup.Id + "_ColCommunicationTool_GrandTotal",
                MergeRowNumber: 2
            });
            headers.push({
                HeaderText: casinoDailyEnhancementResources.Grand_Total,
                Width: 120,
                Name: productHeaderGroup.Id + "_ColCommunicationTool_GrandTotal",
                HeaderAlign: "center",
                CellAlign: "center"
            });
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
                Name: "Grand Total",
                ListOfCommunicationCount: report.GrandTotalOfCommunicationCount,
                ListOfCommunicationSumByProduct: report.GrandTotalOfCommunicationSumByProduct,
                TotalCommunications: report.GrandTotalAllCommunications
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
                Name: "0_ColCommunicationTool_GrandTotal",
                Value: numeral(grandTotal).format(crmConstants.Enums.FormatConstant.IntNumber)
            });

            return dataDetail;
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
                yAxisUnit: casinoDailyEnhancementResources.DailyStatisticsReport_ChartSuffix
            });

            crmChart.viewChart(chartData, chartOption);
            parent.$(".ui-dialog-titlebar-close").click(function () {
                scope.setFocus();
            });
        };

        self.getChartLabels = function (chartInfo) {
            var xAxisCategories = [];
            var communicationToolLength = chartInfo ? chartInfo.CommunicationTools.length : 0;
            for (var i = 0; i < communicationToolLength; i++) {
                var communicationTool = chartInfo.CommunicationTools[i];
                xAxisCategories.push(communicationTool.Name);
            }

            return xAxisCategories;
        };
        self.getChartDatasets = function (chartInfo, crmChart) {
            var chartSeries = [];
            var chartSerie = null;
            var childReportLength = chartInfo ? chartInfo.SubCategories.length : 0;
            for (var xIndex = 0; xIndex < childReportLength; xIndex++) {
                var mainCategoryReport = chartInfo.SubCategories[xIndex];
                var backgroundColor = crmChart.getBackGroundColor(xIndex);
                chartSerie = {
                    fillColor: backgroundColor,
                    strokeColor: backgroundColor,
                    data: mainCategoryReport.NumberOfIssueList,
                    title: mainCategoryReport.Name
                };
                chartSeries.push(chartSerie);
            }

            return chartSeries;
        };
    };

    window.reportDirectiveHelper = new ReportDirectiveHelper();
})();