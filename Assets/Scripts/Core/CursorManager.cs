using UnityEngine;

namespace HeroSurvivor.Core
{
    public class CursorManager : MonoBehaviour
    {
        [SerializeField] private Texture2D _defaultCursor;
        [SerializeField] private Texture2D _combatCursor;

        private Vector2 defaultHotSpot = Vector2.zero;
        private Vector2 combatHotSpot = new Vector2(30,30);

        public void SetDefaultCursor() 
        {
            Cursor.SetCursor(_defaultCursor, defaultHotSpot, CursorMode.Auto);
        }

        public void SetCombatCursor()
        {
            Cursor.SetCursor(_combatCursor, combatHotSpot, CursorMode.Auto);
        }
    }
}
