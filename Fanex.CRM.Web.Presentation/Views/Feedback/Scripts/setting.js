'use strict';
function Setting(allowDeleteFeedback) {
    var self = this;
    var allowDeleteFeedback = allowDeleteFeedback;

    function populateSelectWithGroup(element, setting, settingDefault) {
        element.empty();
        if (setting) {
            var group = null, option = null;
            if (settingDefault) {
                element.append($('<option>').attr('value', settingDefault.Id).html(settingDefault.Value));
            }
            $.each(setting, function (index, grp) {
                group = $('<optgroup>').attr('label', grp.Group);
                $.each(grp.Childs, function (ind, val) {
                    option = $('<option>').attr('value', val.Id).html(val.Value);
                    group.append(option);
                });

                element.append(group);
            });
        }
    }

    function populateSelect(element, setting, settingDefault) {
        element.empty();
        if (setting) {
            var option = null;
            if (settingDefault) {
                option = $('<option>').attr('value', settingDefault.Id).html(settingDefault.Value);
                element.append(option);
            }

            $.each(setting, function (index, val) {
                option = $('<option>').attr('value', val.Id).html(val.Value);
                element.append(option);
            });
        }
    }

    function buildSiteSetting(element, setting, settingDefault) {
        if (!setting) {
            return;
        }

        var sites = [
            { Group: 'Credit', Childs: [] },
            { Group: 'Licensee', Childs: []}];

        $.each(setting, function (index, val) {
            sites[val.SiteType].Childs.push({ Id: val.SiteId, Value: val.SiteName })
        });

        populateSelectWithGroup(element, sites, settingDefault);
    }

    function buildCountrySetting(element, setting, settingDefault) {
        if (!setting) {
            return;
        }

        var countries = [];
        $.each(setting, function (index, val) {
            countries.push({ Id: val.Id, Value: val.CountryName })
        });

        populateSelect(element, countries, settingDefault);
    }

    function bindingChildByParentId(element, childElement, setting, parentValue, settingDefault) {
        var totalItems = setting ? setting.length : 0;
        for (var i = 0; i < totalItems; i++) {
            if (setting[i].Id == parentValue) {
                if ((setting[i].Child && setting[i].Child.length > 0) || settingDefault) {
                    clearMessage('errmsg');
                    populateSelect(childElement, setting[i].Child, settingDefault);
                }
                else {
                    childElement.empty();                   
                    showAlertMessage('errmsg', 'There is no Subcategory for Category [' +
                        element.find('option:selected').text() + 
                        ']. Please add a Subcategory or choose another Category.', 1);
                }
                return;
            }
        }

        /* don't have child categories */
        childElement.empty();
        childElement.append($('<option>').attr('value', 0).html('N/A'));
    };

    function bindingCategoryAndProductEvents() {
        $('#ddlcategory').change(function () {
            var $this = $(this);
            bindingChildByParentId($this, $('#ddlsubcategory'), self.settings.Category, $this.val());
        });

        $('#ddlproduct').change(function () {
            var $this = $(this);
            bindingChildByParentId($this, $('#ddlbettype'), self.settings.Product, $this.val(), { Id: 0, Value: 'N/A' });
        });
    }

    /* public methods */
    self.settings = undefined;

    self.getSettings = function (callbackSuccess) {
        $.ajax({
            type: 'POST',
            url: window.rootUrl + 'AdminTool/SelAllSetting',
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (rs) {
                self.settings = rs;
                bindingCategoryAndProductEvents();

                populateSelect($('#ddltitle'), self.settings.Title);
                buildCountrySetting($('#ddlcountry'), self.settings.Country, { Id: 0, Value: 'N/A' });
                buildSiteSetting($('#ddlsite'), self.settings.Site, { Id: 0, Value: 'Select' });

                populateSelect($('#ddlcsextension'), self.settings.Extension);
                populateSelect($('#ddlcommunication'), self.settings.Communication);
                populateSelect($('#ddllanguage'), self.settings.Language, { Id: 0, Value: 'N/A' });

                var statusElement = $('#ddlstatus');
                populateSelect(statusElement, self.settings.Status);
                if (!allowDeleteFeedback) {
                    statusElement.find('option[value="11004"]').remove();
                }

                populateSelect($('#ddlresponse'), self.settings.Response);

                var categoryElement = $('#ddlcategory');
                populateSelect(categoryElement, self.settings.Category);
                bindingChildByParentId(categoryElement, $('#ddlsubcategory'), self.settings.Category, categoryElement.val());

                var productElement = $('#ddlproduct'); ;
                populateSelect(productElement, self.settings.Product, { Id: 0, Value: 'N/A' });
                bindingChildByParentId(productElement, $('#ddlbettype'), self.settings.Product, productElement.val(), { Id: 0, Value: 'N/A' });
            },
            error: function () {
            },
            complete: function () {
                if (callbackSuccess && (typeof callbackSuccess === 'function')) {
                    callbackSuccess();
                }
            }
        });
    };
}