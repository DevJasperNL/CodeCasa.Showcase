using CodeCasa.CustomEntities.Core.Switches;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Automation.Switches
{
    public class OfficeWallModuleSingleRocker(ITriggerManager triggerManager)
        : HueWallModuleSingleRocker(triggerManager, SwitchDeviceNames.OfficeWallSwitch);
}
