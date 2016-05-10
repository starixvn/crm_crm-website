/*global currentDate, initFromDate */

(function () {
    "use strict";
    /*
     currentDate: true,
     initFromDate: true
     */

    window.registerOptions = function ($scope) {
        $scope.productOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedProduct
        };
        $scope.subProductOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedSubProduct
        };

        $scope.fromWeekOptions = {
            CurrentDate: currentDate,
            InitDate: initFromDate,
            MaxDate: currentDate,
            IsRerenderPicker: false,
            OnChangeDateEvent: $scope.updateFromWeek
        };

        $scope.toWeekOptions = {
            CurrentDate: currentDate,
            InitDate: initFromDate,
            MaxDate: currentDate,
            IsRerenderPicker: false,
            OnChangeDateEvent: $scope.updateToWeek
        };

        $scope.fromWeekOptions.IsRerenderPicker = true;
        $scope.toWeekOptions.IsRerenderPicker = true;
        $scope.$apply();

        $scope.reportTypeOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateReportType
        };

        $scope.$apply();

        currentDate = new Date(currentDate);
        initFromDate = new Date(initFromDate);
        $scope.fromMonthOptions = {
            CurrentDate: currentDate,
            InitDate: initFromDate,
            MaxDate: initFromDate,
            MinDate: new Date(initFromDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection,
            initFromDate.getMonth(),
            initFromDate.getDate()),
            IsRerenderPicker: false,
            OnChangeEvent: $scope.updateFromMonth,
            SelectedYear: initFromDate.getFullYear(),
            SelectedMonth: initFromDate.getMonth() + 1
        };

        $scope.fromMonthOptions.IsRerenderPicker = true;
        $scope.$apply();

        $scope.toMonthOptions = {
            CurrentDate: currentDate,
            InitDate: currentDate,
            MaxDate: currentDate,
            MinDate: currentDate,
            IsRerenderPicker: false,
            OnChangeEvent: $scope.updateToMonth,
            SelectedYear: currentDate.getFullYear(),
            SelectedMonth: currentDate.getMonth() + 1
        };

        $scope.toMonthOptions.IsRerenderPicker = true;
        $scope.$apply();

        $scope.fromYearOptions = {
            CurrentDate: currentDate,
            InitDate: initFromDate,
            MaxDate: initFromDate,
            MinDate: new Date(initFromDate.getFullYear() - sharedScriptManager.cachedData.numberOfYearPerSelection,
            initFromDate.getMonth(),
            initFromDate.getDate()),
            IsRerenderPicker: false,
            OnChangeEvent: $scope.updateFromYear,
            SelectedYear: initFromDate.getFullYear()
        };

        $scope.fromYearOptions.IsRerenderPicker = true;
        $scope.$apply();

        $scope.toYearOptions = {
            CurrentDate: currentDate,
            InitDate: currentDate,
            MaxDate: currentDate,
            MinDate: currentDate,
            IsRerenderPicker: false,
            OnChangeEvent: $scope.updateToYear,
            SelectedYear: currentDate.getFullYear()
        };

        $scope.toYearOptions.IsRerenderPicker = true;
        $scope.$apply();

        $scope.siteOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedSite,
            AllText: viewResources.All,
            IsIncludeNA: true
        };

        $scope.statusOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedStatus
        };
        $scope.responseOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedResponse
        };

        $scope.roleOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedRole
        };
        $scope.communicationToolOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedCommunicationTool
        };
        $scope.mainCategoryOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedMainCategory
        };
        $scope.subCategoryOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedSubCategory
        };
        $scope.extensionOptions = {
            DataList: [],
            OnChangeEvent: $scope.updateSelectedExtension
        };

        $scope.$apply();

        $scope.fromDateOptions = {
            CurrentDate: currentDate,
            InitDate: initFromDate,
            MaxDate: currentDate,
            IsRerenderPicker: false,
            OnChangeDateEvent: $scope.updateFromDate,
            CompleteRender: $scope.CompleteRenderFromDate
        };

        $scope.toDateOptions = {
            CurrentDate: currentDate,
            InitDate: currentDate,
            MaxDate: currentDate,
            IsRerenderPicker: false,
            OnChangeDateEvent: $scope.updateToDate,
            CompleteRender: $scope.CompleteRenderToDate
        };

        $scope.updateFromDatePicker();
        $scope.updateToDatePicker();
    };
})();