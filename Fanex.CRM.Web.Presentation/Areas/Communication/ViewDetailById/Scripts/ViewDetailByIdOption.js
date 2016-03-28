(function () {
    "use strict";

    window.registerOptions = function ($scope) {
        $scope.productOptions = {
            DataList: [],
            SelectedId: $scope.communicationLog.Product.Id,
            OnChangeEvent: null
        };
        $scope.subProductOptions = {
            DataList: [],
            SelectedId: $scope.communicationLog.SubProduct.Id,
            OnChangeEvent: null
        };

        $scope.statusOptions = {
            DataList: [],
            OnChangeEvent: null,
            SelectedId: $scope.communicationLog.Status.Id
        };
        $scope.responseOptions = {
            DataList: [],
            OnChangeEvent: null,
            SelectedId: $scope.communicationLog.Response.Id
        };

        $scope.roleOptions = {
            DataList: [],
            OnChangeEvent: null,
            SelectedId: $scope.communicationLog.RoleInfo.Id
        };
        $scope.communicationToolOptions = {
            DataList: [],
            OnChangeEvent: null,
            SelectedId: $scope.communicationLog.CommunicationTool.Id
        };
        $scope.mainCategoryOptions = {
            DataList: [],
            OnChangeEvent: null,
            SelectedId: $scope.communicationLog.MainCategory.Id
        };
        $scope.subCategoryOptions = {
            DataList: [],
            OnChangeEvent: null,
            SelectedId: $scope.communicationLog.SubCategory.Id
        };
        $scope.extensionOptions = {
            DataList: [],
            OnChangeEvent: null,
            SelectedId: $scope.communicationLog.Extension.Id
        };
        $scope.countryOptions = {
            DataList: [],
            OnChangeEvent: null,
            SelectedId: $scope.communicationLog.Country.Id
        };
        $scope.languageOptions = {
            DataList: [],
            OnChangeEvent: null,
            SelectedId: $scope.communicationLog.Language.Id
        };
        $scope.betTypeOptions = {
            DataList: [],
            OnChangeEvent: null,
            SelectedId: $scope.communicationLog.BetType.Id
        };
        $scope.remarkOptions = {
            OnChangeEvent: null,
            SelectedText: $scope.communicationLog.Remark,
            EditIconText: viewResources.Insert_Edit_Block
        };
        $scope.solutionOptions = {
            OnChangeEvent: null,
            SelectedText: $scope.communicationLog.Solution,
            EditIconText: viewResources.Insert_Edit_Block
        };
        $scope.reviewOptions = {
            OnChangeEvent: null,
            SelectedText: $scope.communicationLog.Review,
            EditIconText: viewResources.Insert_Edit_Block
        };
        $scope.subjectOptions = {
            OnChangeEvent: null,
            NumberOfSuggestItems: 15,
            SubjectList: $scope.feedbackSubjectTemplateList,
            Subject: $scope.communicationLog.Subject
        };
        $scope.startTimeOptions = {
            OnChangeEvent: null,
            SelectedTime: $scope.communicationLog.StartTime,
            CurrentTime: currentTime
        };
        $scope.endTimeOptions = {
            OnChangeEvent: null,
            SelectedTime: $scope.communicationLog.EndTime,
            CurrentTime: currentTime
        };

        $scope.$apply();
    }
})();