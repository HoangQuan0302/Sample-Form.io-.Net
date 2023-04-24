using HtmlAgilityPack;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using System.Text;

namespace SimpleEmbedding.Pages
{
    public partial class InfoList
    {
        private string formDesign = string.Empty;
        private string markupHtml = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var fileStream = new FileStream(@$"C:\Users\SPC - DEVELOPER\Downloads\forminfolist.json", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                formDesign = streamReader.ReadToEnd();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!string.IsNullOrEmpty(formDesign))
                {
                    await JSRuntime.InvokeVoidAsync(identifier: "renderForm", formDesign);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync(identifier: "renderInfoList");
                }
            }
        }

        protected async Task RefreshUI()
        {
            var fileStream = new FileStream(@$"C:\Users\SPC - DEVELOPER\Downloads\forminfolist.json", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                formDesign = streamReader.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(formDesign))
            {
                await JSRuntime.InvokeVoidAsync(identifier: "renderForm", formDesign);
            }
        }

        protected async Task CreateFormAfterRender()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument document = web.Load("http://localhost:5111/infolist");
        }
    }
}
