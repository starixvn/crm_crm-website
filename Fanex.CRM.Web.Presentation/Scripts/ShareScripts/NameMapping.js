var nameMapping;
var NameMapping = function () {
    var self = this;
    
    self.getMainCategoryName = function(mainCategoryId, mainCategories) {
        for (var i = 0; i < mainCategories.length; i++) {
            if (mainCategories[i].Id == mainCategoryId) {
                return mainCategories[i].Value;
            }
        }

        return "N/A";
    }

    self.getSubCategoryName = function (mainCategoryId, subCategoryId, mainCategories) {
        var mainCategory = null;
        for (var i = 0; i < mainCategories.length; i++) {
            if (mainCategories[i].Id == mainCategoryId) {
                mainCategory = mainCategories[i];
                break;
            }
        }

        if (mainCategory) {
            for (var j = 0; j < mainCategory.ChildSettings.length; j++) {
                if (mainCategory.ChildSettings[j].Id == subCategoryId) {
                    return mainCategory.ChildSettings[j].Name;
                }
            }
        }

        return '';
    }

    self.renderCategoryColumn = function(feedback, mainCategories, notCheckCategory) {
        var settings = feedback.Settings;

        var mainCategoryIds = [];
        for (var i = 0; i < settings.length; i++) {
            if ((notCheckCategory || settings[i].SettingType == "Category")
                    && mainCategoryIds.indexOf(settings[i].SettingIdLv1) < 0) {
                mainCategoryIds.push(settings[i].SettingIdLv1);
            }
        }

        var categories = [];
        for (var i = 0; i < mainCategoryIds.length; i++) {
            var subCategoryIds = [];
            for (var j = 0; j < settings.length; j++) {
                if ((notCheckCategory || settings[j].SettingType == "Category")
                        && settings[j].SettingIdLv1 == mainCategoryIds[i]) {
                    subCategoryIds.push(settings[j].SettingIdLv2);
                }
            }

            categories.push({
                mainCategoryId: mainCategoryIds[i],
                subCategoryIds: subCategoryIds
            })
        }

        var result = '';
        for (var i = 0; i < categories.length; i++) {
            var categoryHtml = '<div class="main-category">' + self.getMainCategoryName(categories[i].mainCategoryId, mainCategories);
            var subCategoryIds = categories[i].subCategoryIds;
            for (var j = 0; j < subCategoryIds.length; j++) {
                categoryHtml += '<div><span class="sub-category">' + self.getSubCategoryName(categories[i].mainCategoryId, subCategoryIds[j], mainCategories) + '</span></div>';
            }
            categoryHtml += '</div>';
            result += categoryHtml;
        }

        return result;
    }

    self.getProductName = function(productId, productList) {
        for (var i = 0; i < productList.length; i++) {
            if (productList[i].Id == productId) {
                return productList[i].Name;
            }
        }

        return "N/A";
    }

    self.renderProductColumn = function(feedback, productList, subProductList) {
        var settings = feedback.Settings;

        var productIds = [];
        for (var i = 0; i < settings.length; i++) {
            if (settings[i].SettingType == "Product" && productIds.indexOf(settings[i].SettingIdLv1) < 0) {
                productIds.push(settings[i].SettingIdLv1);
            }
        }

        var products = [];
        for (var i = 0; i < productIds.length; i++) {
            var subProductIds = [];
            for (var j = 0; j < settings.length; j++) {
                if (settings[j].SettingType == "Product"
                        && settings[j].SettingIdLv1 == productIds[i]
                        && subProductIds.indexOf(settings[j].SettingIdLv2) < 0) {
                    subProductIds.push(settings[j].SettingIdLv2);
                }
            }

            products.push({
                productId: productIds[i],
                subProductIds: subProductIds
            })
        }

        var result = '';
        for (var i = 0; i < products.length; i++) {
            var productHtml = '<div class="main-category">' + self.getProductName(products[i].productId, productList);
            var subProductIds = products[i].subProductIds;
            for (var j = 0; j < subProductIds.length; j++) {
                productHtml += '<div><span class="sub-category">' + self.getProductName(subProductIds[j], subProductList) + '</span></div>';
            }
            productHtml += '</div>';
            result += productHtml;
        }

        return result;
    }

    self.getCommunicationName = function(id, communicationList) {
        for (var i = 0; i < communicationList.length; i++) {
            if (communicationList[i].Id == id) {
                return communicationList[i].Name;
            }
        }

        return "N/A";
    }

    self.renderCommunicationColumn = function(feedback, communicationList) {
        var settings = feedback.Settings;

        var result = '';
        for (var i = 0; i < settings.length; i++) {
            if (settings[i].SettingType == "Communication") {
                result += '<div class="">' + self.getCommunicationName(settings[i].SettingIdLv1, communicationList) + '</div>';
            }
        }

        return result;
    }


    self.getExtensionName = function(id, extensionList) {
        for (var i = 0; i < extensionList.length; i++) {
            if (extensionList[i].Id == id) {
                return extensionList[i].Value;
            }
        }

        return "N/A";
    }

    self.renderExtensionColumn = function(feedback, extensionList) {
        var settings = feedback.Settings;

        var result = '';
        for (var i = 0; i < settings.length; i++) {
            if (settings[i].SettingType == "Station") {
                result += '<div class="">' + self.getExtensionName(settings[i].SettingIdLv1, extensionList) + '</div>';
            }
        }

        return result;
    }    
};

nameMapping = new NameMapping();