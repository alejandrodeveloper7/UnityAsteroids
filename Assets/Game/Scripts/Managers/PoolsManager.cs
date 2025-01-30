using UnityEngine;

public class PoolsManager : MonoBehaviour
{
    private static PoolsManager m_instance;
    public static PoolsManager Instance => m_instance;
      

    private void Awake()
    {
        GenerateSingleton();
    }

    private void GenerateSingleton()
    {
        if (m_instance != null)
            Destroy(this);
        else
            m_instance = this;
    }

}
