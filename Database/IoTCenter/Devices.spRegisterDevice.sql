USE [IoTCenter]
GO

CREATE procedure [devices].[spRegisterDevice]
(
	@Mac nvarchar(17),
	@Register bit
)
as
	if exists (select 1 from Devices.Device where Mac = @Mac)
	begin
		update Devices.Device 
		set Registered = @Register,
		DateRegistered = case @Register when 1 then getdate() else null end
		where Mac = @Mac
	end
GO


