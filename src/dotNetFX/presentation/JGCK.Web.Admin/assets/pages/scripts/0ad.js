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
            console.log($(this).data("value"));

            var _data = {
                ad: { Ad_ID: $(this).data("val") },
                toDelete: true
            }
            $.ajax({
                complete: function () { },
                beforeSend: function () { },
                type: "POST",
                url: "ad/update",
                data: _data,
                dataType: "json",
                success: function (data) {
                    if (data.Value) {
                        window.location.reload();
                    }
                    else {
                        alert(data.Err);
                    }
                }
            })

        });



        table.on('click', '.edit', function (e) {
            e.preventDefault();
            var data = $(this).data("val");
            $("#AddOrUpdateViewObject_Ad_ID").val(data.Ad_ID);
            $("#hid_headLogo").val(data.Ad_Pic);
            $("#img_headLogo").attr("src", data.Ad_Pic);
            $("#hid_headLogo").val(data.Ad_Pic);
            $("#txtAdTitle").val(data.Title);
            $("#txt_adContent").val(data.Content);
            $("#txt_adJumpUrl").val(data.JumpUrl);
            $("#txt_adSort").val(data.Sort);
            data.PositionId == 1 ? ($("#rdPositionId1").prop("checked", true)) : ($("#rdPositionId2").prop("checked", true));
            $.uniform.update();
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

            $("#btn_submit").click(function () {
                if (!$("#txtAdTitle").val()) {
                    alert("请输入广告标题!"); return;
                }
                if (!$("#txt_adJumpUrl").val()) {
                    alert("请输入跳转链接!"); return;
                }          
                if (!$("#hid_headLogo").val()) {
                    alert("请先上传图片!"); return;
                }
                if ($(".upfiles").length>0) {
                    alert("请先上传图片!"); return;
                }
                var _data = {
                    Ad_ID: $("#AddOrUpdateViewObject_Ad_ID").val(),
                    Ad_Pic: $("#hid_headLogo").val(),
                    Title: $("#txtAdTitle").val(),         
                    Content: $("#txt_adContent").val(),//
                    JumpUrl: $("#txt_adJumpUrl").val(),//
                    Sort: $("#txt_adSort").val(),//
                    PositionId: $("#rdPositionId1").prop("checked")?1:2,
                }
                var url = "ad/add";
                if (!!_data.Ad_ID) {
                    url = "ad/update";
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



$(function () {
    'use strict';
    var url = 'ajax/UploadFile',
      uploadButton = $('<button type="button"/>')
          .addClass('btn btn-primary upfiles')
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
        previewMaxHeight: 155,
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
});