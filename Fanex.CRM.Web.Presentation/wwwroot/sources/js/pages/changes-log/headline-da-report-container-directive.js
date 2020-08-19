/*
 * Global variable: logActions
 */
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
            return crmConstants.MarginBottomPage + 185;
        }

        function getGripOtions(report) {
            let bodyData = [];
            let reportLength = report ? report.DataList.length : 0; 
            if (reportLength > 0) {
                for (let i = 0; i < reportLength; i++) {
                    let data = report.DataList[i];
                    let no = i + 1 + cachedData.scope.lastHeadlineDASelectedParams.PageSize * (cachedData.scope.lastHeadlineDASelectedParams.PageIndex - 1);
                    let detailValue = '';

                    switch (data.ActionId) {
                        case logActions.RemoveDirectAccountFromPic:
                        case logActions.AssignDirectAccountToPic:
                        case logActions.RemoveDefaultPicForAGroupHeadline:
                            detailValue = getDetailValuePic(data);
                            break;
                        case logActions.UpdateThePicOfAHeadline:
                            detailValue = getDetailValueGroupHeadlinePicBoth(data);
                            break;
                        case logActions.UpdateDefaultPicForAGroupHeadline:
                            detailValue = getDetailValuePicBoth(data);
                            break;
                        case logActions.AddCustomerToCreditTermGroup:
                        case logActions.RemoveCustomerFromCreditTermGroup:
                        case logActions.AddTheGeneralInformationForSuperFromExistingSuper:
                            detailValue = getDetailValueGroupHeadline(data)
                            break;
                        case logActions.UpdateCreditTerm:
                            detailValue = getDetailValueIsGroupHeadlineBoth(data);
                            break;
                        case logActions.SetCustomerToCreditTermGroup:
                            detailValue = getDetailValueIsGroupHeadline();
                            break;
                    }

                    let columns = [
                        { Name: "ColNo", Value: `<div class="padding-5px">${no}</div>`, CellAlign: "center" },
                        { Name: "ColLogId", Value: `<div class="padding-5px">${data.LogId}</div>`, CellAlign: "center" },
                        { Name: "ColLogTime", Value: `<div class="padding-5px">${data.LogTime}</div>`, CellAlign: "center" },
                        { Name: "ColEditBy", Value: `<div class="padding-5px">${data.EditBy}</div>`, CellAlign: "center" },
                        { Name: "ColHeadlineDA", Value: `<div class="padding-5px">${data.DirectAccount}</div>`, CellAlign: "center" },
                        { Name: "ColAction", Value: `<div class="padding-5px">${data.Action}</div>`, CellAlign: "left" },
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
            let backgroundChange = isChange ? ' background-change' : '';
            let valueChange = isChange ? `
                <div class="new-value"><div class="padding-5px">${newValue}</div></div>
                <div class="old-value"><div class="padding-5px">${oldValue}</div></div>
                ` : `
                <div><div class="padding-5px">${newValue}</div></div>
                <div><div class="padding-5px">${oldValue}</div></div>`;

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

        function preExecutePics(pics) {
            if (pics && pics.length > 0) {
                return pics.map(pic => `${pic.PicName} (${pic.RoleName})`).join(', ');
            }

            return '-';
        }

        function preExecuteValue(value) {
            if (value) {
                return value;
            }

            return '-';
        }

        function getDetailValuePic(data) {
            let picsValue = preExecutePics(data.NewDAInfo.Pics);

            return `
                <div class="container-detail border-bottom-none min-height-38px">
                    <div class="detail-child-col col-title-detail">
                        <div class="padding-5px">${viewResources.PIC}</div>
                    </div>
                    <div class="detail-child-col border-right-none col-value-larger">
                        <div class="padding-5px">${picsValue}</div>
                    </div>
                </div>                            
            `;
        }

        function getDetailValueGroupHeadlinePicBoth(data) {
            let groupHeadlineValue = changeValueToHtmlString(preExecuteValue(data.OldDAInfo.GroupHeadline), preExecuteValue(data.NewDAInfo.GroupHeadline));
            let picsValue = changeValueToHtmlString(preExecutePics(data.OldDAInfo.Pics), preExecutePics(data.NewDAInfo.Pics));

            return `
                <div class="container-detail border-bottom-none">
                    <div class="detail-child-col col-title-detail">
                        <div class="padding-5px">${viewResources.GroupHeadline}</div>
                    </div>
                    <div class="detail-child-col col-value-detail${groupHeadlineValue.backgroundChange}">
                    ${groupHeadlineValue.valueChange}
                    </div>
                    <div class="detail-child-col col-title-detail">
                        <div class="padding-5px">${viewResources.PIC}</div>
                    </div>
                    <div class="detail-child-col col-value-detail border-right-none${picsValue.backgroundChange}">
                    ${picsValue.valueChange}
                    </div>
                </div>                            
            `;
        }

        function getDetailValuePicBoth(data) {
            let picsValue = changeValueToHtmlString(preExecutePics(data.OldDAInfo.Pics), preExecutePics(data.NewDAInfo.Pics));

            return `
                <div class="container-detail border-bottom-none">
                    <div class="detail-child-col col-title-detail">
                        <div class="padding-5px">${viewResources.PIC}</div>
                    </div>
                    <div class="detail-child-col border-right-none col-value-larger${picsValue.backgroundChange}">
                    ${picsValue.valueChange}
                    </div>
                </div>                            
            `;
        }

        function getDetailValueGroupHeadline(data) {
            return `
                <div class="container-detail border-bottom-none">
                    <div class="detail-child-col col-title-detail min-height-38px">
                        <div class="padding-5px">${viewResources.GroupHeadline}</div>
                    </div>
                    <div class="detail-child-col border-right-none col-value-larger">
                        <div class="padding-5px">${data.NewDAInfo.GroupHeadline}</div>
                    </div>
                </div>                            
            `;
        }

        function getDetailValueIsGroupHeadlineBoth(data) {
            let groupHeadlineValue = changeValueToHtmlString(preExecuteValue(data.OldDAInfo.GroupHeadline), data.NewDAInfo.GroupHeadline)
            let isGroupHeadlineValue = changeValueToHtmlString(convertBoolToDecicion(data.OldDAInfo.IsGroupHeadline), convertBoolToDecicion(data.NewDAInfo.IsGroupHeadline));
            return `
                <div class="container-detail border-bottom-none">
                    <div class="detail-child-col col-title-detail">
                        <div class="padding-5px">${viewResources.GroupHeadline}</div>
                    </div>
                    <div class="detail-child-col col-value-detail${groupHeadlineValue.backgroundChange}">
                    ${groupHeadlineValue.valueChange}
                    </div>
                    <div class="detail-child-col col-title-detail">
                        <div class="padding-5px">${viewResources.IsGroupHeadline}</div>
                    </div>
                    <div class="detail-child-col col-value-detail border-right-none${isGroupHeadlineValue.backgroundChange}">
                    ${isGroupHeadlineValue.valueChange}
                    </div>
                </div>                            
            `;
        }

        function getDetailValueIsGroupHeadline() {
            return `
                <div class="container-detail border-bottom-none">
                    <div class="detail-child-col col-title-detail min-height-38px">
                        <div class="padding-5px">${viewResources.IsGroupHeadline}</div>
                    </div>
                    <div class="detail-child-col border-right-none col-value-larger">
                        <div class="padding-5px">Yes</div>
                    </div>
                </div>                            
            `;
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