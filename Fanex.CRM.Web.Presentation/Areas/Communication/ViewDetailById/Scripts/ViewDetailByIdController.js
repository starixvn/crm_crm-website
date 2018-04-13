(function () {
    "use strict";
    /* global communicationLog,
    sharedScriptManager,
    SettingManager,
    viewResources,
    hasReviewPermission,
    currentTime,
    crmConstants */
    /* jshint camelcase: false */
    angular.module('communicationlog-viewdetailbyid-application').controller('viewDetailByIdController', ['$scope', '$http', '$viewDetailByIdService', '$sce',
    function ($scope, $http, $viewDetailByIdService, $sce) {
        $scope.communicationLog = communicationLog;
        $scope.feedbackSubjectTemplateList = [];
        $scope.feedbackSubjectTemplateTotalPage = 0;
        $scope.isLoadedFeedbackSubjectTemplate = false;
        $scope.hasReviewPermission = hasReviewPermission;
        $scope.IsUpdatingTime = false;

        $scope.statusOptions = null;
        $scope.responseOptions = null;
        $scope.roleOptions = null;
        $scope.communicationToolOptions = null;
        $scope.mainCategoryOptions = null;
        $scope.subCategoryOptions = null;
        $scope.extensionOptions = null;
        $scope.countryOptions = null;
        $scope.languageOptions = null;
        $scope.betTypeOptions = null;
        $scope.remarkOptions = null;
        $scope.solutionOptions = null;
        $scope.reviewOptions = null;
        $scope.subjectOptions = null;
        $scope.startTimeOptions = null;
        $scope.endTimeOptions = null;
        $scope.licenseeTypeOptions = null;
        $scope.communicationLog.AutoField = $sce.trustAsHtml($scope.communicationLog.AutoField);
        $scope.TextSettings = {
            Filter: "Search",
            NoItemChecked: "N/A",
            CheckAll: "All"
        };
        

        $scope.initController = function () {
            $scope.getAllSetting();
            $scope.getNumberOfFeedbackSubjectTemplate();
        };

        $scope.getNumberOfFeedbackSubjectTemplate = function () {
            $scope.BlockUI();
            $viewDetailByIdService.getNumberOfFeedbackSubjectTemplate()
            .then(function (data) {
                var numberOfFeedbackItem = data.Result;
                $scope.feedbackSubjectTemplateList = [];
                var totalPage = Math.round((numberOfFeedbackItem / sharedScriptManager.cachedData.maxFeedbackSubjectTemplateNumberOnLoad) + 0.5);
                $scope.feedbackSubjectTemplateTotalPage = totalPage;
                $scope.isLoadedFeedbackSubjectTemplate = false;
                for (var i = 1; i <= totalPage; i++) {
                    $scope.getFeedbackSubjectTemplateByPagingIndex(i);
                }
            },
            function () {
                $scope.isLoadedFeedbackSubjectTemplate = true;
            });
        };
        $scope.getFeedbackSubjectTemplateByPagingIndex = function (pagingIndex) {
            $scope.BlockUI();
            $viewDetailByIdService.getFeedbackSubjectTemplateByPagingIndex({ PagingIndex: pagingIndex })
            .then(function (data) {
                var feedbackSubjectTemplate = data.Result;
                if (feedbackSubjectTemplate != null && feedbackSubjectTemplate.length > 0) {
                    $scope.feedbackSubjectTemplateList = $scope.feedbackSubjectTemplateList.concat(feedbackSubjectTemplate);
                }

                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.completedLoadFilterInfo();
                }
            },
            function () {
                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.completedLoadFilterInfo();
                }
            });
        };
        $scope.completedLoadFilterInfo = function () {
            $scope.initSelectedLogInfo();
            $scope.relatedCustomerResultDirective.loadRelatedCustomers($scope.communicationLog.RelatedCustomers);
            $scope.unblockUI();
        };
        $scope.initSelectedLogInfo = function () {           
            //$scope.updateSelectedProduct($scope.communicationLog.Product);
        };

        $scope.updateSelectedProduct = function (product) {
            $scope.subProductOptions.DataList = SettingManager.getChildSettings(product.Id, $scope.productOptions.DataList);
            $scope.updateSelectedSubProduct($scope.communicationLog.SubProduct);
        };
        $scope.updateSelectedSubProduct = function (subProduct) {
            var childSettings = SettingManager.getChildSettings(subProduct.Id, $scope.subProductOptions.DataList);
            $scope.betTypeOptions.DataList = childSettings;
        };

        $scope.unblockUI = function () {
            try {
                parent.$.unblockUI();
            } catch (e) {
            }
        };
        $scope.BlockUI = function () {
            try {
                parent.BlockUI();
            } catch (e) {
            }
        };

        $scope.productChangedHandler = function () {
            $scope.subProductDropdownSettings = {
                idMember: "Id",
                nameMember: "Name",
                sourceData: $scope.getSubProducts($scope.selectedProducts),
                childrenMember: "ChildSettings",
            };
        }

        $scope.subProductChangedHandler = function () {
            $scope.bettypeDropdownSettings = {
                idMember: "Id",
                nameMember: "Name",
                sourceData: $scope.getBettypes($scope.selectedSubProducts, $scope.allSettings.ProductList),
                childrenMember: "ChildSettings",
            };
        }

        $scope.getSubProducts = function (seletedProducts) {
            var cloneSeletedProducts = [];
            if (seletedProducts) {
                cloneSeletedProducts = angular.copy(seletedProducts);
            }

            var productsWithSubProducts = [];
            angular.forEach(cloneSeletedProducts, function (product) {
                // only get product having subproducts
                if (product.ChildSettings.length != 0) {
                    angular.forEach(product.ChildSettings, function (subProducts) {
                        subProducts.ChildSettings = [];
                    })
                    productsWithSubProducts.push(product);
                }
            });

            return productsWithSubProducts;
        }

        $scope.getBettypes = function (seletedSubProducts, allProducts) {
            var selectedProductIds = [];
            angular.forEach(seletedSubProducts, function (selectedSubProduct) {
                if (selectedProductIds.indexOf(selectedSubProduct.ParentId) < 0) {
                    selectedProductIds.push(selectedSubProduct.ParentId);
                }
            });

            var selectProductandSubProductIds = [];
            angular.forEach(selectedProductIds, function (productId) {
                var Ids = {
                    ProductId: productId,
                    SubProductIds: []
                };

                angular.forEach(seletedSubProducts, function (selectedSubProduct) {
                    if (selectedSubProduct.ParentId == productId) {
                        Ids.SubProductIds.push(selectedSubProduct.Id);
                    }
                });

                selectProductandSubProductIds.push(Ids);
            })

            var selectedProducts = [];
            angular.forEach(selectProductandSubProductIds, function (ids) {
                angular.forEach(allProducts, function (product) {
                    if (product.Id == ids.ProductId) {
                        var subProducts = [];
                        angular.forEach(product.ChildSettings, function (subProduct) {
                            // only get subproduct having bettypes
                            if (ids.SubProductIds.indexOf(subProduct.Id) >= 0 && subProduct.ChildSettings.length > 0) {
                                subProducts.push(angular.copy(subProduct));
                            }
                        });

                        // only get product having subproducts
                        if (subProducts.length > 0) {
                            var p = angular.copy(product);
                            p.ChildSettings = subProducts;
                            selectedProducts.push(p)
                        }
                    }
                })
            });

            return selectedProducts;
        }

        $scope.getAllSetting = function () {
            $scope.BlockUI();
            $scope.isLoadedAllSite = false;
            $viewDetailByIdService.getAllSetting()
            .then(function (data) {
                var allSetting = data.Result;
                $scope.allSettings = data.Result;

                $scope.siteList = allSetting.SiteList;
                $scope.statusOptions.DataList = allSetting.StatusList;
                $scope.responseOptions.DataList = allSetting.ResponseList;
                $scope.roleOptions.DataList = allSetting.RoleList;
                
                $scope.languageOptions.DataList = allSetting.LanguageList;
                
                $scope.countryOptions.DataList = [];
                $scope.countryOptions.DataList.push({
                    Id: crmConstants.Enums.Country.NotApply,
                    Name: viewResources.Symbol_NotApply
                });
                $scope.licenseeTypeOptions = {
                    DataList: allSetting.LicenseeType,
                    OnChangeEvent: $scope.updateSelectedLicenseeType,
                    SelectedId: $scope.communicationLog.LicenseeType.Id
                };

                $scope.countryOptions.DataList = $scope.countryOptions.DataList.concat(allSetting.CountryList);

                $scope.communicationDropdownSettings = {
                    idMember: "Id",
                    nameMember: "Name",
                    sourceData: $scope.allSettings.CommunicationToolList
                };

                $scope.extensionDropdownSettings = {
                    idMember: "Id",
                    nameMember: "Value",
                    sourceData: $scope.allSettings.ExtensionList
                };

                $scope.categoryDropdownSettings = {
                    idMember: "Id",
                    nameMember: "Name",
                    sourceData: $scope.allSettings.MainCategoryList,
                    childrenMember: "ChildSettings"
                };

                $scope.productDropdownSettings = {
                    idMember: "Id",
                    nameMember: "Name",
                    sourceData: $scope.allSettings.ProductList
                };

                $scope.subProductDropdownSettings = {
                    idMember: "Id",
                    nameMember: "Name",
                    sourceData: $scope.getSubProducts($scope.selectedProducts),
                    childrenMember: "ChildSettings",
                };

                $scope.bettypeDropdownSettings = {
                    idMember: "Id",
                    nameMember: "Name",
                    sourceData: $scope.getBettypes($scope.selectedSubProducts, $scope.allSettings.ProductList),
                    childrenMember: "ChildSettings",
                };

                $scope.updateAllSettingsForLog(allSetting);
                $scope.completedLoadFilterInfo();
            },
            function () {
                $scope.completedLoadFilterInfo();
            });
        };

        $scope.updateAllSettingsForLog = function () {            
            //$scope.updateProductSettings();
            $scope.updateSettingCommunication();
            $scope.updateSettingExtension();
            $scope.updateSettingCategory();
            $scope.updateSettingProduct();
            $scope.updateSettingSubProduct();
            $scope.updateSettingBettype();

            SettingManager.updateSettingSelectList($scope.communicationLog.Language, $scope.languageOptions.DataList);
            SettingManager.updateSettingSelectList($scope.communicationLog.Status, $scope.statusOptions.DataList);
            SettingManager.updateSettingSelectList($scope.communicationLog.RoleInfo, $scope.roleOptions.DataList);
            SettingManager.updateSettingSelectList($scope.communicationLog.Response, $scope.responseOptions.DataList);

            SettingManager.updateSettingSelectList($scope.communicationLog.SiteInfo, $scope.siteList);
            SettingManager.updateSettingSelectList($scope.communicationLog.LicenseeType, $scope.licenseeTypeOptions.DataList);
        };

        $scope.updateSettingCommunication = function () {
            var length = $scope.communicationDropdownSettings.sourceData.length;
            for (var i = 0; i < length; i++) {
                var data = $scope.communicationDropdownSettings.sourceData[i];
                angular.forEach($scope.communicationLog.Settings, function (setting) {
                    if (setting.SettingType == "Communication" && data.Id == setting.SettingIdLv1) {
                        data.checked = true;
                    }
                });
            }
        }

        $scope.updateSettingExtension = function () {
            var length = $scope.extensionDropdownSettings.sourceData.length;
            for (var i = 0; i < length; i++) {
                var data = $scope.extensionDropdownSettings.sourceData[i];
                angular.forEach($scope.communicationLog.Settings, function (setting) {
                    if (setting.SettingType == "Station" && data.Id == setting.SettingIdLv1) {
                        data.checked = true;
                    }
                });
            }
        }

        $scope.updateSettingCategory = function () {
            var length = $scope.categoryDropdownSettings.sourceData.length;
            for (var i = 0; i < length; i++) {
                var data = $scope.categoryDropdownSettings.sourceData[i];
                angular.forEach($scope.communicationLog.Settings, function (setting) {
                    if (setting.SettingType == "Category" && data.Id == setting.SettingIdLv1) {
                        for (var j = 0; j < data.ChildSettings.length; j++) {
                            if (data.ChildSettings[j].Id == setting.SettingIdLv2) {
                                data.ChildSettings[j].checked = true;
                            }
                        }
                    }
                });
            }
        }

        $scope.updateSettingProduct = function () {
            var length = $scope.productDropdownSettings.sourceData.length;
            for (var i = 0; i < length; i++) {
                var data = $scope.productDropdownSettings.sourceData[i];
                angular.forEach($scope.communicationLog.Settings, function (setting) {
                    if (setting.SettingType == "Product" && data.Id == setting.SettingIdLv1) {
                        data.checked = true;
                    }
                });
            }
        }

        $scope.updateSettingSubProduct = function () {
            var length = $scope.productDropdownSettings.sourceData.length;
            for (var i = 0; i < length; i++) {
                var data = $scope.productDropdownSettings.sourceData[i];
                angular.forEach($scope.communicationLog.Settings, function (setting) {
                    if (setting.SettingType == "Product" && data.Id == setting.SettingIdLv1) {
                        for (var j = 0; j < data.ChildSettings.length; j++) {
                            if (data.ChildSettings[j].Id == setting.SettingIdLv2) {
                                data.ChildSettings[j].checked = true;
                            }
                        }
                    }
                });
            }
        }

        $scope.updateSettingBettype = function () {
            var length = $scope.productDropdownSettings.sourceData.length;
            for (var i = 0; i < length; i++) {
                var data = $scope.productDropdownSettings.sourceData[i];
                angular.forEach($scope.communicationLog.Settings, function (setting) {
                    if (setting.SettingType == "Product" && data.Id == setting.SettingIdLv1) {
                        for (var j = 0; j < data.ChildSettings.length; j++) {
                            if (data.ChildSettings[j].Id == setting.SettingIdLv2) {
                                var childData = data.ChildSettings[j].ChildSettings;
                                for (var k = 0; k < childData.length; k++) {
                                    if (childData[k].Id == setting.SettingIdLv3) {
                                        childData[k].checked = true;
                                    }
                                }
                            }
                        }
                    }
                });
            }
        }

        $scope.updateProductSettings = function () {
            var product = $scope.communicationLog.Product;
            var productList = $scope.productOptions.DataList;
            SettingManager.updateSettingSelectList(product, productList);

            var subProduct = $scope.communicationLog.SubProduct;
            var subProductList = $scope.subProductOptions.DataList;
            SettingManager.updateInActiveChildSettings(subProduct, productList, product.Id);
            subProductList = SettingManager.getChildSettings(product.Id, productList);
            SettingManager.updateInActiveChildSettings($scope.communicationLog.BetType, subProductList, subProduct.Id);
        };

        $scope.getReviewedInfo = function () {
            return sharedScriptManager.stringFormat(
            viewResources.ReviewedInformation,
            [$scope.communicationLog.Reviewer, $scope.communicationLog.ReviewedTimeString]);
        };
        angular.element(document).ready(function () {
            registerOptions($scope);
            $scope.initController();
        });
    } ]);
})();