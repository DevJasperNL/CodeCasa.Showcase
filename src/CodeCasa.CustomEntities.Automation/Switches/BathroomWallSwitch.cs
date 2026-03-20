using CodeCasa.CustomEntities.Core.Switches;
using NetDaemon.HassModel;

namespace CodeCasa.CustomEntities.Automation.Switches
{
    public class BathroomWallSwitch(ITriggerManager triggerManager)
        : HueWallModuleSingleRocker(triggerManager, SwitchDeviceNames.BathroomWallSwitch);
}
