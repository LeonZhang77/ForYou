$(function () {
    $("#dialog").dialog({
      autoOpen: false,
      width:900,
      show: {
        effect: "blind",
        duration: 1000
      },
      hide: {
        effect: "explode",
        duration: 1000
      }
    });
    $("#dialog_update_contactInfo").dialog({
        autoOpen: false,
        width: 900,
        modal: true,
        buttons: {
            "确认": function () {
                $(this).dialog("close");
            },
            "放弃": function () {
                $(this).dialog("close");
            }
        }
    });
});

function update_ContactInfo(text_id) {
    var emelement_id = 'contact_info_' + text_id;
    var contact_info = document.getElementById(emelement_id).value;
    //#contact_info_0d783795-2629-46c1-adeb-ba553f6156e5
    $("#dialog_update_contactInfo").dialog({
        autoOpen: false,
        width: 400,
        modal: true,
        buttons: {
            "确认": function () {
                update_ContactInfo_post(text_id, contact_info);
                $(this).dialog("close");
            },
            "放弃": function () {
                $(this).dialog("close");
            }
        }
    });
    $("#dialog_update_contactInfo").dialog("open");
}

function update_ContactInfo_post(text_id, contact_info) {
    $.ajax({
        type: "POST",
        url: "/Vistor/Add_ContactInfo",
        data: {
            'id': text_id,
            'contactInfo': contact_info
        }
    });
}

function open_dialog(text_id){

    $.ajax({
        url: "/Vistor/Details",
        data: {'id':text_id},
        type: "post",
        dataType:"json",
        success: function (item) {
            $("#records_table").empty();
            tr = "<tr><th>申请人</th><th>单位</th><th></th></tr>";
            $("#records_table").append(tr)

          tr = "";
          tr += "<td>" + item.RequestPeoopleName + "</td>";
          tr += "<td>" + item.Company + "</td>";
          $("#records_table").append("<tr>"+tr+"</tr>");

          tr = "";
          tr += "<th>申请日期</th>";
          tr += "<th>开始时间</th>";
          tr += "<th>结束时间</th>";
            $("#records_table").append("<tr>" + tr + "</tr>");

          tr = "";
          tr += "<td>" + item.RequestDate + "</td>";
          tr += "<td>" + item.BeginTime + "</td>";
          tr += "<td>" + item.EndTime + "</td>";
            $("#records_table").append("<tr>" + tr + "</tr>");

          tr = "";
          tr += "<th>进入区域</th>";
          tr += "<th>携带事物</th>";
            $("#records_table").append("<tr>" + tr + "</tr>");

          tr = "";
          tr += "<td>" + item.Area + "</td>";
          tr += "<td>" + item.Belongings + "</td>";
            $("#records_table").append("<tr>" + tr + "</tr>");

            $("#records_table").append("<tr><th>随行人员：</th></tr>")
            tr = "";
            tr += "<th>姓名</th>";
            tr += "<th>证件号</th>";
            tr += "<th>单位</th>";
            $("#records_table").append("<tr>" + tr + "</tr>");
            var entourage_list = jQuery.parseJSON(item.Entourage);
            for (var i = 0; i < entourage_list.length; i++) {
                tr = "";
                tr += "<td>" + entourage_list[i].Name + "</td>";
                tr += "<td>" + entourage_list[i].Identity + "</td>";
                tr += "<td>" + entourage_list[i].Company + "</td>";
                $("#records_table").append("<tr>" + tr + "</tr>");
            }
            $("#matter_details").html(item.Matter_Details);
            $("#admin_confirm").html(item.Admin_Confirm);
            $("#manager_confirm").html(item.Manager_Confirm);

          $( "#dialog" ).dialog( "open" );
        }
    });
}