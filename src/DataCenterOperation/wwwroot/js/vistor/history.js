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
});

function open_dialog(text_id){

    $.ajax({
        url: "/Vistor/Details",
        data: {'id':text_id},
        type: "post",
        dataType:"json",
        success: function (item) {

          tr = "";
          tr += "<td>" + item.RequestPeoopleName + "</td>";
          tr += "<td>" + item.Company + "</td>";
          $("#records_table").append("<tr>"+tr+"</tr>");
          tr = "";
          tr += "<th>申请日期</th>";
          tr += "<th>开始时间</th>";
          tr += "<th>结束时间</th>";
          $("#records_table").append("<tr>"+tr+"</tr>");
          tr = "";
          tr += "<td>" + item.RequestDate + "</td>";
          tr += "<td>" + item.BeginTime + "</td>";
          tr += "<td>" + item.EndTime + "</td>";
          $("#records_table").append("<tr>"+tr+"</tr>");
          tr = "";
          tr += "<th>进入区域</th>";
          tr += "<th>携带事物</th>";
          $("#records_table").append("<tr>"+tr+"</tr>");
          tr = "";
          tr += "<td>" + item.Area + "</td>";
          tr += "<td>" + item.Belongings + "</td>";
          $("#records_table").append("<tr>"+tr+"</tr>");
          $( "#dialog" ).dialog( "open" );
        }
    });
}