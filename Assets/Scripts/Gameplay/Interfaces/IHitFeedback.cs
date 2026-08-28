using System;
using UnityEngine;

namespace HeroSurvivor.Gameplay.Interfaces
{
    public interface IHitFeedback
    {
        event Action<Vector3> OnHit;
    }
}
