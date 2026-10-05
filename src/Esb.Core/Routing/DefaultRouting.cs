namespace Esb.Core.Routing;

/// <summary>
/// Стандартная таблица маршрутов для холдинга.
/// </summary>
public static class DefaultRouting
{
    public static RoutingTable Build() => new(new[]
    {
        new RoutingRule("1C_ERP", "CRM", "crm_inbox"),
        new RoutingRule("CRM", "1C_ERP", "erp_inbox"),
        new RoutingRule("1C_ERP", "BI", "bi_stream"),
        new RoutingRule("1C_UT", "WMS", "wms_inbox"),
        new RoutingRule("WMS", "1C_UT", "ut_inbox"),
        new RoutingRule("1C_ZUP", "1C_ERP", "erp_inbox"),
        new RoutingRule("1C_ERP", "1C_ZUP", "zup_inbox"),
        new RoutingRule("SAP", "1C_ERP", "erp_inbox"),
    });
}
