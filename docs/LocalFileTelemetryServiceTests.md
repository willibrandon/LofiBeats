PS D:\SRC\LofiBeats> dotnet test --filter LocalFileTelemetryServiceTests
Restore complete (0.3s)
You are using a preview version of .NET. See: https://aka.ms/dotnet-support-policy
  LofiBeats.Core succeeded (0.1s) → src\LofiBeats.Core\bin\Debug\net9.0\LofiBeats.Core.dll
  LofiBeats.Service succeeded (2.2s) → src\LofiBeats.Service\bin\Debug\net9.0\LofiBeats.Service.dll
  LofiBeats.Cli succeeded (2.3s) → src\LofiBeats.Cli\bin\Debug\net9.0\LofiBeats.Cli.dll
  LofiBeats.Tests succeeded (0.1s) → tests\LofiBeats.Tests\bin\Debug\net9.0\LofiBeats.Tests.dll
[xUnit.net 00:00:00.00] xUnit.net VSTest Adapter v3.0.2+dd36e86129 (64-bit .NET 9.0.1)
[xUnit.net 00:00:00.04]   Discovering: LofiBeats.Tests
[xUnit.net 00:00:00.06]   Discovered:  LofiBeats.Tests
[xUnit.net 00:00:00.07]   Starting:    LofiBeats.Tests
[xUnit.net 00:00:00.25]     LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackMetric_CreatesValidTelemetryFile [FAIL]
[xUnit.net 00:00:00.25]       Assert.Single() Failure: The collection contained 33 items
[xUnit.net 00:00:00.25]       Collection: [TelemetryMetric { Name = "Metric1", Properties = [], SessionId = "34cd9a8d-33a8-40ac-b375-c8d0f188d268", Timestamp = 2025-02-15T04:50:10.7446513+00:00, Value = 42 }, TelemetryMetric { Name = "TimestampMetric", Properties = [], SessionId = "75864c4b-1650-4a34-b59b-9d6dcd405c26", Timestamp = 2025-02-15T04:50:10.8549740+00:00, Value = 42 }, TelemetryMetric { Name = "Performance.TestOperation", Properties = [], SessionId = "4b1a3931-0d10-47bd-8bf5-b05896bbaedf", Timestamp = 2025-02-15T04:50:10.8702490+00:00, Value = 100 }, TelemetryMetric { Name = "TestMetric", Properties = [], SessionId = "cf917c53-dbef-4286-86a7-c7ace4952b31", Timestamp = 2025-02-15T04:50:10.6668040+00:00, Value = 42 }, TelemetryMetric { Name = "TimestampMetric", Properties = [], SessionId = "0bdc5d91-422b-46ce-a497-365342853ea1", Timestamp = 2025-02-15T04:52:39.2103357+00:00, Value = 42 }, ···]
[xUnit.net 00:00:00.25]       Stack Trace:
[xUnit.net 00:00:00.25]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(105,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackMetric_CreatesValidTelemetryFile()
[xUnit.net 00:00:00.25]         --- End of stack trace from previous location ---
[xUnit.net 00:00:00.30]     LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_CreatesValidTelemetryFile [FAIL]
[xUnit.net 00:00:00.30]       Assert.Single() Failure: The collection contained 896 items
[xUnit.net 00:00:00.30]       Collection: [TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }, TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090010+00:00 }, TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }, TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }, TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }, ···]
[xUnit.net 00:00:00.30]       Stack Trace:
[xUnit.net 00:00:00.30]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(85,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_CreatesValidTelemetryFile()
[xUnit.net 00:00:00.30]         --- End of stack trace from previous location ---
[xUnit.net 00:00:00.33]     LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_AllowsTestEvents_InTestEnvironment [FAIL]
[xUnit.net 00:00:00.33]       Assert.Single() Failure: The collection contained 897 items
[xUnit.net 00:00:00.33]       Collection: [TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }, TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090010+00:00 }, TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }, TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }, TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }, ···]
[xUnit.net 00:00:00.33]       Stack Trace:
[xUnit.net 00:00:00.33]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(407,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_AllowsTestEvents_InTestEnvironment()
[xUnit.net 00:00:00.33]         --- End of stack trace from previous location ---
[xUnit.net 00:00:00.36]     LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.SessionId_IsConsistentAcrossEvents [FAIL]
[xUnit.net 00:00:00.36]       Assert.Single() Failure: The collection contained 108 items
[xUnit.net 00:00:00.36]       Collection: ["f4d51922-448d-49dc-8410-094a4034e595", "42f199d2-34b3-4ef0-889b-48fb90d0a286", "138d7792-0de7-4002-910b-b64cadb31ff1", "19649abd-8362-4cb9-8589-9758f4462487", "334f918b-f4b2-418f-bcce-b8b5550bef96", ···]
[xUnit.net 00:00:00.36]       Stack Trace:
[xUnit.net 00:00:00.36]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(193,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.SessionId_IsConsistentAcrossEvents()
[xUnit.net 00:00:00.36]         --- End of stack trace from previous location ---
[xUnit.net 00:00:00.45]     LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.ConcurrentEvents_AreAllRecorded [FAIL]
[xUnit.net 00:00:00.45]       Assert.Equal() Failure: Values differ
[xUnit.net 00:00:00.45]       Expected: 50
[xUnit.net 00:00:00.45]       Actual:   908
[xUnit.net 00:00:00.45]       Stack Trace:
[xUnit.net 00:00:00.45]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(256,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.ConcurrentEvents_AreAllRecorded()
[xUnit.net 00:00:00.45]         --- End of stack trace from previous location ---
[xUnit.net 00:00:00.48]     LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_RejectsFutureTimestamps [FAIL]
[xUnit.net 00:00:00.48]       Assert.Empty() Failure: Collection was not empty
[xUnit.net 00:00:00.48]       Collection: [TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }, TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090010+00:00 }, TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }, TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }, TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }, ···]
[xUnit.net 00:00:00.48]       Stack Trace:
[xUnit.net 00:00:00.48]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(428,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_RejectsFutureTimestamps()
[xUnit.net 00:00:00.48]         --- End of stack trace from previous location ---
[xUnit.net 00:00:00.51]     LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.LargePropertyValues_AreHandledCorrectly [FAIL]
[xUnit.net 00:00:00.51]       Assert.Single() Failure: The collection contained 910 items
[xUnit.net 00:00:00.51]       Collection: [TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }, TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090010+00:00 }, TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }, TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }, TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }, ···]
[xUnit.net 00:00:00.51]       Stack Trace:
[xUnit.net 00:00:00.51]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(300,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.LargePropertyValues_AreHandledCorrectly()
[xUnit.net 00:00:00.51]         --- End of stack trace from previous location ---
[xUnit.net 00:00:00.54]     LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackException_IncludesExceptionDetails [FAIL]
[xUnit.net 00:00:00.54]       Assert.Single() Failure: The collection contained 911 items
[xUnit.net 00:00:00.54]       Collection: [TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }, TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090010+00:00 }, TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }, TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }, TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }, ···]
[xUnit.net 00:00:00.54]       Stack Trace:
[xUnit.net 00:00:00.54]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(124,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackException_IncludesExceptionDetails()
[xUnit.net 00:00:00.54]         --- End of stack trace from previous location ---
[xUnit.net 00:00:00.60]     LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.BufferFlush_TriggersAtBufferSize [FAIL]
[xUnit.net 00:00:00.60]       Assert.Equal() Failure: Values differ
[xUnit.net 00:00:00.60]       Expected: 101
[xUnit.net 00:00:00.60]       Actual:   1002
[xUnit.net 00:00:00.60]       Stack Trace:
[xUnit.net 00:00:00.60]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(169,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.BufferFlush_TriggersAtBufferSize()
[xUnit.net 00:00:00.60]         --- End of stack trace from previous location ---
[xUnit.net 00:00:00.67]     LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TimestampsAreUtc_AcrossAllTelemetry [FAIL]
[xUnit.net 00:00:00.67]       Assert.All() Failure: 1002 out of 1004 items in the collection did not pass.
[xUnit.net 00:00:00.67]       [0]:    Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [1]:    Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090010+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [2]:    Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [3]:    Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [4]:    Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [5]:    Item:  TelemetryEvent { Name = "ConcurrentEvent_43", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700689+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [6]:    Item:  TelemetryEvent { Name = "ConcurrentEvent_44", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700694+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [7]:    Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700727+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [8]:    Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700734+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [9]:    Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700788+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [10]:   Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700803+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [11]:   Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700811+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [12]:   Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700814+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [13]:   Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "334f918b-f4b2-418f-bcce-b8b5550bef96", Timestamp = 2025-02-15T04:50:10.7962089+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [14]:   Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "c8a349aa-606a-46e6-b598-0f5ed05ff029", Timestamp = 2025-02-15T04:50:10.8901225+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [15]:   Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "34cd9a8d-33a8-40ac-b375-c8d0f188d268", Timestamp = 2025-02-15T04:50:10.7433841+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [16]:   Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "1280d44d-2363-4f53-9521-2125ebb80900", Timestamp = 2025-02-15T04:50:10.8052804+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [17]:   Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "947dc9ab-c4b5-4445-9e24-2b5a81824272", Timestamp = 2025-02-15T04:50:10.7341468+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [18]:   Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "75864c4b-1650-4a34-b59b-9d6dcd405c26", Timestamp = 2025-02-15T04:50:10.8549610+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [19]:   Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "75864c4b-1650-4a34-b59b-9d6dcd405c26", Timestamp = 2025-02-15T04:50:10.8549858+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [20]:   Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "af240f73-6f14-4d05-b58c-5e789effa89a", Timestamp = 2025-02-15T04:50:10.8188358+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [21]:   Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327762+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [22]:   Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327776+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [23]:   Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327780+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [24]:   Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327784+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [25]:   Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327788+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [26]:   Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327791+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [27]:   Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327800+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [28]:   Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327828+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [29]:   Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327832+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [30]:   Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327835+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [31]:   Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327854+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [32]:   Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327860+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [33]:   Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327865+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [34]:   Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327871+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [35]:   Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327904+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [36]:   Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327910+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [37]:   Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327915+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [38]:   Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327920+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [39]:   Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327926+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [40]:   Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327931+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [41]:   Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327936+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [42]:   Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327988+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [43]:   Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327994+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [44]:   Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328000+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [45]:   Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328005+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [46]:   Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328010+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [47]:   Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328047+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [48]:   Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328052+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [49]:   Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328057+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [50]:   Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328062+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [51]:   Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328067+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [52]:   Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328073+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [53]:   Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328078+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [54]:   Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328158+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [55]:   Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328164+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [56]:   Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328169+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [57]:   Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328219+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [58]:   Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328225+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [59]:   Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328230+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [60]:   Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328235+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [61]:   Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328240+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [62]:   Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328278+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [63]:   Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328319+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [64]:   Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328325+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [65]:   Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328331+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [66]:   Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328336+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [67]:   Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328341+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [68]:   Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328346+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [69]:   Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328352+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [70]:   Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328384+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [71]:   Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328389+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [72]:   Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328394+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [73]:   Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328399+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [74]:   Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328405+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [75]:   Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328435+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [76]:   Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328441+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [77]:   Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328446+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [78]:   Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328451+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [79]:   Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328456+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [80]:   Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328462+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [81]:   Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328490+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [82]:   Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328496+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [83]:   Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328501+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [84]:   Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328506+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [85]:   Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328512+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [86]:   Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328517+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [87]:   Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328551+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [88]:   Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328557+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [89]:   Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328562+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [90]:   Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328567+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [91]:   Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328572+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [92]:   Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328577+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [93]:   Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328583+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [94]:   Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328612+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [95]:   Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328617+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [96]:   Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328622+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [97]:   Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328628+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [98]:   Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328633+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [99]:   Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328638+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [100]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328665+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [101]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328670+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [102]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328675+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [103]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328680+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [104]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328685+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [105]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328690+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [106]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328731+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [107]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328736+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [108]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328741+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [109]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328747+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [110]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328752+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [111]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328757+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [112]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "064f0939-3810-42b7-a52a-2f88ac7e1ff6", Timestamp = 2025-02-15T04:50:10.6985845+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [113]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "25556dbc-a206-486d-b684-f2ab31a1ae98", Timestamp = 2025-02-15T04:52:38.9701889+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [114]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "fa99dec8-7b1b-4cba-b175-355a5316e1d8", Timestamp = 2025-02-15T04:52:39.0428777+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [115]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "87465394-4990-4629-ab5f-9161acc4d9e6", Timestamp = 2025-02-15T04:52:39.0509070+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [116]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "87465394-4990-4629-ab5f-9161acc4d9e6", Timestamp = 2025-02-15T04:52:39.0509141+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [117]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "87465394-4990-4629-ab5f-9161acc4d9e6", Timestamp = 2025-02-15T04:52:39.0509149+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [118]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "35a70fc3-e2c3-4357-b05b-daef3ea3fe38", Timestamp = 2025-02-15T04:52:39.1495794+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [119]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "0bdc5d91-422b-46ce-a497-365342853ea1", Timestamp = 2025-02-15T04:52:39.2103229+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [120]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "0bdc5d91-422b-46ce-a497-365342853ea1", Timestamp = 2025-02-15T04:52:39.2103534+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [121]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "6c1fd7f6-82d7-4144-ac04-97636704fd17", Timestamp = 2025-02-15T04:52:39.0326790+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [122]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "e5a75053-7f03-4342-ae3f-6fd0fc7efe9c", Timestamp = 2025-02-15T04:52:39.0815315+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [123]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "ba7b3b23-abe0-4200-90d8-04b569055036", Timestamp = 2025-02-15T04:52:39.0720270+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [124]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "68c9c7c0-6dab-4b7f-8e87-a3763d66fbeb", Timestamp = 2025-02-15T04:52:39.1338393+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [125]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "10f43db4-ef22-414e-87fb-ee84d12811b8", Timestamp = 2025-02-15T04:52:39.1638494+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [126]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "5915370a-8245-4f28-a401-6f7693372511", Timestamp = 2025-02-15T04:52:39.2521491+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [127]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821380+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [128]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821395+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [129]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821399+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [130]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821403+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [131]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821407+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [132]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821410+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [133]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821421+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [134]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821428+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [135]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821433+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [136]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821436+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [137]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821454+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [138]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821461+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [139]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821467+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [140]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821472+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [141]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821479+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [142]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821485+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [143]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821490+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [144]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821495+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [145]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821500+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [146]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821505+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [147]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821511+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [148]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821519+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [149]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821526+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [150]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821531+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [151]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821536+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [152]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821541+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [153]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821549+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [154]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821555+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [155]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821560+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [156]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821565+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [157]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821570+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [158]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821576+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [159]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821581+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [160]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821707+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [161]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821712+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [162]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821717+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [163]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821723+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [164]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821728+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [165]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821737+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [166]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821742+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [167]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821747+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [168]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821788+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [169]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821794+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [170]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821799+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [171]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821815+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [172]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821821+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [173]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821826+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [174]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821831+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [175]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821837+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [176]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821842+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [177]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821849+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [178]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821855+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [179]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821860+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [180]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821865+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [181]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821872+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [182]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821881+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [183]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821887+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [184]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821892+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [185]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821897+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [186]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821903+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [187]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821908+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [188]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821913+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [189]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821923+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [190]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821929+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [191]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821934+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [192]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821939+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [193]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821944+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [194]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821949+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [195]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821959+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [196]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821964+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [197]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821969+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [198]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821974+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [199]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821979+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [200]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821985+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [201]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821997+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [202]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822002+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [203]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822007+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [204]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822013+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [205]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822018+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [206]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822023+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [207]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822028+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [208]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822038+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [209]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822043+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [210]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822048+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [211]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822053+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [212]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822059+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [213]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822064+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [214]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822072+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [215]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822077+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [216]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822082+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [217]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822088+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [218]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "8b53aac9-b5ac-4f47-aec0-ea2147d37504", Timestamp = 2025-02-15T04:52:39.1087988+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [219]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "8b53aac9-b5ac-4f47-aec0-ea2147d37504", Timestamp = 2025-02-15T04:52:39.1087992+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [220]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "8b53aac9-b5ac-4f47-aec0-ea2147d37504", Timestamp = 2025-02-15T04:52:39.1087998+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [221]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "8b53aac9-b5ac-4f47-aec0-ea2147d37504", Timestamp = 2025-02-15T04:52:39.1087995+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [222]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "8b53aac9-b5ac-4f47-aec0-ea2147d37504", Timestamp = 2025-02-15T04:52:39.1088046+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [223]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_44", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661533+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [224]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661548+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [225]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661565+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [226]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661569+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [227]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661580+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [228]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661589+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [229]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661608+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [230]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "5229070f-98b7-4c7f-abcc-c4728eac600a", Timestamp = 2025-02-15T04:52:48.4744234+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [231]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "5229070f-98b7-4c7f-abcc-c4728eac600a", Timestamp = 2025-02-15T04:52:48.4744506+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [232]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "d21dbbaa-c775-4491-980d-2730431d8f23", Timestamp = 2025-02-15T04:52:48.5326819+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [233]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "0d81d902-9185-409d-aeaf-aed86949c936", Timestamp = 2025-02-15T04:52:48.2828917+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [234]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "7a865db5-5e5d-406e-bb49-a5032f96ab68", Timestamp = 2025-02-15T04:52:48.2680392+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [235]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4417965+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [236]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4417988+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [237]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4417993+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [238]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4417996+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [239]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418000+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [240]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418003+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [241]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418012+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [242]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418016+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [243]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418020+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [244]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418027+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [245]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418051+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [246]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418058+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [247]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418064+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [248]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418069+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [249]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418075+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [250]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418084+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [251]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418090+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [252]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418095+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [253]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418100+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [254]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418105+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [255]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418111+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [256]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418116+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [257]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418127+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [258]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418132+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [259]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418137+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [260]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418142+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [261]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418148+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [262]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418156+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [263]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418161+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [264]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418167+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [265]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418172+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [266]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418178+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [267]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418183+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [268]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418257+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [269]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418263+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [270]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418268+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [271]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418274+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [272]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418279+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [273]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418284+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [274]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418294+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [275]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418300+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [276]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418338+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [277]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418345+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [278]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418350+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [279]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418356+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [280]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418364+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [281]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418370+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [282]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418375+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [283]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418381+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [284]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418386+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [285]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418391+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [286]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418397+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [287]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418408+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [288]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418413+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [289]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418420+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [290]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418426+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [291]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418434+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [292]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418440+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [293]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418445+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [294]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418451+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [295]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418456+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [296]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418461+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [297]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418467+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [298]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418476+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [299]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418481+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [300]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418486+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [301]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418492+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [302]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418497+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [303]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418502+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [304]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418511+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [305]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418517+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [306]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418522+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [307]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418528+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [308]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418536+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [309]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418541+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [310]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418550+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [311]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418555+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [312]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418560+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [313]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418566+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [314]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418571+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [315]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418576+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [316]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418582+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [317]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418591+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [318]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418596+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [319]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418601+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [320]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418606+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [321]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418612+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [322]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418617+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [323]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418626+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [324]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418632+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [325]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418637+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [326]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "3a69a7a3-bb81-4e49-9ffa-be19aaed175f", Timestamp = 2025-02-15T04:52:48.1924472+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [327]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "c8310e2d-5bf5-459d-9854-f8816c866146", Timestamp = 2025-02-15T04:52:48.3916401+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [328]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "2abf599f-3606-400c-8577-d2d0f48fc22f", Timestamp = 2025-02-15T04:52:48.4050904+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [329]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "956d50e3-497a-42f4-95f1-36d29d5c8afb", Timestamp = 2025-02-15T04:52:48.4208530+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [330]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "b2b2ae05-c7a0-4aa8-aa87-f08815e2a40a", Timestamp = 2025-02-15T04:52:48.2938487+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [331]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "b2b2ae05-c7a0-4aa8-aa87-f08815e2a40a", Timestamp = 2025-02-15T04:52:48.2938598+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [332]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "b2b2ae05-c7a0-4aa8-aa87-f08815e2a40a", Timestamp = 2025-02-15T04:52:48.2938612+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [333]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "2499d494-fac0-4c4f-8657-358886877107", Timestamp = 2025-02-15T04:52:48.3337137+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [334]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "8b620f3a-aacf-4a7f-83a7-6be42eb2bf70", Timestamp = 2025-02-15T04:52:48.3211666+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [335]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "c17b0e6c-c846-45b5-8759-b63e61c71a53", Timestamp = 2025-02-15T04:53:25.9766277+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:25 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [336]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "49cab4c9-e548-4114-a38d-bf56179be458", Timestamp = 2025-02-15T04:53:26.0493767+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [337]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "b5fe8d1d-1d59-4271-8c8e-b04c84313394", Timestamp = 2025-02-15T04:53:26.0658058+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [338]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "902fe17a-e545-440b-86bf-28a02a50a45d", Timestamp = 2025-02-15T04:53:26.3367814+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [339]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "b25a9261-829a-42cf-a15d-92c0db0f5274", Timestamp = 2025-02-15T04:53:26.1928670+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [340]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_44", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590816+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [341]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590833+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [342]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590847+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [343]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590863+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [344]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590871+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [345]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590914+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [346]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590921+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [347]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "37b98da6-6c45-4cd2-bd5c-bedd2fd7d493", Timestamp = 2025-02-15T04:53:26.1134333+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [348]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "c52f6652-01ad-4d9f-ae19-35b98393c768", Timestamp = 2025-02-15T04:53:26.2180105+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [349]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "85a3c5d5-940f-45ef-aa60-6378e7956d67", Timestamp = 2025-02-15T04:53:26.0865591+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [350]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "85a3c5d5-940f-45ef-aa60-6378e7956d67", Timestamp = 2025-02-15T04:53:26.0865699+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [351]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "85a3c5d5-940f-45ef-aa60-6378e7956d67", Timestamp = 2025-02-15T04:53:26.0865705+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [352]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "2450138d-e56e-4d4c-84eb-e4dec0202355", Timestamp = 2025-02-15T04:53:26.2863069+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [353]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "2450138d-e56e-4d4c-84eb-e4dec0202355", Timestamp = 2025-02-15T04:53:26.2863310+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [354]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "acaa6e60-689c-4178-9f68-d662cde63c50", Timestamp = 2025-02-15T04:53:26.1259268+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [355]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "75e2ada7-3705-4b1a-9ba0-14dda1bb0912", Timestamp = 2025-02-15T04:53:26.2353144+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [356]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575935+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [357]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575951+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [358]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575955+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [359]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575959+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [360]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575963+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [361]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575967+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [362]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575974+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [363]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575982+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [364]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575986+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [365]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575990+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [366]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576007+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [367]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576014+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [368]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576020+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [369]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576026+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [370]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576033+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [371]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576038+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [372]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576044+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [373]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576049+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [374]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576055+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [375]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576060+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [376]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576065+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [377]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576074+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [378]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576081+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [379]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576087+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [380]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576092+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [381]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576097+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [382]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576105+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [383]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576110+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [384]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576115+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [385]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576121+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [386]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576126+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [387]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576132+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [388]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576137+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [389]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576202+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [390]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576208+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [391]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576213+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [392]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576218+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [393]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576223+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [394]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576233+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [395]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576238+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [396]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576243+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [397]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576290+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [398]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576296+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [399]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576301+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [400]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576315+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [401]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576320+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [402]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576325+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [403]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576331+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [404]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576336+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [405]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576342+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [406]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576349+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [407]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576355+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [408]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576361+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [409]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576366+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [410]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576373+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [411]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576381+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [412]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576387+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [413]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576392+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [414]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576398+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [415]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576403+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [416]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576409+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [417]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576414+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [418]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576423+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [419]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576428+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [420]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576434+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [421]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576439+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [422]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576444+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [423]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576450+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [424]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576458+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [425]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576464+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [426]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576469+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [427]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576475+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [428]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576480+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [429]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576485+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [430]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576495+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [431]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576501+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [432]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576506+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [433]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576511+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [434]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576516+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [435]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576522+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [436]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576527+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [437]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576538+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [438]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576543+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [439]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576549+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [440]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576554+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [441]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576559+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [442]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576564+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [443]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576577+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [444]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576583+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [445]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576588+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [446]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576594+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [447]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "80de192a-52c9-4d04-b615-90305deca4c2", Timestamp = 2025-02-15T04:53:44.9351124+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [448]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "80de192a-52c9-4d04-b615-90305deca4c2", Timestamp = 2025-02-15T04:53:44.9351251+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [449]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "80de192a-52c9-4d04-b615-90305deca4c2", Timestamp = 2025-02-15T04:53:44.9351257+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [450]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "c8575510-4ee2-477f-be92-103be110b79e", Timestamp = 2025-02-15T04:53:44.7964030+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [451]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "8c5f8ca5-f807-4e97-8655-aec9514e0858", Timestamp = 2025-02-15T04:53:44.9155473+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [452]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "05c389be-1ecd-4ab1-a053-a91d4735d0f9", Timestamp = 2025-02-15T04:53:44.9702478+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [453]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "e9aabf0b-85e4-4537-af3f-bd4081ee1dea", Timestamp = 2025-02-15T04:53:44.9834322+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [454]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "8ee55608-adaa-4cc4-950f-1f8d2c7f4d79", Timestamp = 2025-02-15T04:53:45.0044678+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [455]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "8ee55608-adaa-4cc4-950f-1f8d2c7f4d79", Timestamp = 2025-02-15T04:53:45.0044719+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [456]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "8ee55608-adaa-4cc4-950f-1f8d2c7f4d79", Timestamp = 2025-02-15T04:53:45.0044727+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [457]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "8ee55608-adaa-4cc4-950f-1f8d2c7f4d79", Timestamp = 2025-02-15T04:53:45.0044719+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [458]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "8ee55608-adaa-4cc4-950f-1f8d2c7f4d79", Timestamp = 2025-02-15T04:53:45.0044771+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [459]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "8ee55608-adaa-4cc4-950f-1f8d2c7f4d79", Timestamp = 2025-02-15T04:53:45.0044790+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [460]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "f069abec-4a84-4224-9adf-7c7930d6b028", Timestamp = 2025-02-15T04:53:44.8895055+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [461]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "920d867f-5fc4-4f53-89ca-a60cc1955a74", Timestamp = 2025-02-15T04:53:45.0726663+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [462]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "d59cfe1f-81e5-478b-bd97-c31deeb0b1bf", Timestamp = 2025-02-15T04:53:45.0430767+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [463]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "88b5a1c8-3b03-407b-97bb-c8379601122b", Timestamp = 2025-02-15T04:53:45.1761036+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [464]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "88b5a1c8-3b03-407b-97bb-c8379601122b", Timestamp = 2025-02-15T04:53:45.1761350+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [465]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "510537a6-13b8-4570-b41e-b9bb132840c9", Timestamp = 2025-02-15T04:53:45.0998590+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [466]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339648+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [467]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339679+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [468]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339689+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [469]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339695+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [470]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339701+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [471]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339705+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [472]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339719+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [473]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339730+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [474]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339734+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [475]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339738+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [476]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339764+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [477]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339771+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [478]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339776+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [479]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339781+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [480]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339790+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [481]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339796+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [482]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339801+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [483]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339806+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [484]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339811+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [485]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339817+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [486]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339822+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [487]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339832+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [488]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339839+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [489]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339845+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [490]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339850+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [491]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339856+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [492]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339874+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [493]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339884+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [494]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339900+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [495]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339908+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [496]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339917+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [497]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339925+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [498]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339935+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [499]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340041+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [500]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340049+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [501]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340058+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [502]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340125+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [503]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340152+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [504]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340171+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [505]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340180+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [506]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340188+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [507]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340245+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [508]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340254+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [509]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340262+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [510]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340280+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [511]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340288+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [512]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340296+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [513]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340304+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [514]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340314+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [515]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340322+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [516]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340330+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [517]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340345+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [518]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340353+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [519]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340361+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [520]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340375+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [521]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340388+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [522]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340398+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [523]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340407+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [524]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340420+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [525]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340428+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [526]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340437+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [527]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340446+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [528]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340459+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [529]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340467+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [530]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340476+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [531]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340484+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [532]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340494+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [533]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340502+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [534]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340514+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [535]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340523+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [536]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340531+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [537]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340539+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [538]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340547+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [539]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340555+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [540]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340573+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [541]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340582+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [542]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340590+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [543]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340599+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [544]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340607+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [545]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340616+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [546]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340624+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [547]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340637+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [548]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340646+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [549]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340654+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [550]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340663+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [551]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340682+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [552]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340691+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [553]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340729+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [554]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340793+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [555]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340809+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [556]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340821+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [557]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "0a539fa7-93de-48fa-885b-a8120f8f350f", Timestamp = 2025-02-15T04:53:45.2420085+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [558]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "8873f27d-4a4a-41e9-a8c8-8e9fd3ef8b39", Timestamp = 2025-02-15T04:54:47.3247606+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [559]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "7f6b8828-5675-4e6e-8c16-8ee2aa4a17eb", Timestamp = 2025-02-15T04:54:47.4930229+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [560]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "1c70e69e-d4df-496e-bd8a-4971c70ba9ef", Timestamp = 2025-02-15T04:54:47.3015739+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [561]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "5e9d6e9b-abc2-4921-af94-c3487c9572bd", Timestamp = 2025-02-15T04:54:47.5549647+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [562]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "5e9d6e9b-abc2-4921-af94-c3487c9572bd", Timestamp = 2025-02-15T04:54:47.5549889+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [563]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "6087c0e3-8b44-44cc-80f2-9fe46baba59b", Timestamp = 2025-02-15T04:54:47.2164627+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [564]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_44", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132588+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [565]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132591+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [566]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132602+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [567]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132623+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [568]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132653+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [569]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132657+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [570]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132700+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [571]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "73331991-6c1e-4cc3-92b9-af97e7a9ad56", Timestamp = 2025-02-15T04:54:47.4719221+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [572]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "f0d17914-30be-46eb-9a84-2b21fbece80d", Timestamp = 2025-02-15T04:54:47.3777291+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [573]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "583fd4a3-b101-4c11-8fc2-55a78e2867db", Timestamp = 2025-02-15T04:54:47.3882986+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [574]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "da3736e1-a18e-4d7c-8c88-5483aa18a4b0", Timestamp = 2025-02-15T04:54:47.4500058+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [575]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217817+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [576]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217830+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [577]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217834+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [578]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217838+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [579]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217842+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [580]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217845+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [581]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217853+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [582]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217861+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [583]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217865+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [584]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217869+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [585]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217889+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [586]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217895+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [587]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217901+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [588]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217906+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [589]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217915+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [590]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217920+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [591]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217925+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [592]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217930+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [593]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217935+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [594]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217940+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [595]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217945+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [596]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217954+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [597]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217961+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [598]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217966+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [599]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217971+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [600]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217976+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [601]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217985+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [602]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217990+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [603]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217996+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [604]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218001+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [605]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218006+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [606]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218011+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [607]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218016+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [608]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218081+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [609]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218087+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [610]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218092+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [611]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218097+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [612]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218102+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [613]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218111+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [614]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218116+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [615]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218121+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [616]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218167+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [617]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218172+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [618]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218177+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [619]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218192+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [620]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218197+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [621]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218202+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [622]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218207+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [623]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218212+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [624]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218217+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [625]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218224+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [626]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218231+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [627]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218236+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [628]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218241+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [629]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218247+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [630]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218255+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [631]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218260+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [632]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218265+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [633]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218270+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [634]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218276+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [635]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218281+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [636]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218286+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [637]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218295+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [638]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218301+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [639]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218306+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [640]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218311+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [641]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218316+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [642]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218321+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [643]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218329+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [644]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218334+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [645]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218339+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [646]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218345+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [647]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218350+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [648]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218355+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [649]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218363+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [650]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218368+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [651]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218373+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [652]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218378+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [653]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218383+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [654]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218388+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [655]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218393+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [656]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218401+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [657]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218406+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [658]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218412+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [659]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218417+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [660]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218422+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [661]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218427+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [662]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218435+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [663]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218440+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [664]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218445+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [665]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218451+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [666]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "eb562f74-27ea-4903-a453-0b11a9f79f7a", Timestamp = 2025-02-15T04:54:47.3467089+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.67]       [667]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "eb562f74-27ea-4903-a453-0b11a9f79f7a", Timestamp = 2025-02-15T04:54:47.3467206+00:00 }
[xUnit.net 00:00:00.67]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [668]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "eb562f74-27ea-4903-a453-0b11a9f79f7a", Timestamp = 2025-02-15T04:54:47.3467229+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [669]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "920102ee-dc89-46e5-9ae5-7abc8d50a1e7", Timestamp = 2025-02-15T04:54:47.6133427+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [670]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "0af70fa1-d2bd-464f-a319-1b333bd6daa5", Timestamp = 2025-02-15T04:55:01.8908354+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [671]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "618e3272-6c62-44b2-bb40-8dbdeb06d49f", Timestamp = 2025-02-15T04:55:01.9139878+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [672]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "618e3272-6c62-44b2-bb40-8dbdeb06d49f", Timestamp = 2025-02-15T04:55:01.9139995+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [673]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "618e3272-6c62-44b2-bb40-8dbdeb06d49f", Timestamp = 2025-02-15T04:55:01.9140002+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [674]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "a7d1b805-a2ab-4fec-8650-21c5d3526040", Timestamp = 2025-02-15T04:55:01.9512407+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [675]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "b7bb6059-a8bf-4f5a-af8c-5d4b95fea28d", Timestamp = 2025-02-15T04:55:01.7611658+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [676]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "ab67640b-d9ce-4f92-a82f-6403f5d51d36", Timestamp = 2025-02-15T04:55:01.9628967+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [677]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_44", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945217+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [678]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945252+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [679]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945279+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [680]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945292+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [681]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945301+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [682]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945316+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [683]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945360+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [684]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "ab86fe09-c1cb-4260-9368-f6ba6099f32a", Timestamp = 2025-02-15T04:55:01.8689788+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [685]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "99ee3905-ac39-4262-af54-6b8a027d2885", Timestamp = 2025-02-15T04:55:02.1555927+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [686]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "99ee3905-ac39-4262-af54-6b8a027d2885", Timestamp = 2025-02-15T04:55:02.1556173+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [687]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "01c2d80c-8fe3-494c-a02d-f457d54b14bf", Timestamp = 2025-02-15T04:55:02.0315845+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [688]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188751+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [689]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188768+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [690]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188772+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [691]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188776+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [692]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188780+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [693]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188783+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [694]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188791+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [695]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188801+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [696]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188805+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [697]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188809+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [698]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188830+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [699]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188837+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [700]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188843+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [701]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188848+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [702]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188855+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [703]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188860+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [704]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188865+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [705]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188870+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [706]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188875+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [707]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188880+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [708]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188885+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [709]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188893+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [710]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188900+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [711]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188906+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [712]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188910+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [713]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188916+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [714]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188926+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [715]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188931+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [716]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188936+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [717]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188941+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [718]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188946+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [719]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188951+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [720]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188956+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [721]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189029+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [722]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189034+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [723]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189039+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [724]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189044+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [725]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189049+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [726]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189057+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [727]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189062+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [728]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189067+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [729]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189101+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [730]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189107+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [731]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189112+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [732]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189122+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [733]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189127+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [734]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189132+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [735]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189137+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [736]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189142+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [737]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189147+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [738]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189152+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [739]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189163+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [740]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189168+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [741]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189173+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [742]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189179+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [743]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189190+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [744]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189196+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [745]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189201+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [746]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189206+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [747]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189211+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [748]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189216+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [749]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189221+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [750]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189233+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [751]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189238+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [752]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189243+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [753]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189248+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [754]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189253+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [755]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189258+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [756]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189269+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [757]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189274+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [758]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189280+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [759]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189285+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [760]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189290+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [761]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189295+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [762]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189306+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [763]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189311+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [764]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189317+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [765]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189322+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [766]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189327+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [767]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189332+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [768]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189337+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [769]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189350+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [770]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189355+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [771]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189360+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [772]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189365+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [773]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189370+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [774]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189375+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [775]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189389+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [776]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189394+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [777]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189399+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [778]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189405+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [779]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "92ebf1c4-6069-4bca-b379-7b3fc4a96971", Timestamp = 2025-02-15T04:55:02.0824390+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [780]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "a5c67bbd-e073-4bcc-9b5c-2d164f80ca55", Timestamp = 2025-02-15T04:55:02.2204372+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [781]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "890ef49f-25ab-4eeb-9b08-edabca37af00", Timestamp = 2025-02-15T04:55:02.0584724+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [782]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "c19ff0c5-5df5-4857-8647-b134bfc089a2", Timestamp = 2025-02-15T04:55:37.8100886+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:37 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [783]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "22d40e1d-bcb0-4148-a1ff-69fd44c36767", Timestamp = 2025-02-15T04:55:37.9358532+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:37 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [784]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "2cbb3aba-654b-4890-9749-9b7d4f7d6c18", Timestamp = 2025-02-15T04:55:37.9666141+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:37 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [785]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "35d59729-ea21-4760-b21b-d87e61eb32dd", Timestamp = 2025-02-15T04:55:37.9944263+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:37 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [786]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "35d59729-ea21-4760-b21b-d87e61eb32dd", Timestamp = 2025-02-15T04:55:37.9944370+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:37 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [787]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "35d59729-ea21-4760-b21b-d87e61eb32dd", Timestamp = 2025-02-15T04:55:37.9944377+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:37 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [788]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "314a311f-13e5-4624-b98f-44f3f066d1ca", Timestamp = 2025-02-15T04:55:38.1841249+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [789]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844393+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [790]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_44", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844399+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [791]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844421+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [792]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844432+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [793]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844491+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [794]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844485+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [795]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844508+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [796]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "19a625e6-0be0-4230-9f5a-874452132bec", Timestamp = 2025-02-15T04:55:38.2595146+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [797]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "19a625e6-0be0-4230-9f5a-874452132bec", Timestamp = 2025-02-15T04:55:38.2595420+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [798]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "0e02f8a1-0217-404e-a956-29622e3bce77", Timestamp = 2025-02-15T04:55:38.0500181+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [799]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "14691a72-efc5-4b37-a055-46c169133e24", Timestamp = 2025-02-15T04:55:38.3274055+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [800]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "39373b6f-fe7a-46e0-8f0a-0c2e7277bf3a", Timestamp = 2025-02-15T04:55:38.1238429+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [801]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198474+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [802]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198487+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [803]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198495+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [804]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198499+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [805]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198503+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [806]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198508+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [807]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198515+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [808]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198519+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [809]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198522+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [810]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198526+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [811]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198556+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [812]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198563+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [813]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198569+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [814]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198574+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [815]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198579+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [816]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198584+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [817]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198589+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [818]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198598+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [819]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198603+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [820]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198608+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [821]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198613+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [822]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198619+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [823]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198628+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [824]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198633+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [825]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198638+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [826]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198644+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [827]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198649+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [828]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198654+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [829]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198659+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [830]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198667+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [831]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198673+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [832]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198678+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [833]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198683+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [834]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198746+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [835]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198755+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [836]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198760+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [837]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198765+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [838]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198771+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [839]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198776+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [840]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198781+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [841]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198790+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [842]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198826+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [843]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198831+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [844]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198836+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [845]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198841+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [846]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198847+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [847]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198852+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [848]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198860+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [849]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198866+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [850]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198871+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [851]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198876+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [852]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198881+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [853]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198886+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [854]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198898+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [855]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198905+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [856]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198910+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [857]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198915+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [858]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198920+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [859]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198928+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [860]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198934+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [861]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198939+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [862]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198944+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [863]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198949+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [864]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198954+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [865]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198970+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [866]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198975+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [867]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198980+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [868]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198985+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [869]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198990+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [870]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198996+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [871]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199004+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [872]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199009+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [873]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199014+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [874]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199019+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [875]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199024+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [876]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199030+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [877]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199035+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [878]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199047+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [879]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199052+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [880]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199057+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [881]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199063+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [882]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199068+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [883]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199073+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [884]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199081+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [885]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199087+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [886]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199092+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [887]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199097+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [888]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199102+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [889]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199107+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [890]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199119+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [891]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199125+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [892]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "b2026fc5-5bb1-4cb0-8a9f-97809d57361a", Timestamp = 2025-02-15T04:55:38.1509002+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [893]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "0d61015b-0c1f-434b-a3a0-2ee67cab37a6", Timestamp = 2025-02-15T04:55:38.0395169+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [894]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "fe0a20b6-a95b-4873-91a2-64517df7d920", Timestamp = 2025-02-15T04:56:27.9848148+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:27 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [895]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "e4a0119b-850d-4a4c-9689-acac0f79853a", Timestamp = 2025-02-15T04:56:27.8435991+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:27 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [896]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "a692ed54-732a-4bcf-ad37-daa2ee9f23ee", Timestamp = 2025-02-15T04:56:28.2149256+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [897]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "5d49eb6c-2f69-4f05-b686-65680ad58676", Timestamp = 2025-02-15T04:56:28.1041441+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [898]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "3074e629-c374-428e-9623-0ffd621514e3", Timestamp = 2025-02-15T04:56:28.2461552+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [899]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "486019ed-cb5d-4195-a71c-fa9c3708fc09", Timestamp = 2025-02-15T04:56:28.1807680+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [900]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "a67c622b-f89e-4735-9d39-de6694642f5e", Timestamp = 2025-02-15T04:56:28.1152114+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [901]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840098+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [902]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840113+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [903]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840116+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [904]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840120+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [905]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840123+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [906]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840127+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [907]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840135+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [908]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840141+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [909]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840145+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [910]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840149+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [911]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840170+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [912]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840176+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [913]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840182+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [914]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840187+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [915]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840195+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [916]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840200+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [917]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840205+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [918]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840211+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [919]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840216+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [920]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840221+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [921]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840227+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [922]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840235+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [923]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840241+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [924]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840247+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [925]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840253+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [926]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840258+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [927]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840266+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [928]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840271+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [929]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840276+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [930]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840282+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [931]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840287+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [932]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840292+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [933]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840297+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [934]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840365+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [935]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840370+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [936]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840375+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [937]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840381+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [938]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840386+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [939]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840401+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [940]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840406+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [941]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840412+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [942]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840447+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [943]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840453+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [944]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840458+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [945]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840468+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [946]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840473+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [947]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840479+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [948]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840484+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [949]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840489+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [950]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840495+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [951]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840503+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [952]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840509+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [953]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840515+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [954]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840520+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [955]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840527+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [956]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840535+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [957]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840540+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [958]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840546+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [959]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840551+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [960]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840556+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [961]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840562+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [962]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840567+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [963]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840576+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [964]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840581+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [965]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840588+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [966]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840593+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [967]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840599+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [968]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840604+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [969]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840613+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [970]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840618+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [971]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840624+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [972]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840629+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [973]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840634+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [974]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840640+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [975]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840648+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [976]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840653+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [977]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840659+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [978]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840664+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [979]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840669+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [980]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840675+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [981]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840680+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [982]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840689+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [983]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840694+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [984]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840699+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [985]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840704+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [986]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840710+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [987]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840715+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [988]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840724+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [989]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840730+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [990]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840735+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [991]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840741+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [992]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "e31c725b-1e1b-4a9f-abc9-61e1d6c3488d", Timestamp = 2025-02-15T04:56:28.0286817+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [993]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "cb192657-af40-4279-8027-d911e3f9a834", Timestamp = 2025-02-15T04:56:28.1383979+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [994]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "cb192657-af40-4279-8027-d911e3f9a834", Timestamp = 2025-02-15T04:56:28.1383993+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [995]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "cb192657-af40-4279-8027-d911e3f9a834", Timestamp = 2025-02-15T04:56:28.1384064+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [996]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "cb192657-af40-4279-8027-d911e3f9a834", Timestamp = 2025-02-15T04:56:28.1386680+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [997]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "cb192657-af40-4279-8027-d911e3f9a834", Timestamp = 2025-02-15T04:56:28.1386703+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [998]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "cb192657-af40-4279-8027-d911e3f9a834", Timestamp = 2025-02-15T04:56:28.1386733+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [1001]: Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "8b7ec67e-ffbd-4c63-af85-c988db455f0a", Timestamp = 2025-02-15T04:56:28.0610900+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [1002]: Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "8b7ec67e-ffbd-4c63-af85-c988db455f0a", Timestamp = 2025-02-15T04:56:28.0610999+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       [1003]: Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "8b7ec67e-ffbd-4c63-af85-c988db455f0a", Timestamp = 2025-02-15T04:56:28.0611005+00:00 }
[xUnit.net 00:00:00.68]               Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
[xUnit.net 00:00:00.68]       Stack Trace:
[xUnit.net 00:00:00.68]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(360,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TimestampsAreUtc_AcrossAllTelemetry()
[xUnit.net 00:00:00.68]         --- End of stack trace from previous location ---
[xUnit.net 00:00:00.68]     LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackPerformance_RecordsCorrectDuration [FAIL]
[xUnit.net 00:00:00.68]       Assert.Single() Failure: The collection contained 36 items
[xUnit.net 00:00:00.68]       Collection: [TelemetryMetric { Name = "Metric1", Properties = [], SessionId = "34cd9a8d-33a8-40ac-b375-c8d0f188d268", Timestamp = 2025-02-15T04:50:10.7446513+00:00, Value = 42 }, TelemetryMetric { Name = "TimestampMetric", Properties = [], SessionId = "75864c4b-1650-4a34-b59b-9d6dcd405c26", Timestamp = 2025-02-15T04:50:10.8549740+00:00, Value = 42 }, TelemetryMetric { Name = "Performance.TestOperation", Properties = [], SessionId = "4b1a3931-0d10-47bd-8bf5-b05896bbaedf", Timestamp = 2025-02-15T04:50:10.8702490+00:00, Value = 100 }, TelemetryMetric { Name = "TestMetric", Properties = [], SessionId = "cf917c53-dbef-4286-86a7-c7ace4952b31", Timestamp = 2025-02-15T04:50:10.6668040+00:00, Value = 42 }, TelemetryMetric { Name = "TimestampMetric", Properties = [], SessionId = "0bdc5d91-422b-46ce-a497-365342853ea1", Timestamp = 2025-02-15T04:52:39.2103357+00:00, Value = 42 }, ···]
[xUnit.net 00:00:00.68]       Stack Trace:
[xUnit.net 00:00:00.68]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(145,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackPerformance_RecordsCorrectDuration()
[xUnit.net 00:00:00.68]         --- End of stack trace from previous location ---
[xUnit.net 00:00:00.74]     LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.InvalidBasePath_ThrowsException [FAIL]
[xUnit.net 00:00:00.74]       Assert.Throws() Failure: No exception was thrown
[xUnit.net 00:00:00.74]       Expected: typeof(System.IO.IOException)
[xUnit.net 00:00:00.74]       Stack Trace:
[xUnit.net 00:00:00.74]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(334,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.InvalidBasePath_ThrowsException()
[xUnit.net 00:00:00.74]         --- End of stack trace from previous location ---
[xUnit.net 00:00:00.78]     LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_FiltersTestEvents_InProduction [FAIL]
[xUnit.net 00:00:00.78]       Assert.Single() Failure: The collection contained 1005 items
[xUnit.net 00:00:00.78]       Collection: [TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }, TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090010+00:00 }, TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }, TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }, TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }, ···]
[xUnit.net 00:00:00.78]       Stack Trace:
[xUnit.net 00:00:00.78]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(388,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_FiltersTestEvents_InProduction()
[xUnit.net 00:00:00.78]         D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(394,0): at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_FiltersTestEvents_InProduction()
[xUnit.net 00:00:00.78]         --- End of stack trace from previous location ---
[xUnit.net 00:00:00.78]   Finished:    LofiBeats.Tests
  LofiBeats.Tests test failed with 13 error(s) (1.2s)
    D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(105): error TESTERROR:
      LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackMetric_CreatesValidTelemetryFile (23ms): Error Message: Assert.Single() Failure: The collection contained 33 items
      Collection: [TelemetryMetric { Name = "Metric1", Properties = [], SessionId = "34cd9a8d-33a8-40ac-b375-c8d0f188d268", Timestamp = 2025-02-15T04:50:10.7446513+00:00, Value = 42 }, TelemetryMetric { Name = "TimestampMetric", Properties = [], SessionId = "75864c4b-1650-4a34-b5
      9b-9d6dcd405c26", Timestamp = 2025-02-15T04:50:10.8549740+00:00, Value = 42 }, TelemetryMetric { Name = "Performance.TestOperation", Properties = [], SessionId = "4b1a3931-0d10-47bd-8bf5-b05896bbaedf", Timestamp = 2025-02-15T04:50:10.8702490+00:00, Value = 100 }, TelemetryM
      etric { Name = "TestMetric", Properties = [], SessionId = "cf917c53-dbef-4286-86a7-c7ace4952b31", Timestamp = 2025-02-15T04:50:10.6668040+00:00, Value = 42 }, TelemetryMetric { Name = "TimestampMetric", Properties = [], SessionId = "0bdc5d91-422b-46ce-a497-365342853ea1", Ti
      mestamp = 2025-02-15T04:52:39.2103357+00:00, Value = 42 }, ···]
      Stack Trace:
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackMetric_CreatesValidTelemetryFile() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 105
      --- End of stack trace from previous location ---
    D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(85): error TESTERROR:
      LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_CreatesValidTelemetryFile (44ms): Error Message: Assert.Single() Failure: The collection contained 896 items
      Collection: [TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }, TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286",
       Timestamp = 2025-02-15T04:50:10.7090010+00:00 }, TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }, TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2
      -34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }, TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }, ···]
      Stack Trace:
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_CreatesValidTelemetryFile() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 85
      --- End of stack trace from previous location ---
    D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(407): error TESTERROR:
      LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_AllowsTestEvents_InTestEnvironment (31ms): Error Message: Assert.Single() Failure: The collection contained 897 items
      Collection: [TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }, TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286",
       Timestamp = 2025-02-15T04:50:10.7090010+00:00 }, TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }, TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2
      -34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }, TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }, ···]
      Stack Trace:
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_AllowsTestEvents_InTestEnvironment() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 407
      --- End of stack trace from previous location ---
    D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(193): error TESTERROR:
      LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.SessionId_IsConsistentAcrossEvents (30ms): Error Message: Assert.Single() Failure: The collection contained 108 items
      Collection: ["f4d51922-448d-49dc-8410-094a4034e595", "42f199d2-34b3-4ef0-889b-48fb90d0a286", "138d7792-0de7-4002-910b-b64cadb31ff1", "19649abd-8362-4cb9-8589-9758f4462487", "334f918b-f4b2-418f-bcce-b8b5550bef96", ···]
      Stack Trace:
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.SessionId_IsConsistentAcrossEvents() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 193
      --- End of stack trace from previous location ---
    D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(256): error TESTERROR:
      LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.ConcurrentEvents_AreAllRecorded (56ms): Error Message: Assert.Equal() Failure: Values differ
      Expected: 50
      Actual:   908
      Stack Trace:
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.ConcurrentEvents_AreAllRecorded() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 256
      --- End of stack trace from previous location ---
    D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(428): error TESTERROR:
      LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_RejectsFutureTimestamps (33ms): Error Message: Assert.Empty() Failure: Collection was not empty
      Collection: [TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }, TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286",
       Timestamp = 2025-02-15T04:50:10.7090010+00:00 }, TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }, TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2
      -34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }, TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }, ···]
      Stack Trace:
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_RejectsFutureTimestamps() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 428
      --- End of stack trace from previous location ---
    D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(300): error TESTERROR:
      LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.LargePropertyValues_AreHandledCorrectly (30ms): Error Message: Assert.Single() Failure: The collection contained 910 items
      Collection: [TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }, TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286",
       Timestamp = 2025-02-15T04:50:10.7090010+00:00 }, TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }, TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2
      -34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }, TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }, ···]
      Stack Trace:
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.LargePropertyValues_AreHandledCorrectly() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 300
      --- End of stack trace from previous location ---
    D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(124): error TESTERROR:
      LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackException_IncludesExceptionDetails (30ms): Error Message: Assert.Single() Failure: The collection contained 911 items
      Collection: [TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }, TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286",
       Timestamp = 2025-02-15T04:50:10.7090010+00:00 }, TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }, TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2
      -34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }, TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }, ···]
      Stack Trace:
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackException_IncludesExceptionDetails() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 124
      --- End of stack trace from previous location ---
    D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(169): error TESTERROR:
      LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.BufferFlush_TriggersAtBufferSize (47ms): Error Message: Assert.Equal() Failure: Values differ
      Expected: 101
      Actual:   1002
      Stack Trace:
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.BufferFlush_TriggersAtBufferSize() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 169
      --- End of stack trace from previous location ---
    D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(360): error TESTERROR:
      LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TimestampsAreUtc_AcrossAllTelemetry (65ms): Error Message: Assert.All() Failure: 1002 out of 1004 items in the collection did not pass.
      [0]:    Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [1]:    Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090010+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [2]:    Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [3]:    Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [4]:    Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [5]:    Item:  TelemetryEvent { Name = "ConcurrentEvent_43", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700689+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [6]:    Item:  TelemetryEvent { Name = "ConcurrentEvent_44", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700694+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [7]:    Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700727+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [8]:    Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700734+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [9]:    Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700788+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [10]:   Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700803+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [11]:   Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700811+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [12]:   Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "19649abd-8362-4cb9-8589-9758f4462487", Timestamp = 2025-02-15T04:50:10.7700814+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [13]:   Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "334f918b-f4b2-418f-bcce-b8b5550bef96", Timestamp = 2025-02-15T04:50:10.7962089+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [14]:   Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "c8a349aa-606a-46e6-b598-0f5ed05ff029", Timestamp = 2025-02-15T04:50:10.8901225+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [15]:   Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "34cd9a8d-33a8-40ac-b375-c8d0f188d268", Timestamp = 2025-02-15T04:50:10.7433841+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [16]:   Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "1280d44d-2363-4f53-9521-2125ebb80900", Timestamp = 2025-02-15T04:50:10.8052804+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [17]:   Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "947dc9ab-c4b5-4445-9e24-2b5a81824272", Timestamp = 2025-02-15T04:50:10.7341468+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [18]:   Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "75864c4b-1650-4a34-b59b-9d6dcd405c26", Timestamp = 2025-02-15T04:50:10.8549610+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [19]:   Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "75864c4b-1650-4a34-b59b-9d6dcd405c26", Timestamp = 2025-02-15T04:50:10.8549858+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [20]:   Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "af240f73-6f14-4d05-b58c-5e789effa89a", Timestamp = 2025-02-15T04:50:10.8188358
      +00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [21]:   Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327762+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [22]:   Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327776+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [23]:   Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327780+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [24]:   Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327784+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [25]:   Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327788+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [26]:   Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327791+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [27]:   Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327800+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [28]:   Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327828+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [29]:   Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327832+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [30]:   Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327835+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [31]:   Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327854+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [32]:   Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327860+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [33]:   Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327865+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [34]:   Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327871+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [35]:   Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327904+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [36]:   Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327910+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [37]:   Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327915+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [38]:   Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327920+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [39]:   Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327926+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [40]:   Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327931+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [41]:   Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327936+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [42]:   Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327988+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [43]:   Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8327994+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [44]:   Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328000+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [45]:   Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328005+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [46]:   Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328010+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [47]:   Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328047+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [48]:   Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328052+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [49]:   Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328057+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [50]:   Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328062+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [51]:   Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328067+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [52]:   Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328073+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [53]:   Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328078+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [54]:   Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328158+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [55]:   Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328164+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [56]:   Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328169+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [57]:   Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328219+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [58]:   Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328225+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [59]:   Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328230+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [60]:   Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328235+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [61]:   Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328240+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [62]:   Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328278+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [63]:   Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328319+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [64]:   Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328325+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [65]:   Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328331+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [66]:   Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328336+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [67]:   Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328341+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [68]:   Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328346+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [69]:   Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328352+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [70]:   Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328384+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [71]:   Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328389+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [72]:   Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328394+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [73]:   Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328399+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [74]:   Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328405+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [75]:   Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328435+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [76]:   Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328441+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [77]:   Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328446+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [78]:   Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328451+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [79]:   Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328456+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [80]:   Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328462+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [81]:   Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328490+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [82]:   Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328496+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [83]:   Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328501+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [84]:   Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328506+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [85]:   Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328512+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [86]:   Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328517+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [87]:   Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328551+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [88]:   Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328557+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [89]:   Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328562+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [90]:   Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328567+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [91]:   Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328572+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [92]:   Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328577+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [93]:   Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328583+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [94]:   Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328612+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [95]:   Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328617+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [96]:   Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328622+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [97]:   Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328628+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [98]:   Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328633+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [99]:   Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328638+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [100]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328665+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [101]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328670+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [102]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328675+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [103]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328680+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [104]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328685+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [105]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328690+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [106]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328731+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [107]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328736+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [108]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328741+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [109]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328747+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [110]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328752+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [111]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "f70faf01-b10d-4d34-904e-bfae10f4302e", Timestamp = 2025-02-15T04:50:10.8328757+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [112]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "064f0939-3810-42b7-a52a-2f88ac7e1ff6", Timestamp = 2025-02-15T04:50:10.6985845+00:00 }
              Error: Event timestamp 2/15/2025 4:50:10 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [113]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "25556dbc-a206-486d-b684-f2ab31a1ae98", Timestamp = 2025-02-15T04:52:38.9701889+00:00 }
              Error: Event timestamp 2/15/2025 4:52:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [114]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "fa99dec8-7b1b-4cba-b175-355a5316e1d8", Timestamp = 2025-02-15T04:52:39.0428777+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [115]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "87465394-4990-4629-ab5f-9161acc4d9e6", Timestamp = 2025-02-15T04:52:39.0509070+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [116]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "87465394-4990-4629-ab5f-9161acc4d9e6", Timestamp = 2025-02-15T04:52:39.0509141+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [117]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "87465394-4990-4629-ab5f-9161acc4d9e6", Timestamp = 2025-02-15T04:52:39.0509149+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [118]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "35a70fc3-e2c3-4357-b05b-daef3ea3fe38", Timestamp = 2025-02-15T04:52:39.1495794+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [119]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "0bdc5d91-422b-46ce-a497-365342853ea1", Timestamp = 2025-02-15T04:52:39.2103229+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [120]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "0bdc5d91-422b-46ce-a497-365342853ea1", Timestamp = 2025-02-15T04:52:39.2103534+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [121]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "6c1fd7f6-82d7-4144-ac04-97636704fd17", Timestamp = 2025-02-15T04:52:39.0326790+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [122]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "e5a75053-7f03-4342-ae3f-6fd0fc7efe9c", Timestamp = 2025-02-15T04:52:39.0815315+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [123]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "ba7b3b23-abe0-4200-90d8-04b569055036", Timestamp = 2025-02-15T04:52:39.0720270+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [124]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "68c9c7c0-6dab-4b7f-8e87-a3763d66fbeb", Timestamp = 2025-02-15T04:52:39.1338393+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [125]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "10f43db4-ef22-414e-87fb-ee84d12811b8", Timestamp = 2025-02-15T04:52:39.1638494
      +00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [126]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "5915370a-8245-4f28-a401-6f7693372511", Timestamp = 2025-02-15T04:52:39.2521491+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [127]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821380+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [128]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821395+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [129]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821399+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [130]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821403+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [131]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821407+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [132]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821410+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [133]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821421+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [134]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821428+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [135]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821433+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [136]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821436+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [137]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821454+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [138]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821461+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [139]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821467+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [140]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821472+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [141]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821479+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [142]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821485+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [143]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821490+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [144]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821495+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [145]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821500+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [146]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821505+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [147]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821511+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [148]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821519+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [149]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821526+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [150]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821531+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [151]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821536+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [152]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821541+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [153]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821549+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [154]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821555+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [155]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821560+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [156]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821565+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [157]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821570+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [158]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821576+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [159]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821581+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [160]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821707+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [161]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821712+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [162]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821717+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [163]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821723+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [164]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821728+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [165]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821737+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [166]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821742+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [167]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821747+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [168]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821788+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [169]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821794+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [170]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821799+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [171]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821815+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [172]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821821+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [173]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821826+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [174]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821831+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [175]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821837+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [176]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821842+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [177]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821849+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [178]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821855+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [179]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821860+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [180]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821865+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [181]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821872+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [182]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821881+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [183]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821887+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [184]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821892+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [185]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821897+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [186]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821903+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [187]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821908+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [188]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821913+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [189]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821923+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [190]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821929+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [191]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821934+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [192]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821939+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [193]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821944+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [194]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821949+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [195]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821959+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [196]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821964+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [197]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821969+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [198]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821974+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [199]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821979+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [200]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821985+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [201]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1821997+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [202]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822002+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [203]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822007+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [204]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822013+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [205]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822018+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [206]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822023+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [207]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822028+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [208]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822038+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [209]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822043+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [210]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822048+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [211]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822053+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [212]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822059+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [213]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822064+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [214]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822072+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [215]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822077+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [216]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822082+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [217]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "ad5e96ba-c125-470a-acde-678eb4be6057", Timestamp = 2025-02-15T04:52:39.1822088+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [218]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "8b53aac9-b5ac-4f47-aec0-ea2147d37504", Timestamp = 2025-02-15T04:52:39.1087988+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [219]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "8b53aac9-b5ac-4f47-aec0-ea2147d37504", Timestamp = 2025-02-15T04:52:39.1087992+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [220]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "8b53aac9-b5ac-4f47-aec0-ea2147d37504", Timestamp = 2025-02-15T04:52:39.1087998+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [221]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "8b53aac9-b5ac-4f47-aec0-ea2147d37504", Timestamp = 2025-02-15T04:52:39.1087995+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [222]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "8b53aac9-b5ac-4f47-aec0-ea2147d37504", Timestamp = 2025-02-15T04:52:39.1088046+00:00 }
              Error: Event timestamp 2/15/2025 4:52:39 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [223]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_44", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661533+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [224]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661548+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [225]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661565+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [226]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661569+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [227]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661580+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [228]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661589+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [229]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "09e172df-ab50-4a44-877b-d21f1dc5b62b", Timestamp = 2025-02-15T04:52:48.3661608+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [230]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "5229070f-98b7-4c7f-abcc-c4728eac600a", Timestamp = 2025-02-15T04:52:48.4744234+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [231]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "5229070f-98b7-4c7f-abcc-c4728eac600a", Timestamp = 2025-02-15T04:52:48.4744506+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [232]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "d21dbbaa-c775-4491-980d-2730431d8f23", Timestamp = 2025-02-15T04:52:48.5326819+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [233]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "0d81d902-9185-409d-aeaf-aed86949c936", Timestamp = 2025-02-15T04:52:48.2828917+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [234]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "7a865db5-5e5d-406e-bb49-a5032f96ab68", Timestamp = 2025-02-15T04:52:48.2680392+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [235]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4417965+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [236]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4417988+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [237]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4417993+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [238]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4417996+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [239]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418000+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [240]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418003+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [241]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418012+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [242]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418016+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [243]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418020+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [244]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418027+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [245]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418051+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [246]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418058+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [247]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418064+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [248]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418069+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [249]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418075+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [250]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418084+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [251]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418090+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [252]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418095+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [253]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418100+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [254]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418105+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [255]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418111+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [256]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418116+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [257]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418127+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [258]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418132+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [259]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418137+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [260]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418142+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [261]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418148+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [262]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418156+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [263]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418161+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [264]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418167+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [265]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418172+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [266]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418178+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [267]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418183+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [268]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418257+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [269]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418263+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [270]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418268+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [271]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418274+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [272]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418279+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [273]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418284+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [274]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418294+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [275]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418300+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [276]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418338+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [277]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418345+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [278]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418350+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [279]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418356+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [280]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418364+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [281]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418370+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [282]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418375+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [283]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418381+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [284]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418386+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [285]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418391+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [286]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418397+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [287]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418408+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [288]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418413+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [289]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418420+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [290]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418426+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [291]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418434+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [292]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418440+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [293]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418445+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [294]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418451+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [295]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418456+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [296]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418461+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [297]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418467+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [298]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418476+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [299]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418481+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [300]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418486+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [301]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418492+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [302]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418497+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [303]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418502+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [304]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418511+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [305]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418517+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [306]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418522+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [307]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418528+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [308]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418536+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [309]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418541+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [310]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418550+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [311]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418555+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [312]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418560+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [313]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418566+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [314]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418571+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [315]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418576+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [316]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418582+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [317]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418591+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [318]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418596+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [319]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418601+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [320]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418606+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [321]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418612+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [322]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418617+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [323]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418626+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [324]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418632+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [325]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "63b1b96f-a225-4272-a51c-111305eb5512", Timestamp = 2025-02-15T04:52:48.4418637+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [326]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "3a69a7a3-bb81-4e49-9ffa-be19aaed175f", Timestamp = 2025-02-15T04:52:48.1924472+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [327]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "c8310e2d-5bf5-459d-9854-f8816c866146", Timestamp = 2025-02-15T04:52:48.3916401+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [328]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "2abf599f-3606-400c-8577-d2d0f48fc22f", Timestamp = 2025-02-15T04:52:48.4050904+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [329]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "956d50e3-497a-42f4-95f1-36d29d5c8afb", Timestamp = 2025-02-15T04:52:48.4208530
      +00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [330]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "b2b2ae05-c7a0-4aa8-aa87-f08815e2a40a", Timestamp = 2025-02-15T04:52:48.2938487+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [331]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "b2b2ae05-c7a0-4aa8-aa87-f08815e2a40a", Timestamp = 2025-02-15T04:52:48.2938598+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [332]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "b2b2ae05-c7a0-4aa8-aa87-f08815e2a40a", Timestamp = 2025-02-15T04:52:48.2938612+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [333]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "2499d494-fac0-4c4f-8657-358886877107", Timestamp = 2025-02-15T04:52:48.3337137+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [334]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "8b620f3a-aacf-4a7f-83a7-6be42eb2bf70", Timestamp = 2025-02-15T04:52:48.3211666+00:00 }
              Error: Event timestamp 2/15/2025 4:52:48 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [335]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "c17b0e6c-c846-45b5-8759-b63e61c71a53", Timestamp = 2025-02-15T04:53:25.9766277+00:00 }
              Error: Event timestamp 2/15/2025 4:53:25 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [336]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "49cab4c9-e548-4114-a38d-bf56179be458", Timestamp = 2025-02-15T04:53:26.0493767+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [337]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "b5fe8d1d-1d59-4271-8c8e-b04c84313394", Timestamp = 2025-02-15T04:53:26.0658058+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [338]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "902fe17a-e545-440b-86bf-28a02a50a45d", Timestamp = 2025-02-15T04:53:26.3367814+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [339]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "b25a9261-829a-42cf-a15d-92c0db0f5274", Timestamp = 2025-02-15T04:53:26.1928670+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [340]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_44", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590816+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [341]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590833+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [342]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590847+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [343]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590863+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [344]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590871+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [345]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590914+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [346]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "fa5c047a-f1b7-4a1d-8f79-913e5202545f", Timestamp = 2025-02-15T04:53:26.1590921+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [347]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "37b98da6-6c45-4cd2-bd5c-bedd2fd7d493", Timestamp = 2025-02-15T04:53:26.1134333+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [348]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "c52f6652-01ad-4d9f-ae19-35b98393c768", Timestamp = 2025-02-15T04:53:26.2180105+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [349]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "85a3c5d5-940f-45ef-aa60-6378e7956d67", Timestamp = 2025-02-15T04:53:26.0865591+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [350]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "85a3c5d5-940f-45ef-aa60-6378e7956d67", Timestamp = 2025-02-15T04:53:26.0865699+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [351]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "85a3c5d5-940f-45ef-aa60-6378e7956d67", Timestamp = 2025-02-15T04:53:26.0865705+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [352]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "2450138d-e56e-4d4c-84eb-e4dec0202355", Timestamp = 2025-02-15T04:53:26.2863069+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [353]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "2450138d-e56e-4d4c-84eb-e4dec0202355", Timestamp = 2025-02-15T04:53:26.2863310+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [354]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "acaa6e60-689c-4178-9f68-d662cde63c50", Timestamp = 2025-02-15T04:53:26.1259268+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [355]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "75e2ada7-3705-4b1a-9ba0-14dda1bb0912", Timestamp = 2025-02-15T04:53:26.2353144
      +00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [356]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575935+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [357]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575951+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [358]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575955+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [359]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575959+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [360]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575963+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [361]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575967+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [362]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575974+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [363]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575982+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [364]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575986+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [365]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2575990+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [366]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576007+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [367]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576014+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [368]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576020+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [369]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576026+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [370]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576033+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [371]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576038+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [372]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576044+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [373]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576049+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [374]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576055+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [375]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576060+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [376]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576065+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [377]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576074+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [378]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576081+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [379]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576087+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [380]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576092+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [381]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576097+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [382]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576105+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [383]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576110+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [384]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576115+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [385]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576121+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [386]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576126+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [387]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576132+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [388]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576137+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [389]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576202+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [390]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576208+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [391]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576213+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [392]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576218+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [393]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576223+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [394]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576233+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [395]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576238+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [396]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576243+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [397]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576290+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [398]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576296+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [399]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576301+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [400]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576315+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [401]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576320+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [402]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576325+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [403]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576331+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [404]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576336+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [405]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576342+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [406]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576349+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [407]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576355+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [408]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576361+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [409]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576366+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [410]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576373+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [411]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576381+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [412]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576387+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [413]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576392+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [414]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576398+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [415]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576403+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [416]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576409+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [417]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576414+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [418]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576423+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [419]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576428+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [420]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576434+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [421]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576439+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [422]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576444+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [423]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576450+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [424]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576458+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [425]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576464+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [426]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576469+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [427]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576475+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [428]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576480+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [429]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576485+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [430]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576495+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [431]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576501+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [432]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576506+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [433]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576511+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [434]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576516+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [435]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576522+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [436]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576527+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [437]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576538+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [438]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576543+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [439]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576549+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [440]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576554+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [441]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576559+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [442]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576564+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [443]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576577+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [444]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576583+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [445]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576588+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [446]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "7cd5167a-1082-48f5-a977-4cc747301efe", Timestamp = 2025-02-15T04:53:26.2576594+00:00 }
              Error: Event timestamp 2/15/2025 4:53:26 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [447]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "80de192a-52c9-4d04-b615-90305deca4c2", Timestamp = 2025-02-15T04:53:44.9351124+00:00 }
              Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [448]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "80de192a-52c9-4d04-b615-90305deca4c2", Timestamp = 2025-02-15T04:53:44.9351251+00:00 }
              Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [449]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "80de192a-52c9-4d04-b615-90305deca4c2", Timestamp = 2025-02-15T04:53:44.9351257+00:00 }
              Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [450]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "c8575510-4ee2-477f-be92-103be110b79e", Timestamp = 2025-02-15T04:53:44.7964030+00:00 }
              Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [451]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "8c5f8ca5-f807-4e97-8655-aec9514e0858", Timestamp = 2025-02-15T04:53:44.9155473+00:00 }
              Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [452]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "05c389be-1ecd-4ab1-a053-a91d4735d0f9", Timestamp = 2025-02-15T04:53:44.9702478+00:00 }
              Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [453]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "e9aabf0b-85e4-4537-af3f-bd4081ee1dea", Timestamp = 2025-02-15T04:53:44.9834322+00:00 }
              Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [454]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "8ee55608-adaa-4cc4-950f-1f8d2c7f4d79", Timestamp = 2025-02-15T04:53:45.0044678+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [455]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "8ee55608-adaa-4cc4-950f-1f8d2c7f4d79", Timestamp = 2025-02-15T04:53:45.0044719+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [456]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "8ee55608-adaa-4cc4-950f-1f8d2c7f4d79", Timestamp = 2025-02-15T04:53:45.0044727+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [457]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "8ee55608-adaa-4cc4-950f-1f8d2c7f4d79", Timestamp = 2025-02-15T04:53:45.0044719+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [458]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "8ee55608-adaa-4cc4-950f-1f8d2c7f4d79", Timestamp = 2025-02-15T04:53:45.0044771+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [459]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "8ee55608-adaa-4cc4-950f-1f8d2c7f4d79", Timestamp = 2025-02-15T04:53:45.0044790+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [460]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "f069abec-4a84-4224-9adf-7c7930d6b028", Timestamp = 2025-02-15T04:53:44.8895055+00:00 }
              Error: Event timestamp 2/15/2025 4:53:44 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [461]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "920d867f-5fc4-4f53-89ca-a60cc1955a74", Timestamp = 2025-02-15T04:53:45.0726663+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [462]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "d59cfe1f-81e5-478b-bd97-c31deeb0b1bf", Timestamp = 2025-02-15T04:53:45.0430767+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [463]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "88b5a1c8-3b03-407b-97bb-c8379601122b", Timestamp = 2025-02-15T04:53:45.1761036+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [464]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "88b5a1c8-3b03-407b-97bb-c8379601122b", Timestamp = 2025-02-15T04:53:45.1761350+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [465]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "510537a6-13b8-4570-b41e-b9bb132840c9", Timestamp = 2025-02-15T04:53:45.0998590
      +00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [466]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339648+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [467]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339679+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [468]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339689+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [469]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339695+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [470]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339701+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [471]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339705+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [472]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339719+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [473]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339730+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [474]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339734+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [475]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339738+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [476]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339764+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [477]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339771+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [478]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339776+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [479]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339781+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [480]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339790+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [481]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339796+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [482]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339801+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [483]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339806+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [484]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339811+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [485]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339817+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [486]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339822+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [487]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339832+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [488]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339839+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [489]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339845+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [490]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339850+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [491]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339856+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [492]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339874+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [493]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339884+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [494]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339900+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [495]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339908+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [496]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339917+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [497]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339925+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [498]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1339935+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [499]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340041+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [500]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340049+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [501]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340058+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [502]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340125+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [503]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340152+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [504]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340171+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [505]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340180+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [506]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340188+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [507]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340245+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [508]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340254+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [509]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340262+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [510]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340280+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [511]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340288+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [512]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340296+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [513]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340304+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [514]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340314+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [515]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340322+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [516]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340330+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [517]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340345+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [518]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340353+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [519]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340361+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [520]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340375+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [521]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340388+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [522]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340398+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [523]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340407+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [524]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340420+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [525]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340428+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [526]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340437+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [527]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340446+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [528]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340459+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [529]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340467+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [530]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340476+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [531]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340484+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [532]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340494+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [533]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340502+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [534]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340514+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [535]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340523+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [536]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340531+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [537]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340539+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [538]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340547+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [539]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340555+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [540]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340573+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [541]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340582+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [542]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340590+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [543]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340599+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [544]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340607+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [545]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340616+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [546]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340624+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [547]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340637+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [548]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340646+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [549]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340654+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [550]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340663+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [551]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340682+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [552]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340691+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [553]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340729+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [554]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340793+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [555]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340809+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [556]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "ca4babc9-7be1-40b1-86b6-a36552df0752", Timestamp = 2025-02-15T04:53:45.1340821+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [557]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "0a539fa7-93de-48fa-885b-a8120f8f350f", Timestamp = 2025-02-15T04:53:45.2420085+00:00 }
              Error: Event timestamp 2/15/2025 4:53:45 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [558]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "8873f27d-4a4a-41e9-a8c8-8e9fd3ef8b39", Timestamp = 2025-02-15T04:54:47.3247606+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [559]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "7f6b8828-5675-4e6e-8c16-8ee2aa4a17eb", Timestamp = 2025-02-15T04:54:47.4930229
      +00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [560]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "1c70e69e-d4df-496e-bd8a-4971c70ba9ef", Timestamp = 2025-02-15T04:54:47.3015739+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [561]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "5e9d6e9b-abc2-4921-af94-c3487c9572bd", Timestamp = 2025-02-15T04:54:47.5549647+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [562]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "5e9d6e9b-abc2-4921-af94-c3487c9572bd", Timestamp = 2025-02-15T04:54:47.5549889+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [563]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "6087c0e3-8b44-44cc-80f2-9fe46baba59b", Timestamp = 2025-02-15T04:54:47.2164627+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [564]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_44", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132588+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [565]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132591+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [566]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132602+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [567]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132623+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [568]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132653+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [569]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132657+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [570]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "51e46255-3ed3-446f-9024-2e53e6905572", Timestamp = 2025-02-15T04:54:47.4132700+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [571]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "73331991-6c1e-4cc3-92b9-af97e7a9ad56", Timestamp = 2025-02-15T04:54:47.4719221+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [572]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "f0d17914-30be-46eb-9a84-2b21fbece80d", Timestamp = 2025-02-15T04:54:47.3777291+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [573]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "583fd4a3-b101-4c11-8fc2-55a78e2867db", Timestamp = 2025-02-15T04:54:47.3882986+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [574]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "da3736e1-a18e-4d7c-8c88-5483aa18a4b0", Timestamp = 2025-02-15T04:54:47.4500058+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [575]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217817+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [576]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217830+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [577]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217834+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [578]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217838+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [579]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217842+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [580]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217845+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [581]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217853+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [582]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217861+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [583]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217865+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [584]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217869+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [585]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217889+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [586]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217895+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [587]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217901+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [588]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217906+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [589]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217915+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [590]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217920+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [591]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217925+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [592]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217930+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [593]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217935+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [594]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217940+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [595]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217945+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [596]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217954+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [597]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217961+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [598]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217966+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [599]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217971+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [600]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217976+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [601]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217985+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [602]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217990+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [603]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5217996+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [604]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218001+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [605]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218006+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [606]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218011+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [607]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218016+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [608]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218081+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [609]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218087+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [610]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218092+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [611]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218097+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [612]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218102+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [613]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218111+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [614]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218116+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [615]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218121+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [616]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218167+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [617]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218172+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [618]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218177+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [619]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218192+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [620]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218197+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [621]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218202+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [622]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218207+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [623]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218212+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [624]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218217+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [625]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218224+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [626]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218231+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [627]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218236+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [628]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218241+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [629]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218247+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [630]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218255+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [631]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218260+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [632]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218265+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [633]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218270+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [634]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218276+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [635]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218281+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [636]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218286+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [637]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218295+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [638]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218301+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [639]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218306+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [640]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218311+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [641]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218316+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [642]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218321+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [643]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218329+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [644]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218334+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [645]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218339+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [646]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218345+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [647]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218350+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [648]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218355+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [649]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218363+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [650]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218368+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [651]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218373+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [652]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218378+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [653]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218383+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [654]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218388+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [655]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218393+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [656]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218401+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [657]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218406+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [658]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218412+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [659]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218417+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [660]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218422+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [661]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218427+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [662]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218435+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [663]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218440+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [664]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218445+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [665]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "f18957cc-2882-4430-bd91-26844031bdd3", Timestamp = 2025-02-15T04:54:47.5218451+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [666]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "eb562f74-27ea-4903-a453-0b11a9f79f7a", Timestamp = 2025-02-15T04:54:47.3467089+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [667]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "eb562f74-27ea-4903-a453-0b11a9f79f7a", Timestamp = 2025-02-15T04:54:47.3467206+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [668]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "eb562f74-27ea-4903-a453-0b11a9f79f7a", Timestamp = 2025-02-15T04:54:47.3467229+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [669]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "920102ee-dc89-46e5-9ae5-7abc8d50a1e7", Timestamp = 2025-02-15T04:54:47.6133427+00:00 }
              Error: Event timestamp 2/15/2025 4:54:47 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [670]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "0af70fa1-d2bd-464f-a319-1b333bd6daa5", Timestamp = 2025-02-15T04:55:01.8908354+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [671]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "618e3272-6c62-44b2-bb40-8dbdeb06d49f", Timestamp = 2025-02-15T04:55:01.9139878+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [672]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "618e3272-6c62-44b2-bb40-8dbdeb06d49f", Timestamp = 2025-02-15T04:55:01.9139995+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [673]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "618e3272-6c62-44b2-bb40-8dbdeb06d49f", Timestamp = 2025-02-15T04:55:01.9140002+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [674]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "a7d1b805-a2ab-4fec-8650-21c5d3526040", Timestamp = 2025-02-15T04:55:01.9512407+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [675]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "b7bb6059-a8bf-4f5a-af8c-5d4b95fea28d", Timestamp = 2025-02-15T04:55:01.7611658+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [676]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "ab67640b-d9ce-4f92-a82f-6403f5d51d36", Timestamp = 2025-02-15T04:55:01.9628967+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [677]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_44", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945217+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [678]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945252+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [679]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945279+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [680]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945292+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [681]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945301+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [682]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945316+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [683]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "5426169f-d364-4c75-81ae-0f8dd47a0020", Timestamp = 2025-02-15T04:55:01.9945360+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [684]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "ab86fe09-c1cb-4260-9368-f6ba6099f32a", Timestamp = 2025-02-15T04:55:01.8689788+00:00 }
              Error: Event timestamp 2/15/2025 4:55:01 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [685]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "99ee3905-ac39-4262-af54-6b8a027d2885", Timestamp = 2025-02-15T04:55:02.1555927+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [686]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "99ee3905-ac39-4262-af54-6b8a027d2885", Timestamp = 2025-02-15T04:55:02.1556173+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [687]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "01c2d80c-8fe3-494c-a02d-f457d54b14bf", Timestamp = 2025-02-15T04:55:02.0315845+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [688]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188751+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [689]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188768+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [690]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188772+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [691]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188776+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [692]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188780+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [693]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188783+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [694]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188791+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [695]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188801+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [696]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188805+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [697]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188809+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [698]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188830+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [699]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188837+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [700]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188843+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [701]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188848+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [702]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188855+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [703]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188860+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [704]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188865+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [705]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188870+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [706]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188875+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [707]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188880+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [708]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188885+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [709]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188893+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [710]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188900+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [711]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188906+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [712]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188910+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [713]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188916+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [714]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188926+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [715]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188931+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [716]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188936+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [717]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188941+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [718]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188946+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [719]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188951+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [720]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1188956+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [721]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189029+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [722]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189034+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [723]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189039+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [724]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189044+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [725]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189049+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [726]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189057+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [727]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189062+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [728]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189067+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [729]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189101+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [730]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189107+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [731]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189112+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [732]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189122+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [733]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189127+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [734]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189132+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [735]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189137+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [736]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189142+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [737]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189147+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [738]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189152+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [739]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189163+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [740]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189168+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [741]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189173+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [742]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189179+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [743]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189190+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [744]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189196+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [745]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189201+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [746]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189206+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [747]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189211+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [748]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189216+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [749]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189221+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [750]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189233+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [751]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189238+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [752]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189243+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [753]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189248+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [754]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189253+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [755]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189258+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [756]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189269+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [757]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189274+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [758]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189280+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [759]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189285+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [760]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189290+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [761]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189295+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [762]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189306+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [763]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189311+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [764]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189317+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [765]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189322+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [766]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189327+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [767]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189332+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [768]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189337+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [769]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189350+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [770]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189355+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [771]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189360+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [772]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189365+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [773]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189370+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [774]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189375+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [775]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189389+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [776]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189394+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [777]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189399+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [778]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "20acc042-1841-404d-8fab-0d879cb02ee0", Timestamp = 2025-02-15T04:55:02.1189405+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [779]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "92ebf1c4-6069-4bca-b379-7b3fc4a96971", Timestamp = 2025-02-15T04:55:02.0824390
      +00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [780]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "a5c67bbd-e073-4bcc-9b5c-2d164f80ca55", Timestamp = 2025-02-15T04:55:02.2204372+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [781]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "890ef49f-25ab-4eeb-9b08-edabca37af00", Timestamp = 2025-02-15T04:55:02.0584724+00:00 }
              Error: Event timestamp 2/15/2025 4:55:02 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [782]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "c19ff0c5-5df5-4857-8647-b134bfc089a2", Timestamp = 2025-02-15T04:55:37.8100886+00:00 }
              Error: Event timestamp 2/15/2025 4:55:37 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [783]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "22d40e1d-bcb0-4148-a1ff-69fd44c36767", Timestamp = 2025-02-15T04:55:37.9358532+00:00 }
              Error: Event timestamp 2/15/2025 4:55:37 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [784]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "2cbb3aba-654b-4890-9749-9b7d4f7d6c18", Timestamp = 2025-02-15T04:55:37.9666141+00:00 }
              Error: Event timestamp 2/15/2025 4:55:37 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [785]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "35d59729-ea21-4760-b21b-d87e61eb32dd", Timestamp = 2025-02-15T04:55:37.9944263+00:00 }
              Error: Event timestamp 2/15/2025 4:55:37 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [786]:  Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "35d59729-ea21-4760-b21b-d87e61eb32dd", Timestamp = 2025-02-15T04:55:37.9944370+00:00 }
              Error: Event timestamp 2/15/2025 4:55:37 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [787]:  Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "35d59729-ea21-4760-b21b-d87e61eb32dd", Timestamp = 2025-02-15T04:55:37.9944377+00:00 }
              Error: Event timestamp 2/15/2025 4:55:37 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [788]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "314a311f-13e5-4624-b98f-44f3f066d1ca", Timestamp = 2025-02-15T04:55:38.1841249
      +00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [789]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844393+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [790]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_44", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844399+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [791]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844421+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [792]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844432+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [793]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844491+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [794]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844485+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [795]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "a3f41598-439c-487e-a87d-14eafe8165f9", Timestamp = 2025-02-15T04:55:38.0844508+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [796]:  Item:  TelemetryEvent { Name = "TimestampEvent", Properties = [], SessionId = "19a625e6-0be0-4230-9f5a-874452132bec", Timestamp = 2025-02-15T04:55:38.2595146+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [797]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "Exception", ["Message"] = "TimestampException", ["StackTrace"] = "No stack trace"], SessionId = "19a625e6-0be0-4230-9f5a-874452132bec", Timestamp = 2025-02-15T04:55:38.2595420+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [798]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "0e02f8a1-0217-404e-a956-29622e3bce77", Timestamp = 2025-02-15T04:55:38.0500181+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [799]:  Item:  TelemetryEvent { Name = "FinalEvent", Properties = [], SessionId = "14691a72-efc5-4b37-a055-46c169133e24", Timestamp = 2025-02-15T04:55:38.3274055+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [800]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "39373b6f-fe7a-46e0-8f0a-0c2e7277bf3a", Timestamp = 2025-02-15T04:55:38.1238429+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [801]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198474+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [802]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198487+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [803]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198495+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [804]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198499+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [805]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198503+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [806]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198508+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [807]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198515+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [808]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198519+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [809]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198522+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [810]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198526+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [811]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198556+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [812]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198563+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [813]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198569+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [814]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198574+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [815]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198579+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [816]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198584+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [817]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198589+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [818]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198598+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [819]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198603+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [820]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198608+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [821]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198613+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [822]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198619+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [823]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198628+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [824]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198633+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [825]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198638+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [826]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198644+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [827]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198649+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [828]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198654+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [829]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198659+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [830]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198667+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [831]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198673+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [832]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198678+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [833]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198683+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [834]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198746+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [835]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198755+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [836]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198760+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [837]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198765+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [838]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198771+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [839]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198776+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [840]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198781+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [841]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198790+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [842]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198826+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [843]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198831+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [844]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198836+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [845]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198841+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [846]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198847+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [847]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198852+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [848]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198860+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [849]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198866+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [850]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198871+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [851]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198876+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [852]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198881+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [853]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198886+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [854]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198898+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [855]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198905+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [856]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198910+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [857]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198915+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [858]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198920+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [859]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198928+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [860]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198934+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [861]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198939+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [862]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198944+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [863]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198949+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [864]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198954+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [865]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198970+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [866]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198975+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [867]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198980+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [868]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198985+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [869]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198990+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [870]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2198996+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [871]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199004+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [872]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199009+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [873]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199014+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [874]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199019+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [875]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199024+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [876]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199030+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [877]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199035+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [878]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199047+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [879]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199052+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [880]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199057+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [881]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199063+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [882]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199068+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [883]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199073+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [884]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199081+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [885]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199087+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [886]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199092+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [887]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199097+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [888]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199102+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [889]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199107+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [890]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199119+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [891]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "b14616a8-d305-4b4d-829d-2ca0cdb4b15f", Timestamp = 2025-02-15T04:55:38.2199125+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [892]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "b2026fc5-5bb1-4cb0-8a9f-97809d57361a", Timestamp = 2025-02-15T04:55:38.1509002+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [893]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "0d61015b-0c1f-434b-a3a0-2ee67cab37a6", Timestamp = 2025-02-15T04:55:38.0395169+00:00 }
              Error: Event timestamp 2/15/2025 4:55:38 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [894]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "fe0a20b6-a95b-4873-91a2-64517df7d920", Timestamp = 2025-02-15T04:56:27.9848148+00:00 }
              Error: Event timestamp 2/15/2025 4:56:27 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [895]:  Item:  TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "e4a0119b-850d-4a4c-9689-acac0f79853a", Timestamp = 2025-02-15T04:56:27.8435991+00:00 }
              Error: Event timestamp 2/15/2025 4:56:27 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [896]:  Item:  TelemetryEvent { Name = "LargePropertyEvent", Properties = [["LargeProperty"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "a692ed54-732a-4bcf-ad37-daa2ee9f23ee", Timestamp = 2025-02-15T04:56:28.2149256+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [897]:  Item:  TelemetryEvent { Name = "Event2", Properties = [["LargeValue"] = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"···], SessionId = "5d49eb6c-2f69-4f05-b686-65680ad58676", Timestamp = 2025-02-15T04:56:28.1041441+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [898]:  Item:  TelemetryEvent { Name = "Exception", Properties = [["ExceptionType"] = "InvalidOperationException", ["Message"] = "Test exception", ["StackTrace"] = "No stack trace"], SessionId = "3074e629-c374-428e-9623-0ffd621514e3", Timestamp = 2025-02-15T04:56:28.2461552
      +00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [899]:  Item:  TelemetryEvent { Name = "FutureEvent", Properties = [], SessionId = "486019ed-cb5d-4195-a71c-fa9c3708fc09", Timestamp = 2025-02-15T04:56:28.1807680+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [900]:  Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "a67c622b-f89e-4735-9d39-de6694642f5e", Timestamp = 2025-02-15T04:56:28.1152114+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [901]:  Item:  TelemetryEvent { Name = "Event10", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840098+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [902]:  Item:  TelemetryEvent { Name = "Event11", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840113+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [903]:  Item:  TelemetryEvent { Name = "Event12", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840116+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [904]:  Item:  TelemetryEvent { Name = "Event13", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840120+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [905]:  Item:  TelemetryEvent { Name = "Event14", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840123+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [906]:  Item:  TelemetryEvent { Name = "Event15", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840127+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [907]:  Item:  TelemetryEvent { Name = "Event16", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840135+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [908]:  Item:  TelemetryEvent { Name = "Event17", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840141+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [909]:  Item:  TelemetryEvent { Name = "Event18", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840145+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [910]:  Item:  TelemetryEvent { Name = "Event19", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840149+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [911]:  Item:  TelemetryEvent { Name = "Event20", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840170+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [912]:  Item:  TelemetryEvent { Name = "Event21", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840176+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [913]:  Item:  TelemetryEvent { Name = "Event22", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840182+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [914]:  Item:  TelemetryEvent { Name = "Event23", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840187+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [915]:  Item:  TelemetryEvent { Name = "Event24", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840195+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [916]:  Item:  TelemetryEvent { Name = "Event25", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840200+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [917]:  Item:  TelemetryEvent { Name = "Event26", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840205+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [918]:  Item:  TelemetryEvent { Name = "Event27", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840211+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [919]:  Item:  TelemetryEvent { Name = "Event28", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840216+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [920]:  Item:  TelemetryEvent { Name = "Event29", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840221+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [921]:  Item:  TelemetryEvent { Name = "Event30", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840227+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [922]:  Item:  TelemetryEvent { Name = "Event31", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840235+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [923]:  Item:  TelemetryEvent { Name = "Event32", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840241+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [924]:  Item:  TelemetryEvent { Name = "Event33", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840247+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [925]:  Item:  TelemetryEvent { Name = "Event34", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840253+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [926]:  Item:  TelemetryEvent { Name = "Event35", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840258+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [927]:  Item:  TelemetryEvent { Name = "Event36", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840266+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [928]:  Item:  TelemetryEvent { Name = "Event37", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840271+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [929]:  Item:  TelemetryEvent { Name = "Event38", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840276+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [930]:  Item:  TelemetryEvent { Name = "Event39", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840282+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [931]:  Item:  TelemetryEvent { Name = "Event40", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840287+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [932]:  Item:  TelemetryEvent { Name = "Event41", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840292+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [933]:  Item:  TelemetryEvent { Name = "Event42", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840297+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [934]:  Item:  TelemetryEvent { Name = "Event43", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840365+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [935]:  Item:  TelemetryEvent { Name = "Event44", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840370+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [936]:  Item:  TelemetryEvent { Name = "Event45", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840375+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [937]:  Item:  TelemetryEvent { Name = "Event46", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840381+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [938]:  Item:  TelemetryEvent { Name = "Event47", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840386+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [939]:  Item:  TelemetryEvent { Name = "Event48", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840401+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [940]:  Item:  TelemetryEvent { Name = "Event49", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840406+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [941]:  Item:  TelemetryEvent { Name = "Event50", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840412+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [942]:  Item:  TelemetryEvent { Name = "Event51", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840447+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [943]:  Item:  TelemetryEvent { Name = "Event52", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840453+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [944]:  Item:  TelemetryEvent { Name = "Event53", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840458+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [945]:  Item:  TelemetryEvent { Name = "Event54", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840468+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [946]:  Item:  TelemetryEvent { Name = "Event55", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840473+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [947]:  Item:  TelemetryEvent { Name = "Event56", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840479+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [948]:  Item:  TelemetryEvent { Name = "Event57", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840484+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [949]:  Item:  TelemetryEvent { Name = "Event58", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840489+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [950]:  Item:  TelemetryEvent { Name = "Event59", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840495+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [951]:  Item:  TelemetryEvent { Name = "Event60", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840503+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [952]:  Item:  TelemetryEvent { Name = "Event61", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840509+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [953]:  Item:  TelemetryEvent { Name = "Event62", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840515+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [954]:  Item:  TelemetryEvent { Name = "Event63", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840520+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [955]:  Item:  TelemetryEvent { Name = "Event64", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840527+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [956]:  Item:  TelemetryEvent { Name = "Event65", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840535+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [957]:  Item:  TelemetryEvent { Name = "Event66", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840540+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [958]:  Item:  TelemetryEvent { Name = "Event67", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840546+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [959]:  Item:  TelemetryEvent { Name = "Event68", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840551+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [960]:  Item:  TelemetryEvent { Name = "Event69", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840556+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [961]:  Item:  TelemetryEvent { Name = "Event70", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840562+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [962]:  Item:  TelemetryEvent { Name = "Event71", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840567+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [963]:  Item:  TelemetryEvent { Name = "Event72", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840576+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [964]:  Item:  TelemetryEvent { Name = "Event73", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840581+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [965]:  Item:  TelemetryEvent { Name = "Event74", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840588+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [966]:  Item:  TelemetryEvent { Name = "Event75", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840593+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [967]:  Item:  TelemetryEvent { Name = "Event76", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840599+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [968]:  Item:  TelemetryEvent { Name = "Event77", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840604+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [969]:  Item:  TelemetryEvent { Name = "Event78", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840613+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [970]:  Item:  TelemetryEvent { Name = "Event79", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840618+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [971]:  Item:  TelemetryEvent { Name = "Event80", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840624+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [972]:  Item:  TelemetryEvent { Name = "Event81", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840629+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [973]:  Item:  TelemetryEvent { Name = "Event82", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840634+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [974]:  Item:  TelemetryEvent { Name = "Event83", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840640+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [975]:  Item:  TelemetryEvent { Name = "Event84", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840648+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [976]:  Item:  TelemetryEvent { Name = "Event85", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840653+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [977]:  Item:  TelemetryEvent { Name = "Event86", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840659+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [978]:  Item:  TelemetryEvent { Name = "Event87", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840664+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [979]:  Item:  TelemetryEvent { Name = "Event88", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840669+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [980]:  Item:  TelemetryEvent { Name = "Event89", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840675+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [981]:  Item:  TelemetryEvent { Name = "Event90", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840680+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [982]:  Item:  TelemetryEvent { Name = "Event91", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840689+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [983]:  Item:  TelemetryEvent { Name = "Event92", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840694+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [984]:  Item:  TelemetryEvent { Name = "Event93", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840699+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [985]:  Item:  TelemetryEvent { Name = "Event94", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840704+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [986]:  Item:  TelemetryEvent { Name = "Event95", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840710+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [987]:  Item:  TelemetryEvent { Name = "Event96", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840715+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [988]:  Item:  TelemetryEvent { Name = "Event97", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840724+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [989]:  Item:  TelemetryEvent { Name = "Event98", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840730+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [990]:  Item:  TelemetryEvent { Name = "Event99", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840735+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [991]:  Item:  TelemetryEvent { Name = "Event100", Properties = [], SessionId = "ef417f3c-554c-4b4b-81a0-f57f90712a42", Timestamp = 2025-02-15T04:56:28.2840741+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [992]:  Item:  TelemetryEvent { Name = "TestEvent", Properties = [], SessionId = "e31c725b-1e1b-4a9f-abc9-61e1d6c3488d", Timestamp = 2025-02-15T04:56:28.0286817+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [993]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_45", Properties = [], SessionId = "cb192657-af40-4279-8027-d911e3f9a834", Timestamp = 2025-02-15T04:56:28.1383979+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [994]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_46", Properties = [], SessionId = "cb192657-af40-4279-8027-d911e3f9a834", Timestamp = 2025-02-15T04:56:28.1383993+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [995]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_47", Properties = [], SessionId = "cb192657-af40-4279-8027-d911e3f9a834", Timestamp = 2025-02-15T04:56:28.1384064+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [996]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_48", Properties = [], SessionId = "cb192657-af40-4279-8027-d911e3f9a834", Timestamp = 2025-02-15T04:56:28.1386680+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [997]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_49", Properties = [], SessionId = "cb192657-af40-4279-8027-d911e3f9a834", Timestamp = 2025-02-15T04:56:28.1386703+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [998]:  Item:  TelemetryEvent { Name = "ConcurrentEvent_50", Properties = [], SessionId = "cb192657-af40-4279-8027-d911e3f9a834", Timestamp = 2025-02-15T04:56:28.1386733+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [1001]: Item:  TelemetryEvent { Name = "Event1", Properties = [], SessionId = "8b7ec67e-ffbd-4c63-af85-c988db455f0a", Timestamp = 2025-02-15T04:56:28.0610900+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [1002]: Item:  TelemetryEvent { Name = "Event2", Properties = [], SessionId = "8b7ec67e-ffbd-4c63-af85-c988db455f0a", Timestamp = 2025-02-15T04:56:28.0610999+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      [1003]: Item:  TelemetryEvent { Name = "Event3", Properties = [], SessionId = "8b7ec67e-ffbd-4c63-af85-c988db455f0a", Timestamp = 2025-02-15T04:56:28.0611005+00:00 }
              Error: Event timestamp 2/15/2025 4:56:28 AM +00:00 should be between 2/15/2025 4:56:28 AM +00:00 and 2/15/2025 4:56:28 AM +00:00
      Stack Trace:
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TimestampsAreUtc_AcrossAllTelemetry() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 360
      --- End of stack trace from previous location ---
    D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(145): error TESTERROR:
      LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackPerformance_RecordsCorrectDuration (11ms): Error Message: Assert.Single() Failure: The collection contained 36 items
      Collection: [TelemetryMetric { Name = "Metric1", Properties = [], SessionId = "34cd9a8d-33a8-40ac-b375-c8d0f188d268", Timestamp = 2025-02-15T04:50:10.7446513+00:00, Value = 42 }, TelemetryMetric { Name = "TimestampMetric", Properties = [], SessionId = "75864c4b-1650-4a34-b5
      9b-9d6dcd405c26", Timestamp = 2025-02-15T04:50:10.8549740+00:00, Value = 42 }, TelemetryMetric { Name = "Performance.TestOperation", Properties = [], SessionId = "4b1a3931-0d10-47bd-8bf5-b05896bbaedf", Timestamp = 2025-02-15T04:50:10.8702490+00:00, Value = 100 }, TelemetryM
      etric { Name = "TestMetric", Properties = [], SessionId = "cf917c53-dbef-4286-86a7-c7ace4952b31", Timestamp = 2025-02-15T04:50:10.6668040+00:00, Value = 42 }, TelemetryMetric { Name = "TimestampMetric", Properties = [], SessionId = "0bdc5d91-422b-46ce-a497-365342853ea1", Ti
      mestamp = 2025-02-15T04:52:39.2103357+00:00, Value = 42 }, ···]
      Stack Trace:
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackPerformance_RecordsCorrectDuration() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 145
      --- End of stack trace from previous location ---
    D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(334): error TESTERROR:
      LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.InvalidBasePath_ThrowsException (5ms): Error Message: Assert.Throws() Failure: No exception was thrown
      Expected: typeof(System.IO.IOException)
      Stack Trace:
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.InvalidBasePath_ThrowsException() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 334
      --- End of stack trace from previous location ---
    D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs(388): error TESTERROR:
      LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_FiltersTestEvents_InProduction (38ms): Error Message: Assert.Single() Failure: The collection contained 1005 items
      Collection: [TelemetryEvent { Name = "BeforeDispose", Properties = [], SessionId = "f4d51922-448d-49dc-8410-094a4034e595", Timestamp = 2025-02-15T04:50:10.6342841+00:00 }, TelemetryEvent { Name = "Event1", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286",
       Timestamp = 2025-02-15T04:50:10.7090010+00:00 }, TelemetryEvent { Name = "Event2", Properties = [], SessionId = "42f199d2-34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090125+00:00 }, TelemetryEvent { Name = "Event3", Properties = [], SessionId = "42f199d2
      -34b3-4ef0-889b-48fb90d0a286", Timestamp = 2025-02-15T04:50:10.7090132+00:00 }, TelemetryEvent { Name = "TestEvent", Properties = [["TestKey"] = "TestValue"], SessionId = "138d7792-0de7-4002-910b-b64cadb31ff1", Timestamp = 2025-02-15T04:50:10.6834637+00:00 }, ···]
      Stack Trace:
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_FiltersTestEvents_InProduction() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 388
         at LofiBeats.Tests.Telemetry.LocalFileTelemetryServiceTests.TrackEvent_FiltersTestEvents_InProduction() in D:\SRC\LofiBeats\tests\LofiBeats.Tests\Telemetry\LocalFileTelemetryServiceTests.cs:line 394
      --- End of stack trace from previous location ---

Test summary: total: 18, failed: 13, succeeded: 5, skipped: 0, duration: 1.2s
Build failed with 13 error(s) in 4.2s

Workload updates are available. Run `dotnet workload list` for more information.
PS D:\SRC\LofiBeats>