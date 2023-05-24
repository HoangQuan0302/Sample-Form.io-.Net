using System.Security.Policy;

namespace SimpleEmbedding.Shared
{
    public partial class PopupRunCommand
    {
        private string url { get; set; }
        private string classId { get; set; }
        private string subform { get; set; }
        private async Task RunCommand()
        {
            classId = url.Split('/').FirstOrDefault();
            subform = url.Split('/').LastOrDefault();
            await FetchBusinessRuleSchema();
        }

        private async Task FetchBusinessRuleSchema()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:6868//PhoebusAPI/CRM/pbs_BO_Rules_UIFieldRules?ClassName={classId}&Subform={subform}");
            request.Headers.Add("token", "AAEAAAD/////AQAAAAAAAAAMAgAAAEtTUEMuUGhvZWJ1cy5Vc3JNYW4sIFZlcnNpb249NC41LjUuMzg2LCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGwFAQAAABdwYnMuVXNyTWFuLnBic1ByaW5jaXBhbAIAAAALX2F0dHJpYnV0ZXMfQnVzaW5lc3NQcmluY2lwYWxCYXNlK21JZGVudGl0eQME4gFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5EaWN0aW9uYXJ5YDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dFnBicy5Vc3JNYW4ucGJzSWRlbnRpdHkCAAAAAgAAAAkDAAAACQQAAAAEAwAAAOIBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuRGljdGlvbmFyeWAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXQQAAAAHVmVyc2lvbghDb21wYXJlcghIYXNoU2l6ZQ1LZXlWYWx1ZVBhaXJzAAMAAwgWU3lzdGVtLk9yZGluYWxDb21wYXJlcgjmAVN5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLktleVZhbHVlUGFpcmAyW1tTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldLFtTeXN0ZW0uU3RyaW5nLCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODldXVtdBwAAAAkFAAAABwAAAAkGAAAABQQAAAAWcGJzLlVzck1hbi5wYnNJZGVudGl0eQ0AAAANX2VtcGxveWVlQ29kZQZfZW1haWwGX2dyb3VwCV9sYW5ndWFnZQpfaXNBdWRpdG9yCl9pc1dlYlVzZXILX25ldmVyTG9naW4HX3VzZXJJRAZfUm9sZXMQX0lzQXV0aGVudGljYXRlZAVfTmFtZRtSZWFkT25seUJhc2VgMStfaW1hZ2VCYXNlNjQYUmVhZE9ubHlCYXNlYDErX2ltYWdlVXJsAQEBAQAAAQEDAAEBAQEBf1N5c3RlbS5Db2xsZWN0aW9ucy5HZW5lcmljLkxpc3RgMVtbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0BAgAAAAYHAAAABkUwMDAyNwYIAAAAGXF1YW52aEBzcGMtdGVjaG5vbG9neS5jb20GCQAAAANBRE0GCgAAAAAAAAYLAAAAAVkGDAAAAANWSFEJDQAAAAEGDgAAAAxIb8OgbmcgUXXDom4KCgQFAAAAFlN5c3RlbS5PcmRpbmFsQ29tcGFyZXIBAAAAC19pZ25vcmVDYXNlAAEBBwYAAAAAAQAAAAcAAAAD5AFTeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5LZXlWYWx1ZVBhaXJgMltbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XSxbU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5XV0E8f///+QBU3lzdGVtLkNvbGxlY3Rpb25zLkdlbmVyaWMuS2V5VmFsdWVQYWlyYDJbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV0sW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAgAAAANrZXkFdmFsdWUBAQYQAAAACk9wZXJhdG9ySWQJDAAAAAHu////8f///wYTAAAABE5hbWUJDgAAAAHr////8f///wYWAAAACEVtcGxDb2RlCQcAAAAB6P////H///8GGQAAAAVFbWFpbAkIAAAAAeX////x////BhwAAAAFQVVESVQGHQAAAAFOAeL////x////Bh8AAAANT3BlcmF0b3JHcm91cAkJAAAAAd/////x////BiIAAAAJSXNXZWJVc2VyCR0AAAAEDQAAAH9TeXN0ZW0uQ29sbGVjdGlvbnMuR2VuZXJpYy5MaXN0YDFbW1N5c3RlbS5TdHJpbmcsIG1zY29ybGliLCBWZXJzaW9uPTQuMC4wLjAsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49Yjc3YTVjNTYxOTM0ZTA4OV1dAwAAAAZfaXRlbXMFX3NpemUIX3ZlcnNpb24GAAAICAkkAAAAAQAAAAEAAAARJAAAAAQAAAAGJQAAAANBRE0NAws=");
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                if (json != null)
                {
                    if (!File.Exists(@$"D:\\Project Internal\\Sample-Form.io-.Net\\SimpleEmbedding\\BusinessRuleSchema\\businessSchema#{classId}#{subform}.json"))
                    {
                        System.IO.File.WriteAllText(@$"D:\Project Internal\Sample-Form.io-.Net\SimpleEmbedding\BusinessRuleSchema\businessSchema#{classId}#{subform}.json", string.Empty);
                        using (StreamWriter outputFile = new StreamWriter(@$"D:\Project Internal\Sample-Form.io-.Net\SimpleEmbedding\BusinessRuleSchema\businessSchema#{classId}#{subform}.json"))
                        {
                            outputFile.WriteLine(json);
                            outputFile.Close();
                        }
                    }
                }
            }
            //nav.NavigateTo($"phoebusweb/{url.Replace('/', '_')}");
            nav.NavigateTo($"infolist/{classId}_{subform}");
        }
    }
}
