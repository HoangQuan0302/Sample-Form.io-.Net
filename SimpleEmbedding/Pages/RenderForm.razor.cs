using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace SimpleEmbedding.Pages
{
    public partial class RenderForm
    {
        private string formDesign = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            var fileStream = new FileStream(@"D:\Project Internal\Sample-Form.io-.Net\SimpleEmbedding\formdesign.json", FileMode.Open, FileAccess.Read);
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
            }
        }

        protected async Task RefreshUI()
        {
            var fileStream = new FileStream(@"D:\Project Internal\Sample-Form.io-.Net\SimpleEmbedding\formdesign.json", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                formDesign = streamReader.ReadToEnd();
            }
            if (!string.IsNullOrEmpty(formDesign))
            {
                await JSRuntime.InvokeVoidAsync(identifier: "renderForm", formDesign);
            }
        }
    }
}
