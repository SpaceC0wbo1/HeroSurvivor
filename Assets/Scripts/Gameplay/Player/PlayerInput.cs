using UnityEngine;
using HeroSurvivor.Gameplay.Intent;

namespace HeroSurvivor.Gameplay.Player
{
    public class PlayerInput : IntentSource
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float _aimPlaneY = 1f;

        private Vector3 _cameraForward;
        private Vector3 _cameraRight;

        public override Vector2 Direction
        {
            get
            {
                float x = Input.GetAxisRaw("Horizontal");
                float y = Input.GetAxisRaw("Vertical");

                Vector2 input = new Vector2(x, y);

                if (input.sqrMagnitude > 1f)
                    input.Normalize();

                Vector3 world = _cameraRight * input.x + _cameraForward * input.y;

                return new Vector2(world.x, world.z);
            }
        }

        public override Vector3 AimPoint
        {
            get
            {
                Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
                Plane plane = new Plane(Vector3.up, new Vector3(0f, _aimPlaneY, 0f));

                if (plane.Raycast(ray, out float distance) == false)
                    return transform.position;

                return ray.GetPoint(distance);
            }
        }

        public override bool IsShooting => Input.GetMouseButton(0);

        private void Awake()
        {
            _cameraForward = _camera.transform.forward;
            _cameraForward.y = 0f;
            _cameraForward.Normalize();

            _cameraRight = _camera.transform.right;
            _cameraRight.y = 0f;
            _cameraRight.Normalize();
        }

    }
}
