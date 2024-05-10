namespace FEvent.Internal
{
    public interface IEventBase : IEventListener { }

    public interface IGenericEventBase : IEventBase { }

    public interface ISendEventBase : IEventBase { }

    public interface ICallEventBase :IEventBase { }

}
