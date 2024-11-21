using Core.Singleton;

[PersistentSingletonConfig]
public class TestSingleton : PersistentSingleton<TestSingleton>
{
    [SerializeData("Category")]
    private string _name;
}
