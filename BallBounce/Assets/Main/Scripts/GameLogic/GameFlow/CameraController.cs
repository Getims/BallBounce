using Sirenix.OdinInspector;
using UnityEngine;

namespace Main.Scripts.GameLogic.GameFlow
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField]
        private Camera _camera;

        private GameZone _gameZone;
        private Transform _spawnPoint;

        public void Initialize(GameZone gameZone, Transform spawnPoint)
        {
            _gameZone = gameZone;
            _spawnPoint = spawnPoint;

            AdjustCamera();
            MoveSpawnPointToTop();
        }

        [Button]
        void AdjustCamera()
        {
            float levelWidth = _gameZone.Width;
            float levelHeight = _gameZone.Height;

            float newSizeWidth = levelWidth / _camera.aspect / 2;
            float newSizeHeight = levelHeight / 2;

            _camera.orthographicSize = Mathf.Max(newSizeWidth, newSizeHeight);
        }

        [Button]
        void MoveSpawnPointToTop()
        {
            float cameraTop = _camera.transform.position.y + _camera.orthographicSize;
            Vector3 spawnPointPosition = _spawnPoint.position;
            spawnPointPosition.y = cameraTop;
            _spawnPoint.position = spawnPointPosition;
        }
    }
}