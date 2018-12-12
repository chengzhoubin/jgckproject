'use strict';
var url = '/ajaxcommon/UploadFile',
    uploadButton = $('<button type="button"/>')
        .addClass('btn btn-primary btnUpload')
        .prop('disabled', true)
        .text('进行中...')
        .on('click', function () {
            var $this = $(this),data = $this.data();
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
        }),
    deleteButton = $('<span />')
        .addClass('btn red delete-Imgs')
        .text('删除')
        .on('click', function (n) {
            var $this = $(this), data = $this.data();
            $this.parent().parent().remove();
        }),
    hiddenBtn = $("<input type='hidden' />");

var fileuploadModule = function (btnFileupload, spanFiles, isMultiple, callback, uploadtype, width, height, errHandler) {
    var _width = 150;
    var _height = 150;
    var url0;
    if (width) _width = width;
    if (height) _height = height;
    if (uploadtype) url0 = url + "?uploadtype=" + escape(uploadtype)
    $(btnFileupload).fileupload({
        url: url0,
        dataType: 'json',
        autoUpload: false,
        acceptFileTypes: /(\.|\/)(gif|jpe?g|png|zip|rar)$/i,
        maxFileSize: 10000000,
        disableImageResize: /Android(?!.*Chrome)|Opera/.test(window.navigator.userAgent),
        previewMaxWidth: _width,
        previewMaxHeight: _height
        // previewCrop: true,
    })
        .on('fileuploadadd', function (e, data) {
            if (isMultiple) {
                data.context = $('<div/>').appendTo($(spanFiles));
                $.each(data.files, function (index, file) {
                    var node = $('<p class="clearfix" />').append($('<span/>'));
                    if (!index) {
                        node.append('<br>')
                            .append(uploadButton.clone(true).data(data))
                            .append(deleteButton.clone(true).data(data));
                    }
                    node.appendTo(data.context);
                });
            }
            else {
                $(spanFiles).empty();
                data.context = $(spanFiles);
                $.each(data.files, function (index, file) {
                    var node = $('<span />');
                    if (!index) {
                        node.append(uploadButton.clone(true).data(data));
                    }
                    node.appendTo(data.context);
                });
            }
        })
        .on('fileuploadprocessalways', function (e, data) {
            var index = data.index,
                file = data.files[index],
                node = $(data.context.children()[index]);
            if (file.preview) {
                node.prepend(file.preview);
            }
            if (file.error) {
                if (typeof (errHandler) === "function") {
                    $(".btnUpload").hide();
                    errHandler(file.error);
                }
                else {
                    node.append($('<span class="text-danger"/>').text(file.error));
                }
            }
            if (index + 1 === data.files.length) {
                data.context.find('button')
                    .text('上传')
                    .prop('disabled', !!data.files.error);
            }
        })
        .on('fileuploadprogressall', function (e, data) {
            var progress = parseInt(data.loaded / data.total * 100, 10);
            $('#progress .progress-bar').css(
                'width',
                progress + '%'
            );
        })
        .on('fileuploaddone', function (e, data) {
            if (data.result.Value) {
                if (callback) callback(data);

            } else if (data.result.Err) {
                if (typeof (errHandler) === "function") {
                    errHandler(data.result.Err);
                }
                else {
                    var error = $('<span class="text-danger"/>').text(data.result.Err);
                    $(data.context.children()[index])
                        .append('<br>')
                        .append(error);
                }
            }
        })
        .on('fileuploadfail', function (e, data) {
            $.each(data.files, function (index) {
                var error = $('<span class="text-danger"/>').text('File upload failed.');
                $(data.context.children()[index])
                    .append('<br>')
                    .append(error);
            });
        })
        .prop('disabled', !$.support.fileInput)
        .parent().addClass($.support.fileInput ? undefined : 'disabled')
};


$(".files").on("click", ".delete-Imgs", function () {
    $(this).parent().parent().remove();
});

