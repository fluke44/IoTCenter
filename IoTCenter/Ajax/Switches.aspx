<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Switches.aspx.cs" Inherits="IoTCenter.Ajax.Switches" %>
<%@ Import Namespace="IoTCenter.Domain" %>
<%@ Import Namespace="IoTCenter.Domain.ReturnTypes" %>
<%@ Import Namespace="IoTCenter.Domain.Interface" %>
<%@ Import Namespace="IoTCenter.Domain.Enum" %>
<%@ Import Namespace="System.Threading.Tasks" %>

<%  int i = 1;
    Parallel.ForEach(SwitchList, dev =>
    {
        var data = dev.Read(CommandName.State);%>
        <div class="<%=GetCssClassForDevice(dev, data) %>" id="switch_<%=dev.Id %>">
            <div class="<%=IsSleeping(dev) %>"></div>
            <div title="<%=dev.Ip %> | <%=dev.Mac %>" class="<%=GetCssClassForStatus(dev, data) %>"></div>
            <h1><%=dev.Name %></h1>
            <div class="data">
                <a href="#" onclick="Switch(<%=dev.Id %>, <%=IsToggleOn(data) ? 1 : 0 %>);">
                    <img src="<%=IsToggleOn(data) ? "../img/toggleOn.png" : "../img/toggleOff.png" %>" width="80" />
                </a></div>
            <div class="date"><%=data.DateReceived %></div>
            <%if (!data.Success) { %>
            <div class="dataError" title="<%=data.Error %>"></div>
            <%} %>
            <%i++; %>
        </div>
    <% }); %>