namespace EventSourcingSample

open System

type Driver = {
    Id: int
    Name: string
}

type Vehicle = {
    Id: int
    Vin: string
}

type Policy() = 
    member val Id: int = 0 with get, set
    member val EndorsementNumber: int = 0 with get, set
    member val Drivers: Driver list = [] with get, set
    member val Vehicles: Vehicle list = [] with get, set
    
type PolicyDocument() = 
    inherit Document<Guid, Policy>()

module Commands =
    type ReservePolicyIdCommand() =
        interface ICommand with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        
    type AddDriverCommand() =
        interface ICommand with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Driver: Driver = Unchecked.defaultof<Driver> with get, set
        
    type AddVehicleCommand() =
        interface ICommand with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Vehicle: Vehicle = Unchecked.defaultof<Vehicle> with get, set

    type RemoveDriverCommand() =
        interface ICommand with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Id: int = 0 with get, set

    type RemoveVehicleCommand() =
        interface ICommand with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Id: int = 0 with get, set

    type ValidateDriverCommand() =
        interface ICommand with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Id: int = 0 with get, set

    type ValidateVehicleCommand() =
        interface ICommand with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Id: int = 0 with get, set

    type ValidatePolicyCommand() =
        interface ICommand with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Id: int = 0 with get, set

    type RatePolicyCommand() =
        interface ICommand with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Id: int = 0 with get, set

module Events =
    type PolicyCreatedEvent() =
        interface IEvent with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        
    type PolicyLoadedEvent() =
        interface IEvent with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Id: int = 0 with get, set
        member val EndorsementNumber: int = 0 with get, set
        
    type ReservedPolicyIdEvent() =
        interface IEvent with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Id: int = 0 with get, set
        
    type AddedDriverEvent() =
        interface IEvent with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Driver: Driver = Unchecked.defaultof<Driver> with get, set
        
    type AddedVehicleEvent() =
        interface IEvent with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Vehicle: Vehicle = Unchecked.defaultof<Vehicle> with get, set

    type RemovedDriverEvent() =
        interface IEvent with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Id: int = 0 with get, set

    type RemovedVehicleEvent() =
        interface IEvent with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Id: int = 0 with get, set

    type ValidatedDriverEvent() =
        interface IEvent with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Id: int = 0 with get, set
        member val IsValid: bool = false with get, set
        
    type ValidatedVehicleEvent() =
        interface IEvent with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Id: int = 0 with get, set
        member val IsValid: bool = false with get, set

    type ValidatedPolicyEvent() =
        interface IEvent with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set
        member val Id: int = 0 with get, set
        member val IsValid: bool = false with get, set

    type RatedPolicyEvent() =
        interface IEvent with
            member val CreatedDate = DateTimeOffset.Now with get, set
            member val ProcessedDate = None with get, set            
        member val Id: int = 0 with get, set
        member val Premium: decimal = 0m with get, set

module CommandHandlers =
    type ReservePolicyIdCommandHandler() =
        interface ICommandHandler<Policy> with
            member this.Handle state command =
                if state.Id < 1 then
                    command.ProcessedDate <- Some DateTimeOffset.Now
                    [ Events.ReservedPolicyIdEvent(Id = Random().Next(100, 1000)) ]
                else []
                
    type AddDriverCommandHandler() =
        interface ICommandHandler<Policy> with
            member this.Handle state command =
               match command with
                | :? Commands.AddDriverCommand as cmd ->                    
                    command.ProcessedDate <- Some DateTimeOffset.Now                      
                    [ Events.AddedDriverEvent(Driver = cmd.Driver) ]
                | _ -> []

module EventHandlers =
    type ReservedPolicyIdEventHandler() =
        interface IEventHandler<Policy> with
            member this.Handle state event =
                match event with
                | :? Events.ReservedPolicyIdEvent as evt ->                    
                    event.ProcessedDate <- Some DateTimeOffset.Now                      
                    state.Id <- evt.Id
                    state
                | _ -> state

    type AddedDriverEventHandler() =
        interface IEventHandler<Policy> with
            member this.Handle state event =
                match event with
                | :? Events.AddedDriverEvent as evt ->                    
                    event.ProcessedDate <- Some DateTimeOffset.Now                      
                    state.Drivers <- evt.Driver :: state.Drivers
                    state
                | _ -> state
