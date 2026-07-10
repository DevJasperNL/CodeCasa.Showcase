using CodeCasa.CustomEntities.Core.Switches;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Automation.Switches
{
    public class AtticDimmerSwitch(ITriggerManager triggerManager)
        : HueDimmerSwitch(triggerManager, SwitchDeviceNames.AtticDimmerSwitch);
}
