using Improbable.Core;
using Improbable.Life;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Assets.Gamelogic.Core
{

    [EngineType(EnginePlatform.FSim)]
    public class IndividualMovement : MonoBehaviour
    {
        [SerializeField] private Transform transform;
        private Vector3 movement = new Vector3();

        public Vector3 Movement
        { set { movement = value; } }

        private void Start()
        {
            transform = gameObject.GetComponent<Transform>();
        }
        private void OnEnable()
        {
        }
        private void OnDisable()
        {
        }
        private void FixedUpdate()
        {
            transform.position = transform.position + movement;
        }
    }
}