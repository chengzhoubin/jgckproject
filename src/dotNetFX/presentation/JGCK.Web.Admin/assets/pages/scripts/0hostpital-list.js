var PageFuns = function () {
    var initTable = function () {
        //////////////////////////////////////////////////
        var table = $('#table_list');
        var fixedHeaderOffset = 0;
        if (App.getViewPort().width < App.getResponsiveBreakpoint('md')) {
            if ($('.page-header').hasClass('page-header-fixed-mobile')) {
                fixedHeaderOffset = $('.page-header').outerHeight(true);
            }
        } else if ($('.page-header').hasClass('navbar-fixed-top')) {
            fixedHeaderOffset = $('.page-header').outerHeight(true);
        }

        var oTable = table.dataTable({
            "paging": false,
            "ordering": false,
            fixedHeader: {
                header: true,
                headerOffset: fixedHeaderOffset
            },
        });
        table.on('click', '.delete', function (e) {
            e.preventDefault();

            if (confirm("确实要删除该行吗?") == false) {
                return;
            }
            var _data = {
                ID: $(this).data("val"),
                toDelete:true
            }
            $.ajax({
                complete: function () { },
                beforeSend: function () { },
                type: "POST",
                url: "res/data/hospital/update",
                data: _data,
                dataType: "json",
                success: function (data) {
                    if (data.Result) {
                        window.location.reload();
                    }
                    else {
                        alert(data.Err);
                    }
                }
            })
            //var nRow = $(this).parents('tr')[0];
            //oTable.fnDeleteRow(nRow);

        });

        table.on('click', '.edit', function (e) {
            e.preventDefault();
            var data = $(this).data("val");
            $("#hospitalID").val(data.ID);
            $("#txt_hostpitalName").val(data.HospitalName);
            $("#txt_hostpitalAliasName").val(data.AliasName);
            $("#txt_address").val(data.Address);
            $("select#ddl_hospitalRanks option[value='" + data.Rank + "']").prop('selected', 'true');
            $("select#ddl_province option[value='" + data.SelectedProvinceID + "']").prop('selected', 'true');
            getChildCitys(data.SelectedProvinceID, "#ddl_city", function () { $("select#ddl_city option[value='" + data.SelectedCityID + "']").prop('selected', 'true'); });
            getChildCitys(data.SelectedCityID, "#ddl_area", function () { $("select#ddl_area option[value='" + data.SelectedAreaID + "']").prop('selected', 'true'); });
            $('#modal-Edit').modal('show');
        });
    }

    var initDDLcitys = function (ddl1, ddl2, ddl3) {
        $(ddl1).change(function () {
            if ($(ddl1).val() === "0") {
                $(ddl2)[0].selectedIndex = 0;
                $(ddl3)[0].selectedIndex = 0;
                $(ddl2).hide();
                $(ddl3).hide();
                return;
            }
            $.ajax({
                complete: function () { },
                beforeSend: function () { },
                type: "GET",
                url: "ajax/getchildareas",
                data: { areaId: $(this).val() },
                dataType: "json",
                success: function (data) {
                    $(ddl2).show().empty();
                    $(ddl3)[0].selectedIndex = 0;
                    $(ddl3).hide().empty();
                    if (data && data.length > 0) {
                        $(ddl2).append('<option value="0">请选择市</option>');
                        $(data).each(function (i, item) {
                            $(ddl2).append('<option value="' + item.id + '">' + item.name + '</option>');
                        })
                    }
                    else {
                        $(ddl2).append('<option value="0">无</option>');
                    }
                }
            })
        })

        $(ddl2).change(function () {
            if ($(ddl2).val() === "0") {
                $(ddl3)[0].selectedIndex = 0;
                $(ddl3).hide();
                return;
            }

            $.ajax({
                complete: function () { },
                beforeSend: function () { },
                type: "GET",
                url: "ajax/getchildareas",
                data: { areaId: $(this).val() },
                dataType: "json",
                success: function (data) {
                    $(ddl3).show().empty();
                    if (data && data.length > 0) {
                        $(data).each(function (i, item) {
                            $(ddl3).append('<option value="' + item.id + '">' + item.name + '</option>');
                        })
                    }
                    else {
                        $(ddl3).append('<option value="">无</option>');
                    }
                }
            })
        })
    }

    var getChildCitys = function (pid,toDDLid,callback) {
        $.ajax({
            complete: function () { },
            beforeSend: function () { },
            type: "GET",
            url: "ajax/getchildareas",
            data: { areaId: pid },
            dataType: "json",
            success: function (data) {
                $(toDDLid).show().empty();
                if (data && data.length > 0) {
                    $(toDDLid).append('<option value="0">请选择</option>');
                    $(data).each(function (i, item) {
                        $(toDDLid).append('<option value="' + item.id + '">' + item.name + '</option>');
                    })
                    callback();
                }
                else {
                    $(toDDLid).append('<option value="0">无</option>');
                }
            }
        })
    }

    return {
        init: function () {
            if (!jQuery().dataTable) {
                return;
            }
            initTable();

            if ($("#search_province").val() === "0") {
                $("#search_city").hide();
                $("#search_area").hide();
            }
            if ($("#search_city").val() === "0") {
                $("#search_area").hide();
            }

            initDDLcitys("#search_province", "#search_city", "#search_area");
            initDDLcitys("#ddl_province", "#ddl_city", "#ddl_area");


            $("#btnAdd").click(function () {
                $("#txt_hostpitalName").val("");
                $("#txt_hostpitalAliasName").val("");
                $("#txt_address").val("");
                $("#ddl_hospitalRanks")[0].selectedIndex = 0;
                $("#ddl_province")[0].selectedIndex = 0;
                $("#ddl_city")[0].selectedIndex = 0;
                $("#ddl_area")[0].selectedIndex = 0;
                $("#ddl_city").hide();
                $("#ddl_area").hide();
                $('#modal-Edit').modal('show');
                $("#hospitalID").val("");
            })

            
            $("#btnSave").click(function () {
                if (!$("#txt_hostpitalName").val()) {
                    alert("请输入医院名称!"); return;
                }
                if (!$("#txt_hostpitalAliasName").val()) {
                    alert("请输入医院简称!"); return;
                }
                if (!$("#txt_address").val()) {
                    alert("请输入医院地址!"); return;
                }
                if (!$("#ddl_hospitalRanks").val()||$("#ddl_hospitalRanks").val()==0) {
                    alert("请选择医院等级!"); return;
                }
                if (!$("#ddl_area").val() || $("#ddl_area").val() == 0) {
                    alert("请选择城市地区!"); return;
                }

                var _data = {
                    ID:$("#hospitalID").val(),
                    HospitalName: $("#txt_hostpitalName").val(),
                    AliasName: $("#txt_hostpitalAliasName").val(),
                    Address: $("#txt_address").val(),
                    Rank: $("#ddl_hospitalRanks").val(),
                    SelectedAreaID: $("#ddl_area").val(),
                }
                var url = "res/data/hospital/add";
                if (!!_data.ID) {
                    url = "res/data/hospital/update";
                }
                $.ajax({
                    complete: function () { },
                    beforeSend: function () { },
                    type: "POST",
                    url: url,
                    data: _data,
                    dataType: "json",
                    success: function (data) {
                         if (data.Result) {
                            window.location.reload();
                        }
                        else {
                            alert(data.Err);
                        }
                    }
                })            
            })
        }
    };
}();



jQuery(document).ready(function () {
    PageFuns.init();
});



//public long ID { get; set; }        
//public string HospitalName { get; set; }          
//public string Address { get; set; }
//public int Rank { get; set; }
//public long? SelectedAreaID { get; set; }