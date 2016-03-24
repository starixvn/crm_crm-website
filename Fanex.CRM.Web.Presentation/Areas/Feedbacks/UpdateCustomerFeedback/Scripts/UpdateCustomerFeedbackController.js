(function () {
    "use strict";
    /* global crmConstants,
    sharedScriptManager,
    hotKeyManager,
    updateFeedbackResources,
    feedbackId,
    hasReviewFeedbackPermission,
    isShowDeletedStatus,
    isDeletedFeedbackView,
    orgLengt:true */
    /* jshint camelcase: false */
    angular.module('feedbacks-updatecustomerfeedback-application').controller('updateCustomerFeedbackController',
['$scope', 'UpdateCustomerFeedbackService', '$uiBlockerService', 'alertBoxService',
    function ($scope, feedbackService, $uiBlockerService, alertBoxService) {
        $scope.feedback = null;
        $scope.reviewedBy = '';
        $scope.relatedCustomers = [];
        $scope.hasReviewPermission = hasReviewFeedbackPermission === 'True';
        $scope.bettypes = null;
        $scope.subProducts = null;

        var oldReview;
        $scope.panelManager = {
            customer: false,
            detail: true,
            description: true,
            solution: true,
            review: false,
            showDescriptionPreview: true,
            showSolutionPreview: true,
            showReviewPreview: true
        };
        $scope.requiredManager = {
            subject: true,
            description: true,
            solution: true,
            review: true
        };

        // Subject with auto-complete.
        $scope.feedbackSubjectTemplateList = [];
        $scope.feedbackSubjectTemplateTotalPage = 0;
        $scope.isLoadedFeedbackSubjectTemplate = false;

        // Functions of Related customers.
        $scope.verifyUsernames = function () {
            var usernames = [];
            $('#txtrelatedcustomer option').each(function (i, option) {
                usernames[i] = $(option).text();
            });

            var username = usernames.join(',');
            if (username.length !== 0) {
                $uiBlockerService.blockUI();
                feedbackService.verifyUsernames({ Username: username })
                .then(function (data) {
                    var customers = [];
                    angular.forEach(data.Result, function (customer, index) {
                        /* jshint unused: false */
                        customers.push({ Id: customer.Id, Name: customer.Name });
                    });

                    customers = $scope.relatedCustomers.concat(customers);
                    // unique customer.
                    for (var i = 0; i < customers.length; ++i) {
                        for (var j = i + 1; j < customers.length; ++j) {
                            if (customers[i].Id === customers[j].Id) {
                                customers.splice(j--, 1);
                            }
                        }
                    }

                    $scope.relatedCustomers = customers;
                    $uiBlockerService.unblockUI();
                },
                function () {
                    $uiBlockerService.unblockUI();
                });
            }
        };
        // ---------- End of Functions of Related customers ----------

        // Subject functions.
        $scope.updateSelectedSubject = function (subject) {
            $scope.feedback.Subject = subject;
        };
        $scope.getNumberOfFeedbackSubjectTemplate = function () {
            feedbackService.getNumberOfFeedbackSubjectTemplate()
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
            feedbackService.getFeedbackSubjectTemplateByPagingIndex({ PagingIndex: pagingIndex })
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
            if (!$scope.isLoadedFeedbackSubjectTemplate) {
                return;
            }

            $scope.subjectOptions = {
                NumberOfSuggestItems: 15,
                SubjectList: $scope.feedbackSubjectTemplateList,
                Subject: '',
                IsUpdate: true,
                OnChangeEvent: $scope.updateSelectedSubject
            };
        };
        // ---------- End of Subject functions ----------

        // Functions of jHtml.
        $scope.updateDescription = function (description) {
            $scope.descriptionOptions.SelectedText = description;
            $scope.feedback.Description = description;
        };
        $scope.updateSolution = function (solution) {
            $scope.solutionOptions.SelectedText = solution;
            $scope.feedback.Solution = solution;
        };
        $scope.updateReview = function (review) {
            $scope.reviewOptions.SelectedText = review;
            $scope.feedback.Review = review;
        };
        // ---------- End of Functions of jHtml ----------

        $scope.initController = function () {
            $uiBlockerService.blockUI();
            $scope.isSetFocus = true;
            hotKeyManager.CachedData.ActiveDocumentManager.push({
                Value: $scope,
                Id: 'Feedback_UpdateCustomerFeedback'
            });

            $scope.getNumberOfFeedbackSubjectTemplate();
            feedbackService.getSettings()
            .then(function (data) {
                $scope.settings = data.Result;

                if (!isShowDeletedStatus) {
                    feedbackService.removeDeletedStatus($scope.settings.StatusList);
                }

                feedbackService.getFeedback(feedbackId)
                    .then(function (dataFeedback) {
                        $scope.feedback = dataFeedback.Result;

                        if ($scope.feedback.CustomerInfo.Title.Id === 0) {
                            $scope.feedback.CustomerInfo.Title = $scope.settings.Titles[0];
                        }

                        $scope.feedback.OverrideCRMInfo = false;
                        oldReview = $scope.feedback.Review;
                        $scope.panelManager.review = $scope.feedback.IsReviewed;
                        if ($scope.feedback.ReviewerId > 0) {
                            $scope.panelManager.showReviewPreview = false;
                            $scope.reviewedBy = updateFeedbackResources.ReviewedBy
                                .replace('{0}', $scope.feedback.ReviewerName)
                                .replace('{1}', $scope.feedback.SolveTimeString);
                        }

                        $scope.descriptionOptions = {
                            OnChangeEvent: $scope.updateDescription,
                            SelectedText: $scope.feedback.Description,
                            EditIconText: updateFeedbackResources.InsertEditBlock
                        };
                        $scope.solutionOptions = {
                            OnChangeEvent: $scope.updateSolution,
                            SelectedText: $scope.feedback.Solution,
                            EditIconText: updateFeedbackResources.InsertEditBlock
                        };
                        $scope.reviewOptions = {
                            OnChangeEvent: $scope.updateReview,
                            SelectedText: $scope.feedback.Review,
                            EditIconText: updateFeedbackResources.InsertEditBlock
                        };

                        $scope.getSubProducts();
                        $scope.getBettypes();

                        $scope.updateAllSettingsForFeedback();

                        $scope.getSubcategories();

                        $scope.initRelatedCustomer();                       

                        $uiBlockerService.unblockUI();
                    },
                    function () {
                        $uiBlockerService.unblockUI();
                    });
            },
            function () {
                $uiBlockerService.unblockUI();
            });
        };

        $scope.getSubProducts = function () {
            angular.forEach($scope.settings.Products, function (product, index) {
                /* jshint unused: false */
                if (product.Id === $scope.feedback.Product.Id) {
                    $scope.subProducts = product.ChildSettings;
                }
            });

            if (!$scope.subProducts) {
                $scope.subProducts = $scope.settings.Products[0].ChildSettings;
            }
        };
        $scope.getBettypes = function () {
            angular.forEach($scope.subProducts, function (subProduct, index) {
                /* jshint unused: false */
                if (subProduct.Id === $scope.feedback.SubProduct.Id) {
                    $scope.bettypes = subProduct.ChildSettings;
                }
            });

            if (!$scope.bettypes) {
                $scope.bettypes = $scope.subProducts[0].ChildSettings;
            }
        };

        $scope.getSubcategories = function () {
            angular.forEach($scope.settings.Categories, function (category, index) {
                /* jshint unused: false */
                if (category.Id === $scope.feedback.Category.Id) {
                    $scope.subcategories = category.ChildSettings;
                }
            });
        };

        $scope.updateAllSettingsForFeedback = function () {
            $scope.updateFeedbackSetting($scope.feedback.Extension, $scope.settings.Extensions);
            $scope.updateFeedbackSetting($scope.feedback.Communication, $scope.settings.Communications);

            $scope.updateFeedbackSetting($scope.feedback.Category, $scope.settings.Categories);            
            $scope.updateInActiveChildSettings($scope.feedback.SubCategory, $scope.settings.Categories, $scope.feedback.Category.Id);

            //$scope.updateFeedbackSetting($scope.feedback.Product, $scope.settings.Products);
            //$scope.updateInActiveChildSettings($scope.feedback.SubProduct, $scope.settings.Products, $scope.feedback.Product.Id);

            //$scope.updateFeedbackSetting($scope.feedback.SubProduct, $scope.subProducts);
            //$scope.updateInActiveChildSettings($scope.feedback.BetType, $scope.subProducts, $scope.feedback.SubProduct.Id);

            $scope.updateFeedbackSetting($scope.feedback.Language, $scope.settings.Languages);
            $scope.updateFeedbackSetting($scope.feedback.Status, $scope.settings.StatusList);
            $scope.updateFeedbackSetting($scope.feedback.Role, $scope.settings.Roles);
            $scope.updateFeedbackSetting($scope.feedback.Response, $scope.settings.Responses);

            $scope.updateFeedbackSetting($scope.feedback.CustomerInfo.Title, $scope.settings.Titles);
            $scope.updateFeedbackSetting($scope.feedback.CustomerInfo.LicenseeType, $scope.settings.LicenseeTypes);
        };

        $scope.updateFeedbackSetting = function (setting, dataList) {
            if (!$scope.hasSetting(setting.Id, dataList)) {
                dataList.push({
                    Id: setting.Id,
                    Name: setting.Name,
                    Value: setting.Name
                });
            }
        };

        $scope.hasSetting = function (settingId, dataList) {
            for (var i = 0; i < dataList.length; i++) {
                if (settingId == dataList[i].Id) {
                    return true;
                }
            }

            return false;
        };

        $scope.updateInActiveChildSettings = function (setting, parentList, parentId) {
            var childSettings = $scope.getChildSetting(parentId, parentList);

            if (!$scope.hasSetting(setting.Id, childSettings)) {
                childSettings.push({
                    Id: setting.Id,
                    Name: setting.Name
                });
            }
        };

        $scope.getChildSetting = function (parentId, parentDataList) {
            for (var i = 0; i < parentDataList.length; i++) {
                var parent = parentDataList[i];

                if (parentId === parent.Id) {
                    if (!parent.ChildSettings) {
                        parent.ChildSettings = [];
                    }

                    return parent.ChildSettings;
                }
            }
        }
       
        $scope.categoryChanged = function () {
            $scope.subcategories = $scope.feedback.Category.ChildSettings;
            if ($scope.subcategories.length === 0) {
                $scope.errorMessage(updateFeedbackResources.NoSubcategoryError.replace('{0}', '[' + $scope.feedback.Category.Name + ']'));
            }
            else {
                $scope.hideMessage();
                $scope.feedback.SubCategory = $scope.subcategories[0];
            }
        };

        $scope.productChanged = function () {
            $scope.subProducts = $scope.getChildSetting($scope.feedback.Product.Id, $scope.settings.Products);
            $scope.feedback.SubProduct = $scope.subProducts[0];
            $scope.subProductChanged();
        };

        $scope.subProductChanged = function () {
            var childSettings = $scope.getChildSetting($scope.feedback.SubProduct.Id, $scope.subProducts);
            if (!childSettings || childSettings.length === 0) {
                var notApplySetting = $scope.subProducts[0];
                childSettings = [{ Id: notApplySetting.Id, Name: notApplySetting.Name }];
            }

            $scope.bettypes = childSettings;

            if ($scope.bettypes.length === 0) {
                $scope.errorMessage(updateFeedbackResources.NoBettypeError.replace('{0}', '[' + $scope.feedback.Product.Value + ']'));
            }
            else {
                $scope.hideMessage();
                $scope.feedback.BetType = $scope.bettypes[0];
            }
        };

        $scope.validateFeedback = function () {
            $scope.hideMessage();

            var feedback = $scope.feedback;
            var feedbackTime = new Date(feedback.FeedbackTimeString);
            var resolutionTime = new Date(feedback.ResolutionTimeString);
            if (feedbackTime > resolutionTime) {
                $scope.errorMessage(updateFeedbackResources.ErrorFeedbackResolutionTime);
                return false;
            }

            if (feedback.Communication === $scope.settings.Communications[0]) {
                $scope.errorMessage(updateFeedbackResources.NACommmunicationTool);
                return false;
            }

            var category = feedback.Category;
            if (category.ChildSettings && category.ChildSettings.length === 0) {
                $scope.errorMessage(updateFeedbackResources.NoSubcategoryError.replace('{0}', '[' + category.Name + ']'));
                return false;
            }

            var product = feedback.Product;
            if (product.ChildSettings && product.ChildSettings.length === 0) {
                $scope.errorMessage(updateFeedbackResources.NoBettypeError.replace('{0}', '[' + product.Value + ']'));
                return false;
            }

            if (feedback.Language === $scope.settings.Languages[0]) {
                $scope.errorMessage(updateFeedbackResources.NALanguage);
                return false;
            }

            $scope.requiredManager.subject = feedback.Subject.trim().length !== 0;
            if (!$scope.requiredManager.subject) {
                $scope.errorMessage(updateFeedbackResources.EmptySubject);
                return false;
            }

            $scope.requiredManager.description = feedback.Description.trim().length !== 0 && feedback.Description !== '<br>';
            if (!$scope.requiredManager.description) {
                $scope.errorMessage(updateFeedbackResources.EmptyRemark);
                return false;
            }

            $scope.requiredManager.solution = feedback.Solution.trim().length !== 0 && feedback.Solution !== '<br>';
            if (!$scope.requiredManager.solution) {
                $scope.errorMessage(updateFeedbackResources.EmptySolution);
                return false;
            }

            $scope.requiredManager.review = !(feedback.IsReviewed && (feedback.Review.trim().length === 0 || feedback.Review === '<br>'));
            if (!$scope.requiredManager.review) {
                $scope.errorMessage(updateFeedbackResources.EmptyReview);
                return false;
            }

            return true;
        };

        $scope.updateFeedback = function () {
            if (!$scope.validateFeedback()) {
                return;
            }

            var feedback = $scope.feedback;
            if (!feedback.OverrideCRMInfo) {
                $scope.updateCustomerFeedback(feedback);
                return;
            }

            parent.homeManager.showConfirmOverrideCRMInfo(updateFeedbackResources.ConfirmOverrideCRMInfo, function (bool) {
                if (bool) { $scope.updateCustomerFeedback(feedback); }
            });
        };

        $scope.updateCustomerFeedback = function (feedback) {
            $uiBlockerService.blockUI();
            if (feedback.HasRelated) {
                if ($scope.relatedCustomers.length > 0) {
                    var relatedCustomerIds = [];
                    angular.forEach($scope.relatedCustomers, function (customer, index) {
                        /* jshint unused: false */
                        relatedCustomerIds.push(customer.Id);
                    });

                    feedback.RelatedCustomer = relatedCustomerIds.join(',');
                }
                else {
                    feedback.RelatedCustomer = null;
                }
            }

        feedback.UpdateReviewMode = !$scope.hasReviewPermission ? 0 : (!feedback.IsReviewed ? 2 : (oldReview === feedback.Review ? 0 : 1));
        feedbackService.updateFeedback(feedback, isDeletedFeedbackView)
            .then(function (data) {
                var result = data.Result;
                if (result && result.Code === 0) {
                    alertBoxService.show({
                        type: crmConstants.Enums.MessageType.Success,
                        message: updateFeedbackResources.UpdateFeedbackSuccessfully,
                        callback: function () {
                            parent.ClosePopup();
                            if (feedback.OverrideCRMInfo) {
                                parent.leftPanelManager.loadLeftPanel(feedback.CustomerInfo.Name, true);
                            }
                        }
                    });
                }
                else {
                    $scope.errorMessage(result.Code ? result.Message : 'Oppss... An error occurred while processing your request.');
                }

                $uiBlockerService.unblockUI();
            },
            function () {
                $uiBlockerService.unblockUI();
            });
        };

        $scope.errorMessage = function (error) {
            alertBoxService.show({
                type: crmConstants.Enums.MessageType.Error,
                message: error
            });
        };

        $scope.hideMessage = function () {
            alertBoxService.hide();
        };

        $scope.invokeHotKey_CtrlS = function () {
            $scope.updateFeedback();
        };

        $scope.invokeHotKey_CtrlE = function () {
            if (!$scope.feedback.HasRelated) {
                return;
            }

            $scope.verifyUsernames();
        };

        $scope.initRelatedCustomer = function () {
            if ($scope.feedback.HasRelated) {
                var control = $('#txtrelatedcustomer');
                var arrayTemp = $scope.feedback.RelatedCustomer.split(',');
                angular.forEach(arrayTemp, function (name, index) {
                    /* jshint unused: false */
                    control.trigger('addItem', { title: name, value: name });
                });

                orgLengt = arrayTemp.length;
            }
        };

        angular.element(document).ready(function () {
            sharedScriptManager.safeApply($scope);
            $scope.initController();
        });
    } ]);
})();