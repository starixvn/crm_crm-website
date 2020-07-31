/*
 * Global variable: logActions
 */
(function () {
    "use strict";
    angular.module('changes-log-application').directive('picTableReportContainer', function () {
        let cachedData = {
            scope: null,
            reportTable: null,
            picReport: null,
            gridOptions: null
        };

        function getGridHeight() {
            return crmConstants.MarginBottomPage + 140;
        }

        function getGripOtions(report) {
            let bodyData = [];
            let reportLength = report ? report.DataList.length : 0;
            if (reportLength > 0) {
                for (let i = 0; i < reportLength; i++) {
                    let data = report.DataList[i];
                    let no = i + 1 + cachedData.scope.lastPicSelectedParams.PageSize * (cachedData.scope.lastPicSelectedParams.PageIndex - 1);
                    let detailValue = '';

                    switch (data.ActionId) {
                        case logActions.InsertNewPic:
                            detailValue = getDetailValueInsertNewPic(data);
                            break;
                        case logActions.UpdatePic:
                            detailValue = getDetailValueUpdatePic(data);
                            break;
                        case logActions.UpdatePicStatus:
                            detailValue = getDetailValueUpdatePicStatus(data);
                            break;
                    }

                    let columns = [
                        { Name: "ColNo", Value: `<div class="padding-5px">${no}</div>`, CellAlign: "center" },
                        { Name: "ColLogId", Value: `<div class="padding-5px">${data.LogId}</div>`, CellAlign: "center" },
                        { Name: "ColLogTime", Value: `<div class="padding-5px">${data.LogTime}</div>`, CellAlign: "center" },
                        { Name: "ColEditBy", Value: `<div class="padding-5px">${data.EditBy}</div>`, CellAlign: "center" },
                        { Name: "ColPIC", Value: `<div class="padding-5px">${data.PIC}</div>`, CellAlign: "center" },
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
                { HeaderText: viewResources.PIC, Width: 120, Name: "ColPIC", HeaderAlign: "center", CellAlign: "center" },
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
                    itemsPerPage: cachedData.scope.lastPicSelectedParams.PageSize,
                    currentPage: cachedData.scope.lastPicSelectedParams.PageIndex - 1,
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

        function preExecuteObject(object) {
            if (object && object.Name) {
                return object.Name;
            }
            else if (object && object.Value) {
                return object.Name;
            }

            return '-';
        }

        function preExecuteCommTools(tools) {
            if (tools.length > 0) {
                let name = tools[0].Name;
                let value = tools.map(tool => tool.Value).join(', ');
                let result = `<div>${name}: ${value}</div>`;
                return result;
            }

            return '-';
        }

        function preExcecuteArray(arr) {
            if (arr.length > 0) {
                return arr.map(e => e.Name).join(', ');
            }

            return '-';
        }

        function recursiveTool(tools, result) {
            if (tools.length > 0) {
                let pivot = tools[0].Name;
                let childTools = tools.filter(tool => tool.Name === pivot);
                result.push(childTools)
                let moreTools = tools.filter(tool => tool.Name !== pivot);
                recursiveTool(moreTools, result);
            }
        }

        function getDetailValueUpdatePicStatus(data) {
            let directAccountValue = changeValueToHtmlString(preExcecuteArray(data.OldPicInfo.DirectAccounts), preExcecuteArray(data.NewPicInfo.DirectAccounts));
            let isActiveValue = changeValueToHtmlString(convertBoolToDecicion(data.OldPicInfo.IsActive), convertBoolToDecicion(data.NewPicInfo.IsActive));

            return `
                <div class="container-detail border-bottom-none">
                    <div class="detail-child-col col-title-detail">
                    <div class="padding-5px">${viewResources.DirectAccount}</div>
                    </div>
                    <div
                    class="detail-child-col col-value-detail${directAccountValue.backgroundChange}"
                    >
                    ${directAccountValue.valueChange}
                    </div>
                    <div class="detail-child-col col-title-detail">
                    <div class="padding-5px">${viewResources.IsActive}</div>
                    </div>
                    <div
                    class="detail-child-col col-value-detail border-right-none${isActiveValue.backgroundChange}"
                    >
                    ${isActiveValue.valueChange}
                    </div>
                </div>
            `;
        }

        function getDetailValueInsertNewPic(data) {
            let languages = preExcecuteArray(data.NewPicInfo.Languages);
            let countryValue = data.NewPicInfo.Country ? data.NewPicInfo.Country.Name : '-';
            let newCommunicationTool = [];
            recursiveTool(data.NewPicInfo.CommunicationTools, newCommunicationTool);
            let newCommunicationToolValue = newCommunicationTool.map(tool => preExecuteCommTools(tool)).join('');

            return `
                <div class="container-detail">
                    <div class="detail-child-col col-title-detail">
                    <div class="padding-5px">${viewResources.Name}</div>
                    </div>
                    <div class="detail-child-col col-value-detail">
                    <div class="padding-5px">${data.NewPicInfo.Name}</div>
                    </div>
                    <div class="detail-child-col col-title-detail">
                    <div class="padding-5px">${viewResources.Role}</div>
                    </div>
                    <div class="detail-child-col col-value-detail border-right-none">
                    <div class="padding-5px">${data.NewPicInfo.Role.Name}</div>
                    </div>
                </div>
                <div class="container-detail">
                    <div class="detail-child-col col-title-detail">
                    <div class="padding-5px">${viewResources.Language}</div>
                    </div>
                    <div class="detail-child-col col-value-detail">
                    <div class="padding-5px">${languages}</div>
                    </div>
                    <div class="detail-child-col col-title-detail">
                    <div class="padding-5px">${viewResources.Country}</div>
                    </div>
                    <div class="detail-child-col col-value-detail border-right-none">
                    <div>
                        <div class="padding-5px">${countryValue}</div>
                    </div>
                    </div>
                </div>
                <div class="container-detail border-bottom-none">
                    <div class="detail-child-col col-title-detail">
                    <div class="padding-5px">${viewResources.CommunicationTool}</div>
                    </div>
                    <div class="detail-child-col col-value-detail">
                    <div class="padding-5px">${newCommunicationToolValue}</div>
                    </div>
                    <div class="detail-child-col col-title-detail">
                    <div class="padding-5px">${viewResources.IsActive}</div>
                    </div>
                    <div class="detail-child-col col-value-detail border-right-none">
                    <div>
                        <div class="padding-5px">Yes</div>
                    </div>
                    </div>
                </div>
            `;
        }

        function getDetailValueUpdatePic(data) {
            let roleValue = changeValueToHtmlString(preExecuteObject(data.OldPicInfo.Role), preExecuteObject(data.NewPicInfo.Role));
            let nameValue = changeValueToHtmlString(data.OldPicInfo.Name, data.NewPicInfo.Name);
            let newLanguage = preExcecuteArray(data.NewPicInfo.Languages);
            let oldLanguage = preExcecuteArray(data.OldPicInfo.Languages);
            let languageValue = changeValueToHtmlString(oldLanguage, newLanguage)
            let countryValue = changeValueToHtmlString(preExecuteObject(data.OldPicInfo.Country), preExecuteObject(data.NewPicInfo.Country));
            let newCommunicationTool = [];
            recursiveTool(data.NewPicInfo.CommunicationTools, newCommunicationTool);
            let newCommunicationToolValue = newCommunicationTool.map(tool => preExecuteCommTools(tool)).join('');
            let oldCommunicationTool = [];
            recursiveTool(data.OldPicInfo.CommunicationTools, oldCommunicationTool);
            let oldCommunicationToolValue = oldCommunicationTool.map(tool => preExecuteCommTools(tool)).join('');
            let communicationToolValue = changeValueToHtmlString(oldCommunicationToolValue, newCommunicationToolValue);

            return `
                <div class="container-detail">
                    <div class="detail-child-col col-title-detail">
                    <div class="padding-5px">${viewResources.Name}</div>
                    </div>
                    <div class="detail-child-col col-value-detail${nameValue.backgroundChange}">
                    ${nameValue.valueChange}
                    </div>
                    <div class="detail-child-col col-title-detail">
                    <div class="padding-5px">${viewResources.Role}</div>
                    </div>
                    <div
                    class="detail-child-col col-value-detail border-right-none${roleValue.backgroundChange}"
                    >
                    ${roleValue.valueChange}
                    </div>
                </div>
                <div class="container-detail">
                    <div class="detail-child-col col-title-detail">
                    <div class="padding-5px">${viewResources.Language}</div>
                    </div>
                    <div
                    class="detail-child-col col-value-detail${languageValue.backgroundChange}"
                    >
                    ${languageValue.valueChange}
                    </div>
                    <div class="detail-child-col col-title-detail">
                    <div class="padding-5px">${viewResources.Country}</div>
                    </div>
                    <div
                    class="detail-child-col col-value-detail border-right-none${countryValue.backgroundChange}"
                    >
                    ${countryValue.valueChange}
                    </div>
                </div>
                <div class="container-detail border-bottom-none">
                    <div class="detail-child-col col-title-detail">
                    <div class="padding-5px">${viewResources.CommunicationTool}</div>
                    </div>
                    <div
                    class="detail-child-col col-value-detail border-right-none${communicationToolValue.backgroundChange} col-value-lager"
                    >
                    ${communicationToolValue.valueChange}
                    </div>
                </div>                            
            `;
        }

        function ReRenderAfterPaged(page_index, itemsPerPage) {
            cachedData.scope.pagingPicReport(page_index + 1, itemsPerPage);
            return false;
        }

        function renderPicReport(scope, element) {
            if (scope.picReportOptions.DataList !== null) {
                cachedData.scope = scope;
                let tablePicReport = element.find("#pic-table-report");
                cachedData.reportTable = tablePicReport;
                let picReport = scope.picReportOptions.DataList;
                cachedData.picReport = picReport;
                let gridOptions = getGripOtions(picReport);
                cachedData.gridOptions = gridOptions;
                tablePicReport.empty().removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
            }
        }

        return {
            restrict: 'C',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    if (scope.picReportOptions.IsUpdateGrid) {
                        renderPicReport(scope, element);
                        scope.picReportOptions.IsUpdateGrid = false;
                    }
                });
            }
        };
    });
})();