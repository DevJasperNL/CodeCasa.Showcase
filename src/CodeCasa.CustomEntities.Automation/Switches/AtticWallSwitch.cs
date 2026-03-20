using CodeCasa.CustomEntities.Core.Switches;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Automation.Switches
{
    public class AtticWallSwitch(ITriggerManager triggerManager)
        : HueWallModuleSingleRocker(triggerManager, SwitchDeviceNames.AtticHallwayWallSwitch);
}
