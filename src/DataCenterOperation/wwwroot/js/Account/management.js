var text_id = "";

$(function () {       
    
    $("#dialog_add_user").dialog({
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
                User_Do_Add();               
                $(this).dialog("close");
            },
            "放弃": function () {
                $(this).dialog("close");
            }
        }
    });

    $("#dialog_after_modify").dialog({
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
                $(this).dialog("close");
                location.href = "/Account/management";
            }
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
                User_Do_Remove(text_id);
                $(this).dialog("close");
            },
            "放弃": function () {
                $(this).dialog("close");
            }
        }
    });

});

function User_Add() {    
    $("#dialog_add_user").dialog("open");
}

function User_Do_Add() {
    var username = document.getElementById("add_user_name").value;
    var password = document.getElementById("add_user_password").value;
    var role = document.getElementById("add_user_role").value;
     $.ajax({
                    type: "POST",
                    url: "/Account/User_Add",
                    data: {
                        'username': username,
                        'password': password,
                        'role': role
                    },
                    success: function (item) {
                        location.href = item;
                    }
                });
}

function User_Remove(guid_id){
    text_id = guid_id;
    $( "#dialog_remove" ).dialog( "open" );
}

function User_Do_Remove(text_id){
$.ajax({
        type: "POST",
        url: "/Account/User_Remove",
        data: {
            'id': text_id,
        },
        success: function (item) {
            location.href=item;
        }
    });
}

function User_Modify(guid_id){    
    var password = document.getElementById("password_" + guid_id).value;
    var role = document.getElementById("role_" + guid_id).value;
    $.ajax({
        type: "POST",
        url: "/Account/User_Modify",
        data: {
            'id': guid_id,
            'password': password,
            'role': role
        },
        success: function (item) {            
            $("#dialog_after_modify").dialog("open");
        }
    });
}


