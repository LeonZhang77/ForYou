﻿$(function () {    
    $("#dialog").dialog({
        autoOpen: false,
        width: 400,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "explode",
            duration: 1000
        }
    });
});

function open_upload_dialog() {
    var uploader = WebUploader.create({

        // swf文件路径
        swf: '/lib/webuploader/Uploader.swf',

        // 文件接收服务端。
        server: '/Assert/X86Server_Upload',

        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#picker',

        // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
        resize: false,
        //线程数
        threads: 1,
        //上传文件数量限制
        fileNumLimit: 1,
    });

    // 当有文件被添加进队列的时候
    uploader.on('fileQueued', function (file) {
        //alert(123);
        $("#thelist").append('<div id="' + file.id + '" class="item">' +
            '<h4 class="info">' + file.name + '</h4>' +
            '<p class="state">等待上传...</p>' +
            '</div>');
    });

    uploader.on('uploadSuccess', function (file, response) {
        $('#' + file.id).find('p.state').text('已上传');
        $("#dialog").dialog("close");
        //alert(response._raw)
        location.href='/Assert/X86Server_Verify_List' + '?' + response._raw;
    });

    uploader.on('uploadError', function (file) {
        $('#' + file.id).find('p.state').text('上传出错');

    });

    uploader.on('uploadComplete', function (file) {
        $('#' + file.id).find('.progress').fadeOut();
    });

    $("#ctlBtn").on('click', function () {
        if ($(this).hasClass('disabled')) {
            return false;
        }
        uploader.upload();
        $("#dialog").dialog("close");

    });
    
    $("#dialog").dialog("open");
}

function redirect(){
        
    location.href='/Assert/X86Server_Verify_List';
}
