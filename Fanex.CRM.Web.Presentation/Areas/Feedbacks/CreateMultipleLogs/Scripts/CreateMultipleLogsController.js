(function () {
    "use strict";
    /* global sharedScriptManager,
    moment,
    crmConstants,
    viewResources,
    customerId,
    hotKeyManager,
    currentTime*/
    /* jshint camelcase: false */
    angular.module('create-multiple-logs-application').controller('createMultipleLogsController', ['$scope', '$http', '$createMultipleLogsService',
    function ($scope, $http, $createMultipleLogsService) {

        $scope.multipleLogs = {
            StartTime: new Date(currentTime),
            EndTime: new Date(currentTime),
            CommunicationTool: {
                Id: 0,
                Value: ''
            },
            Extension: {
                Id: 0,
                Value: ''
            },
            Country: {
                Id: 0
            },
            LanguageId: 0,
            MainCategory: {
                Name: ''
            },
            SubCategory: {
                Name: ''
            },
            Product: {
                Id: 0,
                Value: ''
            },
            SubProduct: {
                Id: 0,
                Value: ''
            },
            Status: {
                Id: 0,
                Value: ''
            },
            BetType: {
                Id: 0,
                Value: ''
            },
            RoleInfo: {
                Id: 0,
                Value: ''
            },
            SiteInfo: {
                Id: 0,
                Value: ''
            },
            LicenseeType: {
                Id: 0
            },
            Response: {
                Id: 0,
                Value: ''
            },
            Subject: '',
            Remark: '',
            Solution: ''
        };
        $scope.oldmultipleLogs = angular.copy($scope.multipleLogs);
        $scope.extensionList = null;
        $scope.countryList = null;
        $scope.communicationToolList = null;
        $scope.languageList = null;
        $scope.mainCategoryList = null;
        $scope.subCategoryList = null;
        $scope.betTypeList = null;
        $scope.statusList = null;
        $scope.siteList = null;
        $scope.roleList = null;
        $scope.responseList = null;
        $scope.feedbackSubjectTemplateList = [];
        $scope.feedbackSubjectTemplateTotalPage = 0;
        $scope.isLoadedFeedbackSubjectTemplate = false;
        $scope.lastRemarkPrefix = '';
        $scope.isSuccess = false;
        $scope.isShowResultMessage = false;
        $scope.resultMessage = '';
        $scope.isSetFocus = false;
        $scope.isShowRemarkBox = true;
        $scope.isShowSolutionBox = true;
        $scope.licenseeTypes = [];
        $scope.TextSettings = {
            Filter: "Search",
            NoItemChecked: "N/A",
            CheckAll: "All"
        };
        $scope.latestedSelectedCommunication = "N/A";
        $scope.previousSelectedCommunications = [];
        $scope.selectedSubProductIds = [];
        $scope.selectedBettypeIds = [];
        
        $scope.customersInput = '';

        $scope.productChangedHandler = function () {
            $scope.subProductDropdownSettings = {
                idMember: "Id",
                nameMember: "Name",
                sourceData: $scope.getSubProducts($scope.selectedProducts)
            };
        }

        $scope.subProductChangedHandler = function () {
            $scope.selectedSubProductIds = [];
            angular.forEach($scope.selectedSubProducts, function (subProduct) {
                $scope.selectedSubProductIds.push(subProduct.Id);
            });

            $scope.bettypeDropdownSettings = {
                idMember: "Id",
                nameMember: "Name",
                sourceData: $scope.getBettypes($scope.selectedSubProducts)
            };
        }

        $scope.bettypeChangeHandler = function () {
            $scope.selectedBettypeIds = [];
            angular.forEach($scope.selectedBettypes, function (bettype) {
                $scope.selectedBettypeIds.push(bettype.Id);
            });
        }

        $scope.getSubProducts = function (seletedProducts) {
            var selectedProductIds = [];
            angular.forEach(seletedProducts, function (product) {
                selectedProductIds.push(product.Id);
            });

            var subProducts = [];
            angular.forEach($scope.allSettings.SubProductList, function (subProduct) {
                if (selectedProductIds.indexOf(subProduct.ParentId) >= 0) {
                    subProduct.checked = false;
                    subProducts.push(subProduct);
                }
            });

            // check selected sub products
            angular.forEach(subProducts, function (subProduct) {
                if ($scope.selectedSubProductIds.indexOf(subProduct.Id) >= 0) {
                    subProduct.checked = true;
                }
            });

            return subProducts;
        }

        $scope.getBettypes = function (selectedSubProducts) {
            var selectedSubProductIds = [];
            angular.forEach(selectedSubProducts, function (subProduct) {
                selectedSubProductIds.push(subProduct.Id);
            });

            var bettypes = [];
            angular.forEach($scope.allSettings.BettypeList, function (bettype) {
                if (selectedSubProductIds.indexOf(bettype.ParentId) >= 0) {
                    bettypes.push(bettype);
                }
            });

            // remove duplicate bettypes
            var selectedIds = [];
            var distinctBettypes = [];
            angular.forEach(bettypes, function (bettype) {
                if (selectedIds.indexOf(bettype.Id) < 0) {
                    selectedIds.push(bettype.Id);
                    bettype.checked = false;
                    distinctBettypes.push(bettype);
                }
            });

            // check selected bettypes
            angular.forEach(distinctBettypes, function (bettype) {
                if ($scope.selectedBettypeIds.indexOf(bettype.Id) >= 0) {
                    bettype.checked = true;
                }
            });

            return distinctBettypes;
        }

        $scope.initController = function () {
            $scope.BlockUI();
            $scope.isLoadedAllSite = false;
            $createMultipleLogsService.getSettings()
            .then(function (data) {
                var allSetting = data.Result;
                $scope.allSettings = data.Result;
                $scope.countryList = allSetting.CountryList;
                $scope.communicationToolList = allSetting.CommunicationToolList;
                $scope.languageList = allSetting.LanguageList;
                $scope.statusList = allSetting.StatusList;
                $scope.siteList = allSetting.SiteList;
                $scope.roleList = allSetting.RoleList;
                $scope.responseList = allSetting.ResponseList;
                $scope.licenseeTypes = allSetting.LicenseeType;

                $scope.siteDropdownSettings = {
                    idMember: "Id",
                    nameMember: "Name",
                    sourceData: allSetting.SiteList,
                    childrenMember: "ChildSites"
                };

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
                    sourceData: $scope.allSettings.SubProductList
                };

                $scope.bettypeDropdownSettings = {
                    idMember: "Id",
                    nameMember: "Name",
                    sourceData: $scope.allSettings.BettypeList
                };

                $scope.completedLoadFilterInfo();
            },
            function () {
                $scope.completedLoadFilterInfo();
            });

            $scope.getNumberOfFeedbackSubjectTemplate();
        };
        $scope.getNumberOfFeedbackSubjectTemplate = function () {
            $scope.BlockUI();
            $createMultipleLogsService.getNumberOfFeedbackSubjectTemplate()
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
            $createMultipleLogsService.getFeedbackSubjectTemplateByPagingIndex({ PagingIndex: pagingIndex })
            .then(function (data) {
                var feedbackSubjectTemplate = data.Result;
                if (feedbackSubjectTemplate != null && feedbackSubjectTemplate.length > 0) {
                    $scope.feedbackSubjectTemplateList = $scope.feedbackSubjectTemplateList.concat(feedbackSubjectTemplate);
                }

                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.unblockUI();
                }
            },
            function () {
                if ($scope.feedbackSubjectTemplateTotalPage === pagingIndex) {
                    $scope.isLoadedFeedbackSubjectTemplate = true;
                    $scope.unblockUI();
                }
            });
        };
        $scope.completedLoadFilterInfo = function () {
            $scope.initSelectedLogInfo();
            $scope.unblockUI();
        };
        $scope.initSelectedLogInfo = function () {
            if ($scope.communicationToolList === null) {
                return;
            }

            var starttime = sharedScriptManager.newFeedbackTime(currentTime);
            var endtime = sharedScriptManager.newFeedbackTime(currentTime);
            endtime.setMinutes(endtime.getMinutes() + 1);
            $scope.multipleLogs.StartTime = sharedScriptManager.formatTimeToSendToServer(starttime);
            $scope.multipleLogs.EndTime = sharedScriptManager.formatTimeToSendToServer(endtime);

            $scope.multipleLogs.CommunicationTool = $scope.communicationToolList[0];
            $scope.multipleLogs.Status = $scope.statusList[0];
            $scope.multipleLogs.RoleInfo = $scope.roleList[0];
            $scope.multipleLogs.SiteInfo = $scope.siteList[0];
            $scope.multipleLogs.Response = $scope.responseList[0];

            $scope.initRemarkPrefix();
            $scope.oldmultipleLogs = angular.copy($scope.multipleLogs);
        };
        
        /* jshint camelcase: false */
        $scope.invokeHotKey_CtrlS = function () {
            $scope.insertMultipleLogs();
            $scope.$apply();
        };
        /* jshint camelcase: false */
        $scope.invokeHotKey_CtrlE = function () {
            $scope.relatedCustomerResultDirective.verifyUsernames($scope.customersInput)
        };        
        $scope.updateStartTime = function (selectedTime) {
            $scope.multipleLogs.StartTime = selectedTime;
            if (new Date($scope.multipleLogs.EndTime) <= selectedTime) {
                $scope.multipleLogs.EndTime = new Date(selectedTime);
                $scope.multipleLogs.EndTime.setMinutes(selectedTime.getMinutes() + 1);
            }

            $scope.updateTimeAndToolPrefix();
        };
        $scope.updateEndTime = function (selectedTime) {
            $scope.multipleLogs.EndTime = selectedTime;
            if (selectedTime <= new Date($scope.multipleLogs.StartTime)) {
                $scope.multipleLogs.StartTime = new Date(selectedTime);
                $scope.multipleLogs.StartTime.setMinutes(selectedTime.getMinutes() - 1);
            }
        };        
        $scope.updateSelectedCountry = function (countryInfoId) {
            countryInfoId = parseInt(countryInfoId);
            var countryInfo;
            var countryListLength = $scope.countryList.length;
            for (var i = 0; i < countryListLength; i++) {
                var country = $scope.countryList[i];
                if (country.Id === countryInfoId) {
                    countryInfo = country;
                    break;
                }
            }

            if (countryInfo) {
                $scope.multipleLogs.Country = countryInfo;
            } else {
                $scope.multipleLogs.Country = {
                    Id: crmConstants.Enums.Country.NotApply
                };
            }

            $scope.$apply();
        };
        $scope.updateSelectedCommunicationTool = function (communicationToolId) {
            communicationToolId = parseInt(communicationToolId);
            var communicationToolInfo;
            var communicationToolListLength = $scope.communicationToolList.length;
            for (var i = 0; i < communicationToolListLength; i++) {
                var communicationTool = $scope.communicationToolList[i];
                if (communicationTool.Id === communicationToolId) {
                    communicationToolInfo = communicationTool;
                    break;
                }
            }

            if (communicationToolInfo) {
                $scope.multipleLogs.CommunicationTool = communicationToolInfo;
            }

            $scope.updateTimeAndToolPrefix();
            $scope.$apply();
        };
        $scope.updateSelectedLanguage = function (languageId) {
            $scope.multipleLogs.LanguageId = languageId;
            $scope.$apply();
        };        
        $scope.updateSelectedSite = function (siteId) {
            siteId = parseInt(siteId);
            var siteInfo;
            var siteGroupLength = $scope.siteList.length;
            for (var i = 0; i < siteGroupLength; i++) {
                var siteGroup = $scope.siteList[i];
                var siteLength = siteGroup.ChildSites.length;
                for (var j = 0; j < siteLength; j++) {
                    var site = siteGroup.ChildSites[j];
                    if (site.Id === siteId) {
                        siteInfo = site;
                        break;
                    }
                }

                if (siteInfo) {
                    break;
                }
            }

            if (siteInfo) {
                $scope.multipleLogs.SiteInfo = siteInfo;
            } else {
                $scope.multipleLogs.SiteInfo = {
                    Id: crmConstants.Enums.Site.NotApply,
                    Value: viewResources.Select
                };
            }
        };
        $scope.updateSelectedStatus = function (statusId) {
            statusId = parseInt(statusId);
            var statusInfo;
            var statusListLength = $scope.statusList.length;
            for (var i = 0; i < statusListLength; i++) {
                var status = $scope.statusList[i];
                if (status.Id === statusId) {
                    statusInfo = status;
                    break;
                }
            }

            if (statusInfo) {
                $scope.multipleLogs.Status = statusInfo;
                $scope.$apply();
            }
        };
        $scope.updateSelectedRole = function (roleId) {
            roleId = parseInt(roleId);
            var roleInfo;
            var roleListLength = $scope.roleList.length;
            for (var i = 0; i < roleListLength; i++) {
                var role = $scope.roleList[i];
                if (role.Id === roleId) {
                    roleInfo = role;
                    break;
                }
            }

            if (roleInfo) {
                $scope.multipleLogs.RoleInfo = roleInfo;
                $scope.$apply();
            }
        };
        $scope.updateSelectedResponse = function (responseId) {
            responseId = parseInt(responseId);
            var responseInfo;
            var responseListLength = $scope.responseList.length;
            for (var i = 0; i < responseListLength; i++) {
                var response = $scope.responseList[i];
                if (response.Id === responseId) {
                    responseInfo = response;
                    break;
                }
            }

            if (responseInfo) {
                $scope.multipleLogs.Response = responseInfo;
                $scope.$apply();
            }
        };
        $scope.updateSelectedSubject = function (subject) {
            $scope.multipleLogs.Subject = subject;
        };
        $scope.updateRemark = function (remark) {
            $scope.multipleLogs.Remark = remark;
        };
        $scope.updateSolution = function (solution) {
            $scope.multipleLogs.Solution = solution;
        };

        $scope.GetCommunicationParams = function () {
            var communicationIds = [];
            angular.forEach($scope.selectedCommunications, function (communication) {
                communicationIds.push(communication.Id);
            })

            return communicationIds.join(",");
        }

        $scope.GetExtensionParams = function () {
            var extensionIds = [];
            angular.forEach($scope.selectedExtensions, function (extension) {
                extensionIds.push(extension.Id);
            })

            if (extensionIds.length == 0) {
                extensionIds = [$scope.allSettings.ExtensionNA.Id];
            }

            return extensionIds.join(",");
        }

        $scope.CheckSubProductBettype = function (subProductID, bettypeId) {
            var flag = false;
            angular.forEach($scope.allSettings.BettypeList, function (b) {
                if (b.Id == bettypeId && b.ParentId == subProductID) {
                    flag = true;
                }
            })

            return flag;
        }

        $scope.GetProductParams = function () {
            var products = [];
            if ($scope.selectedProducts.length == 0) {
                products = [
                   {
                       ProductId: $scope.allSettings.ProductNA.Id,
                       SubProductId: $scope.allSettings.SubProductNA.Id,
                       BettypeId: $scope.allSettings.BettypeNA.Id
                   }
                ];
            }
            else {
                var processedProductIds = [];
                angular.forEach($scope.selectedSubProducts, function (subProduct) {
                    var subProductBettypes = [];
                    angular.forEach($scope.selectedBettypes, function (bettype) {
                        if ($scope.CheckSubProductBettype(subProduct.Id, bettype.Id)) {
                            subProductBettypes.push({
                                ProductId: subProduct.ParentId,
                                SubProductId: subProduct.Id,
                                BettypeId: bettype.Id
                            })
                        }
                    })

                    if (subProductBettypes.length == 0) {
                        subProductBettypes.push({
                            ProductId: subProduct.ParentId,
                            SubProductId: subProduct.Id,
                            BettypeId: $scope.allSettings.BettypeNA.Id
                        })
                    }

                    products = products.concat(subProductBettypes);
                    processedProductIds.push(subProduct.ParentId);
                })

                angular.forEach($scope.selectedProducts, function (product) {
                    if (processedProductIds.indexOf(product.Id) < 0) {
                        products.push({
                            ProductId: product.Id,
                            SubProductId: $scope.allSettings.SubProductNA.Id,
                            BettypeId: $scope.allSettings.BettypeNA.Id
                        })
                    }
                })
            }

            return products;
        }
        
        $scope.GetCategoryParams = function () {
            var categories = [];
            angular.forEach($scope.selectedCategories, function (category) {
                if (category.ParentId != 0) {
                    categories.push({
                        MainCategoryID: category.ParentId,
                        SubCategoryID: category.Id
                    })
                }
                else {
                    categories.push({
                        MainCategoryID: category.Id,
                        SubCategoryID: ''
                    })
                }
            })

            return categories;
        }

        $scope.CheckInvalidCategories = function () {
            var invalidCategories = [];
            angular.forEach($scope.selectedCategories, function (category) {
                if (category.ParentId == 0) {
                    invalidCategories.push(category.Name);
                }
            })

            return invalidCategories.join(",")
        }

        $scope.insertMultipleLogs = function () {
            $scope.isShowResultMessage = false;
            $scope.isSuccess = true;

            var insertCriteria = {
                ListCustomers: $scope.relatedCustomerResultDirective.getRelatedCustomerName(),
                StartTime: moment(new Date($scope.multipleLogs.StartTime)).format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_String_Moment),
                EndTime: moment(new Date($scope.multipleLogs.EndTime)).format(crmConstants.Enums.FormatConstant.MM_DD_YYYY_HH_MM_TT_String_Moment),
                CountryId: $scope.multipleLogs.Country.Id,
                LanguageId: $scope.multipleLogs.LanguageId,
                StatusId: $scope.multipleLogs.Status.Id,
                RoleId: $scope.multipleLogs.RoleInfo.Id,
                SiteId: $scope.selectedSite ? $scope.selectedSite.Id : crmConstants.Enums.Site.NotApply,
                LicenseeTypeId: $scope.multipleLogs.LicenseeType.Id,
                ResponseId: $scope.multipleLogs.Response.Id,
                Subject: $scope.multipleLogs.Subject,
                Remark: $scope.multipleLogs.Remark,
                Solution: $scope.multipleLogs.Solution,
                AutoField: $scope.autoGeneratedText,
                CommunicationIds: $scope.GetCommunicationParams(),
                ExtensionIds: $scope.GetExtensionParams(),
                Products: $scope.GetProductParams(),
                Categories: $scope.GetCategoryParams()
            };
            if ($scope.validateInsertMultipleLogsCriteria(insertCriteria)) {
                $scope.BlockUI();
                $createMultipleLogsService.insertLog(insertCriteria)
                .then(function (data) {
                    $scope.showResult(data.Result);
                    $scope.unblockUI();
                },
                function () {
                    $scope.unblockUI();
                });
            }
        };
        $scope.showResult = function (result) {
            $scope.resultMessage = result.Message;
            $scope.isShowResultMessage = true;

            if (result.Code !== 0) {
                $scope.isSuccess = false;
                $scope.unblockUI();
                return;
            }

            $scope.unblockUI();
            setTimeout(function () {
                parent.menuManager.goToURL('centerFr', window.rootUrl + 'Feedbacks/ManageFeedback');
                parent.menuManager.setActiveClassForSubMenu('Feedback', 'Manage Feedback');
            }, sharedScriptManager.cachedData.alertSuccessTime);
        };
        $scope.updateChangeFlag = function () {
            if ($scope.isParamsChange()) {
                parent.homeManager.CachedData.isChangingData = true;
                parent.homeManager.CachedData.ignoreConfirmMessage = viewResources.Confirm_Ignore_MultipleLogs_Changes;
            } else {
                parent.homeManager.CachedData.isChangingData = false;
            }

            $scope.safeApply();
        };
        $scope.isParamsChange = function () {
            return $scope.multipleLogs.Country.Id !== $scope.oldmultipleLogs.Country.Id ||
                $scope.multipleLogs.CommunicationTool.Id !== $scope.oldmultipleLogs.CommunicationTool.Id ||
                $scope.multipleLogs.LanguageId !== $scope.oldmultipleLogs.LanguageId ||
                $scope.multipleLogs.Status.Id !== $scope.oldmultipleLogs.Status.Id ||
                $scope.multipleLogs.RoleInfo.Id !== $scope.oldmultipleLogs.RoleInfo.Id ||
                $scope.multipleLogs.SiteInfo.Id !== $scope.oldmultipleLogs.SiteInfo.Id ||
                $scope.multipleLogs.LicenseeType.Id !== $scope.oldmultipleLogs.LicenseeType.Id ||
                $scope.multipleLogs.Response.Id !== $scope.oldmultipleLogs.Response.Id ||
                $scope.multipleLogs.Remark !== $scope.oldmultipleLogs.Remark ||
                $scope.multipleLogs.Solution !== $scope.oldmultipleLogs.Solution ||
                $scope.multipleLogs.Subject !== $scope.oldmultipleLogs.Subject;
        };
        $scope.validateInsertMultipleLogsCriteria = function (criteria) {
            var result = true;
            var feedbackTime = new Date(criteria.StartTime);
            var resolutionTime = new Date(criteria.EndTime);

            if (criteria.ListCustomers.length === 0) {
                $scope.resultMessage = viewResources.Multiple_Logs_EmptyListOfCustomers;
                result = false;
            }

            if (feedbackTime > resolutionTime) {
                $scope.resultMessage = viewResources.Multiple_Logs_ErrorEndTime;
                result = false;
            }

            if (result && criteria.CommunicationIds == "") {
                $scope.resultMessage = viewResources.Multiple_Logs_NACommmunicationTool;
                result = false;
            }

            if (result && criteria.Categories.length == 0) {
                $scope.resultMessage = viewResources.Error_NAMainCategory;
                result = false;
            }

            var invalidCategories = $scope.CheckInvalidCategories();
            if (result && invalidCategories != "") {
                $scope.resultMessage = viewResources.NoSubcategoryError.replace('{0}', '[' + invalidCategories + ']');
                result = false;
            }

            if (result && criteria.LanguageId === crmConstants.Enums.Language.NotApply) {
                $scope.resultMessage = viewResources.Multiple_Logs_NALanguage;
                result = false;
            }

            if (result && criteria.Subject === '') {
                $scope.resultMessage = viewResources.Multiple_Logs_EmptySubject;
                result = false;
            }

            if (result &&
            ($.trim(criteria.Remark) === '' || criteria.Remark === '<br>')) {
                $scope.resultMessage = viewResources.Multiple_Logs_EmptyRemark;
                result = false;
            }

            if (result &&
            ($.trim(criteria.Solution) === '' || criteria.Solution === '<br>')) {
                $scope.resultMessage = viewResources.Multiple_Logs_EmptySolution;
                result = false;
            }

            if (result) {
                $scope.isShowResultMessage = false;
            } else {
                $scope.isShowResultMessage = true;
                $scope.isSuccess = false;
            }

            return result;
        };
        $scope.unblockUI = function () {
            try {
                parent.$.unblockUI();
            } catch (e) {
            }

            hotKeyManager.CachedData.isBlockingUI = false;
        };
        $scope.BlockUI = function () {
            hotKeyManager.CachedData.isBlockingUI = true;
            try {
                parent.BlockUI();
            } catch (e) {
            }
        };
        $scope.completeSetFocus = function () {
            $scope.isSetFocus = false;
        };
        $scope.setFocus = function () {
            $scope.$apply(function () {
                $scope.isSetFocus = true;
            });
        };

        $scope.verifyListCustomer = function () {
            var customerString = $scope.relatedCustomerResultDirective.normalizeUsername($scope.customersInput);

            if ($scope.relatedCustomerResultDirective.isValidUsername(customerString)) {
                $scope.isShowResultMessage = false;
                $scope.relatedCustomerResultDirective.verifyUsernames(customerString);
            }
            else {
                $scope.resultMessage = viewResources.CustomerNameLengthErrorMessage;
                $scope.isShowResultMessage = true;
            }
        };

        $scope.initRemarkPrefix = function () {
            if (!$scope.multipleLogs.StartTime) {
                return;
            }

            $scope.autoGeneratedText = $scope.generateTimeAndToolPrefix() + " \n" + $scope.generateCustomerPrefix(); 
        }

        $scope.updateTimeAndToolPrefix = function () {
            if (!$scope.multipleLogs.StartTime) {
                return;
            }

            var newTimeAndToolPrefix = $scope.generateTimeAndToolPrefix();
            var oldTimeAndToolPrefix = $scope.matchTimeAndToolPrefix();
            if (oldTimeAndToolPrefix != '') {
                $scope.autoGeneratedText = $scope.autoGeneratedText.replace(oldTimeAndToolPrefix, newTimeAndToolPrefix);
            }
            else {
                $scope.autoGeneratedText = newTimeAndToolPrefix + "\n" + $scope.autoGeneratedText;
            }
        }

        $scope.matchTimeAndToolPrefix = function () {
            var communicationTools = $scope.communicationToolList;
            communicationTools.push({ Name: "N/A" });
            var pattern = "Date Time & Tool: \\d{8}\\/\\d{2}:\\d{2}\\D{2}\\/";
            for (var i = 0; i < communicationTools.length; i++) {
                var tool = communicationTools[i];
                var match = $scope.autoGeneratedText.match(new RegExp(pattern + tool.Name));
                if (match != null) {
                    return match[0];
                }
            }

            return '';
        }

        $scope.generateTimeAndToolPrefix = function () {
            return sharedScriptManager.stringFormat("Date Time & Tool: {0}/{1}",
            [moment($scope.multipleLogs.StartTime).format(crmConstants.Enums.FormatConstant.StartTime_RemarkPrefix),
                $scope.latestedSelectedCommunication]);
        }

        $scope.communicationChangeHandler = function () {
            var newSelectedCommunications = [];
            angular.forEach($scope.selectedCommunications, function (item) {
                newSelectedCommunications.push(item.Name);
            });


            if (newSelectedCommunications.length > $scope.previousSelectedCommunications.length) {
                for (var i = 0; i < newSelectedCommunications.length; i++) {
                    var newName = newSelectedCommunications[i];
                    if ($scope.previousSelectedCommunications.indexOf(newName) < 0) {
                        $scope.latestedSelectedCommunication = newName;
                        break;
                    }
                }
            }

            $scope.previousSelectedCommunications = newSelectedCommunications;
            $scope.updateTimeAndToolPrefix();
        }

        $scope.generateCustomerPrefix = function () {
            return "System ID: ";
        }

        $scope.onStartTimeChanged = function () {
            $scope.$apply(function () {
                $scope.updateTimeAndToolPrefix();
            });
        };
       
        angular.element(document).ready(function () {
            sharedScriptManager.safeApply($scope);
            registerOptions($scope);
            $scope.$watch(function ($scope) {
                $scope.updateChangeFlag();
            });
            $scope.initController();
            $scope.setFocus();

            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: "MultipleLogs_Insert"
            });

            hotKeyManager.CachedData.isBubbleToParent = false;
        });
    }]);
})();