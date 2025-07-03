using System.Text.Json;
using Blazor.Diagrams.Core.Geometry;
using WorkflowBuilder.Models;

namespace WorkflowBuilder.Services
{
    public class WorkflowService
    {
        private readonly HttpClient _httpClient;

        public WorkflowService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WorkflowData?> LoadWorkflowFromJson(string jsonPath)
        {
            try
            {
                var response = await _httpClient.GetAsync(jsonPath);
                if (response.IsSuccessStatusCode)
                {
                    var jsonContent = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    return JsonSerializer.Deserialize<WorkflowData>(jsonContent, options);
                }
            }
            catch
            {
            }
            return null;
        }

        public List<WorkflowNode> ConvertToWorkflowNodes(WorkflowData workflowData)
        {
            var nodes = new List<WorkflowNode>();
            var nodePositions = CalculateNodePositions(workflowData.WorkflowActivities);

            foreach (var activity in workflowData.WorkflowActivities)
            {
                var position = nodePositions.GetValueOrDefault(activity.StepNo, new Point(100, 100));
                var node = new WorkflowNode(activity, position);
                nodes.Add(node);
            }

            return nodes;
        }

        private Dictionary<int, Point> CalculateNodePositions(List<WorkflowActivity> activities)
        {
            var positions = new Dictionary<int, Point>();
            var startX = 300;
            var startY = 100;
            var spacingX = 200;
            var spacingY = 150;

            var sortedActivities = activities.OrderBy(a => a.StepNo).ToList();
            
            for (int i = 0; i < sortedActivities.Count; i++)
            {
                var activity = sortedActivities[i];
                
                if (activity.StepNo <= 3)
                {
                    positions[activity.StepNo] = new Point(startX + (activity.StepNo - 1) * spacingX, startY);
                }
                else
                {
                    positions[activity.StepNo] = new Point(startX + (activity.StepNo - 1) * spacingX, startY + spacingY);
                }
            }

            return positions;
        }

        public List<(WorkflowNode from, WorkflowNode to, string jumpType)> CreateLinks(
            List<WorkflowNode> nodes, 
            List<WorkflowActivity> activities)
        {
            var links = new List<(WorkflowNode from, WorkflowNode to, string jumpType)>();
            var nodeDict = nodes.ToDictionary(n => n.StepNo, n => n);

            foreach (var activity in activities)
            {
                if (nodeDict.TryGetValue(activity.StepNo, out var fromNode))
                {
                    foreach (var jump in activity.WorkflowActivityJumps)
                    {
                        if (nodeDict.TryGetValue(jump.ToStepNo, out var toNode))
                        {
                            links.Add((fromNode, toNode, jump.JumpType));
                        }
                    }
                }
            }

            return links;
        }
    }
} 