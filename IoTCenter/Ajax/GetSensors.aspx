<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GetSensors.aspx.cs" Inherits="IoTCenter.Ajax.GetSensors" %>
<%@ Import Namespace="IoTCenter.Domain" %>
<%@ Import Namespace="IoTCenter.Domain.ReturnTypes" %>
<%@ Import Namespace="IoTCenter.Domain.Interface" %>
<%@ Import Namespace="IoTCenter.Domain.Enum" %>
<%@ Import Namespace="System.Threading.Tasks" %>

<%  int i = 1;
    Parallel.ForEach(Sensors, dev =>
    {
        var data = dev.Read();
        var batLevel = dev.Read(CommandName.Bat);%>
        <div class="<%=GetCssClassForDevice(dev, data) %>" id="device<%=i %>">
            <div title="<%=batLevel.Data %>" class="<%=Battery(batLevel) %>"></div>
            <div class="<%=IsSleeping(dev) %>"></div>
            <div title="<%=dev.Ip %> | <%=dev.Mac %>" class="<%=GetCssClassForStatus(dev, data) %>"></div>
            <h1><%=dev.Name %></h1>
            <div class="data"><%=data.Data %></div>
            <div class="date"><%=data.DateReceived %></div>
            <%i++; %>
        </div>
    <% }); %>