using System.Text.Json.Serialization;

namespace WorkflowBuilder.Models
{
    public class WorkflowData
    {
        [JsonPropertyName("WorkFlowId")]
        public int WorkFlowId { get; set; }

        [JsonPropertyName("WorkFlowName")]
        public string WorkFlowName { get; set; } = "";

        [JsonPropertyName("WorkflowTypeId")]
        public int WorkflowTypeId { get; set; }

        [JsonPropertyName("IsActive")]
        public bool IsActive { get; set; }

        [JsonPropertyName("WorkflowActivities")]
        public List<WorkflowActivity> WorkflowActivities { get; set; } = new();
    }

    public class WorkflowActivity
    {
        [JsonPropertyName("StepNo")]
        public int StepNo { get; set; }

        [JsonPropertyName("ActivityId")]
        public int ActivityId { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("ActivityFeatureType")]
        public string ActivityFeatureType { get; set; } = "RegularBlock";

        [JsonPropertyName("Label")]
        public string Label { get; set; } = "";

        [JsonPropertyName("Filters")]
        public List<FilterCondition> Filters { get; set; } = new();

        [JsonPropertyName("WorkflowActivityJumps")]
        public List<WorkflowActivityJump> WorkflowActivityJumps { get; set; } = new();
    }

    public class WorkflowActivityJump
    {
        [JsonPropertyName("ToStepNo")]
        public int ToStepNo { get; set; }

        [JsonPropertyName("JumpType")]
        public string JumpType { get; set; } = "Default";
    }

    public class FilterCondition
    {
        [JsonPropertyName("Field")]
        public string Field { get; set; } = "";
        
        [JsonPropertyName("Operator")]
        public string Operator { get; set; } = "Equals";
        
        [JsonPropertyName("Value")]
        public string Value { get; set; } = "";
        
        public FilterCondition() { }
        
        public FilterCondition(string field, string operatorType, string value)
        {
            Field = field;
            Operator = operatorType;
            Value = value;
        }
    }

    public enum ActivityFeatureType
    {
        RegularBlock,
        Foreach
    }

    // Extension methods for conversion
    public static class WorkflowExtensions
    {
        public static WorkflowNodeType ToWorkflowNodeType(this string activityFeatureType)
        {
            return activityFeatureType switch
            {
                "RegularBlock" => WorkflowNodeType.Basic,
                "Foreach" => WorkflowNodeType.ForEach,
                _ => WorkflowNodeType.Basic
            };
        }

        public static WorkflowNodeType ToWorkflowNodeType(this WorkflowActivity activity)
        {
            if (activity.ActivityFeatureType == "Foreach")
                return WorkflowNodeType.ForEach;

            var name = activity.Name.ToLower();
            
            if (name.Contains("start") || activity.StepNo == 1)
                return WorkflowNodeType.Start;
                
            if (name.Contains("condition") || name.Contains("check") || 
                activity.WorkflowActivityJumps.Any(j => j.JumpType == "True" || j.JumpType == "False"))
                return WorkflowNodeType.IfCondition;
                
            if (name.Contains("switch"))
                return WorkflowNodeType.Switch;
                
            if (name.Contains("scope"))
                return WorkflowNodeType.Scope;
                
            if (name.Contains("end") || name.Contains("complete") || name.Contains("done") || 
                activity.WorkflowActivityJumps.Count == 0)
                return WorkflowNodeType.End;
                
            return WorkflowNodeType.Basic;
        }
    }
} 