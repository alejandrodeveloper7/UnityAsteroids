using UnityEngine;

public class PoolsController : MonoBehaviour
{
    #region Singleton

    private static PoolsController m_instance;
    public static PoolsController Instance => m_instance;

    private void GenerateSingleton()
    {
        if (m_instance != null)
            Destroy(this);
        else
            m_instance = this;
    }

    #endregion

    private void Awake()
    {
        GenerateSingleton();
    }

  
}
