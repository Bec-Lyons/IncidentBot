using Microsoft.Bot.Builder.FormFlow;
using System;

namespace formflow.FormFlow
{
    //The questions are structured for the Form Bot using public properties on the class. 
    //You can see here the Prompt attribute is set and the question options can be complex types like String or enums.

    [Serializable]
    public class HazardousIncident
    {
        public HazardousIncident() { }
        public HazardousIncident(string time)
        {
            TimePeriod = time;
        }

        public HazardousIncident(string location, string time)
        {
            Location = location;
            TimePeriod = TimePeriod;
        }

        [Prompt("What time period are you looking for?")]
        public string TimePeriod { get; set; }
        [Prompt("In what location?")]
        public string Location { get; set; }
        
        public static IForm<HazardousIncident> BuildEnquiryForm()
        {
            return new FormBuilder<HazardousIncident>()
                .AddRemainingFields()
                .Build();
        }
    }


}