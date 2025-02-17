using LofiBeats.Core.Playback;
using LofiBeats.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace LofiBeats.Tests.Integration;

public class ServiceTestFixture : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Replace the real AudioPlaybackService with our test version
            var audioDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAudioPlaybackService));
            if (audioDescriptor != null)
            {
                services.Remove(audioDescriptor);
            }
            services.AddSingleton<IAudioPlaybackService, TestAudioPlaybackService>();
        });
    }
} 