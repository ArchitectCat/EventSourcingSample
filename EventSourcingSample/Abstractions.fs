namespace EventSourcingSample

open System

[<Interface>]
type ICommand =
    abstract member CreatedDate : DateTimeOffset with get, set
    abstract member ProcessedDate : DateTimeOffset option with get, set
   
[<Interface>] 
type IEvent =
    abstract member CreatedDate : DateTimeOffset with get, set
    abstract member ProcessedDate : DateTimeOffset option with get, set

[<Interface>]
type ICommandHandler<'TState> =
    abstract member Handle : state: 'TState -> command: ICommand -> IEvent list
        
[<Interface>]
type IEventHandler<'TState> =
    abstract member Handle : state: 'TState -> event: IEvent -> 'TState

[<AbstractClass>]
type Document<'TKey, 'TState>() =
    member val Id: 'TKey = Unchecked.defaultof<'TKey> with get, set
    member val Commands: ICommand list = [] with get, set
    member val Events: IEvent list = [] with get, set
    member val State: 'TState = Unchecked.defaultof<'TState> with get, set
