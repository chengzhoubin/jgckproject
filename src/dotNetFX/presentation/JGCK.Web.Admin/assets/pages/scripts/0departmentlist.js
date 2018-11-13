var pageFun = function () {
    var init = function () {

        ////////////////////////////////////////////////////////////////
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
                toDelete: true
            }
            $.ajax({
                complete: function () { },
                beforeSend: function () { },
                type: "POST",
                url: "res/data/department/update",
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

        });
        //ID: 13, DepartmentName: "二级1", ParentID: 0, ParentDepartmentName: "内科", IsFirst: false
        table.on('click', '.edit', function (e) {
            e.preventDefault();
            console.log($(this));
            var data = $(this).data("val");
            console.log(data);
            if (_class === 1) {
                $("#txt_Name").val(data.ParentDepartmentName);
            }
            else {                
                $("select#search_ddlClass option[value='" + data.ParentID + "']").prop('selected', 'true');
                $("#txt_Name").val(data.DepartmentName);
            }
            $("#modifyID").val(data.ID);
            $('#modal-edit').modal('show');
        });

        ////////////////////////////////////////////////////////////
        //var TxtAuto = new Bloodhound({
        //    datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Name'),
        //    queryTokenizer: Bloodhound.tokenizers.whitespace,
        //    remote: {
        //        url: '/res/data/disease/list?Name=%QUERY',  //'%QUERY' 将被用户输入的值代替  
        //        filter: function (resp) { //服务端未必直接以json array方式返回搜索结果。如果不是的话，指定一下搜索结果在json中的路径。  
        //            return resp.List;
        //        },
        //        wildcard: '%QUERY',
        //    }

        //});

        //TxtAuto.initialize();

        //$('#search_name').typeahead({ //把这个输入框变成auto complete风格  
        //    hint: true,
        //    highlight: true,
        //    minLength: 1
        //},
        //{
        //    name: 'theTxtAuto',
        //    displayKey: 'Name', //选择好结果后，输入框里显示的字段  
        //    source: TxtAuto.ttAdapter(),
        //    templates: {
        //        suggestion: Handlebars.compile('<p>ID:{{Id}} - name:{{Name}}</p>')
        //    }
        //}).on('typeahead:selected', function (evt, datum) {
        //    console.log(datum);
        //    $('#search_hide_name').val(datum.Id); //
        //});
       //////////////////////////////////////////



    }
    return {
        init: function () {
            if (!jQuery().dataTable) {
                return;
            }

            init();

            $("#btnAdd").click(function () {

                if (_class === 2) { $("#search_ddlClass")[0].selectedIndex = 0; }
                $("#txt_Name").val("");
                $("#modifyID").val("");
                $('#modal-edit').modal('show');
            })


            $("#btnSave").click(function () {
                var _data = null;               
                if (_class === 1) {
                    if (!$("#txt_Name").val()) {
                        alert("请输入一级科室名称!"); return;
                    }
                    _data = {
                        ID: $("#modifyID").val(),
                        DepartmentName: $("#txt_Name").val(),
                        ParentDepartmentName: "",
                        ParentID: "",
                        IsFirst: true,
                    }
                }
                else {
                    if ($("#search_ddlClass").val()==0) {
                        alert("请选择一级科室名称!"); return;
                    }
                    if (!$("#txt_Name").val()) {
                        alert("请输入二级科室名称!"); return;
                    }
                    _data = {
                        ID: $("#modifyID").val(),
                        DepartmentName: $("#txt_Name").val(),
                        ParentDepartmentName: "",
                        ParentID: $("#search_ddlClass").val(),
                        IsFirst: false,
                    }
                }             
                var url = "res/data/department/add";
                if (!!_data.ID) {
                    url = "res/data/department/update";
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
    pageFun.init();
});