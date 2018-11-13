var pageFun = function () {
    var init = function () {
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
                url: "res/data/disease/update",
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
          //  var nRow = $(this).parents('tr')[0];
          //  oTable.fnDeleteRow(nRow);
        });

        table.on('click', '.edit', function (e) {
            e.preventDefault();
            var data = $(this).data("val");
            $("#txt_ClassName").val(data.ClassName);
            $("#modifyID").val(data.ID);
            $('#modal-edit').modal('show');
        });

    }
    return {
        init: function () {
            if (!jQuery().dataTable) {
                return;
            }
            init();
            $("#btnAdd").click(function () {
                $("#txt_ClassName").val("");
                $("#modifyID").val("");
                $('#modal-edit').modal('show');
            })

            $("#btnSave").click(function () {  
                if (!$("#txt_ClassName").val()) {
                    alert("请输入病种名称!"); return;
                }       
                  var  _data = {
                        ID: $("#modifyID").val(),
                        ClassName: $("#txt_ClassName").val(),
                    }      
               var url = "res/data/disease/add";
                if (!!_data.ID) {
                    url = "res/data/disease/update";
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