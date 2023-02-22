using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public interface IModifierProvider
    {
        IEnumerable<float> GetAdditiveModifers(Stat stat);
        IEnumerable<float> GetPercentageModifers(Stat stat);
    }
}