using CodeCasa.CustomEntities.Core.Switches;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Automation.Switches
{
    public class BedroomNightstandRightSwitch(ITriggerManager triggerManager)
        : IkeaRodretDimmer(triggerManager, SwitchDeviceNames.BedroomNightstandRightSwitch);
}
