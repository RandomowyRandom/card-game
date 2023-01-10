using System;

namespace ServiceLocator
{
    public interface IService
    {
        public Type GetServiceType()
        {
            return GetType();
        }
    }
}