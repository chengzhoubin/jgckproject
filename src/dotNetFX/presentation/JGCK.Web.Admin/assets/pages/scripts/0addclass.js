'use strict';

var Components = function () {

    var handleDatetimePicker = function () {

        if (!jQuery().datetimepicker) {
            return;
        }
        var date = new Date();
        $(".form_datetime").datetimepicker({
            autoclose: true,
            isRTL: App.isRTL(),
            format: " yyyy-mm-dd hh:ii",
            startDate: '2016-1-1',
            todayHighlight: true,
            language: "zh-CN",
            pickerPosition: (App.isRTL() ? "bottom-right" : "bottom-left")
        });
        $('body').removeClass("modal-open");
    }

    return {

        init: function () {
            handleDatetimePicker();
            var _n = $(".div_doctorGroup").length;
            $("#btn_addDoctor").click(function () {
                _n++;
                var htmlTemp = `<div class ="form-group div_doctorGroup" >
                                    <label class ="col-md-3 control-label"></label>
                                    <div class = "col-md-4" >
                                        <div class ="input-group">
                                           <input name="SpeakDoctors.Index" type="hidden" value= "`+_n+`" >
                                           <select class ="form-control ddl_doctor" name= "SpeakDoctors[`+_n+`].ID" >
                                           </select>
                                            <span class="input-group-btn">
                                                <button class ="btn red delete_doctor" type="button">删除</button>
                                            </span>
                                        </div>
                                    </div>
                                </div> `;
                $("#div_addDoctor").before(htmlTemp);             
                $(".div_doctorGroup:last .ddl_doctor").append($(".ddl_doctor").eq(0).html()).find('option:first').prop('selected', 'true');               
            });
            $(document).on("click", ".delete_doctor",(function () {
                $(this).closest(".div_doctorGroup").remove();
            }))
            $(document).on("change", ".ddl_doctor", function () {
                var _this=$(this);
                $(".ddl_doctor").each(function (i,item) {
                    console.log(i, $(_this).val(), $(item).val(), $(_this).attr("id"), $(item).attr("id"));
                    if (!!$(_this).val() && ($(_this).attr("id") != $(item).attr("id"))) {
                        if ($(_this).val() == $(item).val()) {
                            alert("该医生已经选择过了");
                            $(_this).find('option:first').prop('selected', 'true');
                            $.uniform.update();
                        }
                    }
                })
            })
        }
    };

}();


jQuery(document).ready(function () {
    Components.init();
});



$(function () {
    var _FilesOfGroup1Index = $("#files1 > div").length;

    var url = 'ajax/UploadFile', 
      uploadButton = $('<button type="button"/>')
          .addClass('btn btn-primary btnUpload')
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
          }),
          deleteButton = $('<span />')
          .addClass('btn red delete-Imgs')
          .text('删除')
          .on('click', function (n) {
              var $this = $(this),data = $this.data();
              $this.parent().parent().remove();             
          }),
        hiddenBtn=$("<input type='hidden' />");

    var fileuploadModule = function (btnFileupload, spanFiles, isMultiple, callback, width, height) {
        var _width = 150;
        var _height = 150;
        if (width) _width = width;
        if (height) _height = height;
        $(btnFileupload).fileupload({
            url: url,
            dataType: 'json',
            autoUpload: false,
            acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
            maxFileSize: 1999000,
            disableImageResize: /Android(?!.*Chrome)|Opera/.test(window.navigator.userAgent),
            previewMaxWidth: _width,
            previewMaxHeight: _height,
            previewCrop: true
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
               node.append($('<span class="text-danger"/>').text(file.error));
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
              /// var link = $('<a>')
                //   .attr('target', '_blank')
               //    .prop('href', data.result.Value);
              // $(data.context.children()).wrap(link);
               if (callback) callback(data);

           } else if (data.result.Err) {
               var error = $('<span class="text-danger"/>').text(data.result.Err);
               $(data.context.children()[index])
                   .append('<br>')
                   .append(error);
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
    }

    fileuploadModule('#fileupload1', '#files1', true, function (data) {
         _FilesOfGroup1Index++;
         $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_1__Index").attr("name", "FilesOfGroup[1].Index").val(_FilesOfGroup1Index).appendTo(data.context);
         $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_1__" + _FilesOfGroup1Index + "__FileUrl").attr("name", "FilesOfGroup[1][" + _FilesOfGroup1Index + "].FileUrl").val(data.result.Value).appendTo(data.context);
         $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_1__" + _FilesOfGroup1Index + "__FileType").attr("name", "FilesOfGroup[1][" + _FilesOfGroup1Index + "].FileType").val(1).appendTo(data.context);
         $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_1__" + _FilesOfGroup1Index + "__IsNull").attr("name", "FilesOfGroup[1][" + _FilesOfGroup1Index + "].IsNull").val("False").appendTo(data.context);

        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_1__Key").attr("name", "FilesOfGroup[1].Key").val(1).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_1__Value_Index").attr("name", "FilesOfGroup[1].Value.Index").val(_FilesOfGroup1Index).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_1__Value_" + _FilesOfGroup1Index + "__FileUrl").attr("name", "FilesOfGroup[1].Value[" + _FilesOfGroup1Index+"].FileUrl").val(data.result.Value).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_1__Value_" + _FilesOfGroup1Index + "__FileType").attr("name", "FilesOfGroup[1].Value[" + _FilesOfGroup1Index+"].FileType").val(1).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_1__Value_" + _FilesOfGroup1Index + "__IsNull").attr("name", "FilesOfGroup[1].Value[" + _FilesOfGroup1Index+"].IsNull").val("False").appendTo(data.context);

    },375,160);
    fileuploadModule('#fileupload2', '#files2', false, function (data) {
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_2__Index").attr("name", "FilesOfGroup[2].Index").val(0).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_2__0__FileUrl").attr("name", "FilesOfGroup[2][0].FileUrl").val(data.result.Value).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_2__0__FileType").attr("name", "FilesOfGroup[2][0].FileType").val(2).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_2__0__IsNull").attr("name", "FilesOfGroup[2][0].IsNull").val("False").appendTo(data.context);

        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_2__Key").attr("name", "FilesOfGroup[2].Key").val(2).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_2__Value_Index").attr("name", "FilesOfGroup[2].Value.Index").val(0).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_2__Value_0__FileUrl").attr("name", "FilesOfGroup[2].Value[0].FileUrl").val(data.result.Value).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_2__Value_0__FileType").attr("name", "FilesOfGroup[2].Value[0].FileType").val(2).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_2__Value_0__IsNull").attr("name", "FilesOfGroup[2].Value[0].IsNull").val("False").appendTo(data.context);

    }, 150, 104);
    fileuploadModule('#fileupload3', '#files3', false, function (data) {
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_3__Index").attr("name", "FilesOfGroup[3].Key").val(0).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_3__0__FileUrl").attr("name", "FilesOfGroup[3][0].FileUrl").val(data.result.Value).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_3__0__FileType").attr("name", "FilesOfGroup[3][0].FileType").val(3).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_3__0__IsNull").attr("name", "FilesOfGroup[3][0].IsNull").val("False").appendTo(data.context);

        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_3__Key").attr("name", "FilesOfGroup[3].Index").val(3).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_3__Value_Index").attr("name", "FilesOfGroup[3].Value.Index").val(0).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_3__Value_0__FileUrl").attr("name", "FilesOfGroup[3].Value[0].FileUrl").val(data.result.Value).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_3__Value_0__FileType").attr("name", "FilesOfGroup[3].Value[0].FileType").val(3).appendTo(data.context);
        $(hiddenBtn).clone(true).attr("id", "FilesOfGroup_3__Value_0__IsNull").attr("name", "FilesOfGroup[3].Value[0].IsNull").val("False").appendTo(data.context);

    }, 100, 100);


    $(".files").on("click", ".delete-Imgs", function () {
        $(this).parent().parent().remove();
    })
});


function checkSubmint() {
    if ($(".btnUpload").length > 0) {
        alert("你有图片没上传");
        return false;
    }
    else {
        return true;
    }    
}