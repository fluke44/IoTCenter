USE [IoTCenter]
GO

create procedure [devices].[spAddDevice]
(
	@Name nvarchar(255),
	@Mac nvarchar(17),
	@Ip nvarchar(15),
	@Type nvarchar(100)
)
as
	if not exists (select 1 from Devices.Device where Mac = @Mac)
	begin
		insert into Devices.Device (Name, Mac, Ip, Type, Registered, DateRegistered)
		values (@Name, @Mac, @Ip, @Type, 1, getdate())
	end
	else
	begin
		update Devices.Device 
		set Name = @Name,
		Ip = @Ip,
		Type = @Type,
		Registered = 1,
		DateREgistered = getdate()
		where Mac = @Mac
	end
GO


