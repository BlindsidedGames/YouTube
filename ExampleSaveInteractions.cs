using UnityEngine;
using static BlindsidedGames.SaveSystem;

public class ExampleSaveInteractions : MonoBehaviour
{
    private void Start()
    {
        InvokeRepeating(nameof(ExampleMethod1), 1, 1); // Run example every 1 second.
        InvokeRepeating(nameof(ExampleMethod2), 1, 1);
    }

    #region Example1

    private void ExampleMethod1()
    {
        ss.saveData.exampleInteger++;
        ss.saveData.exampleSubClass.testInteger++;
    }

    #endregion

    #region Example2

    private SaveData ExampleSaveData => ss.saveData;
    private ExampleSubClass ExampleSubClass => ss.saveData.exampleSubClass;

    private void ExampleMethod2()
    {
        ExampleSaveData.exampleInteger++;
        ExampleSubClass.testInteger++;
    }

    #endregion
}