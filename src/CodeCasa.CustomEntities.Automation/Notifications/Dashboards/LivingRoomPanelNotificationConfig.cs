using System.Text.Json;
using System.Text.Json.Serialization;
using CodeCasa.CustomEntities.Core.Notifications;
using CodeCasa.Notifications.InputSelect.NetDaemon.Config;

namespace CodeCasa.CustomEntities.Automation.Notifications.Dashboards
{
    public record LivingRoomPanelNotificationConfig(string Message) : IInputSelectNotificationConfig
    {
            private const int MaxMessageLength = 255;

            public LivingRoomPanelNotificationConfig() : this(string.Empty)
            {
            }

            /// <summary>
            /// Gets or sets the main message displayed in the notification.
            /// </summary>
            public string Message { get; set; } = Message;

            /// <summary>
            /// Gets or sets an optional secondary message to display alongside the main message.
            /// </summary>
            public string? SecondaryMessage { get; set; }

            /// <summary>
            /// Gets or sets the optional icon name or URI to display with the notification.
            /// </summary>
            public string? Icon { get; set; }

            /// <inheritdoc/>
            public TimeSpan? Timeout { get; set; }

            /// <inheritdoc/>
            public Action? Action { get; set; }

            /// <inheritdoc/>
            public int? Order { get; set; }

            /// <inheritdoc />
            public string ToInputSelectOptionString()
            {
                var notificationInfo = new LivingRoomPanelNotification
                {
                    Message = Message,
                    SecondaryMessage = SecondaryMessage,
                    Icon = Icon,
                    TimeStamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ")
                };

                var jsonSerializerOptions = new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

                var json = JsonSerializer.Serialize(notificationInfo, jsonSerializerOptions);
                if (json.Length > MaxMessageLength)
                {
                    throw new InvalidOperationException("Resulting json is too large for input select option.");
                }

                return json;
            }
    }
}
