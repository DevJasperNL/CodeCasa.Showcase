using CodeCasa.AutomationPipelines;
using CodeCasa.AutomationPipelines.Lights.Context;
using CodeCasa.Lights;

namespace CodeCasa.Notifications.Light.Config;

public class LightNotificationConfig
{
    public LightNotificationOptions Options { get; }
    public Type? NodeType { get; }
    public Func<ILightPipelineContext, IPipelineNode<LightTransition>>? NodeFactory { get; }

    internal LightNotificationConfig(LightNotificationOptions options, Type nodeType)
    {
        Options = options;
        NodeType = nodeType;
    }

    internal LightNotificationConfig(LightNotificationOptions options, Func<ILightPipelineContext, IPipelineNode<LightTransition>> nodeFactory)
    {
        Options = options;
        NodeFactory = nodeFactory;
    }
}