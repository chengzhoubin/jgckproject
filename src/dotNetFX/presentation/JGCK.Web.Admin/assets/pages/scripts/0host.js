//DisplayPosition   1:左边，2右上；3右下；4公告

var FileUploadUrl = 'ajax/UploadFile', mediaRecorder;
var timeInterval = 310000;//录音最大间隔时间ms
var timeMaxEnd = 300;//录音时间s
var data_upload = [];//文件上传数组
var ticker;
var loading = false;
var record_upload;
var handleNotific8 = function (Msg) {
    var settings = {
        theme: "teal",
        sticky: false,
        horizontalEdge: "top",
        verticalEdge: "right",
        life: 5000,
        heading: "通知"
    }
    $.notific8('zindex', 11500);
    $.notific8($.trim(Msg), settings);
}

//MsgType 1纯文字;2图文;3音频
//SendType 1公告；2评论信息3未回复，4，已回复
//UserType
//modalID 需要关闭的弹窗ID 需要加#或.   
//handleNotific8("信息发布成功!");
//handleSendAsync(MsgType, SendType, Msg, Msg_Ext, callback, modalID)
//actionId表示特殊操作动作 0：表示无要求 1：表示撤销
var handleSendAsync = function (MsgType, SendType, Msg, Msg_Ext, callback, modalID,sendTime) {
    var _data = {
        msg: {
            "ID": 0,
            "UserID": UserID,
            "IM_UserName": IM_UserName,
            "IM_UserHeaderUrl": IM_UserHeaderUrl,
            "CourseID": courseid,
            "UserType": UserType,
            "MsgType": MsgType || 1,
            "Msg": Msg || "",
            "Msg_Ext": Msg_Ext || "",
            "SendType": SendType || 2,
            "SendTime": sendTime
        }
    };
    $.ajax({
        complete: function () {
            loading = false;
        },
        beforeSend: function () {
            loading = true;
        },
        type: "POST",
        contentType: "application/json",
        url: "ClassLiveRoom/SendAsync",
        data: JSON.stringify(_data),
        dataType: "JSON",
        success: function (data) {
            if (data.Result) {
                ticker.server.sendSpecGroupMessage(data.Value,0);//0:wu,1:撤销，2：删除
                if (!!callback) callback();
            }
            else {
                alert(data.Err);
            }
            if (!!modalID) $(modalID).modal('hide');
        }
    });
}

//获取消息
var GetMsgAsync = function (msgId) {
    var url = "ClassLiveRoom/GetMsgAsync";
    var _data = { msgId: msgId }
    $.ajax({
        complete: function () { },
        beforeSend: function () { },
        type: "POST",
        contentType: "application/json",
        url: url,
        data: JSON.stringify(_data),
        dataType: "JSON",
        success: function (data) {
            if (data && _.indexOf(data.DisplayPosition, 1) >= 0) {
                if (data.IsHasDeleted) {
                    app.LeftItems = _.filter(app.LeftItems, function (item) { return item.ID!=data.ID; });
                }
                else {
                    app.LeftItems.push(data);
                    setTimeout(function () {
                        $("#scrollerLeft").slimScroll({ scrollTo: ($("#scrollerLeft").slimScroll()[0].scrollHeight - $("#scrollerLeft").slimScroll()[0].offsetHeight) + "px" });
                    }, 400);
                }    
            }
            if ($("#scroller2").length > 0) {
                if (data.IsHasDeleted) {
                    app.RightTopItems = _.filter(app.RightTopItems, function (item) { return item.ID != data.ID; });
                }
                else {
                    if (data && _.indexOf(data.DisplayPosition, 2) >= 0) {           
                        app.RightTopItems = _.map(app.RightTopItems, function (item) {
                            if (item.ID == data.ID) {
                                return data
                            }
                            else {
                               return item
                            }
                        });
                        if (!_.findWhere(app.RightTopItems, { ID: data.ID })) {
                            app.RightTopItems.push(data);
                        }
                        //app.RightTopItems=_.filter(app.RightTopItems, function (it) { return it.ID != data.ID })
                        //app.RightTopItems.push(data);
                        //setTimeout(function () {
                        //    $("#scroller2").slimScroll({ scrollTo: ($("#scroller2").slimScroll()[0].scrollHeight - $("#scroller2").slimScroll()[0].offsetHeight) + "px" });
                        //}, 400);
                    }
                }
            }
            if (data && data.SendType == 2) {
                app.RightBottomItems = _.filter(app.RightBottomItems, function (item) { return item.ID != data.ID; });
                app.RightBottomItems4 = _.filter(app.RightBottomItems4, function (item) { return item.ID != data.ID; });
            }
            if (data && _.indexOf(data.DisplayPosition, 3) >= 0) {
                if (data.IsHasDeleted) {
                    app.RightBottomItems = _.filter(app.RightBottomItems, function (item) { return item.ID != data.ID; });
                    app.RightBottomItems4 = _.filter(app.RightBottomItems4, function (item) { return item.ID != data.ID; });
                }
                else {
                    if (data.SendType == 3) {
                        app.RightBottomItems.push(data);
                        //setTimeout(function () {
                        //    $("#scroller3").slimScroll({ scrollTo: ($("#scroller3").slimScroll()[0].scrollHeight - $("#scroller3").slimScroll()[0].offsetHeight) + "px" });
                        //}, 400);
                    }
                    if (data.SendType == 4) {
                        app.RightBottomItems = _.filter(app.RightBottomItems, function (it) { return it.ID != data.toBeRepliedMsg.ID })
                        app.RightBottomItems4.push(data);
                    }
                }
            }
        }
    });
}

//转发给医生//和撤销
var ResendDoctor = function (index, item, position, callback) {
    var url = "ClassLiveRoom/ResendDoctorAsync";
    var _data = { msgId: item.ID }
    $.ajax({
        complete: function () { },
        beforeSend: function () { },
        type: "POST",
        contentType: "application/json",
        url: url,
        data: JSON.stringify(_data),
        dataType: "JSON",
        success: function (data) {
            if (data.Result) {              //item.SendType==2?"转给医生":"撤回问题"
                ticker.server.sendSpecGroupMessage(data.Value, item.SendType==3?1:0);
               // if (position == 2) app.RightTopItems.splice(index, 1);
                if (position == 3) app.RightBottomItems.splice(index, 1)
                if (!!callback) callback();
            }
        }
    });
}
//设置用户禁言
var SetUserNotAllowComment = function (msgId, callback) {
    var url = "ClassLiveRoom/SetUserNotAllowComment";
    var _data = { msgId: msgId }
    $.ajax({
        complete: function () { },
        beforeSend: function () { },
        type: "POST",
        contentType: "application/json",
        url: url,
        data: JSON.stringify(_data),
        dataType: "JSON",
        success: function (data) {
            if (data.Result) {
                ticker.server.sendSpecGroupMessage(data.Value,0);
                handleNotific8("设置用户禁言");
                if (!!callback) callback();
            }
        }
    });
}

var DeleteMessage = function (index, id, position, callback) {
    var _data = {msgId: id};
    $.ajax({
        complete: function() {},
        beforeSend: function () { },
        type: "POST",
        contentType: "application/json",
        url: "ClassLiveRoom/DeleteMessage",
            data: JSON.stringify(_data),
                dataType: "JSON",
                    success: function (data) {
                if (data.Result) {
                if(position==1) app.LeftItems.splice(index, 1);
                if(position==2) app.RightTopItems.splice(index, 1);
                if(position==3) app.RightBottomItems.splice(index, 1);
                if (position == 4) app.RightBottomItems4.splice(index, 1);     
                ticker.server.sendSpecGroupMessage(id, 2);
            }
                else {
                    handleNotific8(data.Err);
            }
        }
    });
}


var ReplyAsync = function (msgId, callback, MsgType, Msg, Msg_Ext) {
    var url = "ClassLiveRoom/ReplyAsync";
    var _data = {
        toReplyMsgId: msgId,
        replyMsg:{
            "ID": 0,
            "UserID": UserID,
            "IM_UserName": IM_UserName,
            "IM_UserHeaderUrl": IM_UserHeaderUrl,
            "CourseID": courseid,
            "UserType": UserType,
            "MsgType": MsgType || 1,
            "Msg": Msg || "...",
            "Msg_Ext": Msg_Ext || "",
            "SendType": 4
        }
    }
    $.ajax({
        complete: function () { },
        beforeSend: function () { },
        type: "POST",
        contentType: "application/json",
        url: url,
        data: JSON.stringify(_data),
        dataType: "JSON",
        success: function (data) {
            if (data.Result) {
                ticker.server.sendSpecGroupMessage(data.Value,0);
                handleNotific8("回复成功");
                if (!!callback) callback();
            }
        }
    });
}



//图片上传模块
$(function () {

    var CancelButton = $('<a />')
        .addClass('btn btndeleteup')
        .html('<i class="fa fa-trash"></i>');

    $(document).on("click",
        ".btndeleteup",
        function () {
             data_upload = [];
           // data_upload.splice($(this).closest(".fileItemwrap").index(), 1);
            $(this).closest(".fileItemwrap").remove();
            $(".fileinput-button").show();
        })

    var fileuploadModule = function (btnFileupload, spanFiles, callback) {
        $(btnFileupload).fileupload({
            url: FileUploadUrl,
            dataType: 'json',
            autoUpload: false,
            acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
            maxFileSize: 1999000,
            disableImageResize: /Android(?!.*Chrome)|Opera/.test(window.navigator.userAgent),
            previewMaxWidth: 90,
            previewMaxHeight: 90,
            previewCrop: true
        })
            .on('fileuploadadd',
                function (e, data) {
                    data.context = $('<li class="fileItemwrap"/>').appendTo($(spanFiles)); // fileinput - button
                    $.each(data.files,
                        function (index, file) {
                            var node = $('<div/>');
                            if (!index) {
                                data_upload.push(data);
                                node.append(CancelButton.clone(true));
                            }
                            node.appendTo(data.context);
                        });
                    $(".fileinput-button").hide();//.appendTo($(spanFiles));
                })
            .on('fileuploadprocessalways',
                function (e, data) {
                    var index = data.index,
                        file = data.files[index],
                        node = $(data.context.children()[index]);
                    if (file.preview) {
                        node.prepend(file.preview);
                    }
                    if (file.error) {
                        node.append($('<span class="text-danger"/>').text(file.error));
                    }
                })
            .on('fileuploadprogressall',
                function (e, data) {
                    var progress = parseInt(data.loaded / data.total * 100, 10);
                    $('#progress .progress-bar').css(
                        'width',
                        progress + '%'
                    );
                })
            .on('fileuploaddone',
                function (e, data) {
                    if (data.result.Result) {                   
                        if (callback) callback(data.result.Value);
                    }
                    else {
                        alert("图片上传失败！");
                    }
                })
            .on('fileuploadfail',
                function (e, data) {
                    $.each(data.files,
                        function (index) {
                            var error = $('<span class="text-danger"/>').text('File upload failed.');
                            $(data.context.children()[index])
                                .append('<br>')
                                .append(error);
                            data_upload.splice(index, 1);
                        });
                })
            .prop('disabled', !$.support.fileInput)
            .parent().addClass($.support.fileInput ? undefined : 'disabled');
    }

    fileuploadModule('#fileupload1', '#files1', function (ImgUrl) {
        if (loading) return;
        handleSendAsync(2, 2, $("#txtImgText").val(), ImgUrl, function () {}, "#model_SendImgText");
    });

    //测试
    //$(".startupload").click(function() {
    //    if (data_upload.length > 0) {
    //        $.each(data_upload,
    //            function(index, file) {
    //                console.log(index);
    //                file.submit();
    //            });
    //    }
    //    return false;
    //})
});

//录音模块
$(function (){
    var currentTime = 0, codeTime, oTimer;
    var _isRuny = 1;
 //   var recorder;

  //  function startRecording() {
    //    HZRecorder.get(function (rec) {
    //        console.log(rec)
    //        recorder = rec;
    //        recorder.start();
    //    });
    //}

    //function stopRecording() {
    //    recorder.stop();
    //}

    //function uploadAudio() {
    //    recorder.upload("Handler1.ashx", function (state, e) {
    //        switch (state) {
    //            case 'uploading':
    //                //var percentComplete = Math.round(e.loaded * 100 / e.total) + '%';
    //                break;
    //            case 'ok':
    //                //alert(e.target.responseText);
    //                alert("上传成功");
    //                break;
    //            case 'error':
    //                alert("上传失败");
    //                break;
    //            case 'cancel':
    //                alert("上传被取消");
    //                break;
    //        }
    //    });
    //}


    //录音模块
    //var audiosContainer = document.getElementById('audios-container');
    var mediaConstraints = {
        audio: true
    };

    function captureUserMedia(mediaConstraints, successCallback, errorCallback) {
        navigator.mediaDevices.getUserMedia(mediaConstraints).then(successCallback).catch(errorCallback);
    }



    function onMediaSuccess(stream) {
        //var audio = document.createElement('audio');
        //audio = mergeProps(audio, {
        //    controls: true,
        //    muted: true,
        //    src: URL.createObjectURL(stream)
        //});
        //audio.play();
        //audiosContainer.appendChild(audio);
        //audiosContainer.appendChild(document.createElement('hr'));

        mediaRecorder = new MediaStreamRecorder(stream);
        mediaRecorder.stream = stream;
        mediaRecorder.mimeType = 'audio/webm';
      //  mediaRecorder.recorderType = StereoAudioRecorder;
        mediaRecorder.audioChannels = 1;
        mediaRecorder.ondataavailable = function (blob) {
            //var a = document.createElement('a');
            //a.target = '_blank';
            //a.innerHTML = 'Open Recorded Audio No. ' + (index++) + ' (Size: ' + bytesToSize(blob.size) + ') Time Length: ' + getTimeLength(timeInterval);

            //a.href = URL.createObjectURL(blob);

            //audiosContainer.appendChild(a);
            //audiosContainer.appendChild(document.createElement('hr'));
            uploadToPHPServer(blob);
        };
        mediaRecorder.start(timeInterval);
        //  document.querySelector('#stop-recording').disabled = false;
    }

    function onMediaError(e) {
        handleNotific8("媒体发生错误："+e);
        $("#model_SendVoice").modal('hide');       
        console.error('media error', e);
        _isRuny = 1;
        clearTimeout(oTimer);
        $(".voice-timing").html("00:00");
        $("#recording .txt").html("开始录音");
        $("#span2-tip").hide();
        $("#span1-tip").show();
        $("#recording").removeClass("stop").addClass("start");
    }    

    function uploadToPHPServer(blob) {
        var file = new File([blob],
            'msr-' + (new Date).toISOString().replace(/:|\./g, '-') + '.webm',
            {
                type: 'video/webm'
            });
     
        var formData = new FormData();
        formData.append('video-filename', file.name);
        formData.append('video-blob', file);
        formData.append('courseid', courseid);
        formData.append('uploadType', "microclass");

        $.ajax({
            url: FileUploadUrl,
            type: "POST",
            data: formData,
            cache: !1,
            contentType: !1,
            processData: !1,
            success: function (data) { 
                if (!!$("#replayMsgId").val()) {
                    ReplyAsync($("#replayMsgId").val(), function () {
                       // $("#model_SendVoice").modal('hide');
                    }, 3, currentTime, data.Value);
                }
                else {
                    handleSendAsync(3, 2, currentTime, data.Value, function () { handleNotific8("信息发送成功!") });
                } 
            },
            error: function (a, b, c) {
            }
        })
    }





    $("#recording").on("click",
        function () {  
            if (_isRuny > 0) {
                 captureUserMedia(mediaConstraints, onMediaSuccess, onMediaError);
              //  startRecording();
                $("#recording .txt").html("停止并发送");
                $(this).removeClass("start").addClass("stop");
                $("#span1-tip").hide();
                $("#span2-tip").show();
                currentTime = 0;
                codeTime = timeMaxEnd;
                TimeStick();
            } else {
               // uploadToPHPServer(recorder.getBlob());
                mediaRecorder.stop();
                mediaRecorder.stream.stop();
                currentTime = timeMaxEnd - codeTime - 1;
                codeTime = timeMaxEnd;
                clearTimeout(oTimer);
                $(".voice-timing").html("00:00");
                $("#recording .txt").html("开始录音");
                $("#span2-tip").hide();
                $("#span1-tip").show();
                $(this).removeClass("stop").addClass("start");
            }
            _isRuny = -_isRuny;
        })

    function TimeStick() {
        codeTime >= 0
            ? ($(".voice-timing").html(setTime(timeMaxEnd - codeTime)), codeTime--, oTimer = setTimeout(TimeStick, 1000))
            : $("#recording").click()
    }
    function setTime(sec) {
        var date = new Date(0, 0)
        date.setSeconds(sec);
        var m = date.getMinutes(), s = date.getSeconds();
        return two_char(m) + ":" + two_char(s);
    }



    function two_char(n) {
        return n >= 10 ? n : "0" + n;
    }


    //function bytesToSize(bytes) {
    //    var k = 1000;
    //    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    //    if (bytes === 0) return '0 Bytes';
    //    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(k)), 10);
    //    return (bytes / Math.pow(k, i)).toPrecision(3) + ' ' + sizes[i];
    //}

    //function getTimeLength(milliseconds) {
    //    var data = new Date(milliseconds);
    //    return data.getUTCHours() +
    //        " hours, " +
    //        data.getUTCMinutes() +
    //        " minutes and " +
    //        data.getUTCSeconds() +
    //        " second(s)";
    //}
})


//socket 模块
$(function () {
    var heartTime, heartTime2=1;
    $.connection.hub.url = huburl;
    ticker = $.connection.microClassLivechat;
    //所有返回为json string
    ticker.client.onConnect = function (connected) {
        /*
        {
            "Body" : "" //ConnectionId
        }
        */
        console.log("onConnect", connected);
        var json = JSON.parse(connected);
    }

    ticker.client.receiveConnectMessage = function (data) {
        /*
        {
            "Body" : {
                UserCount :
                ClientConnectionId:
                SignupId:
            }
        }
        */
        console.log("receiveConnectMessage", data);
        var json = JSON.parse(data);
        //if (json.Body.UserCount) {
        //     $("#b-listener").text(json.Body.UserCount);
        //}
    }

    ticker.client.receiveHeartbeat = function (data) {
        /*
        {
            "Body" : "" //Server Time
        }
        */
        var json = JSON.parse(data);
        heartTime = json.Body;
        console.log("receiveHeartbeat", data);

    }

    ticker.client.receiveMessage = function (data) {
        /*
        {
            "Body" : {
                "msgid" : 1232
            }
        }
        */
        console.log("receiveMessage", data);
        var json = JSON.parse(data);
        GetMsgAsync(json.Body.msgid);
    }

    // Start the connection
    $.connection.hub.start({
        jsonp: true
    }).then(function () {
        console.log(" $.connection.hub.start", UserID, UserType, courseid);
        ticker.server.connect(UserID, UserType, courseid);
    });      
    setInterval(function () {
        if (heartTime2 != heartTime) {
            heartTime2 = heartTime;
        }
        else {
            window.location.reload();
            console.log("NetWork Not Running");
        }
        ticker.server.sendHeartbeat();
    }, 600000);

});


//各个局部显示
var dragdiv = function (type, box, lefttop, rightbottom, line) {
    var oBox = $(box)[0], oTop = $(lefttop)[0], oBottom = $(rightbottom)[0], oLine = $(line)[0];
    if (type == "leftright") {
        oLine.onmousedown = function (e) {
            var disX = (e || event).clientX;
            oLine.left = oLine.offsetLeft;
            document.onmousemove = function (e) {
                var iT = oLine.left + ((e || event).clientX - disX);
                // var e = e || window.event, tarnameb = e.target || e.srcElement;
                var maxT = oBox.clientWight - oLine.offsetWidth;
                oLine.style.margin = 0;
                iT < 0 && (iT = 0);
                iT > maxT && (iT = maxT);
                oLine.style.left = oTop.style.width = iT + "px";
                oBottom.style.width = oBox.clientWidth - iT -1 + "px";
                return false
            };
            document.onmouseup = function () {
                document.onmousemove = null;
                document.onmouseup = null;
                oLine.releaseCapture && oLine.releaseCapture()
            };
            oLine.setCapture && oLine.setCapture();
            return false
        };
    }

    if (type == "topbottom") {
        oLine.onmousedown = function (e) {
            var disY = (e || event).clientY;
            oLine.top = oLine.offsetTop;
            $(lefttop).css("flex", "none");
            $(rightbottom).css("flex", "none");
            document.onmousemove = function (e) {
                var iT = oLine.top + ((e || event).clientY - disY);
                var maxT = oBox.clientHeight - oLine.offsetHeight;
                oLine.style.margin = 0;
                iT < 0 && (iT = 0);
                iT > maxT && (iT = maxT);
                oTop.style.height = oLine.style.top = iT + "px";
                oBottom.style.height = oBox.clientHeight - iT + "px";
                return false
            };
            document.onmouseup = function () {
                document.onmousemove = null;
                document.onmouseup = null;
                oLine.releaseCapture && oLine.releaseCapture()
            };
            oLine.setCapture && oLine.setCapture();
            return false
        };

    }

}

$(window).resize(function () {
    setTimeout(function () {
        $(".page-content").css("height", $(".page-content").css("min-height"));
    },
   100);
});


//页面初始化和绑定
var pageHandle = function () {
    var url = "ClassLiveRoom/GetPositionLiveMessageList";
    var initLeft = function () {
       $("#scrollerLeft").css("height", $("#left .portlet-body").height()+"px")
       App.initSlimScroll("#scrollerLeft");
       var _data1 = { courseId: courseid, positionId: 1 }
        $.ajax({
            complete: function () { },
            beforeSend: function () { },
            type: "POST",
            contentType: "application/json",
            url: url,
            data: JSON.stringify(_data1),
            dataType: "JSON",
            success: function (data) {
                console.log(data);
                if (data.Result) {
                    roomData.LeftItems = data.Value;
                    setTimeout(function () {                      
                        $("#scrollerLeft").slimScroll({ scrollTo: ($("#scrollerLeft").slimScroll()[0].scrollHeight - $("#scrollerLeft").slimScroll()[0].offsetHeight) + "px" });
                    }, 400);
                }
                else {
                    alert(data.Err);
                }
            }
        });
    }

    var initRightTop = function () {
        var _data2 = { courseId: courseid, positionId: 2 }
        $("#scroller2").css("height", $("#top2 .portlet-body").height() + "px");
        App.initSlimScroll("#scroller2")
        $.ajax({
            complete: function () { },
            beforeSend: function () { },
            type: "POST",
            contentType: "application/json",
            url: url,
            data: JSON.stringify(_data2),
            dataType: "JSON",
            success: function (data) {
                console.log(data);
                if (data.Result) {
                    roomData.RightTopItems = data.Value;
                    setTimeout(function () {
                        $("#scroller2").slimScroll({ scrollTo: ($("#scroller2").slimScroll()[0].scrollHeight - $("#scroller2").slimScroll()[0].offsetHeight) + "px" });
                    }, 400);
                }
                else {
                    alert(data.Err);
                }
            }
        });
    }

    var initRightBottom = function () {
        var _data3 = { courseId: courseid, positionId: 3 }
        $("#scroller3").css("height", $("#bottom2 .portlet-body").height() + "px");
        App.initSlimScroll("#scroller3");
        App.initSlimScroll("#scroller4");
        $.ajax({
            complete: function () { },
            beforeSend: function () { },
            type: "POST",
            contentType: "application/json",
            url: url,
            data: JSON.stringify(_data3),
            dataType: "JSON",
            success: function (data) {
                console.log(data);
                if (data.Result) {
                    roomData.RightBottomItems = _.where(data.Value, { SendType: 3 });
                    roomData.RightBottomItems4 = _.where(data.Value, { SendType: 4 });
                    setTimeout(function () {
                        $("#scroller3").slimScroll({ scrollTo: ($("#scroller3").slimScroll()[0].scrollHeight - $("#scroller3").slimScroll()[0].offsetHeight) + "px" });
                    }, 400);                   
                }
                else {
                    alert(data.Err);
                }
            }
        });
    }

    
    //发布图文信息
    var doSendImgText = function () {     
        if (data_upload.length > 0) {
            $.each(data_upload,
                function (index, file) {
                    file.submit();
                });
        }
        else {
            if (!$("#txtImgText").val()) {
                alert("请说点什么");
                return
            }
            if (loading) return;
            handleSendAsync(1, 2, $("#txtImgText").val(), "", function () {}, "#model_SendImgText");
        }
        return;
    }

    return {
        init: function () {
            initLeft();
            handleDatetimePicker();
            if ($("#scroller2").length>0) initRightTop();
            initRightBottom();     
            $("#btnSendNotice").click(function () {
                if (loading) return;
                handleSendAsync(1, 1, $("#txtNoticeMsg").val(), "", function () { handleNotific8("公告发布成功!") }, "#model_Notice")
           });
            $("#btnSendComMessage").click(function () {
                if (loading) return;
                handleSendAsync(1, 2, $("#txtSendMsgS").val(), "", function () {}, "#model_SendMessage")
            });
            $("#btnSendImgText").click(doSendImgText);
            dragdiv("leftright", "#box", "#left", "#right", "#line");
            if ($("#top2").length>0) dragdiv("topbottom", "#right", "#top2", "#bottom2", "#line2");

            $(".page-content").css("height", $(".page-content").css("min-height"));
            $(document).on("click",
                ".nav-tabs",
                function () {
                    $(".page-content").css("height", $(".page-content").css("min-height"));
                })

            $("#model_SendMessage").on("click",
                ".dline-usefullmsg",
                 function () {
                    $("#txtSendMsgS").val($(this).text());
                    $(".dline-usefullmsg").removeClass("border");
                    $(this).addClass("border");
                    $("#txtSendMsgSid").val($(this).data("id"));
                })
                //.on("dblclick",//双击修改 勿删
                //".dline-usefullmsg",
                //function () {
                //    $("#hideEditMsg").val($(this).data("id"));
                //    $("#txtEditMsg").val($(this).text());
                //    $("#model_SendEditMessage").modal();
                //})

            $("#btn-deleteMsgS").on("click",
                function () {
                    if (!$("#txtSendMsgSid").val()) {
                        alert("请选择需要删除的消息");
                    } else {
                        alert("delete-" + $("#txtSendMsgSid").val() + "-" + $("#txtSendMsgS").val());
                    }
                })

            $("#btn-showAddMsgs").on("click",
                function () {
                    $("#hideEditMsg").val("");
                    $("#txtEditMsg").val("");
                    $("#model_SendEditMessage").modal();
                })

            $("#modelShowImgText").on("click",
                function () {
                    $("#txtImgText").val("");
                    $(".fileinput-button").show().siblings().remove();
                    data_upload = [];
                    $("#title-SendImgText").text("文字&图片");
                    $("#model_SendImgText").modal();
                })

            $("#modelSendVoice").on("click",
             function () {
                 $("#title-SendVoice").text("语音");
                 $("#replayMsgId").val("");
                 $("#model_SendVoice").modal();
                 record_upload = null;
                 $("#txtTimeLength").val("");
                 $("#recordFileName").html("");       
             });

            $('#fileupload').fileupload({
                url: FileUploadUrl,
                dataType: 'json',
                autoUpload: false,
                maxFileSize: 1999000,
                done: function (e, data) {
                    $.each(data.result, function (index, file) {             
                    });
                }
            }).on('fileuploadadd',
                function (e, data) {
                    record_upload = data;
                    $("#recordFileName").html("文件名： "+data.files[0].name);
                })
            .on('fileuploaddone',
                function (e, data) {
                    if (data.result.Result) {
                        record_upload = null;           
                       // handleSendAsync(3, 2, $("#txtTimeLength").val(), data.result.Value, function () { handleNotific8("信息发送成功!") }, "#model_SendVoice");
                        if (!!$("#replayMsgId").val()) {
                            ReplyAsync($("#replayMsgId").val(), function () {
                             //   $("#model_SendVoice").modal('hide');
                            }, 3, $("#txtTimeLength").val(), data.result.Value);
                        }
                        else {
                            handleSendAsync(3, 2, $("#txtTimeLength").val(), data.result.Value, function () { handleNotific8("信息发送成功!") }, "", $("#txt_StartTime").val());
                        }
                        $("#txtTimeLength").val("");
                        $("#recordFileName").html("");
                    }
                    else {
                        alert("上传失败！");
                    }
                })
            .on('fileuploadfail',
                function (e, data) {
                    alert("上传失败！");
                })
            $(".startupload_record").click(function () {
                if (!$("#txtTimeLength").val()) {
                    alert("请输入时长");
                    return;
                }
                if (!!record_upload) record_upload.submit();
            });

            $("#tab-rb").on("click", "a", function () {
                if ($(this).data("tabid") == 4) {
                  setTimeout(function () {
                      $("#scroller4").slimScroll({ scrollTo: ($("#scroller4").slimScroll()[0].scrollHeight - $("#scroller4").slimScroll()[0].offsetHeight) + "px" });
                   }, 400);
                }
                if ($(this).data("tabid") == 3) {
                    setTimeout(function () {
                        $("#scroller3").slimScroll({ scrollTo: ($("#scroller3").slimScroll()[0].scrollHeight - $("#scroller3").slimScroll()[0].offsetHeight) + "px" });
                    }, 400);
                }
            })

            if (courseStatus == "0") {
                $("#btnStart").val("开始直播");
            }
            if (courseStatus == "1") {
                $("#btnStart").val("停止直播");
            }
            if (courseStatus == "2") {
                $("#btnStart").val("直播已结束");
            }
            $("#btnStart").click(function () {
                if (courseStatus == 0) {
                    bootbox.confirm("确定要开始直播吗?", function (result) {
                        if (result) {
                           doClassState(1)
                        }
                    });
                   }
                if (courseStatus == 1) {
                    bootbox.confirm("确定要停止直播吗?", function (result) {
                        if (result) {
                          doClassState(2)
                        }
                    });
                }
            })
            function doClassState(_op) {
                var _data = { classId: courseid, op: _op };
                var url = "ClassLiveRoom/StartOrEnd";
                $.ajax({
                    complete: function () { },
                    beforeSend: function () { },
                    type: "POST",
                    contentType: "application/json",
                    url: url,
                    data: JSON.stringify(_data),
                    dataType: "JSON",
                    success: function (data) {
                        console.log(data);
                        if (data.Result) {
                            window.location.reload();
                        }
                        else {
                            alert(data.Err);
                        }
                    }
                })
            }

        },
        initLeft:initLeft,
        initRightTop: initRightTop,
        initRightBottom: initRightBottom,
    }
}()



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







//function playAudio(audioUrl,time_length,id){
//    if($("#audioplay").attr("src")!=audioUrl){
//        $("#audioplay").attr("src",audioUrl);
//        clearInterval(timerAudio)
//        timer_n=0;
//    }
//    if ($("#audioplay")[0].paused)
//    {
//        timerAudio = setInterval(function(){ myTimer(time_length,id) }, 100);
//        $("#audioplay")[0].play();
//        $("#item"+id +" .badge").remove();
//        if(_.findIndex(arrAudioPlayedIds, id))
//        {
//            arrAudioPlayedIds.push(id);
//        }
//    }
//    else
//    {
//        clearInterval(timerAudio)
//        $("#audioplay")[0].pause();
//    }
//}