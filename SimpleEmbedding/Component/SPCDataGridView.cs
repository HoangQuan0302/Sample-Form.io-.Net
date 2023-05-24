using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleEmbedding.Data;
using SimpleEmbedding.FormBuilder;
using SimpleEmbedding.Pages;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Policy;
using System.Text;

namespace SimpleEmbedding.Component
{
    public class SPCDataGridView<T>: Microsoft.AspNetCore.Components.ComponentBase
    {
        [Parameter]
        public string classId { get; set; }
        [Parameter]
        public string subform { get; set; }
        [Parameter]
        public IEnumerable<T> DataSource { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public Dictionary<string, object> Settings { get; set; }
        private string businessSchemaUiRule { get; set; }
        private List<Fields> listField {  get; set; }=new List<Fields>();
        public string Alert { get; set; } = "";
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            FetchSourceInfoList();
            ReadUiFieldRuleSchema();
            builder.OpenComponent<DxGrid>(0);
            builder.AddAttribute(1, "Data", (object)DataSource);
            builder.AddAttribute(2, "Columns", this.DxGridAttributre);
            if (Settings != null)
            {
                builder.AddMultipleAttributes(3, Settings);
            }
            builder.CloseComponent();
        }

        private RenderFragment DxGridAttributre => builder =>
        {
            int i = 0;
            foreach (var field in listField)
            {
                if(field.isShow)
                {
                    builder.OpenComponent<DxGridDataColumn>(i);
                    i++;
                    builder.AddAttribute(i, "FieldName", field.fieldName);
                    i++;
                    builder.AddAttribute(i, "Width", "500px");
                    i++;
                    builder.CloseComponent();
                }
            }
        };

        void OnRowDoubleClick(GridRowClickEventArgs e)
        {
            Alert = $"The row {e.VisibleIndex} has been double clicked. The row value is '{e.Grid.GetRowValue(e.VisibleIndex, "Date")}'. ";
        }

        private void FetchSourceInfoList()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:6868/PhoebusAPI/CRM/{classId}?CustomerCode=ADU");
            request.Headers.Add("token", "AAEAAAD/////AQAAAAAAAAAMAgAAAEtTUEMuUGhvZWJ1cy5Vc3JNYW4sIFZlcnNpb249NC41LjUuMzg2LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGwFAQAAABdwYnMuVXNyTWFuLnBic1ByaW5jaXBhbAIAAAALX2F0dHJpYnV0ZXMfQnVzaW5lc3NQcmluY2lwYWxCYXNlK21JZGVudGl0eQME4gFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5EaWN0aW9uYXJ5YDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dFnBicy5Vc3JNYW4ucGJzSWRlbnRpdHkCAAAAAgAAAAkDAAAACQQAAAAEAwAAAOIBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuRGljdGlvbmFyeWAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQQAAAAHVmVyc2lvbghDb21wYXJlcghIYXNoU2l6ZQ1LZXlWYWx1ZVBhaXJzAAMAAwgWU3lzdGVtLk9yZGluYWxDb21wYXJlcgjmAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLktleVZhbHVlUGFpcmAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXVtdBwAAAAkFAAAABwAAAAkGAAAABQQAAAAWcGJzLlVzck1hbi5wYnNJZGVudGl0eQ0AAAANX2VtcGxveWVlQ29kZQZfZW1haWwGX2dyb3VwCV9sYW5ndWFnZQpfaXNBdWRpdG9yCl9pc1dlYlVzZXILX25ldmVyTG9naW4HX3VzZXJJRAZfUm9sZXMQX0lzQXV0aGVudGljYXRlZAVfTmFtZRtSZWFkT25seUJhc2VgMStfaW1hZ2VCYXNlNjQYUmVhZE9ubHlCYXNlYDErX2ltYWdlVXJsAQEBAQAAAQEDAAEBAQEBf1N5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0BAgAAAAYHAAAABkUwMDAyNwYIAAAAGXF1YW52aEBzcGMtdGVjaG5vbG9neS5jb20GCQAAAANBRE0GCgAAAAAAAAYLAAAAAVkGDAAAAANWSFEJDQAAAAEGDgAAAAxIb8OgbmcgUXXDom4KCgQFAAAAFlN5c3RlbS5PcmRpbmFsQ29tcGFyZXIBAAAAC19pZ25vcmVDYXNlAAEBBwYAAAAAAQAAAAcAAAAD5AFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5LZXlWYWx1ZVBhaXJgMltbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XSxbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0E8f///+QBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuS2V5VmFsdWVQYWlyYDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAgAAAANrZXkFdmFsdWUBAQYQAAAACk9wZXJhdG9ySWQJDAAAAAHu////8f///wYTAAAABE5hbWUJDgAAAAHr////8f///wYWAAAACEVtcGxDb2RlCQcAAAAB6P////H///8GGQAAAAVFbWFpbAkIAAAAAeX////x////BhwAAAAFQVVESVQGHQAAAAFOAeL////x////Bh8AAAANT3BlcmF0b3JHcm91cAkJAAAAAd/////x////BiIAAAAJSXNXZWJVc2VyCR0AAAAEDQAAAH9TeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAwAAAAZfaXRlbXMFX3NpemUIX3ZlcnNpb24GAAAICAkkAAAAAQAAAAEAAAARJAAAAAQAAAAGJQAAAANBRE0NAws=");
            var response = client.Send(request);
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var jsonData=JObject.Parse(json)["data"].ToString();
                DataSource = (IEnumerable<T>)JsonConvert.DeserializeObject<List<object>>(jsonData);
            }
        }

        private void ReadUiFieldRuleSchema()
        {
            var fileUIRuleStream = new FileStream(@$"D:\Project Internal\Sample-Form.io-.Net\SimpleEmbedding\BusinessRuleSchema\businessSchema#{classId.Replace('_', '.')}#{subform}.json", FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileUIRuleStream, Encoding.UTF8))
            {
                businessSchemaUiRule = streamReader.ReadToEnd();
            }

            //Build UI Rules
            BuildUIRule();
        }

        private void BuildUIRule()
        {
            listField = new List<Fields>();
            var jsonData = JObject.Parse(businessSchemaUiRule);
            var rules = jsonData["data"]["Rules"];
            var jsonSchema = string.Empty;
            foreach (var itm in rules)
            {
                listField.Add(new Fields() { fieldName = itm["FieldName"].Value<string>(),isShow= itm["ShowInInfoList"].Value<bool>() });
            }
        }

        private class Fields
        {
            public string fieldName { get; set; }
            public bool isShow { get; set; }
        }
    }
}
