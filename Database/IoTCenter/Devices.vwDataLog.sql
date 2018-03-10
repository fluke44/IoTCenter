USE [IoTCenter]
GO

CREATE view [devices].[vwDataLog]
as
select D.Id, D.Name, D.Type, D.Mac, D.Ip, C.Data, C.DateLogged 
from Devices.Device D
left join Devices.DataLog C
	on D.id = C.DeviceId

GO


