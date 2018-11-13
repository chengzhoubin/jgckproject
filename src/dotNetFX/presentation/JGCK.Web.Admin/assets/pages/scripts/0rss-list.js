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
            if (confirm("确实要删除该图片吗?") == false) {
                return;
            }
            $.ajax({
                complete: function () { },
                beforeSend: function () { },
                type: "POST",
                url: "Ops/DelteSubjectPicAsync",
                data: { imageId: $(this).data("val") },
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

        table.on('click', '.img', function (e) {
            e.preventDefault();       
            $("#modifyID").val($(this).data("id"));
            $('#modal-edit').modal('show');
        });

        table.on('click', '.visible', function (e) {
            e.preventDefault();        
            $.ajax({
                complete: function () { },
                beforeSend: function () { },
                type: "POST",
                url: "Ops/ShowOrHiddenSubject",
                data: { subjectId: $(this).data("id") },
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
        var TxtAuto = new Bloodhound({
            datumTokenizer: Bloodhound.tokenizers.obj.whitespace('Name'),
            queryTokenizer: Bloodhound.tokenizers.whitespace,
            remote: {
                url: '/res/data/disease/list?Name=%QUERY',  //'%QUERY' 将被用户输入的值代替  
                filter: function (resp) { //服务端未必直接以json array方式返回搜索结果。如果不是的话，指定一下搜索结果在json中的路径。  
                    return resp.List;
                },
                wildcard: '%QUERY',
            }

        });

        TxtAuto.initialize();

        $('#search_name').typeahead({ //把这个输入框变成auto complete风格  
            hint: true,
            highlight: true,
            minLength: 1
        },
        {
            name: 'theTxtAuto',
            displayKey: 'Name', //选择好结果后，输入框里显示的字段  
            source: TxtAuto.ttAdapter(),
            templates: {
                suggestion: Handlebars.compile('<p>ID:{{Id}} - name:{{Name}}</p>')
            }
        }).on('typeahead:selected', function (evt, datum) {
            console.log(datum);
            $('#search_hide_name').val(datum.Id); //
        });



    }
    return {
        init: function () {
            if (!jQuery().dataTable) {
                return;
            }
            init();
        }
    };
}();

jQuery(document).ready(function () {
    pageFun.init();
    $(document).on("click", "#btnSaveRssImage", (function () {
        if (!$("#modifyID").val()) return;
        //if (!$("#Image_RedirectUrl").val()) {
        //    alert("请填写图片对应的链接");
        //    return;
        //}
        if ($("#a_headLogo").length == 0 || !($("#a_headLogo").attr("href"))) {
            alert("请上传图片");
            return;
        }
        var _data = {
            subjectID: $("#modifyID").val(),
            img: {
                Image_RedirectUrl: $("#Image_RedirectUrl").val(),
                ImageUrl: $("#a_headLogo").attr("href")
            }
        }
        $.ajax({
            complete: function () { },
            beforeSend: function () { },
            type: "POST",
            url: "Ops/AddSubjectPicAsync",
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
    }));

    //粘结版
    var clipboard = new Clipboard('.copy');
    clipboard.on('success', function (e) {
        //console.info('Action:', e.action);
        //console.info('Text:', e.text);
        //console.info('Trigger:', e.trigger);
        e.clearSelection();
        alert("复制链接成功");
    });
    clipboard.on('error', function (e) {
        console.error('Action:', e.action);
        console.error('Trigger:', e.trigger);
    });

});



$(function () {
    'use strict';
    var url = 'ajax/UploadFile',
      uploadButton = $('<button type="button" style="margin: 0 15px;"/>')
          .addClass('btn btn-primary')
          .prop('disabled', true)
          .text('进行中...')
          .on('click', function () {
              var $this = $(this),
                  data = $this.data();
              $this
                 .off('click')
                 .text('取消')
                 .on('click', function () {
                     $this.remove();
                     data.abort();
                 });
              data.submit().always(function () {
                  $this.remove();
              });
          });
    $('#fileupload').fileupload({
        url: url,
        dataType: 'json',
        autoUpload: false,
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
        maxFileSize: 1999000,
        disableImageResize: /Android(?!.*Chrome)|Opera/.test(window.navigator.userAgent),
        previewMaxWidth: 375,
        previewMaxHeight: 160,
        previewCrop: true
    }).on('fileuploadadd', function (e, data) {
        $('#files').empty();
        data.context = $('#files');
        $.each(data.files, function (index, file) {
            var node = $('<span />');
            if (!index) {
                node.append(uploadButton.clone(true).data(data));
            }
            node.appendTo(data.context);
       
        });
    }).on('fileuploadprocessalways', function (e, data) {
        var index = data.index,
            file = data.files[index],
            node = $(data.context.children()[index]);
        if (file.preview) {
            node.prepend(file.preview);
            $("#img_headLogo").hide();
        }
        if (file.error) {
            node.append($('<span class="text-danger"/>').text(file.error));
        }
        if (index + 1 === data.files.length) {
            data.context.find('button')
                .text('上传')
                .prop('disabled', !!data.files.error);
        }
    }).on('fileuploadprogressall', function (e, data) {
        var progress = parseInt(data.loaded / data.total * 100, 10);
        $('#progress .progress-bar').css(
            'width',
            progress + '%'
        );
    }).on('fileuploaddone', function (e, data) {
        //$.each(data.result.files, function (index, file) {
        //    if (file.url) {
        //        var link = $('<a>')
        //            .attr('target', '_blank')
        //            .prop('href', file.url);
        //        $(data.context.children()[index])
        //            .wrap(link);
        //    } else if (file.error) {
        //        var error = $('<span class="text-danger"/>').text(file.error);
        //        $(data.context.children()[index])
        //            .append('<br>')
        //            .append(error);
        //    }
        //});
        if (data.result.Value) {
            var link = $('<a id="a_headLogo">')
                .attr('target', '_blank')
                .prop('href', data.result.Value);
            $(data.context.children()).wrap(link);
            $("#hid_headLogo").val(data.result.Value);
        } else if (data.result.Err) {
            var error = $('<span class="text-danger"/>').text(data.result.Err);
            $(data.context.children()[index])
                .append('<br>')
                .append(error);
        }
    }).on('fileuploadfail', function (e, data) {
        $.each(data.files, function (index) {
            var error = $('<span class="text-danger"/>').text('File upload failed.');
            $(data.context.children()[index])
                .append('<br>')
                .append(error);
        });
    }).prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled');

    $(".files").on("click", ".delete", function () {
        $(this).parent().remove();
    })
});

