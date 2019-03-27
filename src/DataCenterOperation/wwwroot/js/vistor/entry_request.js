function isEmpty(obj) {
    if (typeof obj == "undefined" || obj == null || obj == "") {
        return true;
    } else {
        return false;
    }
}

function getList(list_name) {
    var result = []
    var area_values = document.getElementsByName(list_name);
    for (i = 0; i < area_values.length; i++) {
        if (area_values[i].checked) {
            result.push(area_values[i].value)
        }
    }
    //alert(result.join(','))    
    return result
}

function delRow(){
    var editTable=document.getElementById("tbody");
    if(confirm("确认删除所选?")){
        var checkboxs=document.getElementsByName("checkRow");
        for(var i=0;i<checkboxs.length;i++){
            if(checkboxs[i].checked){
                var n=checkboxs[i].parentNode.parentNode;
                editTable.removeChild(n);
                i--;
            }
        }
        
    }
}

function createRow(){
    var editTable=document.getElementById("tbody");
    var tr=document.createElement("tr");
    var td0=document.createElement("td");
    var checkbox=document.createElement("input");
    checkbox.type="checkbox";
    checkbox.name="checkRow";
    td0.appendChild(checkbox);
    var td1=document.createElement("td");
    td1.innerHTML="<input type='text' />";
    var td2=document.createElement("td");
    td2.innerHTML="<input type='text' />";
    var td3=document.createElement("td");
    td3.innerHTML="<input type='text' />";
    tr.appendChild(td0);
    tr.appendChild(td1);
    tr.appendChild(td2);
    tr.appendChild(td3);
    editTable.appendChild(tr);
}

function TableToJson(tableid) {
    //alert('TableToJson');
    var txt = "[";
    var table = document.getElementById(tableid);
    var row = table.getElementsByTagName("tr");
    var col_name = ['', 'Name', 'Identity', 'Company'];
    for (var j = 1; j < row.length; j++) {
        var r = "{";
        var tds = row[j].getElementsByTagName("input");
        for (var i = 1; i < col_name.length; i++) {
            r += "\"" + col_name[i] + "\"\:\"" + tds[i].value + "\",";            
        }
        r = r.substring(0, r.length - 1)
        r += "},";
        txt += r;
    }
    txt = txt.substring(0, txt.length - 1);
    txt += "]";
    //alert("final" + txt);
    return txt;
}

 function doSubmitForm(from) {
    //alert('doSubmitForm')
       
    var resultValue = []
    resultValue = getList('areas')
    if (!isEmpty(document.getElementById('other_area').value)) {
        resultValue.push(document.getElementById('other_area').value)
    }
    document.getElementById('hidden_area').value = resultValue.join(',')

    resultValue.length = 0
     resultValue = getList('belongings')
     if (!isEmpty(document.getElementById('other_belonging').value)) {
         resultValue.push(document.getElementById('other_belonging').value)
     }
    document.getElementById('hidden_belongings').value = resultValue.join(',')

    resultValue.length = 0
     resultValue = getList('matters')     
     if (!isEmpty(document.getElementById('other_matter').value)) {
         resultValue.push(document.getElementById('other_matter').value)
     }
    document.getElementById('hidden_matters').value = resultValue.join(',')

     var entourages = TableToJson('entry_request_entourages_Table')
     //alert(entourages)
     document.getElementById('hidden_entourages').value = entourages
     //alert(document.getElementById('hidden_entourages').value)
     
    form.submit();
}