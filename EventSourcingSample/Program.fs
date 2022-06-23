namespace EventSourcingSample

open System

module Program =
    
    let getCommandHandlers() =
        dict<Type, obj> [
            (typeof<Commands.ReservePolicyIdCommand>, CommandHandlers.ReservePolicyIdCommandHandler())
            (typeof<Commands.AddDriverCommand>, CommandHandlers.AddDriverCommandHandler())
        ]
    
    let getEventHandlers() =
        dict<Type, obj> [
            (typeof<Events.ReservedPolicyIdEvent>, EventHandlers.ReservedPolicyIdEventHandler())
            (typeof<Events.AddedDriverEvent>, EventHandlers.AddedDriverEventHandler())
        ]
        
    let addCommand (command: ICommand) (doc: Document<_, _>) =
        doc.Commands <- (command :: doc.Commands)
        doc
        
    let addEvent (event: IEvent) (doc: Document<_, _>) =
        doc.Events <- (event :: doc.Events)
        doc
    
    let processCommands (doc: Document<_, _>) =
        let mutable events = doc.Events
        doc.Commands
        |> List.iter (fun cmd ->
                match cmd.ProcessedDate with
                | Some _ -> ()
                | None ->
                    let cmdType = cmd.GetType()
                    match getCommandHandlers().TryGetValue cmdType with
                    | true, value ->
                        let handler = value :?> ICommandHandler<_>
                        events <- events @ handler.Handle doc.State cmd
                    | _ -> ()
            )
        events
    
    let processEvents (doc: Document<_, _>) =
        let mutable state = doc.State
        doc.Events
        |> List.iter (fun evt ->
                match evt.ProcessedDate with
                | Some _ -> ()
                | None ->
                    let evtType = evt.GetType()
                    match getEventHandlers().TryGetValue evtType with
                    | true, value ->
                        let handler = value :?> IEventHandler<_>
                        state <- handler.Handle doc.State evt
                    | _ -> ()
            )
        state
        
    let createPolicyDocument() =
        let doc = PolicyDocument()
        doc.Id <- Guid.NewGuid()
        doc.State <- Policy(Id = 0, EndorsementNumber = 0)
        doc
        |> addEvent (Events.PolicyCreatedEvent())
        |> addCommand (Commands.ReservePolicyIdCommand())
    
    let loadPolicyDocument policyId endorsementNumber =
        let doc = PolicyDocument()
        doc.Id <- Guid.NewGuid()
        doc.State <- Policy(Id = policyId, EndorsementNumber =  endorsementNumber)
        doc
        |> addEvent (Events.PolicyLoadedEvent(Id = policyId, EndorsementNumber = endorsementNumber))
        
    let printDocument title (doc: Document<_, Policy>) =
        printfn "-------------------------------------------"
        printfn "%s" title
        printfn $"Document {doc.Id}"
        doc.Commands |> List.iter (fun x -> printfn $"Command: {x.GetType()} Created: {x.CreatedDate} Processed: {x.ProcessedDate}")
        doc.Events |> List.iter (fun x -> printfn $"Event: {x.GetType()} Created: {x.CreatedDate} Processed: {x.ProcessedDate}")
        printfn $"Policy: #{doc.State.Id} - {doc.State.EndorsementNumber}"
        doc.State.Drivers |> List.iter (fun x -> printfn $"Driver: {x.Id} {x.Name}")
        doc.State.Vehicles |> List.iter (fun x -> printfn $"Vehicle: {x.Id} {x.Vin}")
        printfn "-------------------------------------------"
        
    [<EntryPoint>]
    let main args =
        let doc =
            createPolicyDocument()
            |> addCommand (Commands.AddDriverCommand(Driver = { Id = 1; Name = "Driver 1" }))
            |> addCommand (Commands.AddVehicleCommand(Vehicle = { Id = 1; Vin = "Vehicle 1" }))
        
        printDocument "Initial State" doc
        
        // Commands generate more events
        doc.Events <- processCommands doc
        printDocument "Commands Processed Try 1" doc
        
        doc.Events <- processCommands doc
        printDocument "Commands Processed Try 2" doc
        
        // Events update the current state
        doc.State <- processEvents doc
        printDocument "Events Processed Try 1" doc
        
        doc.State <- processEvents doc
        printDocument "Events Processed Try 2" doc
        
        0 // Return success code



    