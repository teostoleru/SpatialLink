using Assets.Gamelogic.Core;
using Assets.Gamelogic.EntityTemplate;
using Assets.Gamelogic.Utils;
using Improbable.Math;
using Improbable.Worker;
using UnityEngine;
using System.Collections;

namespace Assets.Editor
{
    public static class SnapshotUtil
    {
        private static readonly System.Random rand = new System.Random();
        private static Coordinates[] countries = new Coordinates[10];
        private static int L = 100;

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
            countries[9] =  new Coordinates(2 * L, 0, 3 * L);
        }
        public static void AddSimulationManagerEntity(SnapshotBuilder snapshot)
        {
            var entity = EntityTemplateFactory.CreateSimulationManagerTemplate();
            snapshot.Add(snapshot.GenerateId(), entity);
        }

        public static void AddSimulationIndividuals(SnapshotBuilder snapshot) 
        {
            InitialiseCountries();
            for (int i = 0; i < 2500; i++)
            {
                Coordinates coord = countries[Random.Range(0, 9)];
                AddIndividual(snapshot, coord);
            }    
        }


        public static void AddIndividual(SnapshotBuilder snapshot, Coordinates coordinates)
        {
            var entityId = snapshot.GenerateId();
            var entity = EntityTemplateFactory.CreateIndividualTemplate(coordinates);
            snapshot.Add(entityId, entity);
        }

        public static void AddSimulationCountries(SnapshotBuilder snapshot)
        {
            AddCountry(snapshot, new Coordinates(0, 0, 0));
            AddCountry(snapshot, new Coordinates(0, 0, L));
            AddCountry(snapshot, new Coordinates(L, 0, 0));
            AddCountry(snapshot, new Coordinates(L, 0, L));
            AddCountry(snapshot, new Coordinates(L, 0, 2 * L));
            AddCountry(snapshot, new Coordinates(2 * L, 0, L));
            AddCountry(snapshot, new Coordinates(0, 0, 3 * L));
            AddCountry(snapshot, new Coordinates(0, 0, 4 * L));
            AddCountry(snapshot, new Coordinates(L, 0, 3 * L));
            AddCountry(snapshot, new Coordinates(2 * L, 0, 3 * L));
        }

        public static void AddCountry(SnapshotBuilder snapshot, Coordinates coordinates)
        {
            var entityId = snapshot.GenerateId();
            var entity = EntityTemplateFactory.CreateCountryTemplate(coordinates);
            snapshot.Add(entityId, entity);
        }

        public static void AddTrees(SnapshotBuilder snapshot, Texture2D sampler, float sampleThreshold, int countAproximate, double edgeLength, float placementJitter)
        {
            var treeCountSqrt = Mathf.CeilToInt(Mathf.Sqrt(countAproximate));
            var spawnGridIntervals = edgeLength / treeCountSqrt;

            for (var z = 0; z < treeCountSqrt; z++)
            {
                var zProportion = z / (float)treeCountSqrt;

                for (var x = 0; x < treeCountSqrt; x++)
                {
                    var xProportion = x / (float)treeCountSqrt;
                    var xPixel = (int) (xProportion * sampler.width);
                    var zPixel = (int) (zProportion * sampler.height);
                    var sample = sampler.GetPixel(xPixel, zPixel).maxColorComponent;

                    if (sample > sampleThreshold && Random.value < sample)
                    {
                        var xJitter = Random.Range(-placementJitter, placementJitter);
                        var zJitter = Random.Range(-placementJitter, placementJitter);
                        Vector3d positionJitter = new Vector3d(xJitter, 0d, zJitter);

                        Coordinates worldRoot = new Coordinates(-edgeLength/2, 0, -edgeLength/2);
                        Vector3d offsetFromWorldRoot = new Vector3d(x, 0d, z) * spawnGridIntervals;
                        Coordinates spawnPosition = worldRoot + offsetFromWorldRoot + positionJitter;
                        AddTree(snapshot, spawnPosition);
                    }
                }
            }
        }

        public static void AddTree(SnapshotBuilder snapshot, Coordinates position)
        {
            var treeEntityId = snapshot.GenerateId();
            var spawnRotation = (uint)Mathf.CeilToInt((float)rand.NextDouble() * 360.0f);
            var entity = EntityTemplateFactory.CreateCountryTemplate(position);
            snapshot.Add(treeEntityId, entity);
        }

        public static void AddHQs(SnapshotBuilder snapshot, Coordinates[] locations)
        {
            for (uint teamId = 0; teamId < locations.Length; teamId++)
            {
                var position = locations[teamId];
                var entity = EntityTemplateFactory.CreateHQTemplate(position, 0, teamId);
                snapshot.Add(snapshot.GenerateId(), entity);
            }
        }

        public static void AddNPCsAroundHQs(SnapshotBuilder snapshot, Coordinates[] locations)
        {
            for (uint teamId = 0; teamId < locations.Length; teamId++)
            {
                SpawnNpcsAroundPosition(snapshot, locations[teamId], teamId);
            }
        }

        public static void SpawnNpcsAroundPosition(SnapshotBuilder snapshot, Coordinates position, uint team)
        {
            float totalNpcs = SimulationSettings.HQStartingWizardsCount + SimulationSettings.HQStartingLumberjacksCount;
            float radiusFromHQ = SimulationSettings.NPCSpawnDistanceToHQ;

            for (int i = 0; i < totalNpcs; i++)
            {
                float radians = (i / totalNpcs) * 2 * Mathf.PI;
                Vector3 offset = new Vector3(Mathf.Cos(radians), 0, Mathf.Sin(radians));
                offset *= radiusFromHQ;
                Coordinates coordinates = (position.ToVector3() + offset).ToCoordinates();

                SnapshotEntity entity = null;
                if (i < SimulationSettings.HQStartingLumberjacksCount)
                {
                    entity = EntityTemplateFactory.CreateNPCLumberjackTemplate(coordinates, team);
                }
                else
                {
                    entity = EntityTemplateFactory.CreateNPCWizardTemplate(coordinates, team);
                }

                var id = snapshot.GenerateId();
                snapshot.Add(id, entity);
            }
        }
    }
}