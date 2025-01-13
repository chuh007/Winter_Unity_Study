using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Entities.FSM
{
    [CreateAssetMenu(fileName = "StateListSO", menuName = "SO/FSM/StateList", order = 0) ]
    public class StateListSO : ScriptableObject
    {
        public List<StateSO> states;
    }
}

