using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Gamelogic.Core;
using Assets.Gamelogic.Utils;
using Improbable.Tree;
using Improbable.Unity;
using Improbable.Unity.Visualizer;
using Improbable.Math;
using Assets.Gamelogic.ComponentExtensions;

namespace Assets.Gamelogic.Country
{
    [EngineType(EnginePlatform.FSim)]
    public class PlayerControl : MonoBehaviour
    {
        private static Coordinates[] countries = new Coordinates[10];
        private static int L = 100;

        [Require]
        private TreeState.Writer treeState;
        [SerializeField]
        private GameObject player;
        [SerializeField]
        private Transform transform;

        private void Start()
        {
            transform = gameObject.GetComponent<Transform>();
        }

        private static void InitialiseCountries()
        {
            countries[0] = new Coordinates(0, 0, 0);
            countries[1] = new Coordinates(0, 0, L);
            countries[2] = new Coordinates(L, 0, 0);
            countries[3] = new Coordinates(L, 0, L);
            countries[4] = new Coordinates(L, 0, 2 * L);
            countries[5] = new Coordinates(2 * L, 0, L);
            countries[6] = new Coordinates(0, 0, 3 * L);
            countries[7] = new Coordinates(0, 0, 4 * L);
            countries[8] = new Coordinates(L, 0, 3 * L);
            countries[9] = new Coordinates(2 * L, 0, 3 * L);
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            player = other.gameObject;
            treeState.Send(new TreeState.Update().SetPopulation(treeState.Data.population + 1));
            Coordinates targetCoord = countries[UnityEngine.Random.Range(0, 9)];
            Vector3 targetVector = new Vector3((float)targetCoord.X, (float)targetCoord.Y, (float)targetCoord.Z);
            targetVector = (targetVector - player.transform.position).normalized;


            player.gameObject.GetComponent<IndividualMovement>().Movement = targetVector;
        }

        private void OnTriggerExit(Collider other)
        {
            if (player != null)
                player = null;

            if (treeState.Data.population > 0)
                treeState.Send(new TreeState.Update().SetPopulation(treeState.Data.population - 1));
        }
    }
}
