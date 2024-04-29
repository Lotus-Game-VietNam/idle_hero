public class WorldData
{
    /// <summary>
    /// Đang ở level mấy (màn bao nhiêu)
    /// </summary>
    public int currentLevel;



    public readonly int firstGemsReward = 500;
    public readonly int pointReward = 1;


    public int lastGemsReward;




    public WorldData()
    {
        currentLevel = 1;
        lastGemsReward = 500;
    }


    public int GetGemsReward()
    {
        return currentLevel == 1 ? firstGemsReward : lastGemsReward + (650 * (currentLevel - 1));
    }


    public WorldData NextLevel()
    {
        lastGemsReward = GetGemsReward();
        currentLevel++;
        return this;
    }
}
