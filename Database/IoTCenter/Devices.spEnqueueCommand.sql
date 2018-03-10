use IoTCenter
go

create procedure Devices.spEnqueueCommand (
	@Mac nvarchar(17),
	@Command nvarchar(50),
	@Url nvarchar(1000)
)
as

declare @DeviceId int
select @DeviceId = Id from devices.Device where Mac = @Mac

insert into devices.CommandQueue (DeviceId, Command, Url, Status, DateAdded)
select @DeviceId, @Command, @Url, 'Pending', GETDATE()
