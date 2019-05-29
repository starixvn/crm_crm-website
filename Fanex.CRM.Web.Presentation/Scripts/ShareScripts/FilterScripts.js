var filterScripts;
var FilterScript = function () {
    var self = this;

    self.getSubProducts = function (seletedProducts, subProductList, selectedSubProductIds) {
        var selectedProductIds = [];
        angular.forEach(seletedProducts, function (product) {
            selectedProductIds.push(product.Id);
        });

        var subProducts = [];
        angular.forEach(subProductList, function (subProduct) {
            if (selectedProductIds.indexOf(subProduct.ParentId) >= 0) {
                subProduct.checked = false;
                subProducts.push(subProduct);
            }
        });

        // check selected sub products
        angular.forEach(subProducts, function (subProduct) {
            if (selectedSubProductIds.indexOf(subProduct.Id) >= 0) {
                subProduct.checked = true;
            }
        });

        return subProducts;
    }

    self.getBettypes = function (selectedSubProducts, bettypeList, selectedBettypeIds) {
        var selectedSubProductIds = [];
        angular.forEach(selectedSubProducts, function (subProduct) {
            selectedSubProductIds.push(subProduct.Id);
        });

        var bettypes = [];
        angular.forEach(bettypeList, function (bettype) {
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
            if (selectedBettypeIds.indexOf(bettype.Id) >= 0) {
                bettype.checked = true;
            }
        });

        return distinctBettypes;
    }

    self.getSubCategories = function (selectedCategories, subCategoryList, selectedSubCategoryIds) {
        var selectedCategoryIds = [];
        angular.forEach(selectedCategories, function (category) {
            selectedCategoryIds.push(category.Id);
        });

        var subCategories = [];
        angular.forEach(subCategoryList, function (subCategory) {
            if (selectedCategoryIds.indexOf(subCategory.ParentId) >= 0) {
                subCategories.push(subCategory);
            }
        });

        // remove duplicate bettypes
        var selectedIds = [];
        var distinctSubCategories = [];
        angular.forEach(subCategories, function (subCategory) {
            if (selectedIds.indexOf(subCategory.Id) < 0) {
                selectedIds.push(subCategory.Id);
                subCategory.checked = false;
                distinctSubCategories.push(subCategory);
            }
        });

        // check selected bettypes
        angular.forEach(distinctSubCategories, function (subCategory) {
            if (selectedSubCategoryIds.indexOf(subCategory.Id) >= 0) {
                subCategory.checked = true;
            }
        });

        return distinctSubCategories;
    }

    self.GetCommunicationParams = function (selectedCommunications) {
        var communicationIds = [];
        angular.forEach(selectedCommunications, function (communication) {
            communicationIds.push(communication.Id);
        })

        return communicationIds.join(",");
    }

    self.GetExtensionParams = function (selectedExtensions, extensionNA) {
        var extensionIds = [];
        console.log(selectedExtensions);
        console.log(extensionNA);
        if (selectedExtensions || selectedExtensions.length > 0) {

            angular.forEach(selectedExtensions, function (extension) {
                if (extension["Id"] !== undefined) {
                    extensionIds.push(extension.Id);
                }
            });
        }
        if (extensionIds.length === 0 && extensionNA !== null && extensionNA["Id"] !== undefined) {
            extensionIds.push(extensionNA.Id);
        }
        return extensionIds.join(",");
    };

    self.CheckSubProductBettype = function (subProductID, bettypeId, bettypeList) {
        var flag = false;
        angular.forEach(bettypeList, function (b) {
            if (b.Id == bettypeId && b.ParentId == subProductID) {
                flag = true;
            }
        })

        return flag;
    }

    self.GetProductParams = function (selectProductsConfig, productNAConfig, bettypeList) {
        var products = [];
        if (selectProductsConfig.selectedProducts.length == 0) {
            products = [
                {
                    ProductId: productNAConfig.ProductNA.Id,
                    SubProductId: productNAConfig.SubProductNA.Id,
                    BettypeId: productNAConfig.BettypeNA.Id
                }
            ];
        }
        else {
            var processedProductIds = [];
            angular.forEach(selectProductsConfig.selectedSubProducts, function (subProduct) {
                var subProductBettypes = [];
                angular.forEach(selectProductsConfig.selectedBettypes, function (bettype) {
                    if (self.CheckSubProductBettype(subProduct.Id, bettype.Id, bettypeList)) {
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
                        BettypeId: productNAConfig.BettypeNA.Id
                    })
                }

                products = products.concat(subProductBettypes);
                processedProductIds.push(subProduct.ParentId);
            })

            angular.forEach(selectProductsConfig.selectedProducts, function (product) {
                if (processedProductIds.indexOf(product.Id) < 0) {
                    products.push({
                        ProductId: product.Id,
                        SubProductId: productNAConfig.SubProductNA.Id,
                        BettypeId: productNAConfig.BettypeNA.Id
                    })
                }
            })
        }

        return products;
    }

    self.CheckCategorySubCategory = function (categoryId, subCategoryId, subCategoryList) {
        var flag = false;
        angular.forEach(subCategoryList, function (b) {
            if (b.Id == subCategoryId && b.ParentId == categoryId) {
                flag = true;
            }
        })

        return flag;
    }

    self.GetCategoryParams = function (selectedCategories, selectedSubCategories, subCategoryList) {
        var categories = [];
        angular.forEach(selectedCategories, function (category) {
            angular.forEach(selectedSubCategories, function (subCategory) {
                if (self.CheckCategorySubCategory(category.Id, subCategory.Id, subCategoryList)) {
                    categories.push({
                        MainCategoryID: category.Id,
                        SubCategoryID: subCategory.Id
                    })
                }
            })
        });

        return categories;
    }

    self.CheckCategoryParams = function (selectedCategories, selectedSubCategories, subCategoryList) {
        var inValidCategories = [];
        angular.forEach(selectedCategories, function (category) {
            var validCategory = false;
            angular.forEach(selectedSubCategories, function (subCategory) {
                if (self.CheckCategorySubCategory(category.Id, subCategory.Id, subCategoryList)) {
                    validCategory = true;
                }
            })

            if (!validCategory) {
                inValidCategories.push(category.Name);
            }
        });

        return inValidCategories;
    }
};

filterScripts = new FilterScript();