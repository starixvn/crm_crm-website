"use strict";
/*global showAlertMessage, head */
/* exported rowover, rowout, ShowPopupAddSetting, ShowPopupEditSetting, DeleteSettingByById */

//Contain the template
var tmplSetting = null;

function rowover(obj) {
    $(".tbllayout .overrow").removeClass("overrow");
    $(obj).addClass("overrow");
}
function rowout() {
    $(".tbllayout .overrow").removeClass("overrow");
}

var settingManagement;

function loadMainSetting() {

    try { parent.BlockUI(); } catch (e) { }
    $.ajax({
        type: "POST",
        url: window.rootUrl + "AdminTool/GetAllSetting",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: '{"settingName":""}',
        success: function (rs) {

            $("#ddlMainSetting").children().remove().end();
            $("#tplMainSetting").tmpl(rs).appendTo("#ddlMainSetting");
            $('#btnInsertSubSetting').val('Insert Settintg value for ' + $('#ddlMainSetting').val());
            settingManagement.loadSettingManagement();
        },
        error: function () { },
        complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
    });
}

settingManagement = {
    type: '',
    loadSettingManagement: function () {

        try { parent.BlockUI(); } catch (e) { }
        var mainreport = $("#settingManagementgrid");
        settingManagement.type = $('#ddlMainSetting').val();
        var data = { "type": settingManagement.type };

        $('#btnInsertSubSetting').val('Insert Setting value for ' + settingManagement.type);
        $('#gridtitle').html('Setting Management by ' + settingManagement.type);
        $.ajax({
            type: "POST",
            url: window.rootUrl + "AdminTool/GetSettingByTypeNoChild", //GetAllSetting
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify(data),
            success: function (rs) {
                if (rs != null && rs.length > 0) {
                    var arr = [];
                    var arrLeft = [];
                    var css = 'bgodd';
                    for (var i = 0; i < rs.length; i++) {
                        css = (i % 2) > 0 ? "bgodd" : "bgeven";
                        arr.push(settingManagement.renderSettingManagementRow(rs[i], css));
                        arrLeft.push(settingManagement.renderSettingManagementLeftRow(rs[i], css));
                    }

                    $("#settingManagementbody tbody").empty().append(arr.join(''));
                    $("#settingManagementheader tbody").empty().append(arrLeft.join(''));

                    var gridOpts = {
                        tblContentId: 'settingManagementbody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: 70,
                        freezeWidth: 400,
                        numberFreezeCol: 1
                    };
                    mainreport.removeClass("hidden").gridScroller(gridOpts);
                }
                else {
                    $("#settingManagement").addClass("hidden");
                    loadMainSetting();
                }
            },
            complete: function () {


                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderSettingManagementRow: function (row, css) {
        var html = '<tr class="' + css + '">\
                        <td><div style="width: 400px; height:17px;">' + row.Value + '</div></td>\
                        <td><div class=" r" style="width: 80px; height:17px;">' + row.Id + '</div></td>\
                        <td><div class=" r" style="width: 60px; height:17px;">' + row.DisplayOrder + '</div></td>\
                        <td>\
                            <div class=" c" style="width: 60px; height:17px;">\
                                <a class="c hand editSetting" href="JavaScript:void(0);" onclick="ShowPopupEditSetting(' + row.Id + ');">\
                                    </a>\
                            </div>\
                        </td>\
                        <td>\
                            <div class=" c" style="width: 60px; height:17px;">\
                                <a class="c hand deleteSetting" href="JavaScript:void(0);" onclick="DeleteSettingByById(' + row.Id +
                                    ',\'' + row.Type +
                                    ' ' + row.Value + '\');">\
                                    </a>\
                            </div>\
                        </td>\
                    </tr>';

        return html;
    },
    renderSettingManagementLeftRow: function (row, css) {
        var html = '<tr class="' + css + '">\
                        <td><div style="width: 400px; height:17px;">' + row.Value + '</div></td>\
                    </tr>';
        return html;
    }
};

function ShowPopupAddSetting(type) {
    var Value = '';
    if (type === 1) {
        Value = encodeURI($('#ddlMainSetting').val());
    }

    var title = 'Insert New Setting';
    var url = '../AdminTool/SettingIns?Value=' + Value;
    var width = 480;
    var height = 230;
    parent.OpenPopup(url, title, width, height, true, false, window, 2000, function () { loadMainSetting(); });
}

function ShowPopupEditSetting(Id) {
    var title = 'Edit Setting Value';
    var url = '../AdminTool/SettingEdit?Id=' + Id;
    var width = 480;
    var height = 200;
    parent.OpenPopup(url, title, width, height, true, false, window, 2000, function () { settingManagement.loadSettingManagement(); });
}

function DeleteSettingByById(Id, Value) {

    $("#divPopup").html('Are you sure you want to delete Setting ' + Value + '?');
    $("#divPopup").dialog({ width: 380, height: 120, zIndex: 500, resizable: false, modal: true,
        position: [200, 200],
        close: function (event, ui) {
            /*jshint unused: vars */
        },
        buttons: [
                    {
                        text: "Ok",
                        click: function () {
                            try { parent.BlockUI(); } catch (e) { }
                            $.ajax({
                                type: "POST",
                                url: window.rootUrl + "AdminTool/DeleteSettingById",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                data: '{"id":' + Id + ',"userId":' + $('#userId').val() + '}',
                                success: function (rs) {

                                    if (parseInt(rs.ErrCode) > 0) {
                                        showAlertMessage("errMsg", rs.ErrMsg, 1);
                                    }
                                    else {
                                        setTimeout(function () {
                                            settingManagement.loadSettingManagement();
                                        }, 1000);
                                    }
                                },
                                error: function () { },
                                complete: function () { try { parent.jQuery.unblockUI(); } catch (e) { } }
                            });
                            $(this).dialog("close");
                        }
                    },
                    {
                        text: "Cancel",
                        click: function () {
                            $(this).dialog("close");
                        }
                    }
                ]
    });


}

head.ready(
    function () {
        tmplSetting = $("#tplSetting").template();

        var gridOpts = {
            tblContentId: 'settingManagementbody',
            isShowPager: false
        };
        $("#settingManagementgrid").initGrid(gridOpts);

        loadMainSetting();

        $('body').mousedown(function (e) {
            if (e.button === 1) {
                return false;
            }
        });
    }
);
