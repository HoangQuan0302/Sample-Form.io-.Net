using SimpleEmbedding.Data;
using System.Text;

namespace SimpleEmbedding.Pages
{
    public partial class TestGrid
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
        }
    }
}
