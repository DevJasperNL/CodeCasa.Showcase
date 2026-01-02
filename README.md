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
    - [Automation Pipelines](#automation-pipelines)
- [Projects Overview](#🔧-projects-overview)
- [Local Debugging with CodeCasa Projects](#local-debugging-with-codecasa-projects)
    - [CodeCasa.Showcase.withLocalCodeCasa.sln](#codecasashowcasewithlocalcodecasasln)
    - [Requirements](#requirements)

## 🛠️ Architectures & Implementations

One of the great advantages of using a general-purpose programming language like C# for home automations is the ability to introduce your own architectural patterns. This chapter highlights some of the patterns used in this example project.

### Blazor Frontend (NSPanel Pro)

I've never been a fan of large tablets that display every available entity. Part of making a home "smart" is tailoring it to show only the information the inhabitants actually care about. That’s why I opted for an NSPanel Pro instead.

It was a fun challenge to get it working in a smooth and intuitive way. Rather than using a standard Home Assistant dashboard, I built a **custom dashboard using Blazor**. Here’s a preview:

![Gif demonstrating NSPanel Pro](img/nspanel_pro_demo.gif "NSPanel Pro demo")

The source code for this dashboard is available in this repository.  

#### Key Features
- **Custom webview** Using the posts from [Blakadder](https://blakadder.com/nspanel-pro-sideload/)
- **Proximity detection:** Managed by the Automate app, which calls a Home Assistant webhook. The webhook triggers an automation that the panel subscribes to.
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
        var jasperHome = jasper.CreateHomeObservable();

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

### Automation Pipelines

This automation uses the [`AutomationPipelines`](https://github.com/DevJasperNL/CodeCasa.Libraries) library to handle complex logic in a modular, layered way.

Rather than implementing behavior directly in a single class, logic is split into small, independent pipeline nodes. Each node can contribute to or override the final outcome based on its own conditions. This makes the automation easier to reason about, test, and extend.

Below is the setup used in the `BackyardStringLightsPipeline` app:

```cs
backyardPorchStringLightsPipeline
    .SetDefault(false)
    .RegisterNode(new LightStringRoutineNode<bool>(scheduler, true, TimeSpan.Zero))
    .RegisterNode<BackyardStringLightsEnergySavingNode>()
    .SetOutputHandler(b => UpdateLightState(lightEntities.BackyardPorchStringLights, b));
```

In this example:
- The pipeline starts with a default state of false (lights off).
- The first node (LightStringRoutineNode) schedules the lights to turn on during morning and evening hours.
- The second node (`BackyardStringLightsEnergySavingNode`) can turn them off again if all curtains are closed.
- Finally, `SetOutputHandler` applies the resulting output to the actual light entity.

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