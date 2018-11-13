var PageFun = function () {
    var htmlTemp = '<div class="form-group div_workingSectionId">' +
                                 '<label class="col-md-3 control-label"></label>' +
                                ' <div class="col-md-6">' +
                                     '<div class="input-group">' +
                                         '<input type="text" class="form-control txt_doctorHospital"  placeholder="其他任职医院" name="txt_doctorHospital">' +
                                        ' <span class="input-group-btn">' +
                                            ' <button class="btn red deleteDoctorHospital" type="button">删除</button>' +
                                         '</span>' +
                                     '</div>' +
                                ' </div>' +
                             ' </div>';

    var initTable1 = function () {
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
                toDelete: true
            }
            $.ajax({
                complete: function () { },
                beforeSend: function () { },
                type: "POST",
                url: "res/data/doctor/update",
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
   
        table.on('click', '.edit', function (e) {
            e.preventDefault();
            var data = $(this).data("val");   
            $("#modifyID").val(data.ID);
            $("#txt_doctorName").val(data.DoctorName);
            $("#img_headLogo").attr("src", data.HeadPicUrl);
            $("#hid_headLogo").val(data.HeadPicUrl);
            data.Gender == 1 ? ($("#sexman").prop("checked", true)) : ($("#sexwoman").prop("checked", true));
            $("#txt_mobile").val(data.Phone);
            $("select#ddl_jobTitle option[value='" + data.JobTitleId + "']").prop('selected', 'true');
            $("#txt_jobTitleEx").val(data.JobTitleEx);
            $("#txt_doctorIntroduceDesc").val(data.DoctorIntroduceDesc);
            $('#modal-edit').modal('show');
            $(".checkboxgroup").prop("checked", false)
            if (data.WorkingSectionIds) {
                $(data.WorkingSectionIds).each(function (i,item) {
                    $("#chb_section" + item.SID).prop("checked",true);
                })
            }
            $(".div_workingSectionId").remove();
            $("#txt_workingHospital").val("");
            $("#txt_workingHospital").data("val", "");
            if (data.WorkingHospitals) {
                $(data.WorkingHospitals).each(function (i, item) {
                    if (i == 0) {
                        $("#txt_workingHospital").val(item.Name);
                        $("#txt_workingHospital").data("val", item.HID);
                    }
                    else {
                        $("#div_addDoctorHospital").before(htmlTemp);
                        $(".div_workingSectionId:last .txt_doctorHospital").val(item.Name).data("val", item.HID);
                        do_typeahead($(".div_workingSectionId:last .txt_doctorHospital"));
                    }

                })
            }

            $.uniform.update();
        });
    }
    var remote = new Bloodhound({
        datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            url: 'res/data/hospital/asynclist?f=%QUERY',
            wildcard: '%QUERY'
        }
    });

    var do_typeahead = function (toObjectID) {
        $(toObjectID).typeahead({
            hint: true,
            highlight: true,
            empty:true,
            minLength: 1
        }, {
            name: 'names',
            displayKey: 'name',
            source: remote
        })
        .on('typeahead:selected', function (e, datum) {
            $(this).data("val", datum.id);
        })
        .on('typeahead:asyncrequest ', function (e, datum) {
             $(this).data("val", "");
        })
         .on('typeahead:close', function (e) {
             if (!$(this).data("val")) {
                 $(this).data("val", "");
                 $(this).val("");
             }
         })
    }

    var initEdit = function () {
        $("#modifyID").val("");
        $("#txt_doctorName").val("");
        $("#files").empty();//
        $("#img_headLogo").show();
        $("#sexman").prop("checked", true);
        $(".checkboxgroup").prop("checked", false)
        $("#txt_mobile").val("");
        $("#txt_workingHospital").val("");
        $("#txt_workingHospital").data("val","");
        $(".div_workingSectionId").remove();
        $("#ddl_jobTitle")[0].selectedIndex = 0;
        $("#txt_jobTitleEx").val("");
        $("#txt_doctorIntroduceDesc").val("");
        $.uniform.update();
    }

    var initDDLcitys = function (ddl1, ddl2) {
        $(ddl1).change(function () {
            if ($(ddl1).val() === "0") {
                $(ddl2)[0].selectedIndex = 0;
                $(ddl2).hide();
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
        });
        if ($(ddl1).val() === "0") {
            $(ddl2)[0].selectedIndex = 0;
            $(ddl2).hide();
        }

    }


    return {
        init: function () {
            if (!jQuery().dataTable) {
                return;
            }
            initTable1();
            remote.initialize();
            do_typeahead('#txt_workingHospital');
            initDDLcitys("#search_province", "#search_city");

            $("#btnAdd").click(function () {//新增按钮
                initEdit();
                $('#modal-edit').modal('show');
            })


            $("#modal-edit").on("click", ".deleteDoctorHospital", function () {
                $(this).closest(".form-group").remove();
            })


            $("#btn_submit").click(function () {
                if (!$("#txt_doctorName").val()) {
                    alert("请输入医生姓名!"); return;
                }
                if (!$("#txt_mobile").val()) {
                    alert("请输入手机号!"); return;
                }


              var  _WorkingHospitals = [];
              if (!!$('#txt_workingHospital').val()) {
                  var _d = { HID: $('#txt_workingHospital').data("val"), Name: $('#txt_workingHospital').val() }
                  _WorkingHospitals.push(_d);
              }
              else {
                  alert("请选择任职医院!"); return;
              }
              $(".txt_doctorHospital").each(function (i, item) {
                  if ($(item).data("val")) {
                      var _d = { HID: $(item).data("val"), Name: $(item).val() }
                      _WorkingHospitals.push(_d);
                  }
              });

              var _WorkingSectionIds = [];
              $("#div_sectionList input[type='checkbox']").each(function (i, item) {
                  console.log(i, item);
                  if ($(item).prop("checked")) {
                      var _d = { SID: $(item).val(), Name: $(item).data("val") }
                      _WorkingSectionIds.push(_d);
                  }
              })        
              if (_WorkingSectionIds.length == 0) {
                  alert("请选择科室!"); return;
              }

                var _data = {
                    ID: $("#modifyID").val(),
                    DoctorName: $("#txt_doctorName").val(),
                    HeadPicUrl:  $("#hid_headLogo").val(),
                    Gender: $("#sexman").prop("checked") ? 1 : 0,
                    Phone: $("#txt_mobile").val(),
                    JobTitleId: $("#ddl_jobTitle").val(),
                    JobTitleEx: $("#txt_jobTitleEx").val(),
                    WorkingHospitals:_WorkingHospitals,
                    WorkingSectionIds:_WorkingSectionIds,
                    DoctorIntroduceDesc: $("#txt_doctorIntroduceDesc").val()
                }
                var url = "res/data/doctor/add";
                if (!!_data.ID) {
                    url = "res/data/doctor/update";
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

            $("#btn_addDoctorHospital").click(function () { 
                $("#div_addDoctorHospital").before(htmlTemp);               
                do_typeahead($(".div_workingSectionId:last .txt_doctorHospital"));
            });
        }
    }
}();




jQuery(document).ready(function () {
    PageFun.init();
});





$(function () {
    'use strict';
    var url = 'ajax/UploadFile',
      uploadButton = $('<button type="button"/>')
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
        previewMaxWidth: 100,
        previewMaxHeight: 100,
        previewCrop: true
    }).on('fileuploadadd', function (e, data) {
        $('#files').empty();
        data.context = $('#files');
        $.each(data.files, function (index, file) {
            var node = $('<span/>');
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