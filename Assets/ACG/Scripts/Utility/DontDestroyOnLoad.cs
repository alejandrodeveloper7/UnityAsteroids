using UnityEngine;

namespace ACG.Scripts.Utilitys
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Awake() => DontDestroyOnLoad(gameObject);
    }
}