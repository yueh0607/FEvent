using System.Buffers;
using System.Collections.Generic;

namespace FEvent.Sample
{
    //public static class IUpdatePublishExtensions
    //{
    //    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    //    public static void Send(this IUpdate obj, float deltaTime)
    //    {
    //        obj.Update(deltaTime);
    //    }

    //    public static void Send<T>(this IEventPublisher publisher, float deltaTime) where T : IUpdate
    //    {
    //        DynamicQueue<IEventListener> list = publisher.GetPublishableEvents<T>(typeof(T));
    //        if (list != null)
    //        {
    //            list.StartEnum();
    //            while (list.MoveNext(out IEventListener obj))
    //            {
    //                T eventObj = (T)obj;
    //                eventObj.Send(deltaTime);
    //                list.Return(obj);
    //            }
    //            list.EndEnum();
    //        }
    //    }
    //}

    //public static class IUpdate2PublishExtensions
    //{
    //    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    //    public static void Send(this IUpdate2 obj, int delt0aTime)
    //    {
    //        obj.Update2(delt0aTime,"");
    //    }



    //    public static void Send<T>(this IEventPublisher publisher, int deltaTime) where T : IUpdate2
    //    {
    //        DynamicQueue<IEventListener> list = publisher.GetPublishableEvents<T>(typeof(T));
    //        if (list != null)
    //        {
    //            list.StartEnum();
    //            while (list.MoveNext(out IEventListener obj))
    //            {
    //                T eventObj = (T)obj;
    //                eventObj.Send(deltaTime);
    //                list.Return(obj);
    //            }
    //            list.EndEnum();
    //        }
    //    }
    //}

    public static class IMyCallPublishExtensions
    {
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static string Call(this IMyCallEvent obj, int a)
        {
            return obj.OnCallEvent(a);
        }

        public static string Call<T>(this IEventPublisher publisher, int a) where T : IMyCallEvent
        {
            DynamicQueue<IEventListener> list = publisher.GetPublishableEvents<T>(typeof(T));
            string result = default;
            if (list != null && list.Count > 0)
            {
                list.StartEnum();
                while (list.MoveNext(out IEventListener obj))
                {
                    T eventObj = (T)obj;
                    result = eventObj.Call(a);
                    list.Return(obj);
                }
                list.EndEnum();
                return result;
            }
            return default;
        }

        public static string[] CallAll<T>(this IEventPublisher publisher, int a) where T : IMyCallEvent
        {
            DynamicQueue<IEventListener> list = publisher.GetPublishableEvents<T>(typeof(T));
            if (list != null && list.Count > 0)
            {
                string[] results = ArrayPool<string>.Shared.Rent(list.Count);
                int pos = 0;
                list.StartEnum();
                while (list.MoveNext(out IEventListener obj))
                {
                    T eventObj = (T)obj;
                    results[pos++] = eventObj.Call(a);
                    list.Return(obj);
                }
                list.EndEnum();
            }
            return new string[0];
        }
    }
}
