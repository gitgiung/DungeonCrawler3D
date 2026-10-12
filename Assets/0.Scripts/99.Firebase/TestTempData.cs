using UnityEngine;

[System.Serializable]
public class TestTempData
{
    public string uid;
    public string nickname;

    public int level;
    public int gold;
    public int dia;
    

    public TestTempData(string uid)
    {
        this.uid = uid;
        nickname = $"Guest_{Random.Range(0,100)}";

        level = 5;
        gold = 5000;
        dia = 100;
        
    }
}
