namespace EventSourcingSample

open System

module Program =    
    let addCommand (command: ICommand) (doc: Document<_, _>) =
        doc.Commands <- (command :: doc.Commands)
        doc
        
    let addEvent (event: IEvent) (doc: Document<_, _>) =
        doc.Events <- (event :: doc.Events)
        doc
        
    let createPolicyDocument() =
        let doc = PolicyDocument()
        doc.Id <- Guid.NewGuid()
        doc.State <- Policy(Id = 0, EndorsementNumber = 0)
        doc
        |> addEvent (Events.PolicyCreatedEvent())
    
    let loadPolicyDocument policyId endorsementNumber =
        let doc = PolicyDocument()
        doc.Id <- Guid.NewGuid()
        doc.State <- Policy(Id = policyId, EndorsementNumber =  endorsementNumber)
        doc
        |> addEvent (Events.PolicyLoadedEvent(Id = policyId, EndorsementNumber = endorsementNumber))
        
    [<EntryPoint>]
    let main args =
        let doc = createPolicyDocument()
        
        0 // Return success code



    