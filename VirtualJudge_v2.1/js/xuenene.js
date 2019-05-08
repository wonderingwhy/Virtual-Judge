function addNew() {
    var nodes = $('#tbody').children();
    var num = nodes.length - 1;
    if (num >= 15) {
        alert("Can not Add More");
        return;
    }
    var oj = $("#select").find("option:selected").text();
    var pid = $("#pid")[0].value;
    var alias = $("#alias")[0].value;
    var title = $("#ptitle").html();
    var href = $("#ptitle").attr("href");

    if (title == "NULL" || title.length <= 0 || alias.length <= 0) {
        return;
    }
    var e1 = "<td style='display:none'><input value='" + $("#id")[0].value + "'/></td>";
    var e2 = "<td>" + oj + "</td>";
    var e3 = "<td><input value='" + pid + "' class='form-control' disabled='disabled'/></td>";
    var e4 = "<td><input value='" + alias + "' class='form-control' disabled='disabled'/></td>";
    var e5 = "<td><a href='" + href + "' target='_blank'>" + title + "</a>" + "</td>";
    var e6 = "<td><input value='DEL' type='button' class='btn btn-warning' onclick='del($(this))'/></td>";
    $('#current').before("<tr>" + e1 + e2 + e3 + e5 + e4 + e6 + "</tr>");
    $("#pid")[0].value = "";
    $("#alias")[0].value = "";
    $("#ptitle").html("");
    $("#ptitle").attr("href", "#");
}
function del(obj) {
    obj.parent().parent().remove();
}
function getTitle() {
    ajax("ContestDo.ashx?action=getptitle&oid=" + $("#select").val() +
        "&pid=" + $("#pid")[0].value, function (resText) {
            if (resText == "no") {
                $("#ptitle").html("NULL");
                $("#ptitle").attr("href", "#");
                $("#ptitle").attr("target", "");
                return;
            }
            var p = JSON.parse(resText);
            $("#ptitle").html(p.Title);
            $("#ptitle").attr("href", "ProblemDo.ashx?action=view&pid=" + p.ProblemID);
            $("#ptitle").attr("target", "_blank");
            $("#id")[0].value = p.ProblemID.toString();
            $("#alias")[0].value = p.Title;
        })
}
function msubmit() {
    var nodes = $('#tbody').children();
    var num = nodes.length - 1;
    var s = "";
    for (var i = 0; i < num; ++i) {
        if (i != 0) {
            s += "`";
        }
        s += $($(nodes[i]).children()[0]).children()[0].value;
    }
    var ss = "";
    for (var i = 0; i < num; ++i) {
        if (i != 0) {
            ss += "`";
        }
        var sss=$($(nodes[i]).children()[4]).children()[0].value;
        if (sss.length > 20 || sss.length <= 0) {
            alert("Alias Length Should be Less than 20 and More than 0");
            return;
        }
        ss += $($(nodes[i]).children()[4]).children()[0].value;
    }
    var cnt = 0;
    for (var i = 0; i < s.length; ++i) {
        if (s[i] == '`') {
            ++cnt;
        }
    }

    var len = parseInt($('#dd')[0].value) + ":" + parseInt($('#hh')[0].value) + ":" + parseInt($('#mm')[0].value) + ":" + parseInt($('#ss')[0].value);
    if ($('#ctitle')[0].value.length <= 0) {
        alert("Input the Title");
        return;
    }
    if ($('#ctitle')[0].value.length > 20) {
        alert("Title Length Should be Less than 20");
        return;
    }
    if ($('#stime')[0].value.length <= 0) {
        alert("Input the StartTime");
        return;
    }
    if (s.length <= 0) {
        alert("Choose the Problems");
        return;
    }
    if ($('#psd')[0].value.length > 6) {
        alert("Password Length Should be Less than 6");
        return;
    }
    if ($('#dcl')[0].value.length > 100) {
        alert("Password Length Should be Less than 100");
        return;
    }
    if (len == "0:0:0:0") {
        alert("Length should be a positive Timespan");
        return;
    }
    if (cnt != num - 1) {
        alert("Illegal Char ` in Problems");
        return;
    }
    cnt = 0;
    for (var i = 0; i < ss.length; ++i) {
        if (ss[i] == '`') {
            ++cnt;
        }
    }
    if (cnt != num - 1) {
        alert("Illegal Char ` in Problems");
        return;
    }
    var pattern = /^[a-zA-Z0-9_]{0,6}$/;
    if (!pattern.test($('#psd')[0].value)) {
        $("#ConAddMsg").html("Password Regex ^[a-zA-Z0-9_]{0,6}$");
        return;
    }
    var sURL = "";
    sURL += "&ctitle=" + encodeURIComponent(xss($('#ctitle')[0].value));
    sURL += "&stime=" + encodeURIComponent(xss($('#stime')[0].value));
    sURL += "&tlen=" + encodeURIComponent(xss(len));
    sURL += "&psd=" + encodeURIComponent(xss($('#psd')[0].value));
    sURL += "&dcl=" + encodeURIComponent(xss($('#dcl')[0].value));
    sURL += "&pid=" + encodeURIComponent(xss(s));
    sURL += "&ptitle=" + encodeURIComponent(xss(ss));
    window.location.href = "ContestDo.ashx?action=addnew" + sURL;
}
function medit() {
    var nodes = $('#tbody').children();
    var num = nodes.length - 1;
    var s = "";
    for (var i = 0; i < num; ++i) {
        if (i != 0) {
            s += "`";
        }
        s += $($(nodes[i]).children()[0]).children()[0].value;
    }
    var ss = "";
    for (var i = 0; i < num; ++i) {
        if (i != 0) {
            ss += "`";
        }
        var sss = $($(nodes[i]).children()[4]).children()[0].value;
        if (sss.length > 20 || sss.length <= 0) {
            alert("Alias Length Should be Less than 20 and More than 0");
            return;
        }
        ss += $($(nodes[i]).children()[4]).children()[0].value;
    }
    var cnt = 0;
    for (var i = 0; i < s.length; ++i) {
        if (s[i] == '`') {
            ++cnt;
        }
    }

    var len = parseInt($('#dd')[0].value) + ":" + parseInt($('#hh')[0].value) + ":" + parseInt($('#mm')[0].value) + ":" + parseInt($('#ss')[0].value);
    if ($('#ctitle')[0].value.length <= 0) {
        alert("Input the Title");
        return;
    }
    if ($('#ctitle')[0].value.length > 20) {
        alert("Title Length Should be Less than 20");
        return;
    }
    if ($('#stime')[0].value.length <= 0) {
        alert("Input the StartTime");
        return;
    }
    if (s.length <= 0) {
        alert("Choose the Problems");
        return;
    }
    if ($('#psd')[0].value.length > 6) {
        alert("Password Length Should be Less than 6");
        return;
    }
    if ($('#dcl')[0].value.length > 100) {
        alert("Password Length Should be Less than 100");
        return;
    }
    if (len == "0:0:0:0") {
        alert("Length should be a positive Timespan");
        return;
    }
    if (cnt != num - 1) {
        alert("Illegal Char ` in Problems");
        return;
    }
    cnt = 0;
    for (var i = 0; i < ss.length; ++i) {
        if (ss[i] == '`') {
            ++cnt;
        }
    }
    if (cnt != num - 1) {
        alert("Illegal Char ` in Problems");
        return;
    }
    var pattern = /^[a-zA-Z0-9_]{0,6}$/;
    if (!pattern.test($('#psd')[0].value)) {
        $("#ConAddMsg").html("Password Regex ^[a-zA-Z0-9_]{0,6}$");
        return;
    }
    var sURL = "&cid=" + encodeURIComponent(xss($('#editcid')[0].value));
    sURL += "&ctitle=" + encodeURIComponent(xss($('#ctitle')[0].value));
    sURL += "&stime=" + encodeURIComponent(xss($('#stime')[0].value));
    sURL += "&tlen=" + encodeURIComponent(xss(len));
    sURL += "&psd=" + encodeURIComponent(xss($('#psd')[0].value));
    sURL += "&dcl=" + encodeURIComponent(xss($('#dcl')[0].value));
    sURL += "&pid=" + encodeURIComponent(xss(s));
    sURL += "&ptitle=" + encodeURIComponent(xss(ss));
    window.location.href = "ContestDo.ashx?action=editold" + sURL;
}
function checkNum(obj) {
    obj.value = obj.value.replace(/[^\d]/g, "");
    if (obj.value.length <= 0) {
        obj.value = "0";
    }
    obj.value = parseInt(obj.value).toString();
}
function check(obj, mx) {
    if (obj.value.length <= 0 || obj.value.length > obj.maxLength || parseInt(obj.value) >= mx) {
        obj.value = "0";
    }
    while (obj.value.length < obj.maxLength) {
        obj.value = "0" + obj.value;
    }
}
function input(obj, event, from, to) {
    var keyCode = event.which;
    var num = parseInt(obj.value);
    if (keyCode >= 48 && keyCode <= 57 && num * 10 + keyCode - 48 < to) {
        return true;
    }
    if (keyCode == 8 || keyCode == 0) {
        return true;
    }
    return false;
};
function pshow(num) {
    $("#myTabs a[href='#problem']").tab("show")
    $("#myPills a[href='#" + num + "']").tab('show');
    //window.location.hash = "problem";
    //console.log(window.location.hash);
}
function ShowDiv(show_div, bg_div) {
    
    document.getElementById(show_div).style.display = 'block';
    document.getElementById(bg_div).style.display = 'block';
    var bgdiv = document.getElementById(bg_div);
    bgdiv.style.width = document.body.scrollWidth;
    $("#" + bg_div).height($(document).height());
}
function CloseDiv(show_div, bg_div) {
    $(".text").html("");
    $(".text").val("");
    document.getElementById(show_div).style.display = 'none';
    document.getElementById(bg_div).style.display = 'none';
}
function getCompilers() {
    if ($("#titles").val() == null || $("#titles").val() == "") {
        return;
    }
    var strs = $("#titles").val().split("&");
    var pid = strs[0];
    var oid = strs[1];
    var curid=$("#currentOJID").val();
    if (curid != "0" && curid == oid) {
        return;
    }
    ajax("ContestDo.ashx?action=getcompilers&oid=" + oid, function (resText) {
        //console.log(oid);
        //console.log(resText);
        $("#compilers").html("");
        if (resText == "no") {
            $("#compilers").html("<option>NULL</option>");
            return;
        }
        var compilers = resText.split("|");
        for (var i = 0; i < compilers.length; ++i) {
            var compiler = compilers[i].split("&");
            $("#compilers").append("<option value='" + compiler[0] + "'>" + compiler[1] + "</option>");
        }
    });
}
function login() {
    var pattern = /^[a-zA-Z0-9_]{6,16}$/;
    if (!pattern.test($("#uidlog").val())) {
        alert("Username is not Existing or Password is Wrong");
        return;
    }
    if (!pattern.test($("#psdlog").val())) {
        alert("Username is not Existing or Password is Wrong");
        return;
    }
    var s = "";
    s += "&uid=" + $("#uidlog").val();
    s += "&psd=" + $("#psdlog").val();
    ajax("UserDo.ashx?action=login" + s, function (resText) {
        console.log(resText);
        if (resText == "no") {
            alert("Username is not Existing or Password is Wrong");
        }
        else if (resText == "ok") {
            window.location.reload();
        }
        else {
            alert("Unknown Error");
        }
    });
}
function reg() {
    var pattern = /^[a-zA-Z0-9_]{6,16}$/;
    if (!pattern.test($("#uidreg").val())) {
        $("#RegMsg").html("Username Regex ^[a-zA-Z0-9_]{6,16}$");
        return;
    }
    console.log(pattern);
    console.log($("#psdreg").val());
    if (!pattern.test($("#psdreg").val())) {
        $("#RegMsg").html("Password Regex ^[a-zA-Z0-9_]{6,16}$");
        return;
    }
    pattern = /^.{0,10}$/;
    if (!pattern.test($("#nnmreg").val())) {
        $("#RegMsg").html("Nickname Regex ^.{0,10}$");
        return;
    }
    if ($("#psdreg").val() != $("#psdreg2").val()) {
        $("#RegMsg").html("Passwords are different");
        return;
    }
    var s = "";
    s += "&uid=" + $("#uidreg").val();
    s += "&psd=" + $("#psdreg").val();
    s += "&nnm=" + $("#nnmreg").val();
    ajax("UserDo.ashx?action=reg" + s, function (resText) {
        if (resText == "no") {
            alert("Failed to Register");
        }
        else if (resText == "exist") {
            $("#RegMsg").html("Username is Existing");
        }
        else if (resText == "ok") {
            alert("ok");
            window.location.reload();
        }
        else {
            alert("Unknown Error");
        }
    });
}
function logout() {
    ajax("UserDo.ashx?action=logout", function (resText) {
        if (resText == "no") {
            alert("Failed to Log out");
        }
        else if (resText == "ok") {
            window.location.reload();
        }
        else {
            alert("Unknown Error");
        }
    });
}
function getInfo() {
    ajax("UserDo.ashx?action=getinfo", function (resText) {
        if (resText == "no") {
            alert("Failed to Get Infomation");
            return;
        }
        var p = JSON.parse(resText);
        $("#uidmdf").val(p.Username);
        $("#opsdmdf").val(p.Password);
        $("#nnmmdf").val(p.Nickname);
        $("#npsdmdf").val("");
        $("#npsdmdf2").val("");
        
    });
}
function mdf() {
    var pattern = /^[a-zA-Z0-9_]{6,16}$/;
    if (!pattern.test($("#npsdmdf").val())) {
        $("#MdfMsg").html("Password Regex ^[a-zA-Z0-9_]{6,16}$");
        return;
    }
    if ($("#npsdmdf").val() != $("#npsdmdf2").val()) {
        $("#MdfMsg").html("Passwords are different");
        return;
    }
    var pattern2 = /^.{0,10}$/;
    if (!pattern2.test($("#nnmmdf").val())) {
        $("#MdfMsg").html("Nickname Regex ^.{0,10}$");
        return;
    }
    
    if (!pattern.test($("#psdmdf").val())) {
        alert("Password is Wrong");
        return;
    }
    var s = "";
    s += "&opsd=" + $("#opsdmdf").val();
    s += "&npsd=" + $("#npsdmdf").val();
    s += "&nnm=" + $("#nnmmdf").val();
    ajax("UserDo.ashx?action=modify" + s, function (resText) {
        if (resText == "no") {
            alert("Failed to Modify Infomation");
            return;
        }
        else if(resText == "wrong") {
            alert("old Password is Wrong");
            return;
        }
        else if (resText == "ok") {
            alert("ok");
            window.location.reload();
        }
        else {
            alert("Unknown Error");
        }
    });
}
function autoLog() {
    ajax("UserDo.ashx?action=auto", function (resText) {
        //console.log(resText);
        if (resText == "no") {
            $("#right1").parent().show();
            $("#right2").parent().show();
            $("#right3").parent().hide();
            $("#right4").parent().hide();
            return;
        }
        var uname = resText;
        $("#right3").html(uname);
        $("#right1").parent().hide();
        $("#right2").parent().hide();
        $("#right3").parent().show();
        $("#right4").parent().show();
    });
}
function ShowCode(obj, show_div, bg_div) {
    var children = obj.parent().parent().children();
    console.log(children[3].innerHTML);

    $("#uidcode").html(children[3].innerHTML);
    $("#pidcode").html($(children[1]).children()[0].innerHTML);
    $("#stscode").html(children[2].innerHTML);
    $("#cplcode").html(children[7].innerHTML);

    ajax("ContestDo.ashx?action=code&sid=" + children[0].innerHTML, function (resText) {
        console.log(resText);
        if (resText == "no") {
            alert("No permission");
            return;
        }
        else {
            $("#sourcecode").html(resText);
            return;
        }
    });
    document.getElementById(show_div).style.display = 'block';
    document.getElementById(bg_div).style.display = 'block';
    var bgdiv = document.getElementById(bg_div);
    bgdiv.style.width = document.body.scrollWidth;
    $("#" + bg_div).height($(document).height());
}
function CheckAndView() {
    ajax('ContestDo.ashx?action=checkcpsd&cid=' + $('#choosecid')[0].value + '&psd=' + $('#conpsd')[0].value, function (resText) {
        console.log(resText);
        if (resText == "ok") {
            window.location.href = "ContestDo.ashx?action=view&cid=" + $('#choosecid')[0].value;
            return;
        }
        else if (resText == "no") {
            $('#PsdMsg').html("Password is Wrong");
            return;
        }
        else {
            alert("Unknown Error");
            return;
        }
    });
}
function checkcu() {
    ajax('ContestDo.ashx?action=check&cid=' + $('#choosecid')[0].value, function(resText) {
        if (resText == "ok") {
            window.location.href = "ContestDo.ashx?action=view&cid=" + $('#choosecid')[0].value;
            return;
        }
        else if (resText == "no") {
            ShowDiv('divPsd', 'fade');
            return;
        }
        else {
            alert("Unknown Error");
            return;
        }
    });
}
function checkcode() {
    var len = $('#codetxt')[0].value.length;
    //console.log(len);
    if (len < 50 || len > 65536) {
        alert("Code is Too Long or Too Short");
        return false;
    }
    else {
        $('#codetxt')[0].value = xss($('#codetxt')[0].value);
        return true;
    }
}
function xss(str) {
    str = str.split("<").join("&lt;");
    return str.split(">").join("&gt;");
}
function ToFirst() {
    $("#mPage")[0].value = 1;
    $("#sub").click();
    return false;
}
function ToPrev() {
    $("#mPage")[0].value = parseInt($("#mPage")[0].value) - 1;
    $("#sub").click();
    return false;
}
function ToNext() {
    $("#mPage")[0].value = parseInt($("#mPage")[0].value) + 1;
    console.log($("#mPage")[0].value);
    $("#sub").click();
    return false;
}
function ToLast() {
    $("#mPage")[0].value = 0;
    $("#sub").click();
    return false;
}
function checksearch() {
    var len1 = $('#mtitle')[0].value.length;
    var len2 = $('#mcreator')[0].value.length;
    if (len1 > 50 || len2 > 50) {
        alert("Search String is Too Long");
        return false;
    }
    $('#mtitle')[0].value = xss($('#mtitle')[0].value);
    $('#mcreator')[0].value = xss($('#mcreator')[0].value);
    return true;
}
function sToFirst() {
    $("#toprev1").hide();
    $("#toprev2").show();

    var sum = parseInt($("#cnt")[0].value);
    var base = parseInt($("#base")[0].value);
    var p;
    var npp = parseInt($("#npp")[0].value);
    if (sum % npp == 0 && sum != 0) {
        p = sum / npp;
    }
    else {
        p = parseInt(sum / npp + 1);
    }
    if (p == 1) {
        $("#tonext1").hide();
        $("#tonext2").show();
    }
    else {
        $("#tonext2").hide();
        $("#tonext1").show();
    }

    for (var i = 0; i < Math.min(sum - base, npp) ; ++i) {
        $("#sols" + i).hide();
    }
    $("#base")[0].value = "0";
    for (var i = 0; i < Math.min(sum, npp) ; ++i) {
        $("#sols" + i).show();
    }
}
function sToPrev() {
    $("#tonext1").show();
    $("#tonext2").hide();
    var sum = parseInt($("#cnt")[0].value);
    var base = parseInt($("#base")[0].value);
    var p;
    var npp = parseInt($("#npp")[0].value);
    if (sum % npp == 0 && sum != 0) {
        p = sum / npp;
    }
    else {
        p = parseInt(sum / npp + 1);
    }
    for (var i = 0; i < Math.min(sum - base, npp) ; ++i) {
        $("#sols" + i).hide();
    }
    base -= npp;
    $("#base")[0].value = base;
    for (var i = 0; i < Math.min(sum - base, npp) ; ++i) {
        $("#sols" + i).show();
    }
    if (base < npp) {
        $("#toprev1").hide();
        $("#toprev2").show();
    }
}
function sToNext() {
    $("#toprev1").show();
    $("#toprev2").hide();
    var sum = parseInt($("#cnt")[0].value);
    var base = parseInt($("#base")[0].value);
    var p;
    var npp = parseInt($("#npp")[0].value);
    if (sum % npp == 0 && sum != 0) {
        p = sum / npp;
    }
    else {
        p = parseInt(sum / npp + 1);
    }
    for (var i = 0; i < Math.min(sum - base, npp) ; ++i) {
        $("#sols" + i).hide();
    }
    base += npp;
    $("#base")[0].value = base;
    for (var i = 0; i < Math.min(sum - base, npp) ; ++i) {
        $("#sols" + i).show();
    }
    if (sum - base <= npp) {
        $("#tonext1").hide();
        $("#tonext2").show();
    }
}
function sToLast() {
    $("#tonext1").hide();
    $("#tonext2").show();

    var sum = parseInt($("#cnt")[0].value);
    var base = parseInt($("#base")[0].value);
    var p;
    var npp = parseInt($("#npp")[0].value);
    if (sum % npp == 0 && sum != 0) {
        p = sum / npp;
    }
    else {
        p = parseInt(sum / npp + 1);
    }
    if (p == 1) {
        $("#toprev1").hide();
        $("#toprev2").show();
    }
    else {
        $("#toprev2").hide();
        $("#toprev1").show();
    }

    for (var i = 0; i < Math.min(sum - base, npp) ; ++i) {
        $("#sols" + i).hide();
    }
    base = (p - 1) * npp;
    $("#base")[0].value = base;
    for (var i = 0; i < Math.min(sum - base, npp) ; ++i) {
        $("#sols" + i).show();
    }
}
function statusinit() {
    //console.log("ok");
    var sum = parseInt($("#sum")[0].value);
    var sel1 = $("#mpid").val();
    var sel2 = $("#msolstatus").val();
    var sel3 = $("#muid")[0].value;
    var npp = parseInt($("#npp")[0].value);
    var pattern = ".*" + sel3 + ".*";
    var reg = new RegExp(pattern);
    var cnt = 0;
    //console.log(sel1 + "&" + sel2);
    for (var i = 0; i < sum; ++i) {
        $($("#sols").children()[i]).hide();
        $($("#sols").children()[i]).attr("id", "");
        var pid = $($($("#sols").children()[i]).children()[1].children).html();
        var status = $($($("#sols").children()[i]).children()[2]).html();
        var uid = $($($("#sols").children()[i]).children()[3]).html();
        if ((pid == sel1 || sel1 == "") && 
            (status == sel2 || sel2 == "" || (status != "Accepted" && sel2 == "Other")) &&
            reg.exec(uid) != null) {
            $($("#sols").children()[i]).attr("id", "sols" + cnt);
            ++cnt;
        }
    }
    
    //console.log(cnt);
    $("#cnt")[0].value = cnt;
    var base = parseInt($("#base")[0].value);
    for (var i = 0; i < Math.min(cnt - base, npp) ; ++i) {
        //$($("#sols").children()[base + i]).show();
        $("#sols" + i).show();
    }
    sToFirst();
}
function getTime() {
    var t = new Date(ts);
    with (t) {
        var _time = getFullYear() + "/" + (getMonth() + 1) + "/" + +getDate() + " " + getHours() + ":" + (getMinutes() < 10 ? "0" : "") + getMinutes() + ":" + (getSeconds() < 10 ? "0" : "") + getSeconds();
    }
    document.getElementById("c_time").innerHTML = _time;
    ts += 1000;

}