namespace ServiceLocator.ServicesAbstraction
{
    public interface IRoundButton: IService
    {
        public void BlockButton(bool block);
    }
}