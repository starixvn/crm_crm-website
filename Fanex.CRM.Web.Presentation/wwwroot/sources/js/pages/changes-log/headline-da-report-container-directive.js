(function () {
    "use strict";
    angular.module('changes-log-application').directive('headlineDaTableReportContainer', function () {
        let cachedData = {
            scope: null,
            reportTable: null,
            headlineDAReport: null,
            gridOptions: null
        };

        function getGridHeight() {
            return crmConstants.MarginBottomPage + 180;
        }

        function getGripOtions(report) {
            let bodyData = [];
            let reportLength = report ? report.DataList.length : 0; 
            if (reportLength > 0) {
                for (let i = 0; i < reportLength; i++) {
                    let data = report.DataList[i];
                    let no = i + 1 + cachedData.scope.lastHeadlineDASelectedParams.PageSize * (cachedData.scope.lastHeadlineDASelectedParams.PageIndex - 1);
                    console.log(data)
                    let detailValue = '';
                    if (data.Action.toLowerCase().includes('remove')) {
                        let value = preExecutePics(data.NewDAInfo.Pics);
                        detailValue = `<div class="container-detail full-height border-bottom-none"><div class="detail-child-col col-title-detail"><div class="padding-left-5px">PIC</div></div><div class="detail-child-col border-right-none col-value-lager"><div class="padding-left-5px">${value}</div></div></div>`;
                    }
                    else if (data.Action.toLowerCase().includes('assign') || data.Action.toLowerCase().includes('update')) {
                        let groupHeadlineValue = changeValueToHtmlString(preExecuteValue(data.OldDAInfo.GroupHeadline), preExecuteValue(data.NewDAInfo.GroupHeadline));
                        let picsValue = changeValueToHtmlString(preExecutePics(data.OldDAInfo.Pics), preExecutePics(data.NewDAInfo.Pics));
                        detailValue = `<div class="container-detail full-height border-bottom-none"><div class="detail-child-col col-title-detail"><div class="padding-left-5px">Group Headline</div></div><div class="detail-child-col col-value-detail${groupHeadlineValue.backgroundChange}"><div class="padding-left-5px">${groupHeadlineValue.valueChange}</div></div><div class="detail-child-col col-title-detail"><div class="padding-left-5px">PIC</div></div><div class="detail-child-col border-right-none col-value-detail${picsValue.backgroundChange}"><div class="padding-left-5px">${picsValue.valueChange}</div></div></div>`;
                    }

                    let columns = [
                        { Name: "ColNo", Value: no, CellAlign: "center" },
                        { Name: "ColLogId", Value: data.LogId, CellAlign: "center" },
                        { Name: "ColLogTime", Value: data.LogTime, CellAlign: "center" },
                        { Name: "ColEditBy", Value: data.EditBy, CellAlign: "center" },
                        { Name: "ColHeadlineDA", Value: data.DirectAccount, CellAlign: "center" },
                        { Name: "ColAction", Value: data.Action, CellAlign: "left" },
                        { Name: "ColDetail", Value: detailValue },
                    ];

                    bodyData.push({
                        Columns: columns
                    });
                }
            }

            let headerColumns = [
                { HeaderText: viewResources.No, Width: 50, Name: "ColNo", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.LogId, Width: 80, Name: "ColLogId", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.LogDate, Width: 120, Name: "ColLogTime", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.EditBy, Width: 80, Name: "ColEditBy", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.HeadlineDA, Width: 120, Name: "ColHeadlineDA", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Action, Width: 250, Name: "ColAction", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Detail, Width: 600, Name: "ColDetail", HeaderAlign: "center", CellAlign: "center" },
            ];

            let gridOptions = {
                columns: headerColumns,
                headerColumnHeight: 50,
                showTitle: true,
                titleInfo: {
                    text: viewResources.ReportTitle
                },
                bodyRows: bodyData,
                gridExpandHeight: getGridHeight(),
                showPager: true,
                customPager: true,
                pagerOption: {
                    itemsPerPage: cachedData.scope.lastHeadlineDASelectedParams.PageSize,
                    currentPage: cachedData.scope.lastHeadlineDASelectedParams.PageIndex - 1,
                    itemsPerPageArray: [100, 200, 300, 400, 500, 1000],
                    totalItem: report.Totals,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            return gridOptions;
        }

        function changeValueToHtmlString(oldValue, newValue) {
            let isChange = newValue !== oldValue;
            let valueChange = isChange ? `<div class="new-value"><div class="padding-left-5px">${newValue}</div></div><div class="old-value"><div class="padding-left-5px">${oldValue}</div></div>` : `<div><div class="padding-left-5px">${newValue}</div></div><div><div class="padding-left-5px">${oldValue}</div></div>`;
            let backgroundChange = isChange ? ' background-change' : '';

            return {
                valueChange,
                backgroundChange
            };
        }

        function convertBoolToDecicion(value) {
            if (value === true) {
                return 'Yes';
            }
            else {
                return 'No';
            }

            return '-';
        }

        function preExecuteValue(value) {
            if (value === null) {
                return '-';
            }

            return value;
        }

        function preExecutePics(pics) {
            if (pics && pics.length > 0) {
                return pics.map(pic => `${pic.PicName} (${pic.RoleName})`).join(', ');
            }

            return '-';
        }

        function ReRenderAfterPaged(page_index, itemsPerPage) {
            cachedData.scope.pagingHeadlineDAReport(page_index + 1, itemsPerPage);
            return false;
        }

        function renderHeadlineDAReport(scope, element) {
            if (scope.headlineDAReportOptions.DataList !== null) {
                cachedData.scope = scope;
                let tableHeadlineDAReport = element.find("#headline-da-table-report");
                cachedData.reportTable = tableHeadlineDAReport;
                let headlineDAReport = scope.headlineDAReportOptions.DataList;
                cachedData.headlineDAReport = headlineDAReport;
                let gridOptions = getGripOtions(headlineDAReport);
                cachedData.gridOptions = gridOptions;
                tableHeadlineDAReport.empty().removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
            }
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    if (scope.headlineDAReportOptions.IsUpdateGrid) {
                        renderHeadlineDAReport(scope, element);
                        scope.headlineDAReportOptions.IsUpdateGrid = false;
                    }
                });
            }
        };
    });
})();