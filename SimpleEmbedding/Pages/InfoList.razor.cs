using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using DevExpress.XtraPrinting.Native;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleEmbedding.Component;
using SimpleEmbedding.Data;
using System.Security.Policy;
using System.Text;
using static DevExpress.XtraPrinting.Native.PageSizeInfo;

namespace SimpleEmbedding.Pages
{
    public partial class InfoList
    {
        [Parameter]
        public string url { get; set; }
        private string classId { get; set; }
        private string subform { get; set; }
        private string formDesign = string.Empty;
        public IEnumerable<object> DataSource { get; set; }
        private string businessSchemaUiRule { get; set; }
        private List<Fields> listField { get; set; } = new List<Fields>();
        int GridPageIndex { get; set; } = 1;
        int PageSize { get; set; } = 10;
        public Dictionary<string, object> InputAttributes { get; set; } =
            new Dictionary<string, object>() {
            { "PageSize", 10},
            { "ShowFilterRow",  false},
            { "ShowGroupPanel", true },
            {"CssClass","ch-360" },
                {"RowDoubleClick","OnRowDoubleClick" }
            };
        protected override async Task OnInitializedAsync()
        {
            classId = url.Split('_').FirstOrDefault().Replace('.', '_');
            subform = url.Split('_').LastOrDefault();
            FetchSourceInfoList();
            ReadUiFieldRuleSchema();
        }

        private void FetchSourceInfoList()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:6868/PhoebusAPI/CRM/{classId}");
            request.Headers.Add("$page",GridPageIndex.ToString());
            request.Headers.Add("$perpage","100");
            request.Headers.Add("token", "AAEAAAD/////AQAAAAAAAAAMAgAAAEtTUEMuUGhvZWJ1cy5Vc3JNYW4sIFZlcnNpb249NC41LjUuMzg2LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGwFAQAAABdwYnMuVXNyTWFuLnBic1ByaW5jaXBhbAIAAAALX2F0dHJpYnV0ZXMfQnVzaW5lc3NQcmluY2lwYWxCYXNlK21JZGVudGl0eQME4gFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5EaWN0aW9uYXJ5YDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dFnBicy5Vc3JNYW4ucGJzSWRlbnRpdHkCAAAAAgAAAAkDAAAACQQAAAAEAwAAAOIBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuRGljdGlvbmFyeWAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQQAAAAHVmVyc2lvbghDb21wYXJlcghIYXNoU2l6ZQ1LZXlWYWx1ZVBhaXJzAAMAAwgWU3lzdGVtLk9yZGluYWxDb21wYXJlcgjmAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLktleVZhbHVlUGFpcmAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXVtdBwAAAAkFAAAABwAAAAkGAAAABQQAAAAWcGJzLlVzck1hbi5wYnNJZGVudGl0eQ0AAAANX2VtcGxveWVlQ29kZQZfZW1haWwGX2dyb3VwCV9sYW5ndWFnZQpfaXNBdWRpdG9yCl9pc1dlYlVzZXILX25ldmVyTG9naW4HX3VzZXJJRAZfUm9sZXMQX0lzQXV0aGVudGljYXRlZAVfTmFtZRtSZWFkT25seUJhc2VgMStfaW1hZ2VCYXNlNjQYUmVhZE9ubHlCYXNlYDErX2ltYWdlVXJsAQEBAQAAAQEDAAEBAQEBf1N5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0BAgAAAAYHAAAABkUwMDAyNwYIAAAAGXF1YW52aEBzcGMtdGVjaG5vbG9neS5jb20GCQAAAANBRE0GCgAAAAAAAAYLAAAAAVkGDAAAAANWSFEJDQAAAAEGDgAAAAxIb8OgbmcgUXXDom4KCgQFAAAAFlN5c3RlbS5PcmRpbmFsQ29tcGFyZXIBAAAAC19pZ25vcmVDYXNlAAEBBwYAAAAAAQAAAAcAAAAD5AFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5LZXlWYWx1ZVBhaXJgMltbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XSxbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0E8f///+QBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuS2V5VmFsdWVQYWlyYDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAgAAAANrZXkFdmFsdWUBAQYQAAAACk9wZXJhdG9ySWQJDAAAAAHu////8f///wYTAAAABE5hbWUJDgAAAAHr////8f///wYWAAAACEVtcGxDb2RlCQcAAAAB6P////H///8GGQAAAAVFbWFpbAkIAAAAAeX////x////BhwAAAAFQVVESVQGHQAAAAFOAeL////x////Bh8AAAANT3BlcmF0b3JHcm91cAkJAAAAAd/////x////BiIAAAAJSXNXZWJVc2VyCR0AAAAEDQAAAH9TeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAwAAAAZfaXRlbXMFX3NpemUIX3ZlcnNpb24GAAAICAkkAAAAAQAAAAEAAAARJAAAAAQAAAAGJQAAAANBRE0NAws=");
            var response = client.Send(request);
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync().Result;
                var jsonData = JObject.Parse(json)["data"].ToString();
                DataSource = (IEnumerable<object>)JsonConvert.DeserializeObject<List<object>>(jsonData);
            }
        }
        void OnPageSizeChanged(int newPageSize)
        {
            PageSize = newPageSize;
            StateHasChanged();
        }
        void OnPageIndexChanged(int newPageIndex)
        { 
            if(newPageIndex>100/PageSize)
            {
                GridPageIndex = newPageIndex;
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:6868/PhoebusAPI/CRM/{classId}");
                request.Headers.Add("$page", GridPageIndex.ToString());
                request.Headers.Add("$perpage", "100");
                request.Headers.Add("token", "AAEAAAD/////AQAAAAAAAAAMAgAAAEtTUEMuUGhvZWJ1cy5Vc3JNYW4sIFZlcnNpb249NC41LjUuMzg2LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGwFAQAAABdwYnMuVXNyTWFuLnBic1ByaW5jaXBhbAIAAAALX2F0dHJpYnV0ZXMfQnVzaW5lc3NQcmluY2lwYWxCYXNlK21JZGVudGl0eQME4gFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5EaWN0aW9uYXJ5YDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dFnBicy5Vc3JNYW4ucGJzSWRlbnRpdHkCAAAAAgAAAAkDAAAACQQAAAAEAwAAAOIBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuRGljdGlvbmFyeWAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQQAAAAHVmVyc2lvbghDb21wYXJlcghIYXNoU2l6ZQ1LZXlWYWx1ZVBhaXJzAAMAAwgWU3lzdGVtLk9yZGluYWxDb21wYXJlcgjmAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLktleVZhbHVlUGFpcmAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXVtdBwAAAAkFAAAABwAAAAkGAAAABQQAAAAWcGJzLlVzck1hbi5wYnNJZGVudGl0eQ0AAAANX2VtcGxveWVlQ29kZQZfZW1haWwGX2dyb3VwCV9sYW5ndWFnZQpfaXNBdWRpdG9yCl9pc1dlYlVzZXILX25ldmVyTG9naW4HX3VzZXJJRAZfUm9sZXMQX0lzQXV0aGVudGljYXRlZAVfTmFtZRtSZWFkT25seUJhc2VgMStfaW1hZ2VCYXNlNjQYUmVhZE9ubHlCYXNlYDErX2ltYWdlVXJsAQEBAQAAAQEDAAEBAQEBf1N5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0BAgAAAAYHAAAABkUwMDAyNwYIAAAAGXF1YW52aEBzcGMtdGVjaG5vbG9neS5jb20GCQAAAANBRE0GCgAAAAAAAAYLAAAAAVkGDAAAAANWSFEJDQAAAAEGDgAAAAxIb8OgbmcgUXXDom4KCgQFAAAAFlN5c3RlbS5PcmRpbmFsQ29tcGFyZXIBAAAAC19pZ25vcmVDYXNlAAEBBwYAAAAAAQAAAAcAAAAD5AFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5LZXlWYWx1ZVBhaXJgMltbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XSxbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0E8f///+QBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuS2V5VmFsdWVQYWlyYDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAgAAAANrZXkFdmFsdWUBAQYQAAAACk9wZXJhdG9ySWQJDAAAAAHu////8f///wYTAAAABE5hbWUJDgAAAAHr////8f///wYWAAAACEVtcGxDb2RlCQcAAAAB6P////H///8GGQAAAAVFbWFpbAkIAAAAAeX////x////BhwAAAAFQVVESVQGHQAAAAFOAeL////x////Bh8AAAANT3BlcmF0b3JHcm91cAkJAAAAAd/////x////BiIAAAAJSXNXZWJVc2VyCR0AAAAEDQAAAH9TeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAwAAAAZfaXRlbXMFX3NpemUIX3ZlcnNpb24GAAAICAkkAAAAAQAAAAEAAAARJAAAAAQAAAAGJQAAAANBRE0NAws=");
                var response = client.Send(request);
                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    var jsonData = JObject.Parse(json)["data"].ToString();
                    DataSource = (IEnumerable<object>)JsonConvert.DeserializeObject<List<object>>(jsonData);
                }
                StateHasChanged();
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
                listField.Add(new Fields() { fieldName = itm["FieldName"].Value<string>(), isShow = itm["ShowInInfoList"].Value<bool>() });
            }
        }

        private class Fields
        {
            public string fieldName { get; set; }
            public bool isShow { get; set; }
        }
        void OnRowDoubleClick(GridRowClickEventArgs e)
        {
            nav.NavigateTo($"phoebusweb/{classId.Replace('_','.')}_{subform}/{e.Grid.GetRowValue(e.VisibleIndex,listField.FirstOrDefault().fieldName)}");
        }

    }
}
