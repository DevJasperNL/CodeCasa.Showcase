# Jasper's Code Casa 🏡

[![Build Status](https://github.com/DevJasperNL/CodeCasa/actions/workflows/ci-build-and-test.yml/badge.svg)](https://github.com/DevJasperNL/CodeCasa/actions/workflows/ci-build-and-test.yml)

A smart home implementation example using C# and NetDaemon.

This repository explores creative and powerful ways to use a rich programming language like C# for home automation. From custom logic to seamless integrations, you'll find practical examples and unique ideas to elevate your smart home setup. Stay tuned for ongoing updates and new features!

## 📖 Table of Contents
- [Architectures & Implementations](#🛠️-architectures--implementations)
    - [Blazor Frontend (NSPanel Pro)](#blazor-frontend-nspanel-pro)
    - [People](#people)
    - [Phone Notifications](#phone-notifications)
    - [Input Select Notifications](#input-select-notifications)
    - [Light Pipelines](#light-pipelines)
- [Projects Overview](#🔧-projects-overview)
- [Local Debugging with CodeCasa Projects](#local-debugging-with-codecasa-projects)
    - [CodeCasa.Showcase.withLocalCodeCasa.sln](#codecasashowcasewithlocalcodecasasln)
    - [Requirements](#requirements)

## 🛠️ Architectures & Implementations

One of the great advantages of using a general-purpose programming language like C# for home automations is the ability to introduce your own architectural patterns. This chapter highlights some of the patterns used in this example project.

### Blazor Frontend (NSPanel Pro)

I've never been a fan of large tablets that display every available entity. Part of making a home "smart" is tailoring it to show only the information the inhabitants actually care about. That’s why I opted for an NSPanel Pro instead.

It was a fun challenge to get it working in a smooth and intuitive way. Rather than using a standard Home Assistant dashboard, I built a **custom dashboard using Blazor**. Here’s a preview:

![Gif demonstrating NSPanel Pro Interaction](img/nspanel_pro_interaction_demo.gif "NSPanel Pro interaction demo")
![Gif demonstrating NSPanel Pro Doorbell feed](img/nspanel_pro_doorbell_demo.gif "NSPanel Pro doorbell demo")

The source code for this dashboard is available in this repository.  

#### Key Features
- **Custom webview:** Using the posts from [Blakadder](https://blakadder.com/nspanel-pro-sideload/)
- **Proximity detection:** Managed by the Automate app, which calls a Home Assistant webhook. The webhook triggers an automation that the panel subscribes to.
- **Reolink Doorbell feed:** Displays a WebRTC feed when a person or doorbell press is detected. See [Components](src/CodeCasa.Dashboard/Components/Dashboard/DoorbellStream.razor).
- **Blazor Dashboard:** The razor page/components can be found here: [Components](src/CodeCasa.Dashboard/Components).
- **Panel state:** Stored in an input select value and managed in [LivingRoomPanelNavigation.cs](src/CodeCasa.Automations/Apps/Dashboard/LivingRoomPanelNavigation.cs).
- **Google timers & alarms:** Implemented via the HACS integration [ha-google-home](https://github.com/leikoilja/ha-google-home).
- **Interactive notifications:** Powered by [Input Select Notifications](#input-select-notifications).

### People

This demo shows how to use a custom compound entity to group related properties and services for a person.

In this example, the `Jasper` class provides Jasper’s name and location and making it easy to trigger context-aware notifications to his phone:

```cs
[NetDaemonApp]
internal class OfficeLightsNotifications
{
    public OfficeLightsNotifications(
        LightEntities lightEntities,
        Jasper jasper)
    {
        var notificationId = $"{nameof(OfficeLightsNotifications)}_Notification"; // Note: Using an ID that is consistent between runs also ensures that old notifications are removed/replaced on phones when the app is reloaded.

        var officeLights = lightEntities.OfficeLights.ToOnOffObservable();
        var jasperHome = jasper.HomeWithCurrent();

        // Only notify Jasper if he is at home and the lights are on.
        jasperHome.And(officeLights).SubscribeOnOff(
            () =>
            {
                jasper.Phone.Notify(new AndroidNotificationConfig
                {
                    Message = $"Hey {jasper.Name}, the office lights are on!",
                    StatusBarIcon = "mdi:lightbulb",
                    Actions =
                    [
                        new(() => lightEntities.OfficeLights.TurnOff(), "Click here to turn them off.")
                    ]
                }, notificationId);
            },
            () => jasper.Phone.RemoveNotification(notificationId));
    }
}
```

- The code from this `NetDaemonApp`: [OfficeLightsNotifications.cs](src/CodeCasa.Automations/Apps/Notifications/OfficeLightsNotifications.cs)
- The custom Jasper entity: [Jasper.cs](src/CodeCasa.CustomEntities.Automation/People/Jasper.cs)

### Phone Notifications

This project showcases the use of phone notification, built with the `CodeCasa.NetDaemon.Notifications.Phone` library from [`NetDaemon.Utils`](https://github.com/DevJasperNL/NetDaemon.Utils).

Here’s a preview of the notifications in action:

![Gif demonstrating phone notifications](img/phone_notification_demo.gif "Phone Notifications")

For detailed usage and setup instructions, see the [`CodeCasa.NetDaemon.Notifications.Phone` documentation](https://github.com/DevJasperNL/NetDaemon.Utils?tab=readme-ov-file#codecasanetdaemonnotificationsphone).

- The `NetDaemonApp` demo code: [PhoneDemoNotifications.cs](src/CodeCasa.Automations/Apps/Notifications/PhoneDemoNotifications.cs)

### Input Select Notifications

This project also showcases **rich notifications** using an input select entity, built with the `CodeCasa.NetDaemon.Notifications.InputSelect` library from [`NetDaemon.Utils`](https://github.com/DevJasperNL/NetDaemon.Utils).

Here’s a preview of the notifications in action:

![Gif demonstrating dashboard notifications](img/blazor_dashboard_notification_demo.gif "Dashboard Notifications")

For detailed usage and setup instructions, see the [`CodeCasa.NetDaemon.Notifications.InputSelect` documentation](https://github.com/DevJasperNL/NetDaemon.Utils?tab=readme-ov-file#codecasanetdaemonnotificationsinputselect).

- The `NetDaemonApp` demo code: [DashboardDemoNotifications.cs](src/CodeCasa.Automations/Apps/Notifications/DashboardDemoNotifications.cs)
- The Blazor component: [Notifications.razor](src/CodeCasa.Dashboard/Components/Dashboard/Notifications.razor)

### Light Pipelines

The lights in this project are driven by the [`CodeCasa.AutomationPipelines.Lights`](https://github.com/DevJasperNL/CodeCasa) libraries. Every light gets a **pipeline**: an ordered list of nodes, one per concern (motion, a switch, a notification). A node either sets its own output or passes through the output of the node before it. Later nodes win, so the order of the list is the priority, and the light is only ever driven from the end of the pipeline. When an override ends, nothing needs to be restored: the layers below never stopped tracking their inputs.

Setup is three lines in [ServiceCollectionExtensions.cs](src/CodeCasa.Automations/Extensions/ServiceCollectionExtensions.cs): `AddLightPipelines()`, `AddLightScenes()` for Home Assistant scenes and `AddLightNotifications()` for light notifications.

The rooms below are copied from my own house. Switches and motion sensors are small wrappers around Zigbee2MQTT actions and NetDaemon entities, see [Switches](src/CodeCasa.CustomEntities.Core/Switches) and [Sensors](src/CodeCasa.CustomEntities.Automation/Sensors).

#### Hallway: motion, day and night, and a timed override

```cs
var night = PeriodTimeline.Between(AstroInstants.LocalSunsets, AstroInstants.LocalSunrises).ToBooleanObservable(scheduler);

lightPipelineFactory.SetupLightPipeline(lightEntities.HallwayLight, pipeline => pipeline
    .EnableLogging("Hallway")
    .SwitchWhen(hallwayMotionSensor, night, LightParameters.NightLight, LightParameters.Bright)
    .AddInteractionNode(node => node
        .AddToggle(hallwayWallSwitch, sp => sp.CreateAutoPassThroughLightNode(LightParameters.Bright, TimeSpan.FromMinutes(5)))));
```

- `hallwayMotionSensor` combines the occupancy and illuminance sensors and stays `true` for a minute after the last movement. `SwitchWhen` gives a night light between sunset and sunrise and bright light during the day.
- The wall switch sits in a later node, so it wins: five minutes of bright light, after which the node passes through and motion is in charge again. Pressing the switch while the light is on turns it off until motion changes.
- `AddInteractionNode` also makes a manual off, from the Home Assistant app for example, stick until an earlier node changes.
- `EnableLogging` logs every hop between nodes, so you can read which layer produced the final state.

- The `NetDaemonApp`: [HallwayLightsApp.cs](src/CodeCasa.Automations/Apps/Lights/Hallway/HallwayLightsApp.cs)
- The motion sensor: [HallwayMotionSensor.cs](src/CodeCasa.CustomEntities.Automation/Sensors/HallwayMotionSensor.cs), the switch: [HallwayWallSwitch.cs](src/CodeCasa.CustomEntities.Automation/Switches/HallwayWallSwitch.cs)

#### Office: wall switch, Hue dimmer switch and a Zigbee group

```cs
lightPipelineFactory.SetupLightPipeline(lightEntities.OfficeLights, pipeline => pipeline
    .UseLightGroup(lightEntities.OfficeLightsZ2m)
    .AddInteractionNode(node => node
        .AddToggle(officeWallSwitch, LightParameters.Bright)
        .AddToggle(officeDimmerSwitch.OnOffPressed, LightParameters.Bright)
        .AddCycle(officeDimmerSwitch.ScenePressed, LightParameters.Bright, LightParameters.Concentrate, LightParameters.Relax)
        .AddReactiveDimmer(officeDimmerSwitch)
        .TurnOffWhenLastPersonToAsleepOrAway()));
```

- `AddToggle` toggles between off and bright, `AddCycle` walks through scenes based on the **actual** state of the lights, and `AddReactiveDimmer` implements hold to dim.
- `UseLightGroup` sends one command to the Zigbee group when all bulbs receive the same transition, so the room changes at once.
- `TurnOffWhenLastPersonToAsleepOrAway` is my own extension method, built on the same `AddNodeSource` primitive as the library's methods.

- The `NetDaemonApp`: [OfficeLightsApp.cs](src/CodeCasa.Automations/Apps/Lights/Office/OfficeLightsApp.cs)
- The dimmer switch wrapper: [HueDimmerSwitch.cs](src/CodeCasa.CustomEntities.Core/Switches/HueDimmerSwitch.cs), the custom extension: [LightTransitionReactiveNodeConfiguratorExtensions.cs](src/CodeCasa.CustomEntities.Automation/Extensions/LightTransitionReactiveNodeConfiguratorExtensions.cs)

#### Living room: Home Assistant scenes and notifications

```cs
lightPipelineFactory.SetupLightPipeline(lightEntities.LivingRoomLights, pipeline => pipeline
    .UseLightGroup(lightEntities.LivingRoomLightsZ2m)
    .AddInteractionNode(node => node
        .AddToggle(livingRoomWallSwitch,
            sceneEntities.LivingRoomAmbiance,
            sceneEntities.LivingRoomRelax,
            sceneEntities.LivingRoomBright)
        .TurnOffWhenLastPersonToAsleepOrAway())
    .AddNotifications());
```

Scenes made in the Home Assistant scene editor are accepted wherever light parameters are. `AddNotifications` lets any other app take the lights over for a while, without knowing which pipelines exist. The doorbell does exactly that, with a custom blinking node:

```cs
frontDoorDoorbell.BellPressed
    .PersistTrueFor(TimeSpan.FromSeconds(30), scheduler)
    .SubscribeTrueFalse(
        () => lightNotifications.Notify(notificationId, sp => new BlinkNode(scheduler, Color.Blue, Color.White), priority: 10),
        () => lightNotifications.RemoveNotification(notificationId));
```

- The `NetDaemonApp`: [LivingRoomLightsApp.cs](src/CodeCasa.Automations/Apps/Lights/LivingRoom/LivingRoomLightsApp.cs)
- The doorbell notification: [DoorbellLightNotifications.cs](src/CodeCasa.Automations/Apps/Notifications/DoorbellLightNotifications.cs), the node: [BlinkNode.cs](src/CodeCasa.Automations/Nodes/BlinkNode.cs)

#### Attic: natural light

```cs
Action<ITimelineConfigurator> naturalLight = tl => tl
    .Add(AstroInstants.LocalSunrises, LightParameters.Bright)
    .Add(AstroInstants.LocalSunsets.OffsetHours(-2), LightParameters.Bright)
    .Add(AstroInstants.LocalSunsets, LightParameters.Dimmed)
    .Add(TimeZoneInstants.DailyAt(5), LightParameters.Dimmed);

lightPipelineFactory.SetupLightPipeline(lightEntities.AtticLights, pipeline => pipeline
    .UseLightGroup(lightEntities.AtticLightsZ2m)
    .AddInteractionNode(node => node
        .AddToggle(atticWallSwitch, naturalLight)
        .AddToggle(atticDimmerSwitch.OnOffPressed, naturalLight)
        .AddCycle(atticDimmerSwitch.ScenePressed, slightlyBright, LightParameters.Relax)
        .AddReactiveDimmer(atticDimmerSwitch)
        .TurnOffWhenLastPersonToAsleepOrAway())
    .AddNotifications());
```

A timeline, built with [Occurify](https://github.com/DevJasperNL/Occurify), maps moments of the day to light parameters and the pipeline interpolates between them continuously. Here the switches toggle the timeline instead of a fixed scene.

- The `NetDaemonApp`: [AtticLightsApp.cs](src/CodeCasa.Automations/Apps/Lights/Attic/AtticLightsApp.cs)

#### Backyard: one pipeline, six lights

```cs
lightPipelineFactory.SetupLightPipeline(lightEntities.BackyardLights, pipeline => pipeline
    .ForLight(lightEntities.BackyardPorchStringLights, light => light
        .When(new BackyardLightsRoutine(scheduler, TimeSpan.Zero), LightParameters.On()))
    .ForLight(lightEntities.BackyardPergolaStringLights, light => light
        .When(new BackyardLightsRoutine(scheduler, TimeSpan.FromSeconds(1)), LightParameters.On()))
    .ForLight(lightEntities.BackyardFenceStringLights, light => light
        .When(new BackyardLightsRoutine(scheduler, TimeSpan.FromSeconds(2)), LightParameters.On()))
    .ForLights([lightEntities.BackyardGarageLight, lightEntities.BackyardEntranceLight, lightEntities.BackyardDoorLight], lights => lights
        .When(new BackyardLightsRoutine(scheduler, TimeSpan.FromSeconds(10)), LightParameters.Relax.AsTransitionInSeconds(4)))
    .TurnOffWhen<BackyardLightsEnergySaving>());
```

The pipeline is set up for the Home Assistant group, and `ForLight`/`ForLights` scope nodes to some of its members: the string lights turn on one second apart, the wall lights follow ten seconds later. The energy saving node applies to all of them.

- The `NetDaemonApp`: [BackyardLightsApp.cs](src/CodeCasa.Automations/Apps/Lights/Backyard/BackyardLightsApp.cs)
- The observables: [BackyardLightsRoutine.cs](src/CodeCasa.Automations/Apps/Lights/Backyard/Observables/BackyardLightsRoutine.cs), [BackyardLightsEnergySaving.cs](src/CodeCasa.Automations/Apps/Lights/Backyard/Observables/BackyardLightsEnergySaving.cs)

For the full API see the [CodeCasa repository](https://github.com/DevJasperNL/CodeCasa) and the NuGet packages [CodeCasa.AutomationPipelines.Lights](https://www.nuget.org/packages/CodeCasa.AutomationPipelines.Lights), [CodeCasa.AutomationPipelines.Lights.NetDaemon](https://www.nuget.org/packages/CodeCasa.AutomationPipelines.Lights.NetDaemon) and [CodeCasa.Notifications.Lights](https://www.nuget.org/packages/CodeCasa.Notifications.Lights).

## 🔧 Projects Overview

### 🤖 Automations (`CodeCasa.Automations`)

This project contains the NetDaemon automations. It runs as a console application and can be hosted as a container.

### 📊 Blazor Dashboard (`CodeCasa.Dashboard`)

A Blazor-based web dashboard that demonstrates how to integrate with Home Assistant. This project showcases how to build responsive, interactive UIs that control and reflect your smart home’s state in real-time.

### 🧬 Auto-Generated Code (`CodeCasa.AutoGenerated`)

NetDaemon can auto-generate strongly-typed classes based on the entities in your Home Assistant configuration. For this demo, a curated selection of generated code is included to illustrate how this feature simplifies development and enhances type safety.

### 🧩 Custom Entities (`CodeCasa.CustomEntities.Core`/`CodeCasa.CustomEntities.Automation`)

These projects combine existing entities into compound entities or creates entirely new entities tailored to specific automation or dashboard scenarios. It also includes helper constants to simplify and standardize usage across automations and dashboards.

### 🛠️ NetDaemon Utilities (`CodeCasa.NetDaemon.Utilities`)

A collection of utility classes for working with NetDaemon entities. These utilities are tailored to the use-cases of this project but may be useful for similar implementations in other projects.

## Local Debugging with CodeCasa Projects

This repository can optionally use **local versions of the CodeCasa libraries** for debugging instead of the NuGet packages.

### `CodeCasa.Showcase.withLocalCodeCasa.sln`

- A local solution file that **includes all showcase projects and any local CodeCasa projects**.
- Opening this solution **automatically activates the project reference replacement**, so NuGet packages are replaced with local projects.
- Breakpoints in CodeCasa projects work normally, and projects appear in Solution Explorer.

### Requirements

- You must **checkout the [CodeCasa repository](https://github.com/DevJasperNL/CodeCasa)** as a sibling of this repo (same parent directory as this repo).