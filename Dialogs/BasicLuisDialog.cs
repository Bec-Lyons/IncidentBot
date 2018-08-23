using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

using formflow.FormFlow;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent("HazardousIncidents")]
        public async Task HazardousIncidentsIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            
            var entities = new List<EntityRecommendation>(result.Entities);
            string location="";
            string timePeriod="";
        
            foreach (EntityRecommendation entity in result.Entities)
            {
                
                await context.PostAsync($"Entities found {entity.Entity} of type {entity.Type}");
            }
            HazardousIncident form;
            if (!location.Equals("")&& !timePeriod.Equals(""))
            {
                form = new HazardousIncident(location, timePeriod);
            }

            else if (location.Equals("") && !timePeriod.Equals(""))
            {
                form = new HazardousIncident(timePeriod);
            }
            else
            {
                form = new HazardousIncident();
            }

            var hazardousInfoForm = new FormDialog<HazardousIncident>(form, HazardousIncident.BuildEnquiryForm, FormOptions.PromptInStart, entities);


            context.Call<HazardousIncident>(hazardousInfoForm, HazardousIncidentFormComplete);
        }

       
        private async Task HazardousIncidentFormComplete(IDialogContext context, IAwaitable<HazardousIncident> result)
        {
            HazardousIncident info = null;
            try
            {
                info = await result;
            }
            catch (OperationCanceledException)
            {
                await context.PostAsync("You canceled the form!");
                return;
            }

            if (info != null)
            {
                await context.PostAsync("Your result: " + info.ToString());
            }
            else
            {
                await context.PostAsync("Form returned empty response!");
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("HighestIncidents")]
        public async Task HighestIncidentsIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("LowestIncidents")]
        public async Task LowestIncidentsIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("None")]
        public async Task NoneIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        // Go to https://luis.ai and create a new intent, then train/publish your luis app.
        // Finally replace "Greeting" with the name of your newly created intent in the following handler
        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await this.ShowLuisResult(context, result);
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }
    }
}