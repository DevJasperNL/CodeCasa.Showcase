using CodeCasa.CustomEntities.Core.Switches;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Automation.Switches
{
    public class OfficeDimmerSwitch(ITriggerManager triggerManager)
        : HueDimmerSwitch(triggerManager, SwitchDeviceNames.OfficeDeskDimmerSwitch);
}
