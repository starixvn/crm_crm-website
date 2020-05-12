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
            var footerData = [];
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
                        { Name: "ColGroupHeadline", Value: data.GroupHeadline },
                    ];

                    
                    for (var j = 0; j < communicationTools.length; j++) {
                        console.log("Col" + communicationTools[j].Name);
                        columns.push({
                            Name: "Col" + communicationTools[j].Name,
                            Value: GetCommunicationToolValue(data, communicationTools[j].Id)
                        })
                    }

                    columns.push({
                        Name: "ColEdit",
                        Value: data.Edit
                    })
                    columns.push({
                        Name: "ColActive",
                        Value: '<input type="checkbox" class="activeSetting" ' + checked + ' data-row-index="' + i + '" />'
                    })

                    bodyData.push({
                        Columns: columns
                    });
                }
            }

            var groupHeaderRows = [
                {
                    Columns: [
                        { HeaderText: viewResources.No, Width: 120, Name: "ColNo", HeaderAlign: "center", CellAlign: "center", MergeRowNumber: 2 },
                        { HeaderText: viewResources.Username, Width: 120, Name: "ColUsername", HeaderAlign: "center", CellAlign: "left", MergeRowNumber: 2  },
                        { HeaderText: viewResources.Role, Width: 120, Name: "ColRole", HeaderAlign: "center", CellAlign: "center", MergeRowNumber: 2  },
                        { HeaderText: viewResources.GroupHeadline, Width: 120, Name: "ColGroupHeadline", HeaderAlign: "center", CellAlign: "center", MergeRowNumber: 2  },
                        { HeaderText: viewResources.CommunicationTool, Name: "ColUsername", MergeColNumber: communicationTools.length },
                        { HeaderText: viewResources.Edit, Width: 120, Name: "ColEdit", HeaderAlign: "center", CellAlign: "left", MergeRowNumber: 2  },
                        { HeaderText: viewResources.Active, Width: 120, Name: "ColActive", HeaderAlign: "center", CellAlign: "left", MergeRowNumber: 2  }
                    ]
                }                
            ];

            var headerColumns = [
                { HeaderText: viewResources.No, Width: 120, Name: "ColNo", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Username, Width: 120, Name: "ColUsername", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Role, Width: 120, Name: "ColRole", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.GroupHeadline, Width: 120, Name: "ColGroupHeadline", HeaderAlign: "center", CellAlign: "center" },
            ];

            for (var j = 0; j < communicationTools.length; j++) {
                headerColumns.push(
                    { HeaderText: communicationTools[j].Name, Width: 120, Name: "Col" + communicationTools[j].Name, HeaderAlign: "center", CellAlign: "left" }
                );
            }

            headerColumns.push({ HeaderText: viewResources.Edit, Width: 120, Name: "ColEdit", HeaderAlign: "center", CellAlign: "left" });
            headerColumns.push({ HeaderText: viewResources.Active, Width: 120, Name: "ColActive", HeaderAlign: "center", CellAlign: "left" });

            var gridOptions = {
                customizeHeader: true,
                groupHeaderRows: groupHeaderRows,
                columns: headerColumns,
                showTitle: true,
                titleInfo: {
                    text: viewResources.ReportTitle,
                    showExcelButton: false
                },
                bodyRows: bodyData,
                footerRows: footerData,
                numberOfFrozenColumn: 0,
                fullWidth: true,
                fullHeight: true,
                gridExpandHeight: getGridHeight(),
                showDistinctRow: true,
                showHighlightRow: true,
                headerColumnHeight: 29,
                bodyRowHeight: 29,
                showPager: true,
                showTree: false,
                customPager: true,
                //allowTruncate: true,
                pagerOption: {
                    itemsPerPage: cachedData.scope.lastSelectedParams.PageSize,
                    currentPage: cachedData.scope.lastSelectedParams.PageIndex - 1,
                    itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
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

            var activeSettings = tableCustomerReport.find(".activeSetting");
            activeSettings.unbind("click");
            activeSettings.click(function () {
                var rowIndex = parseInt($(this).attr("data-row-index"));
                console.log(rowIndex);
                console.log(customerReport);
                scope.changePicState(customerReport.DataList[rowIndex]);
            });
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    /* jshint unused: false */
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