(function () {
    "use strict";
    /*crmConstants,
    sharedScriptManager,
    viewResources */
    /* jshint camelcase: false */
    angular.module('dashboard-application').directive('tableNewRemarksContainer', function () {
        function renderNewRemark(scope, element) {
            if (scope.newRemarks == null) {
                return;
            }

            var tableNewRemarks = element.find("#table-new-remarks");
            var newRemarks = scope.newRemarks;
            var bodyData = [];
            var footerData = [];

            tableNewRemarks.empty();
            if (newRemarks && newRemarks.length > 0) {
                var logLength = newRemarks.length;
                for (var i = 0; i < logLength; i++) {
                    var info = newRemarks[i];
                    var dataColumns = [];
                    var userName = info.IsGroupHeadLine ? info.UserName + " (Headline)" : info.UserName;
                    dataColumns.push({ Name: "ColId",Value: info.RemarkID});
                    dataColumns.push({ Name: "ColRemarkTitle", Value: info.RemarkTitle });
                    dataColumns.push({ Name: "ColRemarkContent", Value: renderContentRemarkColumn(info, i) });
                    dataColumns.push({
                        Name: "ColCreatedDate",
                        Value: info.CreatedDateString
                    });
                    dataColumns.push({ Name: "ColCreator", Value: info.Creator });
                    dataColumns.push({ Name: "ColUserName", Value: userName });


                    bodyData.push({
                        Columns: dataColumns,
                        CssClass: 'row' + info.RemarkID
                    });
                }
            }

            var columns = [];
            columns.push({ HeaderText: viewResources.ID, Width: 70, Name: "ColId", HeaderAlign: "center", CellAlign: "center" });
            columns.push({ HeaderText: viewResources.RemarkTitle, Width: 160, Name: "ColRemarkTitle", HeaderAlign: "center", CellAlign: "left" });
            columns.push({ HeaderText: viewResources.RemarkContent, Width: 280, Name: "ColRemarkContent", HeaderAlign: "center", CellAlign: "left" });
            columns.push({ HeaderText: viewResources.CreatedDate, Width: 150, Name: "ColCreatedDate", HeaderAlign: "center", CellAlign: "center" });
            columns.push({ HeaderText: viewResources.Creator, Width: 120, Name: "ColCreator", HeaderAlign: "center", CellAlign: "center" });
            columns.push({ HeaderText: viewResources.Username, Width: 150, Name: "ColUserName", HeaderAlign: "center", CellAlign: "center" });

            var gridOptions = {
                columns: columns,
                showTitle: false,
                bodyRows: bodyData,
                footerRows: footerData,
                numberOfFrozenColumn: 0,
                fullWidth: true,
                fullHeight: false,
                gridHeight: $(window).height() - $("#table-redemption-logs").height() -
                    $("#table-communication-logs").height() - 120,
                showDistinctRow: false,
                showHighlightRow: true,
                headerColumnHeight: 29,
                bodyRowHeight: 25,
                showPager: false,
                noDataText: viewResources.There_is_no_data
            };

            tableNewRemarks.removeClass(crmConstants.Enums.CssClass.DisplayNone).fanexGrid(gridOptions);
            if (newRemarks && newRemarks.length > 0) {
                var tooltips = element.find('.remark-content');
                tooltips.tipsy({
                        fade: true,
                        gravity: 'nw',
                        html: true,
                        title: function () {
                            return newRemarks[$(this).data('index')].Remark;
                        }
                    });
            }

        }

        function renderContentRemarkColumn(remark, number) {
            var html = '<div>\
                <a class="remark-content"  href="javascript:;" data-id="' + remark.RemarkID + '" data-index="' + number + '">'+ remark.Remark +'</a>\
            </div>'

            return html ;
        }

        return {
            restrict: 'A',
            scope: true,
            link: function (scope, element, attrs) {
                scope.$watch(attrs.ngModel, function (oldval, newval) {
                    /* jshint unused: false */
                    renderNewRemark(scope, element);
                });
            }
        };
    });
})();