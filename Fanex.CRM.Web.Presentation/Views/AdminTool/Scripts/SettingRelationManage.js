"use strict";
/*global gup, head */
/* exported rowover, rowout, ShowPopupAddSettingRelation */
//Contain the template
var tmplSetting = null, settingRelation;

function rowover(obj) {
    $(".tbllayout .overrow").removeClass("overrow");
    $(obj).addClass("overrow");
}
function rowout() {
    $(".tbllayout .overrow").removeClass("overrow");
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
            settingRelation.loadSettingRelation();
        },
        error: function () { try { parent.jQuery.unblockUI(); } catch (e) { } },
        complete: function () {}
    });
}

function ShowPopupAddSettingRelation(parentId, settingValue, settingName) {
    var title = 'Setting Relation Management';
    var url = '../AdminTool/AddSettingRelation?parentId=' + parentId +
        '&settingValue=' + encodeURI(settingValue) +
        '&settingName=' + encodeURI(settingName);
    var width = 585;
    var height = 470;
    parent.OpenPopup(url, title, width, height, true, false, window, 2000, function () { settingRelation.loadSettingRelation(); });
}

settingRelation = {
    type: '',
    gridHeight: 70,
    loadSettingRelation: function () {

        try { parent.BlockUI(); } catch (e) { }
        var mainreport = $("#settingrelationgrid");
        settingRelation.type = $('#ddlMainSetting').val();
        var data = '{"type":' + '"' + settingRelation.type + '"}';

        $('#gridtitle').html('Setting Relation by ' + settingRelation.type);

        $.ajax({
            type: "POST",
            url: window.rootUrl + "AdminTool/GetSettingByType",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: data,
            success: function (rs) {
                if (rs != null && rs.length > 0) {
                    var arr = [];
                    var arrLeft = [];
                    var css = 'bgodd';
                    for (var i = 0; i < rs.length; i++) {
                        css = (i % 2) > 0 ? "bgodd" : "bgeven";
                        arr.push(settingRelation.renderParentRow(rs[i], css));
                        arrLeft.push(settingRelation.renderParentLeftRow(rs[i], css));
                        if (rs[i].Child != null && rs[i].Child.length > 0) {
                            for (var j = 0; j < rs[i].Child.length; j++) {
                                css = (j % 2) > 0 ? "childBgOdd" : "childBgEven";
                                arr.push(settingRelation.renderChildRow(rs[i].Child[j], css, rs[i].Id));
                                arrLeft.push(settingRelation.renderChildLeftRow(rs[i].Child[j], css, rs[i].Id));
                            }
                        }
                    }

                    $("#settingrelationbody tbody").empty().append(arr.join(''));
                    $("#settingrelationbg tbody").empty().append(arrLeft.join(''));

                    var gridOpts = {
                        tblContentId: 'settingrelationbody',
                        manualCopyHeader: true,
                        fullWidth: true,
                        fullHeight: true,
                        gridHeight: settingRelation.gridHeight,
                        freezeWidth: 400,
                        numberFreezeCol: 1
                    };
                    mainreport.removeClass("hidden").gridScroller(gridOpts);
                }
            },
            complete: function () {
                try { parent.jQuery.unblockUI(); } catch (e) { }
            }
        });
    },
    renderParentRow: function (row, css) {
        var expanTd = '';
        if (row.Child != null && row.Child.length > 0) {
            expanTd = '<td>\
                          <div style="width: 400px; height: 17px;">\
                              <span class="tree-expanded" onclick="settingRelation.toggleSettingRelation(this,\'' + row.Id + '\');"></span>' +
                              row.Value +
                          '</div>\
                       </td>';
        }
        else {
            expanTd = '<td><div style="width: 400px; height: 17px;">&nbsp;' + row.Value + '</div></td>';
        }
        var html = '<tr class="' + css + '">\
                        ' + expanTd + '\
                        <td>\
                            <div class="c" style="width: 60px; height: 17px;">\
                                <a class="hand addSetting" href="JavaScript:void(0);" onclick="ShowPopupAddSettingRelation(' + row.Id + ', \'' + row.Value + '\', \'' + row.Type + '\');">\
                                    </a>\
                            </div>\
                        </td>\
                        <td><div class="r" style="width: 80px; height: 17px;">' + row.Id + '&nbsp;</div></td>\
                        <td><div class="r" style="width: 60px; height: 17px;">' + row.DisplayOrder + '&nbsp;</div></td>\
                    </tr>';

        return html;
    },
    renderParentLeftRow: function (row, css) {
        var expanTd = '';
        if (row.Child != null && row.Child.length > 0) {
            expanTd = '<td>\
                            <div style="width: 400px; height: 17px;">\
                                <span class="tree-expanded" onclick="settingRelation.toggleSettingRelation(this,\'' + row.Id + '\');"></span>' +
                                row.Value +
                            '</div>\
                        </td>';
        }
        else {
            expanTd = '<td><div style="width: 400px; height: 17px;">&nbsp;' + row.Value + '</div></td>';
        }
        var html = '<tr class="' + css + '">\
                        ' + expanTd + '\
                    </tr>';

        return html;
    },
    renderChildRow: function (row, css, parentId) {
        var html = '<tr class="setting_' + parentId + ' ' + css + '">\
                        <td style="border-right:0px none;">\
                            <div style="height: 17px; width:400px">\
                                <span class="indent">&nbsp;</span>' +
                                row.Value + ' ' +
                                row.Type +
                            '</div>\
                        </td>\
                        <td><div class="r" style="width: 60px; height: 17px;">&nbsp;</div></td>\
                        <td><div class="r" style="width: 80px; height: 17px;">' + row.Id + '&nbsp;</div></td>\
                        <td><div class="r" style="width: 60px; height: 17px;">' + row.DisplayOrder + '&nbsp;</div></td>\
                    </tr>';

        return html;
    },
    renderChildLeftRow: function (row, css, parentId) {
        var html = '<tr class="setting_' + parentId + ' ' + css + '">\
                        <td style="border-right:0px none;">\
                            <div style="height: 17px; width:400px">\
                                <span class="indent">&nbsp;</span>' +
                                row.Value + ' ' +
                                row.Type +
                            '</div>\
                        </td>\
                    </tr>';

        return html;
    },
    toggleSettingRelation: function (obj, id) {
        if ($(obj).hasClass("tree-expand")) {
            $(obj).removeClass("tree-expand");

            $("#settingrelationbody tbody tr.setting_" + id).removeClass("hidden");
            $("#settingrelationbg tbody tr.setting_" + id).removeClass("hidden");
        }
        else {
            $(obj).addClass("tree-expand");

            $("#settingrelationbody tbody tr.setting_" + id).addClass("hidden");
            $("#settingrelationbg tbody tr.setting_" + id).addClass("hidden");
        }

        var gridOpts = {
            tblContentId: 'settingrelationbody',
            manualCopyHeader: true,
            fullWidth: true,
            fullHeight: true,
            gridHeight: settingRelation.gridHeight,
            freezeWidth: 400,
            numberFreezeCol: 1
        };
        $("#settingrelationgrid").gridScroller(gridOpts);
    }
};

head.ready(
    function () {
        tmplSetting = $("#tplSetting").template();
        var settingValue = decodeURI(gup('settingValue'));
        $('#spParent').html(settingValue);

        var gridOpts = {
            tblContentId: 'settingrelationbody',
            isShowPager: false
        };
        $("#settingrelationgrid").initGrid(gridOpts);

        loadMainSetting();

        $('body').mousedown(function (e) {
            if (e.button === 1) {
                return false;
            }
        });
    }
);
