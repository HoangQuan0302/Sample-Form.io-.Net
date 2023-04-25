using HtmlAgilityPack;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using SimpleEmbedding.Data;
using System.Text;

namespace SimpleEmbedding.Pages
{
    public partial class InfoList
    {
        private string formDesign = string.Empty;
        public List<WeatherForecast> Forecasts { get; set; }

        public Dictionary<string, object> InputAttributes { get; set; } =
            new Dictionary<string, object>() {
            { "PageSize", 10},
            { "ShowFilterRow",  true},
            //{ "ShowPager" , false },
            { "ShowGroupPanel", true }
            };
        protected override async Task OnInitializedAsync()
        {
            WeatherForecast[] data = await ForecastService.GetForecastAsync(DateTime.Now);
            Forecasts = data.ToList();
            var fileStream = new FileStream(@$"C:\Users\SPC - DEVELOPER\Downloads\forminfolist.json", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                formDesign = streamReader.ReadToEnd();
            }
        }
    }
}
