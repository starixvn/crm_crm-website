

function onLoadSettingIns() {
    var Value = decodeURI(gup('Value'));
    if (Value.length > 0) {
        $('#txtSettingName').val(Value);
        $('#txtSettingName').attr('readonly', true);
        if (Value.toLowerCase() == 'site') {
            $('#divSiteType').removeClass('hidden');
        }
        else {
            $('#divSiteType').addClass('hidden');
        }
    }

    var data = '{"settingName":"' + Value + '"}';

    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetAllSettingParent",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (rs) {
            $("#ParentTemplate").tmpl(rs).appendTo("#ddlParent");
            
        },
        error: function () { },
        complete: function () { try { parent.unblockUI(); } catch (e) { } }
    });
}


function settingItem() {
    this.Value = "";
    this.Type = "";    
    this.Id = 0;
    this.DisplayOrder = 0;

}

function getParamData() {
    var item = new settingItem();
    item.Value = $('#txtSettingValue').val().trim();
    item.Type = $('#txtSettingName').val().trim();
    item.Id = 0;
    item.DisplayOrder = $('#txtDisplayOrder').val();
    item.PrivateType = $('#chkSiteType').attr('checked') ? 1 : 0;

    var rstext = JSON.stringify(item);
    var data = '{"setting":' + rstext + ',"userId":' + $('#userId').val() + ',"parentId":' + $('#ddlParent').val() + '}';
    return data;
}

function checkParam() {
    var arrId = ['txtSettingName', 'txtSettingValue', 'txtDisplayOrder'];
    var arrMsg = ['Setting Name', 'Setting Value', 'Display Order'];
    for (i = 0; i < arrId.length; i++) {
        if ($('#' + arrId[i]).val().length == 0) {
            showAlertMessage("errMsg", 'Invalid ' + arrMsg[i] , 1);
            $('#' + arrId[i]).focus();
            return false;
        }
    }

    return true;
}


function insertSetting() {
    
    $('#errMsg').html('');

    var f = checkParam();
    if (!f)
        return;

    //input space only
    var settingName = trimAllSpace($('#txtSettingName').val());
    var SettingValue = trimAllSpace($('#txtSettingValue').val());
    if (settingName.length <= 0) {
        showAlertMessage("errMsg", 'Invalid Setting Name', 1);
        return false;
    }
    if (SettingValue.length <= 0) {
        showAlertMessage("errMsg", 'Invalid Setting Value', 1);
        return false;
    }
    if (settingName.length > 100) {
        showAlertMessage("errMsg", 'Max length of Setting Name is 100', 1);
        return false;
    }
    if (SettingValue.length > 100) {
        showAlertMessage("errMsg", 'Max length of Setting Value is 100', 1);
        return false;
    }
    var order = parseFloat($('#txtDisplayOrder').val());
    if (order > 2100000000) {
        showAlertMessage("errMsg", 'Display Order cannot greater than 2.100.000.000', 1);
        return false;
    }
    //---------------
    try { parent.BlockUI(); } catch (e) { }
    var data = getParamData();
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/InsertSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (rs) {
            if (rs.ErrCode > 0) {
                showAlertMessage("errMsg", rs.ErrMsg, 1);
            }
            else {
                showAlertMessage("errMsg", 'Insert Setting successfully', 0);
                parent.afterUpdate(true);
            }
            
        },
        error: function () { },
        complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
    });

}

head.ready(function () {
    onLoadSettingIns();
});