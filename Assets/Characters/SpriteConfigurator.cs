#if UNITY_EDITOR
    using UnityEditor;
    using Characters.Controllers;
#endif

using UnityEngine;

namespace Characters
{
    public class SpriteConfigurator : MonoBehaviour
    {
        #if UNITY_EDITOR
            [ContextMenu("Set Sprites")]
            private void SetSprites()
            {
                string dir = $"Assets/Characters/Models/{gameObject.name}/Graphics";
                foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
                {
                    string filename = renderer.gameObject.name == "Face" ? "Face 01" : renderer.gameObject.name;
                    
                    renderer.sprite = 
                        AssetDatabase
                            .LoadAssetAtPath<Sprite>(
                                $"{dir}/{filename}.png");
                }
            }
        #endif
    }
}
