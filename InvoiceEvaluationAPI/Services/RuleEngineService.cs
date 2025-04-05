using System.Text.Json;
using InvoiceEvaluationAPI.Models;

public class RuleEngineService
{
    private readonly List<Rule> _rules;

    public RuleEngineService()
    {
        // Load rules from JSON file
        var rulesJson = File.ReadAllText("Data/rules.json");
        _rules = JsonSerializer.Deserialize<List<Rule>>(rulesJson);
    }

    public List<string> ApplyRules(decimal amount, string classification, string riskLevel)
    {
        var actions = new List<string>();

        foreach (var rule in _rules)
        {
            if (EvaluateCondition(rule.Condition, amount, classification, riskLevel))
            {
                actions.Add(rule.Action);
            }
        }

        // If no actions match, return "No Action"
        return actions.Count > 0 ? actions : new List<string> { "No Action" };
    }

    private bool EvaluateCondition(string condition, decimal amount, string classification, string riskLevel)
    {
        // Split condition by logical operator '&&' or '||'
        var conditionParts = SplitConditionByLogicalOperators(condition);

        // Initialize the result based on the first part
        bool result = EvaluateSingleCondition(conditionParts[0], amount, classification, riskLevel);

        // Iterate through the rest of the parts and combine them using logical operators
        for (int i = 1; i < conditionParts.Length; i++)
        {
            string currentPart = conditionParts[i];

            // Check the operator in the previous part
            bool partResult = EvaluateSingleCondition(currentPart, amount, classification, riskLevel);

            if (condition.Contains("&&", StringComparison.OrdinalIgnoreCase))
            {
                result = result && partResult;
            }
            else if (condition.Contains("||", StringComparison.OrdinalIgnoreCase))
            {
                result = result || partResult;
            }
        }

        return result;
    }

    private bool EvaluateSingleCondition(string condition, decimal amount, string classification, string riskLevel)
    {
        // Replace placeholders with actual values if present
        condition = ReplaceConditionVariables(condition, amount, classification, riskLevel);

        // Evaluate the individual condition based on operators (<, >, ==, !=)
        return EvaluateSimpleCondition(condition);
    }

    private string[] SplitConditionByLogicalOperators(string condition)
    {
        // Split the condition into parts by logical operators '&&' or '||'
        var parts = condition.Split(new[] { "&&", "||" }, StringSplitOptions.None);

        // Return the parts of the condition
        return parts.Select(p => p.Trim()).ToArray();
    }


    private string ReplaceConditionVariables(string condition, decimal amount, string classification, string riskLevel)
    {
        // Replace 'amount', 'classification', 'riskLevel' with actual values
        if (condition.Contains("amount"))
        {
            condition = condition.Replace("amount", amount.ToString());
        }

        if (condition.Contains("classification"))
        {
            condition = condition.Replace("classification", $"\"{classification}\"");
        }

        if (condition.Contains("riskLevel"))
        {
            condition = condition.Replace("riskLevel", $"\"{riskLevel}\"");
        }

        return condition;
    }

    private bool EvaluateSimpleCondition(string condition)
    {
        // Basic evaluation for conditions like <, >, ==, !=
        if (condition.Contains("<"))
        {
            var parts = condition.Split('<');
            return Convert.ToDecimal(parts[0].Trim()) < Convert.ToDecimal(parts[1].Trim());
        }
        else if (condition.Contains(">"))
        {
            var parts = condition.Split('>');
            return Convert.ToDecimal(parts[0].Trim()) > Convert.ToDecimal(parts[1].Trim());
        }
        else if (condition.Contains("=="))
        {
            var parts = condition.Split(new[] { "==" }, StringSplitOptions.None);
            return parts[0].Trim() == parts[1].Trim();
        }
        else if (condition.Contains("!="))
        {
            var parts = condition.Split(new[] { "!=" }, StringSplitOptions.None);
            return parts[0].Trim() != parts[1].Trim();
        }

        return false;
    }

}

