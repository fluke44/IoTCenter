<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="IoTCenter.Index" %>
<%@ Import Namespace="IoTCenter.Domain" %>
<%@ Import Namespace="IoTCenter.Domain.ReturnTypes" %>
<%@ Import Namespace="IoTCenter.Domain.Interface" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="styles/default.css">
    <link rel="stylesheet" type="text/css" href="Styles/jquery.qtip.min.css">

    <script type="text/javascript" src="Scripts/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery.qtip.min.js"></script>
    <script type="text/javascript" src="Scripts/chroma.js"></script>
    
    <script type="text/javascript">
        var tempScale;

        $(document).ready(function ()
        {
            tempScale = chroma.scale(['008ae5', 'green', 'orange', 'red']).domain([-10, 15, 25, 40]).mode('lrgb');

            var tid = setInterval(GetSensors, 20000);

            $("#stopTimer").click(function () {
                clearInterval(tid);
            });
        });

        function GetSensors() {
            $.ajax({
                url: 'Ajax/GetSensors.aspx',
                //data: {
                //    action: 'loadFbGalleryItems',
                //    photoGalleryId: id
                //},
                type: 'post',
                success: function (output) {
                    $("#devices").fadeOut(50).html(output).fadeIn(50);
                    SetDataColorByValue();
                    $(".network").hide();
                },
                error: function (output) {
                    $("#error").fadeOut(50).html(output).fadeIn(50);
                }
            });
            BindStyling();
        }

        function BindStyling() {
            $('div[title]').qtip();
        }

        function ShowNetInfo(id) {
            $("#device" + id + ".activeDevice.network").show();
        }

        function SetDataColorByValue() {
            $("#devices div[id^='device']").each(function () {
                var dataDiv = $(this).find(".data")[0];
                var data = dataDiv.innerText;
                var temp = data.substring(0, data.indexOf("°"));
                if(temp != "") $(this).find(".data").css("color", tempScale(temp));
            });
        }
    </script>
</head>
<body>
    <div id="error">
        
    </div>
    <form id="form1" runat="server">
        <input type="button" value="Stop" id="stopTimer" />
    <div id="devices">
        
    </div>
    </form>
</body>
</html>
