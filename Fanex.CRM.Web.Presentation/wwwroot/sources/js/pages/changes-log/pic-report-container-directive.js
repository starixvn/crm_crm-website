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
            return crmConstants.MarginBottomPage + 160;
        }

        function getGripOtions(report) {
            let bodyData = [];
            let reportLength = report ? report.DataList.length : 0;
            if (reportLength > 0) {
                for (let i = 0; i < reportLength; i++) {
                    let data = report.DataList[i];
                    let no = i + 1 + cachedData.scope.lastPicSelectedParams.PageSize * (cachedData.scope.lastPicSelectedParams.PageIndex - 1);
                    let detailValue = '';
                    if (data.Action.toLowerCase().includes('status')) {
                        let directAccountValue = changeValueToHtmlString(preExecuteValue(data.OldPicInfo.DirectAccount), preExecuteValue(data.NewPicInfo.DirectAccount));
                        let isActiveValue = changeValueToHtmlString(convertBoolToDecicion(data.OldPicInfo.IsActive), convertBoolToDecicion(data.NewPicInfo.IsActive));
                        detailValue = `<div class="container-detail full-height border-bottom-none"><div class="detail-child-col col-title-detail">Direct Account</div><div class="detail-child-col col-value-detail${directAccountValue.backgroundChange}">${directAccountValue.valueChange}</div><div class="detail-child-col col-title-detail">Is Active</div><div class="detail-child-col col-value-detail border-right-none${isActiveValue.backgroundChange}">${isActiveValue.valueChange}</div></div>`;
                    }
                    else if (data.Action.toLowerCase().includes('ins')) {
                        let languages = data.NewPicInfo.Languages && data.NewPicInfo.Languages.length > 0 ? data.NewPicInfo.Languages.map(lang => lang.Name).join(', ') : '-';
                        let countryValue = data.NewPicInfo.Country ? data.NewPicInfo.Country.Name : '-';
                        let communicationTools = data.NewPicInfo.CommunicationTools.map(comm => `<div>${comm.Name}: ${comm.Value}</div>`).join('');
                        detailValue = `<div class="container-detail"><div class="detail-child-col col-title-detail">Name</div><div class="detail-child-col col-value-detail">${data.NewPicInfo.Name}</div><div class="detail-child-col col-title-detail">Role</div><div class="detail-child-col col-value-detail border-right-none">${data.NewPicInfo.Role.Name}</div></div><div class="container-detail"><div class="detail-child-col col-title-detail">Language</div><div class="detail-child-col col-value-detail">${languages}</div><div class="detail-child-col col-title-detail">Country</div><div class="detail-child-col col-value-detail border-right-none"><div>${countryValue}</div></div></div><div class="container-detail border-bottom-none"><div class="detail-child-col col-title-detail">Communication Tool</div><div class="detail-child-col col-value-detail">${communicationTools}</div><div class="detail-child-col col-title-detail">Is Active</div><div class="detail-child-col col-value-detail border-right-none"><div>Yes</div></div></div>`;

                    }
                    else if (data.Action.toLowerCase().includes('upd')) {
                        let roleValue = changeValueToHtmlString(preExecuteObject(data.OldPicInfo.Role), preExecuteObject(data.NewPicInfo.Role));
                        let nameValue = changeValueToHtmlString(data.OldPicInfo.Name, data.NewPicInfo.Name);
                        let newLanguage = data.NewPicInfo.Languages && data.NewPicInfo.Languages.length > 0 ? data.NewPicInfo.Languages.map(lang => lang.Name).join(', ') : '-';
                        let oldLanguage = data.OldPicInfo.Languages && data.OldPicInfo.Languages.length > 0 ? data.OldPicInfo.Languages.map(lang => lang.Name).join(', ') : '-';
                        let languageValue = changeValueToHtmlString(oldLanguage, newLanguage)
                        let countryValue = changeValueToHtmlString(preExecuteObject(data.OldPicInfo.Country), preExecuteObject(data.NewPicInfo.Country));
                        let newCommunicationTool = data.NewPicInfo.CommunicationTools.map(comm => `<div>Skype: ${comm.Value}</div>`).join('');
                        let oldCommunicationTool = data.OldPicInfo.CommunicationTools.map(comm => `<div>Skype: ${comm.Value}</div>`).join('');
                        let communicationToolValue = changeValueToHtmlString(oldCommunicationTool, newCommunicationTool);
                        let isActiveValue = changeValueToHtmlString(convertBoolToDecicion(data.OldPicInfo.IsActive), convertBoolToDecicion(data.NewPicInfo.IsActive));
                        detailValue = `<div class="container-detail"><div class="detail-child-col col-title-detail">Name</div><div class="detail-child-col col-value-detail${nameValue.backgroundChange}">${nameValue.valueChange}</div><div class="detail-child-col col-title-detail">Role</div><div class="detail-child-col col-value-detail border-right-none${roleValue.backgroundChange}">${roleValue.valueChange}</div></div><div class="container-detail"><div class="detail-child-col col-title-detail">Language</div><div class="detail-child-col col-value-detail${languageValue.backgroundChange}">${languageValue.valueChange}</div><div class="detail-child-col col-title-detail">Country</div><div class="detail-child-col col-value-detail border-right-none${countryValue.backgroundChange}">${countryValue.valueChange}</div></div><div class="container-detail border-bottom-none"><div class="detail-child-col col-title-detail">Communication Tool</div><div class="detail-child-col col-value-detail border-right-none${communicationToolValue.backgroundChange} col-value-lager">${communicationToolValue.valueChange}</div>`;
                    }

                    let columns = [
                        { Name: "ColNo", Value: no, CellAlign: "center" },
                        { Name: "ColLogId", Value: data.LogId, CellAlign: "center" },
                        { Name: "ColLogTime", Value: data.LogTime, CellAlign: "center" },
                        { Name: "ColEditBy", Value: data.EditBy, CellAlign: "center" },
                        { Name: "ColPIC", Value: data.Name, CellAlign: "center" },
                        { Name: "ColAction", Value: data.Action },
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
                    itemsPerPageArray: [10, 200, 300, 400, 500, 1000],
                    totalItem: report.Totals,
                    pagerSelectCallBack: ReRenderAfterPaged
                }
            };

            return gridOptions;
        }

        function changeValueToHtmlString(oldValue, newValue) {
            let isChange = newValue !== oldValue;
            let valueChange = isChange ? `<div class="new-value">${newValue}</div><div class="old-value">${oldValue}</div>` : `<div>${newValue}</div><div>${oldValue}</div>`;
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

        function preExecuteObject(object) {
            if (object && object.Name) {
                return object.Name;
            }
            else if (object && object.Value) {
                return object.Name;
            }

            return '-';
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