using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Policy;
using System.Text;

namespace SimpleEmbedding.Pages
{
    public partial class Index
    {
        private string formDesign = string.Empty;
        private string formUIRule = string.Empty;
        private string uiRules = string.Empty;
        private string formId = string.Empty;

        [Parameter]
        public string classId { get; set; }
        protected override async Task OnInitializedAsync()
        {
            var fileStream = new FileStream(@$"D:\Project Internal\Sample-Form.io-.Net\SimpleEmbedding\EditableSchema\formdesign#{classId.Replace('_', '#')}.json", FileMode.Open, FileAccess.Read);
            var fileUIRuleStream = new FileStream(@$"D:\Project Internal\Sample-Form.io-.Net\SimpleEmbedding\BusinessRuleSchema\businessSchema#{classId.Replace('_', '#')}.json", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileUIRuleStream, Encoding.UTF8))
            {
                formUIRule = streamReader.ReadToEnd();
            }
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                formDesign = streamReader.ReadToEnd();
            }

            //Build UI Rules
            BuildUIRule();
        }

        private string BuildUIRule()
        {
            var jsonData = JObject.Parse(formUIRule);
            var rules = jsonData["data"]["Rules"];
            var jsonSchema = string.Empty;
            foreach (var itm in rules)
            {
                var fieldType = "textfield";
                //Check Field Type
                if (!string.IsNullOrEmpty(itm["LookupCode"].Value<string>()) && !string.IsNullOrEmpty(itm["DefaultLookupCode"].Value<string>()))
                {
                    switch (itm["LookupCode"].Value<string>())
                    {
                        case "Calendar":
                            fieldType = "day";
                            break;
                        default:
                            fieldType = "select";
                            break;
                    }
                }
                var fieldProperty = new FieldProperty()
                {
                    title = itm["Description"].Value<string>(),
                    icon = "terminal",
                    schema = new UIFieldRule()
                    {
                        label = itm["Description"].Value<string>(),
                        type = fieldType,
                        key = itm["FieldName"].Value<string>(),
                        tooltip = itm["Tips"].Value<string>(),
                        disabled = string.IsNullOrEmpty(itm["DisplayField"].Value<string>())?false:true,
                        errorLabel="",
                        ClassCode=itm["ClassCode"].Value<string>(),
                        SubForm=itm["SubForm"].Value<string>(),
                        PropertyNo=itm["PropertyNo"].Value<string>(),
                        NotUsed = itm["NotUsed"].Value<bool>()
                    }
                };
                if(itm!=rules.LastOrDefault())
                {
                    jsonSchema += $"\"{fieldProperty.schema.key}\":" + JsonConvert.SerializeObject(fieldProperty) + ",";
                }
                else
                {
                    jsonSchema += $"\"{fieldProperty.schema.key}\":" + JsonConvert.SerializeObject(fieldProperty);
                }
            }
            uiRules = "{"+jsonSchema+"}";
            return uiRules;
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (!string.IsNullOrEmpty(formDesign))
                {
                    await JSRuntime.InvokeAsync<string>(identifier: "CreateSimpleForm", formDesign,uiRules);
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync(identifier: "formBuilderDefault",uiRules);
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
                System.IO.File.WriteAllText(@$"D:\Project Internal\Sample-Form.io-.Net\SimpleEmbedding\EditableSchema\formdesign#{classId.Replace('_', '#')}.json", string.Empty);
                using (StreamWriter outputFile = new StreamWriter(@$"D:\Project Internal\Sample-Form.io-.Net\SimpleEmbedding\EditableSchema\formdesign#{classId.Replace('_', '#')}.json"))
                {
                    outputFile.WriteLine(jsonData);
                    outputFile.Close();
                }
            }
            nav.NavigateTo($"phoebusweb/{classId}");
        }

        private class FieldProperty
        {
            public string title { get; set; }
            public string icon { get; set; }
            public UIFieldRule schema { get; set; }
        }
        private class UIFieldRule
        {
            public string label { get; set; }
            public string type { get; set; }
            public string key { get; set; }
            public string tooltip { get; set; }
            public bool disabled { get; set; }
            public string errorLabel { get; set; }
            public string ClassCode { get; set; }
            public string SubForm { get; set; }
            public string PropertyNo { get; set; }
            public bool NotUsed { get; set; }
            public bool ShowInInfoList { get; set; }
            public bool ShowInEditList { get; set; }
            public string Dag { get; set; }
            public string DefaultLookupCode { get; set; }
            public string LookupCode { get; set; }
            public string DisplayField { get; set; }
            public object English { get; set; }
            public object Vietnamese { get; set; }
            public object Russian { get; set; }
            public object French { get; set; }
            public string Translation5 { get; set; }
            public long maxLength { get; set; }
            public long DbFieldSize { get; set; }
            public bool ConvertBeforeSaving { get; set; }
            public string GroupName { get; set; }

            public bool input = true;

        }
    }
}
