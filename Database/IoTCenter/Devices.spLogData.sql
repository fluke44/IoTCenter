USE [IoTCenter]
GO

CREATE procedure [devices].[spLogData]
(
	@Mac nvarchar(20),
	@Data nvarchar(max)
)
as

declare @DeviceId int
select @DeviceId = Id from Devices.Device where Mac = @Mac

insert into devices.DataLog (DeviceId, Data, DateLogged)
select @DeviceId, @Data, GETDATE()
GO


