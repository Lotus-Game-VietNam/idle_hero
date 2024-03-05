namespace Lotus.CoreFramework
{
    public abstract class IAction
    {
        public IAction()
        {
            
        }


        protected abstract void SendMessage(params object[] args);


        public void Invoke(params object[] args)
        {
            SendMessage(args);
        }
    }
}


