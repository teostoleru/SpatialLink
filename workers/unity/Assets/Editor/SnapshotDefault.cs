using Assets.Gamelogic.Core;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    public static class SnapshotDefault
    {
        public static void Build(SnapshotBuilder snapshot)
        {
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Editor/Resources/perlin.png");
            
            SnapshotUtil.AddSimulationIndividuals(snapshot);
            SnapshotUtil.AddSimulationCountries(snapshot);
            SnapshotUtil.AddSimulationManagerEntity(snapshot);
        }
    }
}
