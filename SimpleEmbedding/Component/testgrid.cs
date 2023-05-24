using DevExpress.Blazor;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;

namespace SimpleEmbedding.Component
{
    public class testgrid<T>:ComponentBase
    {
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
            builder.AddAttribute(1, "Width", "500px");
            builder.CloseComponent();
            builder.OpenComponent<DxGridDataColumn>(2);
            builder.AddAttribute(3, "FieldName", "Date");
            builder.AddAttribute(4, "Width", "10px");
            builder.CloseComponent();
            builder.OpenComponent<DxGridDataColumn>(5);
            builder.AddAttribute(6, "FieldName", "TemperatureC");
            builder.CloseComponent();
            builder.OpenComponent<DxGridDataColumn>(7);
            builder.AddAttribute(8, "FieldName", "TemperatureF");
            builder.CloseComponent();
            builder.OpenComponent<DxGridDataColumn>(9);
            builder.AddAttribute(10, "FieldName", "Summary");
            builder.CloseComponent();
        };
    }
}
