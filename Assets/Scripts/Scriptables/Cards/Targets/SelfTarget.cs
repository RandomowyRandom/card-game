﻿using System;
using Mirror;
using Scriptables.Cards.Abstractions;

namespace Scriptables.Cards.Targets
{
    [Serializable]
    public class SelfTarget: ITargetProvider
    {
        public NetworkIdentity[] GetTargets()
        {
            throw new System.NotImplementedException();
        }
    }
}