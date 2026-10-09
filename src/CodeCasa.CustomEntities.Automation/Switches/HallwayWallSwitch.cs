using CodeCasa.CustomEntities.Core.Switches;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Automation.Switches
{
    public class HallwayWallSwitch(ITriggerManager triggerManager)
        : HueWallModuleSingleRocker(triggerManager, SwitchDeviceNames.HallwayWallSwitch);
}
