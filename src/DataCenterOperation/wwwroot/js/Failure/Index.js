var text_id = "";

$(function () {    
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
    $("#dialog_show_report").dialog({
        autoOpen: false,
        width: 1200,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "explode",
            duration: 1000
        }
        });
    $("#dialog_remove").dialog({
        autoOpen: false,
        width: 400,
        show: {
            effect: "blind",
            duration: 1000
        },
        hide: {
            effect: "explode",
            duration: 1000
        },
        buttons: {
            "确认": function () {
                do_remove_report(text_id);
                $(this).dialog("close");
            },
            "放弃": function () {
                $(this).dialog("close");
            }
        }
    });
});

function open_upload_dialog(guid_id) {
    text_id = guid_id;
    var uploader = WebUploader.create({

        // swf文件路径
        swf: '/lib/webuploader/Uploader.swf',

        // 文件接收服务端。
        server: '/Failures/Report_Upload',

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
        //?id = ' + text_id
        $('#' + file.id).find('p.state').text('已上传');
        $("#dialog").dialog("close");
        //alert(response._raw)
        $.ajax({
            type: "POST",
            url: "/Failures/Report_Rename",
            data: {
                'id': text_id,
                'filePath': response._raw
            },
            success: function (item) {
                location.href = '/Failures/Index';
            }
        });        
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

function show_report(guid_id) {
    $.ajax({
        type: "POST",
        url: "/Failures/Report_Show",
        data: {
            'id': guid_id,
        },
        success: function (responsedata) {
            if (responsedata=='1')
            {
                $("#img_failure_report").attr('src',"/upload/FailersReport/"+guid_id+".jpg");
                $("#a_fail_report").attr('href',"/upload/FailersReport/"+guid_id+".jpg");
                $("#a_fail_report").attr('download', guid_id+".jpg");
                $("#dialog_show_report").dialog("open");
            }
            else
            {
                alert("找不到相关的故障的报告！");
            }
        }
    });
}

function remove_report(guid_id){
    text_id = guid_id;
    $("#dialog_remove").dialog("open");
}

function do_remove_report(id){
    $.ajax({
        type: "POST",
        url: "/Failures/Report_Remove",
        data: {
            'id': id,
        },
        success: function (responsedata) {
            if (responsedata=='1')
            {
                alert("故障报告己删除！");
            }
            else
            {
                alert("找不到相关的故障的报告！");
            }
        }
    });
}

