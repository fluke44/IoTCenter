<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetSensors.aspx.cs" Inherits="IoTCenter.Ajax.GetSensors" %>
<%@ Import Namespace="IoTCenter.Domain" %>
<%@ Import Namespace="IoTCenter.Domain.ReturnTypes" %>
<%@ Import Namespace="IoTCenter.Domain.Interface" %>
<%@ Import Namespace="System.Threading.Tasks" %>

<% Parallel.ForEach(Sensors, dev =>
    {
    dev.Read();%>
        <div class="<%=GetCssClassForDevice(dev) %>">
            <h1><%=dev.Device.Name %></h1>
            <div class="network"><%=dev.Device.Ip %> | <%=dev.Device.Mac %></div>
            <div class="type"><%=dev.Device.Type %></div>
            <div class="data"><%=dev.ToString() %></div>
        </div>
    <% }); %>