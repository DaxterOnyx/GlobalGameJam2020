public class PlayersManager : Location
{
    private static PlayersManager _instance;
    public static PlayersManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<PlayersManager>();
            return _instance;
        }
    }
    public PlayerManagerData data;
    void Start()
    {
        _instance = this;
        Initialize(data);
    }

}
