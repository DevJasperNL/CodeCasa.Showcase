using CodeCasa.CustomEntities.Core.Switches;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Automation.Switches
{
    public class BedroomNightstandLeftSwitch(ITriggerManager triggerManager)
        : IkeaRodretDimmer(triggerManager, SwitchDeviceNames.BedroomNightstandLeftSwitch);
}
