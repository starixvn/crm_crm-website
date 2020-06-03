(function () {
    "use strict";
    /* global crmConstants,
    viewResources,
    sharedScriptManager */
    /* jshint camelcase: false */
    angular.module('picmanagement-application').directive('tableReportContainer', function () {
        var cachedData = {
            scope: null,
            reportTable: null,
            customerReport: null,
            gridOptions: null
        };

        function getGridHeight() {
            return crmConstants.MarginBottomPage + (cachedData.scope.isShowFilter ? 230 : 50);
        }

        function getGripOtions(report, communicationTools) {
            var bodyData = [];
            var reportLength = report ? report.DataList.length : 0; 
            if (reportLength > 0) {
                for (var i = 0; i < reportLength; i++) {
                    var data = report.DataList[i];
                    var no = i + 1 + cachedData.scope.lastSelectedParams.PageSize * (cachedData.scope.lastSelectedParams.PageIndex - 1);
                    var checked = data.IsActive == true ? "checked" : "";

                    var columns = [
                        { Name: "ColNo", Value: no },
                        { Name: "ColUsername", Value: data.Username },
                        { Name: "ColRole", Value: data.Role },
                        { Name: "ColLanguage", Value: data.Language },
                        { Name: "ColCountry", Value: data.Country },
                        { Name: "ColGroupHeadline", Value: data.GroupHeadline },
                    ];

                    
                    for (var j = 0; j < communicationTools.length; j++) {
                        columns.push({
                            Name: "Col" + communicationTools[j].Name,
                            Value: GetCommunicationToolValue(data, communicationTools[j].Id)
                        })
                    }

                    columns.push({
                        Name: "ColRemark",
                        Value: data.Remark
                    })

                    if (isHostOnPICManagement) {
                        columns.push({
                            Name: "ColEdit",
                            Value: '<a class="c hand editSetting" href="JavaScript:void(0);" data-row-index="' + i + '"></a>'
                        })

                        columns.push({
                            Name: "ColActive",
                            Value: '<input type="checkbox" class="activeSetting" ' + checked + ' data-row-index="' + i + '" />'
                        })
                    }

                    bodyData.push({
                        Columns: columns
                    });
                }
            }

            var groupHeaderRowColumns = [
                { HeaderText: viewResources.No, Width: 50, Name: "ColNo", HeaderAlign: "center", CellAlign: "center", MergeRowNumber: 2 },
                { HeaderText: viewResources.Username, Width: 120, Name: "ColUsername", HeaderAlign: "center", CellAlign: "left", MergeRowNumber: 2 },
                { HeaderText: viewResources.Role, Width: 60, Name: "ColRole", HeaderAlign: "center", CellAlign: "center", MergeRowNumber: 2 },
                { HeaderText: viewResources.Language, Width: 120, Name: "ColLanguage", HeaderAlign: "center", CellAlign: "center", MergeRowNumber: 2 },
                { HeaderText: viewResources.Country, Width: 120, Name: "ColCountry", HeaderAlign: "center", CellAlign: "center", MergeRowNumber: 2 },
                { HeaderText: viewResources.GroupHeadline, Width: 80, Name: "ColGroupHeadline", HeaderAlign: "center", CellAlign: "center", MergeRowNumber: 2 },
                { HeaderText: viewResources.CommunicationTool, Name: "ColUsername", MergeColNumber: communicationTools.length },
                { HeaderText: viewResources.Remark, Width: 200, Name: "ColRemark", HeaderAlign: "center", CellAlign: "center", MergeRowNumber: 2 }
            ];

            if (isHostOnPICManagement) {
                groupHeaderRowColumns.push(
                    { HeaderText: viewResources.Edit, Width: 50, Name: "ColEdit", HeaderAlign: "center", CellAlign: "center", MergeRowNumber: 2 }
                );

                groupHeaderRowColumns.push(
                    { HeaderText: viewResources.Active, Width: 50, Name: "ColActive", HeaderAlign: "center", CellAlign: "center", MergeRowNumber: 2 }
                );
            }

            var groupHeaderRows = [
                {
                    Columns: groupHeaderRowColumns
                }                
            ];

            var headerColumns = [
                { HeaderText: viewResources.No, Width: 50, Name: "ColNo", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Username, Width: 120, Name: "ColUsername", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Role, Width: 60, Name: "ColRole", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Language, Width: 120, Name: "ColLanguage", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Country, Width: 120, Name: "ColCountry", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.GroupHeadline, Width: 80, Name: "ColGroupHeadline", HeaderAlign: "center", CellAlign: "left" },
            ];

            for (var j = 0; j < communicationTools.length; j++) {
                headerColumns.push(
                    { HeaderText: communicationTools[j].Name, Width: 120, Name: "Col" + communicationTools[j].Name, HeaderAlign: "center", CellAlign: "left" }
                );
            }

            headerColumns.push({ HeaderText: viewResources.Remark, Width: 200, Name: "ColRemark", HeaderAlign: "center", CellAlign: "left" });

            if (isHostOnPICManagement) {
                headerColumns.push({ HeaderText: viewResources.Edit, Width: 50, Name: "ColEdit", HeaderAlign: "center", CellAlign: "center" });

                headerColumns.push({ HeaderText: viewResources.Active, Width: 50, Name: "ColActive", HeaderAlign: "center", CellAlign: "center" });
            }

            var gridOptions = {
                customizeHeader: true,
                groupHeaderRows: groupHeaderRows,
                columns: headerColumns,
                showTitle: true,
                titleInfo: {
                    text: viewResources.ReportTitle,
                    showExcelButton: true,
                    exportExcelFunction: function () {
                        cachedData.scope.exportReport();
                    }
                },
                bodyRows: bodyData,
                gridExpandHeight: 200,
                showPager: true,
                customPager: true,
                //allowTruncate: true,
                pagerOption: {
                    itemsPerPage: cachedData.scope.lastSelectedParams.PageSize,
                    currentPage: cachedData.scope.lastSelectedParams.PageIndex - 1,
                    itemsPerPageArray: [100, 200, 300, 400, 500, 1000],
                    totalItem: report.Total,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            return gridOptions;
        }

        function GetCommunicationToolValue(item, settingId) {
            if (item.CommunicationTools) {
                var communicationTool = item.CommunicationTools.find(c => c.SettingId == settingId);
                if (communicationTool) {
                    return communicationTool.Value;
                }
            }

            return '';
        }

        function ReRenderAfterPaged(page_index, itemsPerPage) {
            cachedData.scope.pagingCustomerReport(page_index + 1, itemsPerPage);
            return false;
        }

        function renderCustomerReport(scope, element) {
            if (scope.reportOptions.DataList === null) {
                return;
            }

            cachedData.scope = scope;
            var tableCustomerReport = element.find("#table-report");
            cachedData.reportTable = tableCustomerReport;
            var customerReport = scope.reportOptions.DataList;
            var communicationTools = scope.communicationTools;
            cachedData.customerReport = customerReport;
            var gridOptions = getGripOtions(customerReport, communicationTools);
            cachedData.gridOptions = gridOptions;
            tableCustomerReport.empty().removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);

            var editSettings = tableCustomerReport.find(".editSetting");
            editSettings.unbind("click");
            editSettings.click(function () {
                var rowIndex = parseInt($(this).attr("data-row-index"));
                scope.editPic(customerReport.DataList[rowIndex]);
            });

            var activeSettings = tableCustomerReport.find(".activeSetting");
            activeSettings.unbind("click");
            activeSettings.click(function () {
                var rowIndex = parseInt($(this).attr("data-row-index"));
                scope.changePicState(customerReport.DataList[rowIndex]);
            });
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    if (!scope.reportOptions.IsUpdateGrid) {
                        return;
                    }

                    if (scope.reportOptions.IsResize && cachedData.reportTable) {
                        cachedData.reportTable.fanexGrid("resizeHeight", getGridHeight());
                        scope.customerReportOptions.IsResize = false;
                    } else {
                        renderCustomerReport(scope, element);
                    }

                    scope.reportOptions.IsUpdateGrid = false;
                });
            }
        };
    });
})();