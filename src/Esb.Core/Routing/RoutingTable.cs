namespace Esb.Core.Routing;

/// <summary>
/// Таблица маршрутов ESB.
/// </summary>
public class RoutingTable
{
    private readonly List<RoutingRule> _rules;

    public RoutingTable(IEnumerable<RoutingRule> rules)
    {
        _rules = rules.ToList();
    }

    /// <summary>
    /// Находит очередь для пары (source, target).
    /// </summary>
    public string? ResolveQueue(string source, string target)
    {
        var rule = _rules.FirstOrDefault(r =>
            r.Enabled &&
            string.Equals(r.Source, source, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(r.Target, target, StringComparison.OrdinalIgnoreCase));

        return rule?.QueueName;
    }

    public IReadOnlyList<RoutingRule> All => _rules;
}
