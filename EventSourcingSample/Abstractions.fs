namespace EventSourcingSample

type ICommand = interface end
type IEvent = interface end

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
