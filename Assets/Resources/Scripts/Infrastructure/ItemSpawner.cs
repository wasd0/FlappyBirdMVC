using System.Collections;
using System.Collections.Generic;
using Resources.Scripts.MonoBehaviours;
using Resources.Scripts.Presenters;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Resources.Scripts.Infrastructure
{
    public class ItemSpawner
    {
        private readonly SceneData _sceneData;
        private readonly ItemPresenterFactory _itemPresenterFactory;
        private readonly List<ItemPresenter> _items;
        private const int PREFABS_COUNT = 2;

        public bool AllSpawnedFlag { get; private set; }

        public ItemSpawner(SceneData sceneData)
        {
            _sceneData = sceneData;
            _items = new List<ItemPresenter>();
            _itemPresenterFactory =
                new ItemPresenterFactory(_sceneData.ItemEndPoint.position, _sceneData.ItemsFallMultiplier);
        }

        public IEnumerator Spawn()
        {
            int counter = 0;
            var spawnPoints = _sceneData.ItemSpawnPoints;
            while (counter < _sceneData.ItemsCount)
            {
                int pointIndex = Random.Range(0, spawnPoints.Length);
                Vector2 spawn = spawnPoints[pointIndex].position;
                counter++;
                SpawnRandom(spawn);
                yield return new WaitForSeconds(_sceneData.SpawnDelaySecs);
            }

            AllSpawnedFlag = true;
        }

        public IEnumerator Respawn()
        {
            yield return new WaitForSeconds(_sceneData.RespawnDelaySec);
            AllSpawnedFlag = false;
            var spawnPoints = _sceneData.ItemSpawnPoints;

            foreach (var i in _items)
            {
                yield return new WaitForSeconds(_sceneData.SpawnDelaySecs);
                int pointIndex = Random.Range(0, spawnPoints.Length);
                Vector2 spawn = spawnPoints[pointIndex].position;
                i.Reset(spawn, _sceneData.ItemEndPoint.position);
            }

            AllSpawnedFlag = true;
        }

        public void Update()
        {
            foreach (var item in _items)
            {
                item.Update();
            }
        }

        public void LateUpdate()
        {
            foreach (var item in _items)
            {
                item.LateUpdate();
            }
        }

        private void SpawnRandom(Vector2 spawn)
        {
            int prefabNumber = Random.Range(0, PREFABS_COUNT);
            var prefab = prefabNumber == 1 ? _sceneData.BonusPrefab : _sceneData.ObstaclePrefab;
            var item = Object.Instantiate(prefab, spawn, Quaternion.identity, null);
            _items.Add(_itemPresenterFactory.Create(item, spawn));
        }
    }
}
