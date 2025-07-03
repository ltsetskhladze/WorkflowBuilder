using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Geometry;

namespace WorkflowBuilder.Models
{
    public class WorkflowNode : NodeModel
    {
        public new string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public Dictionary<string, object> Properties { get; set; } = new();
        public WorkflowNodeType NodeType { get; set; }
        public string Icon { get; set; } = "";
        public string Color { get; set; } = "#ffffff";
        public bool IsSelected { get; set; } = false;
        
        // New properties for enhanced functionality
        public string Label { get; set; } = "";
        public List<FilterCondition> Filters { get; set; } = new();
        public int StepNo { get; set; }
        public int ActivityId { get; set; }

        public WorkflowNode(WorkflowNodeType nodeType, Point? position = null) : base(position)
        {
            NodeType = nodeType;
            SetupNode();
        }

        public WorkflowNode(WorkflowActivity activity, Point? position = null) : base(position)
        {
            ActivityId = activity.ActivityId;
            StepNo = activity.StepNo;
            Title = activity.Name;
            Label = activity.Label;
            Filters = new List<FilterCondition>(activity.Filters);
            
            // Directly map from activity to node type
            NodeType = activity.ToWorkflowNodeType();
            
            SetupNode();
        }

        private void SetupNode()
        {
            // Set larger size to accommodate ports around the edges
            Size = new Size(120, 120);
            
            // Don't override Title if it's already set (preserve JSON activity names)
            // Add ports for all node types
            
            switch (NodeType)
            {
                case WorkflowNodeType.Start:
                    if (string.IsNullOrEmpty(Title)) Title = "Start";
                    AddPort(PortAlignment.Right); // output
                    break;
                    
                case WorkflowNodeType.IfCondition:
                    if (string.IsNullOrEmpty(Title)) Title = "If Condition";
                    AddPort(PortAlignment.Left);   // input
                    AddPort(PortAlignment.Right);  // output
                    break;
                    
                case WorkflowNodeType.Switch:
                    if (string.IsNullOrEmpty(Title)) Title = "Switch";
                    AddPort(PortAlignment.Left);   // input
                    AddPort(PortAlignment.Right);  // output
                    break;
                    
                case WorkflowNodeType.ForEach:
                    if (string.IsNullOrEmpty(Title)) Title = "For Each";
                    AddPort(PortAlignment.Left);   // input
                    AddPort(PortAlignment.Right);  // output
                    break;
                    
                case WorkflowNodeType.Scope:
                    if (string.IsNullOrEmpty(Title)) Title = "Scope";
                    AddPort(PortAlignment.Left);   // input
                    AddPort(PortAlignment.Right);  // output
                    break;
                    
                case WorkflowNodeType.End:
                    if (string.IsNullOrEmpty(Title)) Title = "End";
                    AddPort(PortAlignment.Left); // input
                    break;
                    
                case WorkflowNodeType.Basic:
                default:
                    if (string.IsNullOrEmpty(Title)) Title = "Action";
                    AddPort(PortAlignment.Left);   // input
                    AddPort(PortAlignment.Right);  // output
                    break;
            }
        }

        private string GetStartIcon()
        {
            return @"<svg width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" xmlns=""http://www.w3.org/2000/svg"">
                <path d=""M13 3L4 14h7v7l9-11h-7V3z"" fill=""#7c3aed"" stroke=""#7c3aed"" stroke-width=""2"" stroke-linejoin=""round""/>
            </svg>";
        }

        private string GetIfConditionIcon()
        {
            return @"<svg width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" xmlns=""http://www.w3.org/2000/svg"">
                <path d=""M7 8l5 8 5-8M7 8h10M7 8V6a2 2 0 012-2h6a2 2 0 012 2v2"" stroke=""#0ea5e9"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""/>
            </svg>";
        }

        private string GetSwitchIcon()
        {
            return @"<svg width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" xmlns=""http://www.w3.org/2000/svg"">
                <path d=""M12 3v6m0 6v6m-6-9h12M6 15h12"" stroke=""#0ea5e9"" stroke-width=""2"" stroke-linecap=""round""/>
            </svg>";
        }

        private string GetForEachIcon()
        {
            return @"<svg width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" xmlns=""http://www.w3.org/2000/svg"">
                <path d=""M8 12a4 4 0 108 0 4 4 0 00-8 0z"" fill=""none"" stroke=""#0ea5e9"" stroke-width=""2""/>
                <path d=""m15 9 3 3-3 3"" stroke=""#0ea5e9"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round""/>
            </svg>";
        }

        private string GetScopeIcon()
        {
            return @"<svg width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" xmlns=""http://www.w3.org/2000/svg"">
                <rect x=""3"" y=""3"" width=""18"" height=""18"" rx=""2"" ry=""2"" stroke=""#0ea5e9"" stroke-width=""2"" fill=""none""/>
                <path d=""M9 9h6v6H9z"" stroke=""#0ea5e9"" stroke-width=""1.5"" fill=""none""/>
            </svg>";
        }

        private string GetEndIcon()
        {
            return @"<svg width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" xmlns=""http://www.w3.org/2000/svg"">
                <circle cx=""12"" cy=""12"" r=""9"" fill=""#ef4444"" stroke=""#dc2626"" stroke-width=""2""/>
                <path d=""M15 9l-6 6M9 9l6 6"" stroke=""white"" stroke-width=""2"" stroke-linecap=""round""/>
            </svg>";
        }
    }

    public enum WorkflowNodeType
    {
        Start,
        IfCondition, 
        Switch,
        ForEach,
        Scope,
        End,
        Basic // For regular activities
    }
} 