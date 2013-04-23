var currItem = new settingItem();
var oldSettingValue = '';

function onLoadSettingEdit() {
   
    try { parent.BlockUI(); } catch (e) { }
    var Id = gup('Id');
    var data = '{"Id":' + Id + '}';

    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetById",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (rs) {
            currItem = new settingItem();
            currItem.Value = rs.Value;
            currItem.Type = rs.Type;
            currItem.Id = rs.Id;
            currItem.DisplayOrder = rs.DisplayOrder;
            oldSettingValue = currItem.Value;

            $('#txtSettingName').val(currItem.Type);
            $('#txtSettingValue').val(currItem.Value);
            $('#txtDisplayOrder').val(currItem.DisplayOrder);

            $('#txtSettingName').attr('readonly', true);

            
        },
        error: function () { },
        complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
    });
}


function settingItem() {
    this.Value = "";
    this.Type = "";
    this.Id = 0;
    this.DisplayOrder = 0;

}

//
function checkParam() {
    var arrId = ['txtSettingName', 'txtSettingValue', 'txtDisplayOrder'];
    var arrMsg = ['Setting Name', 'Setting Value', 'Display Order'];
    for (i = 0; i < arrId.length; i++) {
        if ($('#' + arrId[i]).val().length == 0) {
            showAlertMessage("errMsg", 'Invalid ' + arrMsg[i], 1);
            $('#' + arrId[i]).focus();
            return false;
        }
    }

    return true;
}

function getParamData() {
    var item = currItem;
    item.Value = $('#txtSettingValue').val().trim();
    item.Type = $('#txtSettingName').val().trim();
    item.DisplayOrder = $('#txtDisplayOrder').val();
    var rstext = JSON.stringify(item);
    var data = '{"setting":' + rstext + ',"oldSettingName":"' + oldSettingValue + '","userId":' + $('#userId').val() + '}';
    return data;
}


function udpSetting() {
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
        url: window.rootUrl + "AdminTool/UpdateSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (rs) {
            if (rs.ErrCode > 0) {
                showAlertMessage("errMsg", rs.ErrMsg, 1);
            }
            else {
                showAlertMessage("errMsg", 'Update Setting successfully', 0);
                parent.afterUpdate(true);
            }
            try
            {
                parent.jQuery.unblockUI();
            } 
            catch (e){}
        }
    });
}

head.ready(function () {
    onLoadSettingEdit();
});