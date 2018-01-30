<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="IoTCenter.Index" %>
<%@ Import Namespace="IoTCenter.Domain" %>
<%@ Import Namespace="IoTCenter.Domain.ReturnTypes" %>
<%@ Import Namespace="IoTCenter.Domain.Interface" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="styles/default.css">
    <script type="text/javascript" src="Scripts/jquery-3.2.1.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            GetSensors();
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
                    $("#devices").html(output);
                    $("#devices").fadeIn(200);
                },
                error: function (output) {
                    $("#devices").fadeIn(200);
                    alert(output);
                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <input type="button" value="Stop" id="stopTimer" />
    <div id="devices">
        
    </div>
    </form>
</body>
</html>
