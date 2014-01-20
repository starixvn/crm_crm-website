
customerApplication.controller('customerController', function ($scope,
    $http,
    $customerService) {
    $scope.customer = {
        SiteSetting: {
            Id: 0,
            Name: ""
        },
        Country: {
            Id: 0,
            Name: ""
        },
        FirstName: "",
        LastName: "",
        NickName: "",
        Languages: [],
        PhoneInfo: [],
        MobilePhone: {
            CountryCode: "",
            AreaCode: "",
            PhoneNumber: ""
        },

        FaxInfo: {
            CountryCode: "",
            AreaCode: "",
            PhoneNumber: ""
        },
        Email: "",
        MSN: "",
        Skype: "",
        PersonalInfoContact: [],
        BirthDayString: "",
        BankInfo: {
            Address: "",
            SwiftCode: "",
            AccountName: "",
            AccountNumber: ""
        },
        IsVIP: false,
        Question: "",
        Answer: "",
        Remark: "",
        Role: {
            Id: 0,
            Name: ""
        },
        OriginalDangerLevel: 0,
        ABTDangerLevel: 0,
        OtherSportsDangerLevel: 0,
        RefCode: [],
        Remark2: "",
        UplineInfo: []
    };

    $scope.searchUplineInfo = function (username) {
        customerManager.searchUplineInfo(username);
    };

    $scope.getCustomerInfoByUsername = function (getCustomerInfoParams) {
        parent.BlockUI();
        $customerService.getCustomerInfoByUsername(getCustomerInfoParams)
            .then(function (data) {
                var customer = data.Result;
                var isHavingData = customerManager.checkCustomerInfoResult(customer);
                if (isHavingData) {
                    $scope.customer = customer;
                }

                customerManager.comletedGetCustomerInfoByUsername(customer, isHavingData);
                parent.jQuery.unblockUI();
            },
            function (data) {
                parent.$.unblockUI();
            });
    };
    $scope.getGuestCustomerInfoByUsername = function (getCustomerInfoParams) {
        parent.BlockUI();
        $customerService.getCustomerInfoByUsername(getCustomerInfoParams)
            .then(function (data) {
                var customer = data.Result;
                var isHavingData = customerManager.checkGuestCustomerInfoResult(customer);
                if (isHavingData) {
                    $scope.customer = customer;
                }

                customerManager.comletedGetCustomerInfoByUsername(customer, isHavingData);
                parent.jQuery.unblockUI();
            },
            function (data) {
                parent.$.unblockUI();
            });
    };
    $scope.viewCRMInfo = function () {
        var url = '../CustomerSetting/GoToCustomerInfoDetail?' + $.param({
            custid: $scope.customer.Id
        });
        parent.menuManager.goToURL('centerFr', url);
    };
    $scope.addFeedback = function () {
        var url = '../Feedback/AddFeedback?' + $.param({
            custid: $scope.customer.Id,
            custName: $scope.customer.Name
        });
        parent.menuManager.goToURL('centerFr', url);
    };
    $scope.viewLatestFeedback = function () {
        var url = '../Feedback/CustomerLastestFeedback?' + $.param({
            custid: $scope.customer.Id,
            custName: $scope.customer.Name
        });
        parent.menuManager.goToURL('centerFr', url);

    };
    $scope.manageFeedback = function () {
        var url = '../Feedback/ManageFeedback?' + $.param({
            custid: $scope.customer.Id,
            custName: $scope.customer.Name,
            isGuest: 0
        });
        parent.menuManager.goToURL('centerFr', url);
    };
});