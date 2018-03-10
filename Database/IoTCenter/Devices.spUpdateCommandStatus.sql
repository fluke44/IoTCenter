use IoTCenter
go

create procedure Devices.spUpdateCommandStatus (
	@CommandId int,
	@Status nvarchar(50)
)
as

if exists (select 1 from devices.CommandQueue where Id = @CommandId)
begin
	update devices.CommandQueue
	set Status = @Status
	where Id = @CommandId
end
else
begin
	declare @msg nvarchar(1000)
	set @msg = 'Command with ID ' + CAST(@commandId as nvarchar(10)) + ' does not exist';
	throw 50001, @msg, 1
end