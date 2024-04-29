using System;
using System.Collections.Generic;


namespace Lotus.CoreFramework
{
    public class EventDispatcher
    {
        private static Dictionary<string, Dictionary<string, IAction>> m_Events = new Dictionary<string, Dictionary<string, IAction>>();


        #region AddListener
        public static void Addlistener(string eventName, string subscribers, Action action)
        {
            if (!m_Events.ContainsKey(eventName))
                m_Events.Add(eventName, new Dictionary<string, IAction>());

            if (m_Events[eventName].ContainsKey(subscribers))
                m_Events[eventName][subscribers] = new LotusAction(action);
            else
                m_Events[eventName].Add(subscribers, new LotusAction(action));
        }

        public static void Addlistener<T1>(string eventName, string subscribers, Action<T1> action)
        {
            if (!m_Events.ContainsKey(eventName))
                m_Events.Add(eventName, new Dictionary<string, IAction>());

            if (m_Events[eventName].ContainsKey(subscribers))
                m_Events[eventName][subscribers] = new LotusAction<T1>(action);
            else
                m_Events[eventName].Add(subscribers, new LotusAction<T1>(action));
        }

        public static void Addlistener<T1, T2>(string eventName, string subscribers, Action<T1, T2> action)
        {
            if (!m_Events.ContainsKey(eventName))
                m_Events.Add(eventName, new Dictionary<string, IAction>());

            if (m_Events[eventName].ContainsKey(subscribers))
                m_Events[eventName][subscribers] = new LotusAction<T1, T2>(action);
            else
                m_Events[eventName].Add(subscribers, new LotusAction<T1, T2>(action));
        }

        public static void Addlistener<T1, T2, T3>(string eventName, string subscribers, Action<T1, T2, T3> action)
        {
            if (!m_Events.ContainsKey(eventName))
                m_Events.Add(eventName, new Dictionary<string, IAction>());

            if (m_Events[eventName].ContainsKey(subscribers))
                m_Events[eventName][subscribers] = new LotusAction<T1, T2, T3>(action);
            else
                m_Events[eventName].Add(subscribers, new LotusAction<T1, T2, T3>(action));
        }

        public static void Addlistener<T1, T2, T3, T4>(string eventName, string subscribers, Action<T1, T2, T3, T4> action)
        {
            if (!m_Events.ContainsKey(eventName))
                m_Events.Add(eventName, new Dictionary<string, IAction>());

            if (m_Events[eventName].ContainsKey(subscribers))
                m_Events[eventName][subscribers] = new LotusAction<T1, T2, T3, T4>(action);
            else
                m_Events[eventName].Add(subscribers, new LotusAction<T1, T2, T3, T4>(action));
        }

        public static void Addlistener<T1, T2, T3, T4, T5>(string eventName, string subscribers, Action<T1, T2, T3, T4, T5> action)
        {
            if (!m_Events.ContainsKey(eventName))
                m_Events.Add(eventName, new Dictionary<string, IAction>());

            if (m_Events[eventName].ContainsKey(subscribers))
                m_Events[eventName][subscribers] = new LotusAction<T1, T2, T3, T4, T5>(action);
            else
                m_Events[eventName].Add(subscribers, new LotusAction<T1, T2, T3, T4, T5>(action));
        }

        #endregion


        #region SendMessage
        public static void SendMessage(string eventName, string subscribers)
        {
            if (m_Events.ContainsKey(eventName))
            {
                if (m_Events[eventName].ContainsKey(subscribers))
                    m_Events[eventName][subscribers].Invoke();
                else
                    LogTool.LogErrorEditorOnly($"{subscribers} chưa đăng ký eventName: {eventName}");
                return;
            }
            LogTool.LogErrorEditorOnly($"Không tồn tại eventName: {eventName}");
        }

        public static void SendMessage(string eventName)
        {
            if (m_Events.ContainsKey(eventName))
            {
                foreach (var subscriber in m_Events[eventName])
                    subscriber.Value.Invoke();
                return;
            }
            LogTool.LogErrorEditorOnly($"Không tồn tại eventName: {eventName}");
        }

        public static void SendMessage<T1>(string eventName, string subscribers, T1 args1)
        {
            if (m_Events.ContainsKey(eventName))
            {
                if (m_Events[eventName].ContainsKey(subscribers))
                    m_Events[eventName][subscribers].Invoke(args1);
                else
                    LogTool.LogErrorEditorOnly($"{subscribers} chưa đăng ký eventName: {eventName}");
                return;
            }
            LogTool.LogErrorEditorOnly($"Không tồn tại eventName: {eventName}");
        }

        public static void SendMessage<T1>(string eventName, T1 args1)
        {
            if (m_Events.ContainsKey(eventName))
            {
                foreach (var subscriber in m_Events[eventName])
                    subscriber.Value.Invoke(args1);
                return;
            }
            LogTool.LogErrorEditorOnly($"Không tồn tại eventName: {eventName}");
        }

        public static void SendMessage<T1, T2>(string eventName, string subscribers, T1 args1, T2 args2)
        {
            if (m_Events.ContainsKey(eventName))
            {
                if (m_Events[eventName].ContainsKey(subscribers))
                    m_Events[eventName][subscribers].Invoke(args1, args2);
                else
                    LogTool.LogErrorEditorOnly($"{subscribers} chưa đăng ký eventName: {eventName}");
                return;
            }
            LogTool.LogErrorEditorOnly($"Không tồn tại eventName: {eventName}");
        }

        public static void SendMessage<T1, T2>(string eventName, T1 args1, T2 args2)
        {
            if (m_Events.ContainsKey(eventName))
            {
                foreach (var subscriber in m_Events[eventName])
                    subscriber.Value.Invoke(args1, args2);
                return;
            }
            LogTool.LogErrorEditorOnly($"Không tồn tại eventName: {eventName}");
        }

        public static void SendMessage<T1, T2, T3>(string eventName, string subscribers, T1 args1, T2 args2, T3 args3)
        {
            if (m_Events.ContainsKey(eventName))
            {
                if (m_Events[eventName].ContainsKey(subscribers))
                    m_Events[eventName][subscribers].Invoke(args1, args2, args3);
                else
                    LogTool.LogErrorEditorOnly($"{subscribers} chưa đăng ký eventName: {eventName}");
                return;
            }
            LogTool.LogErrorEditorOnly($"Không tồn tại eventName: {eventName}");
        }

        public static void SendMessage<T1, T2, T3>(string eventName, T1 args1, T2 args2, T3 args3)
        {
            if (m_Events.ContainsKey(eventName))
            {
                foreach (var subscriber in m_Events[eventName])
                    subscriber.Value.Invoke(args1, args2, args3);
                return;
            }
            LogTool.LogErrorEditorOnly($"Không tồn tại eventName: {eventName}");
        }

        public static void SendMessage<T1, T2, T3, T4>(string eventName, string subscribers, T1 args1, T2 args2, T3 args3, T4 args4)
        {
            if (m_Events.ContainsKey(eventName))
            {
                if (m_Events[eventName].ContainsKey(subscribers))
                    m_Events[eventName][subscribers].Invoke(args1, args2, args3, args4);
                else
                    LogTool.LogErrorEditorOnly($"{subscribers} chưa đăng ký eventName: {eventName}");
                return;
            }
            LogTool.LogErrorEditorOnly($"Không tồn tại eventName: {eventName}");
        }

        public static void SendMessage<T1, T2, T3, T4>(string eventName, T1 args1, T2 args2, T3 args3, T4 args4)
        {
            if (m_Events.ContainsKey(eventName))
            {
                foreach (var subscriber in m_Events[eventName])
                    subscriber.Value.Invoke(args1, args2, args3, args4);
                return;
            }
            LogTool.LogErrorEditorOnly($"Không tồn tại eventName: {eventName}");
        }

        public static void SendMessage<T1, T2, T3, T4, T5>(string eventName, string subscribers, T1 args1, T2 args2, T3 args3, T4 args4, T5 args5)
        {
            if (m_Events.ContainsKey(eventName))
            {
                if (m_Events[eventName].ContainsKey(subscribers))
                    m_Events[eventName][subscribers].Invoke(args1, args2, args3, args4, args5);
                else
                    LogTool.LogErrorEditorOnly($"{subscribers} chưa đăng ký eventName: {eventName}");
                return;
            }
            LogTool.LogErrorEditorOnly($"Không tồn tại eventName: {eventName}");
        }

        public static void SendMessage<T1, T2, T3, T4, T5>(string eventName, T1 args1, T2 args2, T3 args3, T4 args4, T5 args5)
        {
            if (m_Events.ContainsKey(eventName))
            {
                foreach (var subscriber in m_Events[eventName])
                    subscriber.Value.Invoke(args1, args2, args3, args4, args5);
                return;
            }
            LogTool.LogErrorEditorOnly($"Không tồn tại eventName: {eventName}");
        }
        #endregion


        #region RemoveListener
        public static void RemoveListener(string eventName, string subscribers)
        {
            if (m_Events.ContainsKey(eventName) && m_Events[eventName].ContainsKey(subscribers))
                m_Events[eventName].Remove(subscribers);
        }

        public static void RemoveAllListener(string eventName)
        {
            if (m_Events.ContainsKey(eventName))
                m_Events.Remove(eventName);
        }

        public static void RemoveSubscribers(string subscribers)
        {
            foreach (var eventName in m_Events)
            {
                foreach (var subs in eventName.Value)
                {
                    if (subs.Key.Equals(subscribers))
                        m_Events[eventName.Key].Remove(subs.Key);
                }
            }
        }
        #endregion
    }
}

