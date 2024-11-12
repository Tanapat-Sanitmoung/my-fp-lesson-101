
// to enable call "Inital" rather than "Extensions.Initial"
using static Tanapat.Playground.Lesson241112.Extensions;

namespace Tanapat.Playground.Lesson241112;

// from this link: https://weblogs.asp.net/dixin/functional-csharp-higher-order-function-currying-and-first-class-function

public static class Extensions
{
    public static TNextResult PipeTo<TCurrResult, TNextResult>(this TCurrResult result, Func<TCurrResult, TNextResult> next) 
        => next(result);

    public static TCurrResult Initial<TCurrResult>(Func<TCurrResult> init) => init();
    
    public static TCurrResult Initial<TCurrResult>(TCurrResult initValue) => initValue;
}

public static class UseCase
{
    public static void UseCase1()
    {
        // just design pipeline
        _ = Initial(2)
            .PipeTo(s1 => s1 + 2)
            .PipeTo(s2 => s2 * 2)
            .PipeTo(s3 => $"Result is {s3}") 
            switch {
                var x when x == "8" => HandleSuccess(),
                _ => HandleSuccess(),
            };
    }

    public static void UseCase2()
    {
        // Thank to co-pilot : https://copilot.microsoft.com/chats/
        var customerIds = new List<string> { "customer1", "customer2", "customer3" };

        var results = customerIds
            .Select(GetCustomerProfile)
            .Select(GetLowRiskCustomerProfile)
            .Select(ProcessCustomerProfile)
            .ToList();

        results.ForEach(Console.WriteLine);

        // ==========

        var v2Results = from customerId in customerIds 
            let profile = GetCustomerProfile(customerId) 
            let lowRiskProfile1 = GetLowRiskCustomerProfile(profile) 
            select ProcessCustomerProfile(lowRiskProfile1); 

        v2Results.ToList().ForEach(Console.WriteLine);

        // ==========

        var v3Results = from customerId in customerIds 
            let profile = GetCustomerProfile(customerId) 
            let lowRiskProfile1 = GetLowRiskCustomerProfile(profile) 
            let lowRiskProfile2 = GetLowRiskCustomerProfile(profile) 
            let lowRiskProfile3 = GetLowRiskCustomerProfile(profile) 
            select ProcessCustomerProfile(lowRiskProfile1); 

        v3Results.ToList().ForEach(Console.WriteLine);

        // ==========

        // I REALLY LIKE THIS ONE !!!

        var v4Results = customerIds
            .Select(customerId => new { customerId, profile = GetCustomerProfile(customerId) })
            .Select(x => new { x.profile, lowRiskProfile1 = GetLowRiskCustomerProfile(x.profile) })
            .Select(x => new { x.profile, x.lowRiskProfile1, lowRiskProfile2 = GetLowRiskCustomerProfile(x.profile) })
            .Select(x => new { x.profile, x.lowRiskProfile1, x.lowRiskProfile2, lowRiskProfile3 = GetLowRiskCustomerProfile(x.profile) })
            .Select(x => ProcessCustomerProfile(x.lowRiskProfile1))
            .ToList();
        
        v4Results.ToList().ForEach(Console.WriteLine);

        // ==========
    }

    public static void JustCurious1()
    {
        var list1 = new List<int>();

        var result = list1 switch 
        {
            [] => "Empty lisst",
            [1, _, 5] => "Has 3 element, start with 1 and end with 5",
            [1, .., 3] => "Start with 1 and end with 3",
            _ => "I don't care"
        };
    }

    static CustomerProfile GetCustomerProfile(string customerId)
    {
        // Simulate fetching a customer profile
        return new CustomerProfile { Id = customerId, Name = "Customer Name" };
    }

    static LowRiskCustomerProfile GetLowRiskCustomerProfile(CustomerProfile profile)
    {
        // Simulate processing a low risk customer profile
        return new LowRiskCustomerProfile { Id = profile.Id, Name = profile.Name, IsLowRisk = true };
    }

    static string ProcessCustomerProfile(LowRiskCustomerProfile lowRiskProfile)
    {
        // Simulate final processing and return success or failure
        return lowRiskProfile.IsLowRisk ? "Success" : "Failed";
    }
    

    class CustomerProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    class LowRiskCustomerProfile : CustomerProfile
    {
        public bool IsLowRisk { get; set; }
    }

    

    public static Unit HandleSuccess() => Unit.Default;
    public static Unit HandleFail() => Unit.Default;
}