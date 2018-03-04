using System;
using UnityEngine;
using UniRx;

namespace RedChild
{
    public enum RcList
    {
        solar = 0,
        thundershock,
        megaflare2,
        fantasticExplosion,
        ultima2,
        ritual,
        twister,
        atomic,
        none
    }

    public class RcListReactiveProperty : ReactiveProperty<RcList>
    {
        public RcListReactiveProperty()
        {
        }

        public RcListReactiveProperty(RcList initialValue) : base(initialValue)
        {
        }
    }
}
