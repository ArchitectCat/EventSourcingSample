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
        interface ICommand
        
    type AddDriverCommand() =
        interface ICommand
        member val Driver: Driver = Unchecked.defaultof<Driver> with get, set
        
    type AddVehicleCommand() =
        interface ICommand
        member val Vehicle: Vehicle = Unchecked.defaultof<Vehicle> with get, set

    type RemoveDriverCommand() =
        interface ICommand
        member val Id: int = 0 with get, set

    type RemoveVehicleCommand() =
        interface ICommand
        member val Id: int = 0 with get, set

    type ValidateDriverCommand() =
        interface ICommand
        member val Id: int = 0 with get, set

    type ValidateVehicleCommand() =
        interface ICommand
        member val Id: int = 0 with get, set

    type ValidatePolicyCommand() =
        interface ICommand
        member val Id: int = 0 with get, set

    type RatePolicyCommand() =
        interface ICommand
        member val Id: int = 0 with get, set

module Events =
    type PolicyCreatedEvent() =
        interface IEvent
        
    type PolicyLoadedEvent() =
        interface IEvent
        member val Id: int = 0 with get, set
        member val EndorsementNumber: int = 0 with get, set
        
    type ReservedPolicyIdEvent() =
        interface IEvent
        member val Id: int = 0 with get, set
        
    type AddedDriverEvent() =
        interface IEvent
        member val Driver: Driver = Unchecked.defaultof<Driver> with get, set
        
    type AddedVehicleEvent() =
        interface IEvent
        member val Vehicle: Vehicle = Unchecked.defaultof<Vehicle> with get, set

    type RemovedDriverEvent() =
        interface IEvent
        member val Id: int = 0 with get, set

    type RemovedVehicleEvent() =
        interface IEvent
        member val Id: int = 0 with get, set

    type ValidatedDriverEvent() =
        interface IEvent
        member val Id: int = 0 with get, set
        member val IsValid: bool = false with get, set
        
    type ValidatedVehicleEvent() =
        interface IEvent
        member val Id: int = 0 with get, set
        member val IsValid: bool = false with get, set

    type ValidatedPolicyEvent() =
        interface IEvent
        member val Id: int = 0 with get, set
        member val IsValid: bool = false with get, set

    type RatedPolicyEvent() =
        interface IEvent
        member val Id: int = 0 with get, set
        member val Premium: decimal = 0m with get, set
