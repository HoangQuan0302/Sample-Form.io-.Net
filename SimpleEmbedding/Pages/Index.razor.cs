using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System.Text;

namespace SimpleEmbedding.Pages
{
    public partial class Index
    {
        private string formDesign = string.Empty;
        private string formId = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            var uri = nav.ToAbsoluteUri(nav.Uri);
            QueryHelpers.ParseQuery(uri.Query).TryGetValue("formId", out var param);
            formId = param.ToString();
            var fileStream = new FileStream(@$"C:\Users\SPC - DEVELOPER\Downloads\{formId}.json", FileMode.Open, FileAccess.Read);
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
                    await JSRuntime.InvokeAsync<string>(identifier: "CreateSimpleForm", formDesign);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync(identifier: "formBuilderDefault");
                }
            }
        }

        private async void SaveForm()
        {
            var xxx = await JSRuntime.InvokeAsync<string>(identifier: "SaveForm");
            //Handle form Design
            if (!string.IsNullOrEmpty(xxx))
            {
                var jsonData = JObject.Parse(xxx).Children().Values().FirstOrDefault().ToString();
                System.IO.File.WriteAllText(@$"C:\Users\SPC - DEVELOPER\Downloads\{formId}.json", string.Empty);
                using (StreamWriter outputFile = new StreamWriter(@$"C:\Users\SPC - DEVELOPER\Downloads\{formId}.json"))
                {
                    outputFile.WriteLine(jsonData);
                    outputFile.Close();
                }
            }
        }
    }
}
