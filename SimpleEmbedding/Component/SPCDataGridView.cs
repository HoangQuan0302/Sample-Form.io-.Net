using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using SimpleEmbedding.Data;

namespace SimpleEmbedding.Component
{
    public class SPCDataGridView<T>:ComponentBase
    {
        [Parameter]
        public string? JsonLayout { get; set; }
        [Parameter]
        public IEnumerable<T> DataSource { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public Dictionary<string, object> Settings { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
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
            builder.OpenComponent<DxGridCommandColumn>(0);
            builder.AddAttribute(1, "Width", "150px");
            builder.CloseComponent();
            builder.OpenComponent<DxGridDataColumn>(2);
            builder.AddAttribute(3, "FieldName", "Date");
            builder.CloseComponent();
            builder.OpenComponent<DxGridDataColumn>(4);
            builder.AddAttribute(5, "FieldName", "TemperatureC");
            builder.CloseComponent();
            builder.OpenComponent<DxGridDataColumn>(6);
            builder.AddAttribute(7, "FieldName", "TemperatureF");
            builder.CloseComponent();
            builder.OpenComponent<DxGridDataColumn>(8);
            builder.AddAttribute(9, "FieldName", "Summary");
            builder.CloseComponent();
        };
    }
}
