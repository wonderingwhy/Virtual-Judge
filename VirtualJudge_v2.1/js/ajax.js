function ajax(url, onsuccess)
{
    
    var xmlhttp = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject('Microsoft.XMLHTTP'); 
    xmlhttp.open("POST", url, true);
    xmlhttp.onreadystatechange = function ()
    {
        if (xmlhttp.readyState == 4) 
        {
            if (xmlhttp.status == 200) 
            {
                onsuccess(xmlhttp.responseText);
            }
            else
            {
                alert("AJAX Server Internal Error！");
            }

        }
    }
    xmlhttp.send(); 
}