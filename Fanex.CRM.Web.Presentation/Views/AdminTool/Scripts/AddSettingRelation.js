"use strict";
/*global gup, showAlertMessage, head */
/* exported SetRelationSetting, RemoveSettingRelation */

//Contain the template
var tmplSetting = null;
function bindingSetting(rs) {
    var html = '';

    for (var i = 0; i < rs.length; i++) {
        var item = rs[i];
        var checked = item.IsSub ? ' checked="checked" ' : '';
        html += '<tr>\
                <td class="l"><div style="width:400px; height:17px;">' + item.Value + '</div></td>\
                <td class="c"><div style="width:60px; height:17px;">' +
                    '<input type="checkbox" id="chk' + item.Id + '"' + checked + ' class="hand" name="chk" value="' + item.Id + '"/></div></td>\
                <td class="c"><div style="width:60px; height:17px;">' +
                    '<input class="c" type="text" style="width:40px;" id="txt' + item.Id +
                        '" onkeypress="return CheckDecNumber(event);" value="' + item.DisplayOrder + '" /></div></td>\
            </tr>';
    }
    return html;
}

function bindingSettingLeft(rs) {
    var html = '';
    for (var i = 0; i < rs.length; i++) {
        var item = rs[i];
        html += '<tr>\
                <td class="l"><div style="width:400px; height:17px;">' + item.Value + '</div></td>\
            </tr>';
    }

    return html;
}

function getSettingRelationByName() {
    $('#errMsg').html('');
    var mainreport = $("#addsettingrelationgrid");
    $('#btnSubmit').addClass('hidden');
    try { parent.parent.BlockUI(); } catch (e) { parent.BlockUI(); }

    var settingName = $('#ddlMainSetting').val();
    var parentId = gup('parentId');
    var data = '{"settingName":"' + settingName + '","parentId":' + parentId + '}';
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetSettingRelationByName",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (rs) {

            if (rs && rs.length > 0) {
                var settings = bindingSetting(rs);
                var settingsLeft = bindingSettingLeft(rs);
                $('#btnSubmit').removeClass('hidden');

                $("#addsettingrelationbody tbody").empty().append(settings);
                $("#addsettingrelationheader tbody").empty().append(settingsLeft);

                var gridOpts = {
                    tblContentId: 'addsettingrelationbody',
                    manualCopyHeader: true,
                    fullWidth: true,
                    fullHeight: true,
                    gridHeight: 90,
                    freezeWidth: 400,
                    numberFreezeCol: 1
                };
                mainreport.removeClass("hidden").gridScroller(gridOpts);
            }
            else {

                mainreport.addClass('hidden');
            }
        },
        error: function () {
            mainreport.addClass('hidden');
        },
        complete: function () {
            try { parent.parent.jQuery.unblockUI(); } catch (e) { parent.jQuery.unblockUI(); }
        }
    });
}

function loadMainSetting() {
    try { parent.BlockUI(); } catch (e) { }
    var data = '{"settingName":"' + decodeURI(gup('settingName')) + '"}';
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetAllSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (rs) {

            $("#ddlMainSetting").children().remove().end();
            $("#tplMainSetting").tmpl(rs).appendTo("#ddlMainSetting");
            getSettingRelationByName();
        },
        error: function () { },
        complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
    });
}

function SetRelationSetting() {
    var settings = [];

    $(':checkbox:checked').each(function (i) {
        settings[i] = $(this).val() + "_" + $("#" + $(this).attr("id").replace("chk", "txt")).val();
    });


    var userId = $('#userId').val();
    var parentId = gup('parentId');
    var settingName = $("#ddlMainSetting").val();
    var data = '{"settingId":"' + settings.join("|") + '","settingName":"' + settingName + '","userId":' + userId + ',"parentId":' + parentId + '}';



    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/UpdateSettingRelation",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (rs) {
            parent.jQuery.unblockUI();

            if (rs.ErrCode > 0) {
                showAlertMessage("errMsg", rs.ErrMsg, 1);
            }
            else {
                showAlertMessage("errMsg", 'Set setting relation successfully', 0);
                parent.afterUpdate(true);
            }
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

function RemoveSettingRelation(settingId, settingValue) {
    if (!confirm('Are you sure you want to remove sub ' + settingValue + ' out of setting ' + decodeURI(gup('settingValue')) + '?')) {
        return;
    }

    try { parent.BlockUI(); } catch (e) { }
    var userId = $('#userId').val();
    var parentId = gup('parentId');
    var data = '{"settingId":' + settingId + ',"userId":' + userId + ',"parentId":' + parentId + '}';

    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/DeleteSettingRelationById",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: data,
        success: function (rs) {

            if (rs.ErrCode > 0) {
                showAlertMessage("errMsg", rs.ErrMsg, 1);
            }
            else {
                showAlertMessage("errMsg", 'Remove setting relation successfully', 0);
                //parent.afterUpdate();
                setTimeout(function () { getSettingRelationByName(); }, 2000);
            }
            try { parent.jQuery.unblockUI(); } catch (e) { }
        }
    });
}

head.ready(
    function () {
        tmplSetting = $("#tplSetting").template();
        var settingValue = decodeURI(gup('settingValue'));
        $('#spParent').html(settingValue);

        var gridOpts = {
            tblContentId: 'addsettingrelationbody',
            isShowPager: false
        };
        $("#addsettingrelationgrid").initGrid(gridOpts);

        loadMainSetting();
    }
);
