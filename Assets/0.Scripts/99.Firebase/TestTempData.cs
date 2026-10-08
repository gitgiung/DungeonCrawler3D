using UnityEngine;

public class TestTempData
{
    public string uid;
    public int level;
    public int gold;

    public TestTempData(string uid)
    {
        this.uid = uid;

        level = 1;
        gold = 0;
    }
}
