
using UnityEngine;

namespace LPUnityUtils
{
    static class MaterialUtils
    {
        public static void SubstituteMaterial(MeshRenderer renderer, string nameContains, Material material)
        {
            Material[] materials = renderer.materials;
            for ( int i = 0; i < materials.Length; ++i )
            {
                if ( materials[i].name.Contains(nameContains) )
                {
                    materials[i] = material;
                }
            }
            renderer.materials = materials;
        }
    }

}  // namespace LPUnityUtils