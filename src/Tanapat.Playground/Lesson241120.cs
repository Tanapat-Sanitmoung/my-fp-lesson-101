namespace Tanapat.Playground.Lesson241120;

public static class Practice
{
    public static void Take01()
    {
        // different between Map and Bind
        
        Option<int> a = 1;
        Option<string> b = "Hello,";
    
        // wrapped 
        Option<Option<string>> result1 = a.Map(i => b);

        // not wrapped
        Option<string>         result2 = a.Bind(i => b);
    }
}