$(function () {
    $("#dialog_restore_success").dialog({
      autoOpen: false,
      width:500,
      show: {
        effect: "blind",
        duration: 1000
      },
      hide: {
        effect: "explode",
        duration: 1000
      }
    });
    $("#dialog_clear_success").dialog({
      autoOpen: false,
      width:500,
      show: {
        effect: "blind",
        duration: 1000
      },
      hide: {
        effect: "explode",
        duration: 1000
      }
    });
    $("#dialog_clear_confirm").dialog({
        autoOpen: false,
        width: 500,
        modal: true,
        buttons: {
            "确认": function () {
                do_remove_all();
            },
            "放弃": function () {
                $(this).dialog("close");
            }
        }
        });
    $("#dialog_upload").dialog({
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

function backup()
{
    $.ajax({
        url: "/MysqlDB/Backup",
        type: "post",        
        success: function (item) {
            document.getElementById("backup-file-name").value = item + ".json";
            document.getElementById("div-download").style.visibility = "visible";
        }
    });
}

function download()
{
    var filename = document.getElementById("backup-file-name").value;
    location.href = "/MysqlDB/Download?filename=" + filename;
}

function delete_backup_files()
{
    var filename = document.getElementById("backup-file-name").value;
    $.ajax({
        url: "/MysqlDB/DeleteFile",
        type: "post",        
        data: {"filename":filename},
        success: function () {
            document.getElementById("div-download").style.visibility = "hidden";
        }
    });    
}

function remove_all()
{
    $("#dialog_clear_confirm").dialog("open");
}

function do_remove_all()
{
    $("#dialog_clear_confirm").dialog("close");

    $.ajax({
        url: "/MysqlDB/RemoveAll",
        type: "post",        
        success: function (item) {
            $("#dialog_clear_success").dialog("open");
        }
    });
}

function open_upload_dialog() {
    var uploader = WebUploader.create({

        // swf文件路径
        swf: '/lib/webuploader/Uploader.swf',

        // 文件接收服务端。
        server: '/MysqlDB/Backup_Upload',

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
        $("#dialog_upload").dialog("close");
        //alert(response._raw)
        $.ajax({
            url: "/MysqlDB/Restore",
            type: "post",
            data: {"filepath":response._raw},
            success: function (item) {
                $("#dialog_restore_success").dialog("open");
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
        $("#dialog_upload").dialog("close");

    });
    
    $("#dialog_upload").dialog("open");
}