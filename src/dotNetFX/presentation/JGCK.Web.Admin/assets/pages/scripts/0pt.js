var pageFun = function () {

    //移除已选择项
    function removeSelected(opt, removeId) {
        $(opt).remove();
        var selectRkey = $("#selectedRecipients").val();
        var arr = selectRkey.split("@@");
        var arr2 = $(arr).map(function (ind, val) {
            if (parseInt(val) == parseInt(removeId)) {
            } else {
                return val;
            }
        }).get().join("@@");
        $("#selectedRecipients").val(arr2);
    }
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
                url: "res/data/pt/update",
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
            $("#txt_Name").val(data.TitleName);
            $("#modifyID").val(data.ID);
            $('#modal-edit').modal('show');         
        });      

  
        var remote = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('TitleName'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: '/res/data/pt/asynclist?filter=%QUERY',
                wildcard: '%QUERY'
            }
        });
        remote.initialize();
        $('#search_name').typeahead({
            hint: true,
            highlight: true,
            minLength: 1
        },{
            name: 'names',
            displayKey: 'TitleName',
            source: remote
        })
        //    .on('typeahead:selected', function (e, datum) {
        //    console.log(e, datum);  
        //});



    }
    return {
        init: function () {
            if (!jQuery().dataTable) {
                return;
            }
            init();
            $("#btnAdd").click(function () {
                $("#txt_Name").val("");
                $("#modifyID").val("");
                $('#modal-edit').modal('show');
            })
            $("#btnSave").click(function () {  
                if (!$("#txt_Name").val()) {
                    alert("请输入职称名称!"); return;
                }           
                var  _data = {
                        ID: $("#modifyID").val(),
                        TitleName: $("#txt_Name").val(),
                    }         
                var url = "res/data/pt/add";
                if (!!_data.ID) {
                    url = "res/data/pt/update";
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

