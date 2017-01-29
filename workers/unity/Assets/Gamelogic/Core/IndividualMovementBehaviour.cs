using Improbable.Core;
using Improbable.Life;
using Improbable.Unity;
using Improbable.Unity.Core;
using Improbable.Unity.Visualizer;
using UnityEngine;

namespace Assets.Gamelogic.Core
{

    [EngineType(EnginePlatform.FSim)]
    public class IndividualMovementBehaviour : MonoBehaviour
    {
        private Transform transform;
        private void Start()
        {
            transform = GetComponent<Transform>();
        }
        private void OnEnable()
        { }
        private void OnDisable()
        { }
        private void FixedUpdate()
        {
            Vector3 movement = new Vector3(1, 0, 1);
            transform.position = transform.position + movement;
        }
    }
}