using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager _instance;
    static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject() { name = @"GameManager" };
                _instance = go.AddComponent<GameManager>();
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }
    RenjuRuleManager _rule = new RenjuRuleManager();
    public static RenjuRuleManager Rule { get { return Instance._rule; } }
    NetworkManager _network = new NetworkManager();
    public static NetworkManager Network { get { return Instance._network; } }

}
