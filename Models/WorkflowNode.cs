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
            NodeType = activity.ToWorkflowNodeType();
            SetupNode();
        }

        private void SetupNode()
        {
            Size = new Size(120, 120);
            
            switch (NodeType)
            {
                case WorkflowNodeType.Start:
                    if (string.IsNullOrEmpty(Title)) Title = "Start";
                    AddPort(PortAlignment.Right);
                    break;
                    
                case WorkflowNodeType.IfCondition:
                    if (string.IsNullOrEmpty(Title)) Title = "If Condition";
                    AddPort(PortAlignment.Left);
                    AddPort(PortAlignment.Right);
                    break;
                    
                case WorkflowNodeType.Switch:
                    if (string.IsNullOrEmpty(Title)) Title = "Switch";
                    AddPort(PortAlignment.Left);
                    AddPort(PortAlignment.Right);
                    break;
                    
                case WorkflowNodeType.ForEach:
                    if (string.IsNullOrEmpty(Title)) Title = "For Each";
                    AddPort(PortAlignment.Left);
                    AddPort(PortAlignment.Right);
                    break;
                    
                case WorkflowNodeType.Scope:
                    if (string.IsNullOrEmpty(Title)) Title = "Scope";
                    AddPort(PortAlignment.Left);
                    AddPort(PortAlignment.Right);
                    break;
                    
                case WorkflowNodeType.End:
                    if (string.IsNullOrEmpty(Title)) Title = "End";
                    AddPort(PortAlignment.Left);
                    break;
                    
                case WorkflowNodeType.Basic:
                default:
                    if (string.IsNullOrEmpty(Title)) Title = "Action";
                    AddPort(PortAlignment.Left);
                    AddPort(PortAlignment.Right);
                    break;
            }
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
        Basic
    }
} 