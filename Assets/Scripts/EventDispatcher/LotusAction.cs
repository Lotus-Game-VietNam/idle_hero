using System;


namespace Lotus.CoreFramework
{
    public class LotusAction : IAction
    {
        private Action m_Action = null;


        public LotusAction(Action action) : base()
        {
            m_Action = action;
        }


        protected override void SendMessage(params object[] args)
        {
            m_Action();
        }
    }


    public class LotusAction<T1> : IAction
    {
        private Action<T1> m_Action = null;


        public LotusAction(Action<T1> action) : base()
        {
            m_Action = action;
        }


        protected override void SendMessage(params object[] args)
        {
            m_Action((T1)args[0]);
        }
    }


    public class LotusAction<T1, T2> : IAction
    {
        private Action<T1, T2> m_Action = null;


        public LotusAction(Action<T1, T2> action) : base()
        {
            m_Action = action;
        }


        protected override void SendMessage(params object[] args)
        {
            m_Action((T1)args[0], (T2)args[1]);
        }
    }


    public class LotusAction<T1, T2, T3> : IAction
    {
        private Action<T1, T2, T3> m_Action = null;


        public LotusAction(Action<T1, T2, T3> action) : base()
        {
            m_Action = action;
        }


        protected override void SendMessage(params object[] args)
        {
            m_Action((T1)args[0], (T2)args[1], (T3)args[2]);
        }
    }


    public class LotusAction<T1, T2, T3, T4> : IAction
    {
        private Action<T1, T2, T3, T4> m_Action = null;


        public LotusAction(Action<T1, T2, T3, T4> action) : base()
        {
            m_Action = action;
        }


        protected override void SendMessage(params object[] args)
        {
            m_Action((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
        }
    }


    public class LotusAction<T1, T2, T3, T4, T5> : IAction
    {
        private Action<T1, T2, T3, T4, T5> m_Action = null;


        public LotusAction(Action<T1, T2, T3, T4, T5> action) : base()
        {
            m_Action = action;
        }


        protected override void SendMessage(params object[] args)
        {
            m_Action((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3], (T5)args[4]);
        }
    }
}

