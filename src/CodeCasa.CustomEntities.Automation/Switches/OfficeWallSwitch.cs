using CodeCasa.CustomEntities.Core.Switches;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Automation.Switches
{
    public class OfficeWallSwitch(ITriggerManager triggerManager)
        : HueWallModuleSingleRocker(triggerManager, SwitchDeviceNames.OfficeWallSwitch);
}
