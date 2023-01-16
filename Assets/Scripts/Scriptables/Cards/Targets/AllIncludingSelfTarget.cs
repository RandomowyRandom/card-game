using System;
using Mirror;
using Scriptables.Cards.Abstractions;

namespace Scriptables.Cards.Targets
{
    [Serializable]
    public class AllIncludingSelfTarget: ITargetProvider
    {
        public NetworkIdentity[] GetTargets()
        {
            throw new System.NotImplementedException();
        }
    }
}