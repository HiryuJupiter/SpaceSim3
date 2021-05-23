using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HiryuTK.EnvironemntalSpawner.Spawner
{
    /// <summary>
    /// For spawning objects in 3D space
    /// </summary>
    public class TopDown_ZoneSpawner : MonoBehaviour
    {
        //Enum for setting the spawn mode
        public enum SpawnMode { XYZ, XY, XZ }

        #region Fields
        [Header("Spawn area")]
        [SerializeField] private SpawnMode spawnMode = SpawnMode.XYZ;
        [SerializeField] private Vector3 size = Vector3.one;

        [Header("Spawn item")]
        [SerializeField] private GameObject prefab = null;
        [SerializeField] private int spawnCount = 100;

        [Header("Spawn interval")]
        [SerializeField, Range(0.01f, 10f)] private float spawnCDMin = 0f;
        [SerializeField, Range(0.02f, 5f)]  private float spawnCDMax = 5f;

        //Status
        private float spawnTimer = 0;

        //Cache
        private float extentX;
        private float extentY;
        private float extentZ;
        #endregion

        #region Mono
        private void Awake()
        {
            //Cache the outer bounds of the spawn zone
            extentX = size.x * transform.localScale.x * .5f;
            extentY = size.y * transform.localScale.y * .5f;
            extentZ = size.z * transform.localScale.z * .5f;
        }

        private void Start()
        {
            Spawn();
        }

#if UNITY_EDITOR
        /// <summary>
        /// Ensures the min number is always smaller than the max number.
        /// </summary>
        private void OnValidate()
        {
            spawnCDMin = Mathf.Clamp(spawnCDMin, 0f, spawnCDMax - 0.01f);
            spawnCDMax = Mathf.Clamp(spawnCDMax, spawnCDMin + 0.01f, float.MaxValue);
        }

        /// <summary>
        /// Visualizes the spawn locations.
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            //Set the gizmo's matrix to the same value as this object's transform, give it a transparent green color
            Gizmos.matrix = Matrix4x4.TRS(transform.localPosition, transform.localRotation, transform.localScale);
            Gizmos.color = new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(Vector3.zero, new Vector3(
                size.x,
                spawnMode == SpawnMode.XZ ? 0f : size.y,
                spawnMode == SpawnMode.XY ? 0f : size.z));
        }
#endif
        #endregion

        private void Spawn()
        {
            StartCoroutine(DoSpawn());
        }

        /// <summary>
        /// Spawn objects with a tiny bit of delay in between.
        /// </summary>
        private IEnumerator DoSpawn()
        {
            for (int i = 0; i < spawnCount; i++)
            {
                //Wait for a random duration between these two numbers
                yield return new WaitForSeconds(Random.Range(spawnCDMin, spawnCDMax));

                //Instantiate the prefab at the spawn location, with the same rotation as this object's transform
                Instantiate(prefab, GetSpawnPosition(), transform.rotation);
            }
        }

        /// <summary>
        /// Returns a random spawn point based on the spawn parameters.
        /// </summary>
        private Vector3 GetSpawnPosition()
        {
            float x = Random.Range(-extentX, extentX);
            float y = spawnMode == SpawnMode.XZ ? 0f : Random.Range(-extentY, extentY);
            float z = spawnMode == SpawnMode.XY ? 0f : Random.Range(-extentZ, extentZ);
            return transform.TransformPoint(new Vector3(x, y, z)); //Make it relative
        }
    }
}