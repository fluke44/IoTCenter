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
        var tid;
		var loadingData;

        $(document).ready(function ()
        {
            tempScale = chroma.scale(['008ae5', 'green', 'orange', 'red']).domain([-10, 15, 25, 40]).mode('lrgb');

            LoadDevices();

            $("#load").click(function () {
                LoadDevices();
            })
            $("#startTimer").click(function () {
                tid = setInterval(LoadDevices, 30000);
            });
            $("#stopTimer").click(function () {
                clearInterval(tid);
            });
        });

        function LoadDevices() {
            GetSensors();
            GetSwitches();
        }

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
                    BindStyling();
                },
                error: function (output) {
                    $("#error").fadeOut(50).html(output).fadeIn(50);
                }
            });
            
        }

        function GetSwitches() {
            $.ajax({
                url: 'Ajax/Switches.aspx',
                //data: {
                //    action: 'loadFbGalleryItems',
                //    photoGalleryId: id
                //},
                type: 'post',
                success: function (output) {
                    $("#switches").html(output);
                    SetDataColorByValue();
                    BindStyling();
                },
                error: function (output) {
                    $("#error").fadeOut(50).html(output).fadeIn(50);
                }
            });
        }

        function Switch(id, state) {
			FadeOut($("#switch_" + id));
            $.ajax({
				url: 'Ajax/Switches.aspx',
                data: {
                    action: 'switchState',
                    deviceId: id,
                    state: state
                },
                type: 'get',
				dataType: 'text',
                success: function (output) {
                    GetSwitches();
                },
                error: function (output) {
                    $("#error").html(output);
                },
				complete: function () {
					FadeIn($("#switch_" + id));
				}
            });
        }
		
		function FadeOut(element) {
			element.fadeTo(100, 0.4);
		}
		
		function FadeIn(element) {
			element.fadeIn(300);
		}

        function BindStyling() {
            $('div[title]').qtip({
                style: { classes: 'qtip-rounded' }
            });
            $('.dataError[title]').qtip({
                style: { classes: 'qtip-red' }
            });
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
        <input type="button" value="Load" id="load" />
        <input type="button" value="Start" id="startTimer" />
        <input type="button" value="Stop" id="stopTimer" />
    
        <div id="devices">
        </div>

        <div id="switches"> 
        </div>
    </form>
</body>
</html>
